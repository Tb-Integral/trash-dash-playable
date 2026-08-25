using System.Runtime.InteropServices;
using UnityEngine;

public class PlayableCTA : MonoBehaviour
{
    [SerializeField] string storeUrl = "https://github.com/Unity-Technologies/EndlessRunnerSampleGame";

#if UNITY_WEBGL && !UNITY_EDITOR && !UNITY_LUNA
    [DllImport("__Internal")]
    static extern void PlayableOpenStore(string url);
#endif

    public void OpenStore()
    {
#if UNITY_LUNA
        Luna.Unity.Playable.InstallFullGame();
#elif UNITY_WEBGL && !UNITY_EDITOR
        if (string.IsNullOrEmpty(storeUrl))
        {
            Debug.LogWarning("Playable CTA: storeUrl is empty.");
            return;
        }
        PlayableOpenStore(storeUrl);
#else
        if (string.IsNullOrEmpty(storeUrl))
        {
            Debug.LogWarning("Playable CTA: storeUrl is empty.");
            return;
        }
        Application.OpenURL(storeUrl);
#endif
    }
}
