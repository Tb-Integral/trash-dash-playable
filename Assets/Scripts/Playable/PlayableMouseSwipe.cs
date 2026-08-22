using UnityEngine;
using UnityEngine.EventSystems;

public class PlayableMouseSwipe : MonoBehaviour
{
    Vector2 _start;
    bool _swiping;

    void Update()
    {
        if (!PlayableSegmentQueue.IsActive)
            return;

        TrackManager tm = TrackManager.instance;
        if (tm == null || !tm.isMoving || tm.characterController == null)
            return;

        if (Input.touchCount > 0)
            return;

        CharacterInputController player = tm.characterController;

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;
            _start = Input.mousePosition;
            _swiping = true;
            return;
        }

        if (!_swiping)
            return;

        if (Input.GetMouseButtonUp(0))
        {
            _swiping = false;
            return;
        }

        if (!Input.GetMouseButton(0))
            return;

        Vector2 diff = (Vector2)Input.mousePosition - _start;
        diff = new Vector2(diff.x / Screen.width, diff.y / Screen.width);
        if (diff.magnitude <= 0.01f)
            return;

        if (Mathf.Abs(diff.y) > Mathf.Abs(diff.x))
        {
            if (diff.y < 0) player.Slide();
            else player.Jump();
        }
        else
        {
            player.ChangeLane(diff.x < 0 ? -1 : 1);
        }

        _swiping = false;
    }
}