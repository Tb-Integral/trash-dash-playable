using System.Runtime.InteropServices;
using UnityEngine;

public class PlayableCTA : MonoBehaviour
{
    [SerializeField] string storeUrl = "https://github.com/Unity-Technologies/EndlessRunnerSampleGame";

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    static extern void PlayableOpenStore(string url);
#endif

    public void OpenStore()
    {
        if (string.IsNullOrEmpty(storeUrl))
        {
            Debug.LogWarning("Playable CTA: storeUrl is empty.");
            return;
        }

#if UNITY_WEBGL && !UNITY_EDITOR
        PlayableOpenStore(storeUrl);
#else
        Application.OpenURL(storeUrl);
#endif
    }
}