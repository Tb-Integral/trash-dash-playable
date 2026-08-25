using UnityEditor;
using UnityEngine;

public static class FixCatRigForLuna
{
    const string CatFbxPath = "Assets/Models/Cat.fbx";
    const string PrefabPath = "Assets/Bundles/Characters/Cat/character.prefab";

    [MenuItem("Playable/Fix Cat Rig For Luna")]
    public static void Fix()
    {
        DisableOptimizeGameObjects();
        RestorePrefabRig();
        SetupLegacyAnimation();
    }

    [InitializeOnLoadMethod]
    static void AutoFixIfNeeded()
    {
        EditorApplication.delayCall += () =>
        {
            if (SessionState.GetBool("FixCatRigForLuna.Attempted", false))
                return;

            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath);
            if (prefab == null)
                return;

            var animator = prefab.GetComponent<Animator>();
            if (animator == null || animator.hasTransformHierarchy)
                return;

            SessionState.SetBool("FixCatRigForLuna.Attempted", true);
            Fix();
        };
    }

    static void DisableOptimizeGameObjects()
    {
        var importer = AssetImporter.GetAtPath(CatFbxPath) as ModelImporter;
        if (importer == null)
        {
            Debug.LogError("Cat.fbx ModelImporter not found at " + CatFbxPath);
            return;
        }

        if (importer.optimizeGameObjects)
        {
            importer.optimizeGameObjects = false;
            importer.SaveAndReimport();
        }
    }

    static void RestorePrefabRig()
    {
        var prefabRoot = PrefabUtility.LoadPrefabContents(PrefabPath);
        try
        {
            var animator = prefabRoot.GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("No Animator on cat prefab " + PrefabPath);
                return;
            }

            animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;

            if (!animator.hasTransformHierarchy)
                AnimatorUtility.DeoptimizeTransformHierarchy(prefabRoot);

            var skeleton = prefabRoot.transform.Find("Skeleton");
            if (skeleton == null)
            {
                foreach (Transform child in prefabRoot.transform)
                {
                    if (child.name == "Skeleton")
                    {
                        skeleton = child;
                        break;
                    }
                }
            }

            if (skeleton != null)
            {
                AddKeepTransformRecursive(skeleton);
                Debug.Log("Cat Skeleton restored with " + CountTransforms(skeleton) + " transforms.");
            }
            else
            {
                Debug.LogWarning("Cat prefab has no Skeleton child after deoptimize. Luna may still strip bones.");
            }

            PrefabUtility.SaveAsPrefabAsset(prefabRoot, PrefabPath);
            Debug.Log("Cat prefab rig restored for Luna: transform hierarchy + LunaKeepTransform.");
        }
        finally
        {
            PrefabUtility.UnloadPrefabContents(prefabRoot);
        }
    }

    static void SetupLegacyAnimation()
    {
        const string folder = "Assets/Animation/Legacy";
        if (!AssetDatabase.IsValidFolder(folder))
            AssetDatabase.CreateFolder("Assets/Animation", "Legacy");

        AnimationClip run = CopyAsLegacy("Assets/Animation/Cat_RunShort.fbx", "Cat_RunShort", folder + "/Cat_RunShort.anim", true);
        AnimationClip jump = CopyAsLegacy("Assets/Animation/Cat_Jump.fbx", "Cat_Jump", folder + "/Cat_Jump.anim", false);
        AnimationClip slide = CopyAsLegacy("Assets/Animation/Cat_Slide.fbx", "Cat_Slide", folder + "/Cat_Slide.anim", false);
        if (run == null || jump == null || slide == null)
        {
            Debug.LogError("Could not copy cat clips as legacy AnimationClips.");
            return;
        }

        var prefabRoot = PrefabUtility.LoadPrefabContents(PrefabPath);
        try
        {
            var anim = prefabRoot.GetComponent<Animation>();
            if (anim == null)
                anim = prefabRoot.AddComponent<Animation>();

            anim.playAutomatically = false;
            anim.clip = run;
            anim.wrapMode = WrapMode.Loop;
            anim.AddClip(run, run.name);
            anim.AddClip(jump, jump.name);
            anim.AddClip(slide, slide.name);

            var driver = prefabRoot.GetComponent<LunaCatLegacyAnim>();
            if (driver == null)
                driver = prefabRoot.AddComponent<LunaCatLegacyAnim>();

            driver.clipPlayer = anim;
            driver.mecanim = prefabRoot.GetComponent<Animator>();
            driver.runClip = run;
            driver.jumpClip = jump;
            driver.slideClip = slide;

            PrefabUtility.SaveAsPrefabAsset(prefabRoot, PrefabPath);
            Debug.Log("Cat legacy Animation wired for Playworks jump/slide.");
        }
        finally
        {
            PrefabUtility.UnloadPrefabContents(prefabRoot);
        }
    }

    static AnimationClip CopyAsLegacy(string fbxPath, string clipName, string destPath, bool loop)
    {
        AnimationClip source = null;
        Object[] assets = AssetDatabase.LoadAllAssetsAtPath(fbxPath);
        for (int i = 0; i < assets.Length; i++)
        {
            var clip = assets[i] as AnimationClip;
            if (clip == null || clip.name != clipName)
                continue;
            source = clip;
            break;
        }

        if (source == null)
        {
            Debug.LogError("Clip " + clipName + " not found in " + fbxPath);
            return null;
        }

        var copy = Object.Instantiate(source);
        copy.name = clipName;
        copy.legacy = true;
        copy.wrapMode = loop ? WrapMode.Loop : WrapMode.Once;

        var existing = AssetDatabase.LoadAssetAtPath<AnimationClip>(destPath);
        if (existing != null)
            AssetDatabase.DeleteAsset(destPath);

        AssetDatabase.CreateAsset(copy, destPath);
        return AssetDatabase.LoadAssetAtPath<AnimationClip>(destPath);
    }

    static int CountTransforms(Transform t)
    {
        int count = 1;
        for (int i = 0; i < t.childCount; i++)
            count += CountTransforms(t.GetChild(i));
        return count;
    }

    static void AddKeepTransformRecursive(Transform t)
    {
        if (t.GetComponent<LunaKeepTransform>() == null)
            t.gameObject.AddComponent<LunaKeepTransform>();

        for (int i = 0; i < t.childCount; i++)
            AddKeepTransformRecursive(t.GetChild(i));
    }
}
