using UnityEngine;

public class PlayableRunEnd : MonoBehaviour
{
    bool _ended;
    bool _lifePinned;

    void LateUpdate()
    {
        if (!PlayableSegmentQueue.IsActive)
            return;

        TrackManager tm = TrackManager.instance;
        if (tm == null || !tm.isLoaded)
            return;

        CharacterInputController player = tm.characterController;
        if (player == null)
            return;

        if (!_lifePinned)
        {
            player.maxLife = 1;
            player.currentLife = 1;
            _lifePinned = true;
        }

        if (_ended)
            return;

        if (player.currentLife <= 0)
        {
            EndRun(tm, won: false);
            return;
        }

        if (!tm.isMoving)
            return;

        if (PlayableSegmentQueue.HasRemaining)
            return;

        int passed = PlayableSegmentQueue.TotalCount - tm.segments.Count;
        if (passed != PlayableSegmentQueue.AuthoredCount - 2)
            return;

        if (tm.currentSegmentDistance >= tm.currentSegment.worldLength * 0.85f)
            EndRun(tm, won: true);
    }

    void EndRun(TrackManager tm, bool won)
    {
        if (PlayableSwipeHint.Instance != null)
            PlayableSwipeHint.DismissCurrent();

        _ended = true;
        Time.timeScale = 1f;
        tm.StopMove();
        if (PlayableEndCard.Instance != null)
        PlayableEndCard.Instance.Show(won);
    }
}