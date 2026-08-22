using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayableBootstrap : MonoBehaviour
{
    [SerializeField] string gameplaySceneName = "Main";

    IEnumerator Start()
    {
        DontDestroyOnLoad(gameObject);

        yield return SceneManager.LoadSceneAsync(gameplaySceneName, LoadSceneMode.Single);

        while (GameManager.instance == null)
            yield return null;

        while (!ThemeDatabase.loaded || !CharacterDatabase.loaded)
            yield return null;

        PlayerData.instance.tutorialDone = true; // только память, без Save()

        GameManager.instance.SwitchState("Game");
    }
}