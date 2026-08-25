using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayableBootstrap : MonoBehaviour
{
    [SerializeField] string gameplaySceneName = "Main";
    [SerializeField] GameObject loadingCover;
    [SerializeField] int readySegmentCount = 4;

    bool _pausedByNetwork;
    bool _wasMoving;
    float _timeScaleBeforePause = 1f;

    void OnEnable()
    {
#if UNITY_LUNA
        Luna.Unity.LifeCycle.OnPause += OnNetworkPause;
        Luna.Unity.LifeCycle.OnResume += OnNetworkResume;
        Luna.Unity.LifeCycle.OnMute += OnNetworkMute;
        Luna.Unity.LifeCycle.OnUnmute += OnNetworkUnmute;
#endif
    }

    void OnDestroy()
    {
#if UNITY_LUNA
        Luna.Unity.LifeCycle.OnPause -= OnNetworkPause;
        Luna.Unity.LifeCycle.OnResume -= OnNetworkResume;
        Luna.Unity.LifeCycle.OnMute -= OnNetworkMute;
        Luna.Unity.LifeCycle.OnUnmute -= OnNetworkUnmute;
#endif
    }

    IEnumerator Start()
    {
        DontDestroyOnLoad(gameObject);
        Time.timeScale = 0f;

        if (loadingCover != null)
            loadingCover.SetActive(true);

        yield return SceneManager.LoadSceneAsync(gameplaySceneName, LoadSceneMode.Single);

        while (GameManager.instance == null)
            yield return null;

        while (!ThemeDatabase.loaded || !CharacterDatabase.loaded)
            yield return null;

        PlayerData.instance.characters.Clear();
        PlayerData.instance.characters.Add("Trash Cat");
        PlayerData.instance.usedCharacter = 0;
        PlayerData.instance.usedAccessory = -1;

        PlayerData.instance.themes.Clear();
        PlayerData.instance.themes.Add("Day");
        PlayerData.instance.usedTheme = 0;

        PlayerData.instance.tutorialDone = true;

        GameManager.instance.SwitchState("Game");

        while (TrackManager.instance == null ||
            !TrackManager.instance.isLoaded ||
            TrackManager.instance.segments.Count < readySegmentCount)
        {
            yield return null;
        }

        yield return null;

        Time.timeScale = 1f;

        if (loadingCover != null)
            loadingCover.SetActive(false);
    }

    void OnNetworkPause()
    {
        if (_pausedByNetwork)
            return;

        _pausedByNetwork = true;
        _timeScaleBeforePause = Time.timeScale;
        Time.timeScale = 0f;

        TrackManager tm = TrackManager.instance;
        if (tm != null)
        {
            _wasMoving = tm.isMoving;
            tm.StopMove();
        }
    }

    void OnNetworkResume()
    {
        if (!_pausedByNetwork)
            return;

        _pausedByNetwork = false;
        Time.timeScale = _timeScaleBeforePause > 0.01f ? _timeScaleBeforePause : 1f;

        if (_wasMoving && TrackManager.instance != null)
            TrackManager.instance.StartMove(false);
    }

    void OnNetworkMute()
    {
        if (MusicPlayer.instance != null)
            MusicPlayer.instance.SetNetworkMuted(true);
    }

    void OnNetworkUnmute()
    {
        if (MusicPlayer.instance != null)
            MusicPlayer.instance.SetNetworkMuted(false);
    }
}
