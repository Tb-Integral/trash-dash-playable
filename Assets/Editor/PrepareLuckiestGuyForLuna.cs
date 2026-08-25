using UnityEditor;
using UnityEngine;

/// <summary>
/// Playworks FontBMs a TTF only when Unity exported no baked glyphs.
/// LuckiestGuy is CustomSet with an empty charset so the Alpha8 atlas is not restored.
/// </summary>
public static class PrepareLuckiestGuyForLuna
{
    const string FontPath = "Assets/Font/LuckiestGuy.ttf";

    [MenuItem("Playable/Prepare LuckiestGuy For Luna")]
    public static void Prepare()
    {
        StripBakedGlyphs(true);
    }

    [InitializeOnLoadMethod]
    static void AutoPrepare()
    {
        EditorApplication.delayCall += () => StripBakedGlyphs(false);
    }

    static void StripBakedGlyphs(bool logAlways)
    {
        var font = AssetDatabase.LoadAssetAtPath<Font>(FontPath);
        if (font == null)
            return;

        int count = font.characterInfo != null ? font.characterInfo.Length : 0;
        if (count == 0)
        {
            if (logAlways || !SessionState.GetBool("PrepareLuckiestGuyForLuna.Logged", false))
            {
                Debug.Log("[LuckiestGuy] No baked glyphs. Playworks should FontBM Assets/Font/LuckiestGuy.ttf.");
                SessionState.SetBool("PrepareLuckiestGuyForLuna.Logged", true);
            }
            return;
        }

        font.characterInfo = new CharacterInfo[0];
        EditorUtility.SetDirty(font);
        Debug.Log("[LuckiestGuy] Stripped " + count + " baked glyphs so Playworks FontBMs the TTF instead of Unity's Alpha8 atlas.");
        SessionState.SetBool("PrepareLuckiestGuyForLuna.Logged", true);
    }
}
