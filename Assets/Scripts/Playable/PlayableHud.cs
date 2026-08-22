using UnityEngine;

public class PlayableHud : MonoBehaviour
{
    bool _hidden;

    void LateUpdate()
    {
        if (_hidden || !PlayableSegmentQueue.IsActive)
            return;

        GameState gs = FindObjectOfType<GameState>();
        if (gs == null || gs.wholeUI == null || !gs.canvas.gameObject.activeInHierarchy)
            return;

        gs.wholeUI.gameObject.SetActive(false);
        if (gs.pauseMenu != null)
            gs.pauseMenu.gameObject.SetActive(false);
        if (gs.gameOverPopup != null)
            gs.gameOverPopup.SetActive(false);

        _hidden = true;
    }
}