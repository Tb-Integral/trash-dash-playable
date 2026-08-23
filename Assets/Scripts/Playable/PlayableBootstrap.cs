using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayableBootstrap : MonoBehaviour
{
    [SerializeField] string gameplaySceneName = "Main";
    [SerializeField] GameObject loadingCover;
    [SerializeField] int readySegmentCount = 4;

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
}