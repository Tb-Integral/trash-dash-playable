using UnityEngine;

public enum PlayableGestureHint
{
    None,
    Lane,
    Jump,
    Slide
}

public sealed class PlayableAuthoredSegment : MonoBehaviour
{
    [SerializeField] PlayableGestureHint hint;
    [SerializeField, Range(0f, 1f)] float hintAtRatio = 0.35f;
    [SerializeField] bool swipeRight = true;

    public PlayableGestureHint Hint => hint;
    public float HintAtRatio => hintAtRatio;
    public bool SwipeRight => swipeRight;
}