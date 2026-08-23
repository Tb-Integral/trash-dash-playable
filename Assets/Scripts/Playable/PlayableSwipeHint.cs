using UnityEngine;

public class PlayableSwipeHint : MonoBehaviour
{
    public static PlayableSwipeHint Instance { get; private set; }

    [SerializeField] float hintTimeScale = 0.4f;
    [SerializeField] GameObject laneHintRoot;
    [SerializeField] GameObject jumpHintRoot;
    [SerializeField] GameObject slideHintRoot;

    bool _shown;
    bool _inputUnlocked;
    TrackSegment _activeSegment;
    TrackSegment _handledSegment;
    PlayableGestureHint _expectedGesture;
    GameObject _activeHintRoot;

    public static bool CanUseInput =>
    !PlayableSegmentQueue.IsActive ||
    (Instance != null && Instance._inputUnlocked);

    void Awake()
    {
        Instance = this;
        SetHintRootsActive(false);
    }

    void SetHintRootsActive(bool value)
    {
        if (laneHintRoot != null)
            laneHintRoot.SetActive(value);

        if (jumpHintRoot != null)
            jumpHintRoot.SetActive(value);

        if (slideHintRoot != null)
            slideHintRoot.SetActive(value);
    }

    GameObject GetHintRoot(PlayableGestureHint gesture)
    {
        switch (gesture)
        {
            case PlayableGestureHint.Lane:
                return laneHintRoot;
            case PlayableGestureHint.Jump:
                return jumpHintRoot;
            case PlayableGestureHint.Slide:
                return slideHintRoot;
            default:
                return null;
        }
    }

    public static void NotifyLaneChanged()
    {
        NotifyGesture(PlayableGestureHint.Lane);
    }

    public static void NotifyJump()
    {
        NotifyGesture(PlayableGestureHint.Jump);
    }

    public static void NotifySlide()
    {
        NotifyGesture(PlayableGestureHint.Slide);
    }

    public static void DismissCurrent()
    {
        if (Instance != null)
            Instance.Dismiss();
    }

    static void NotifyGesture(PlayableGestureHint gesture)
    {
        if (Instance != null &&
            Instance._shown &&
            Instance._expectedGesture == gesture)
        {
            Instance.Dismiss();
        }
    }

    void LateUpdate()
    {
        if (_shown || !PlayableSegmentQueue.IsActive)
            return;

        TrackManager tm = TrackManager.instance;
        if (tm == null || !tm.isLoaded || !tm.isMoving ||
            tm.currentSegment == null ||
            tm.currentSegment == _handledSegment)
        {
            return;
        }

        PlayableAuthoredSegment authored =
            tm.currentSegment.GetComponent<PlayableAuthoredSegment>();

        if (authored == null || authored.Hint == PlayableGestureHint.None)
            return;

        float ratio =
            tm.currentSegmentDistance / tm.currentSegment.worldLength;

        if (ratio < authored.HintAtRatio)
            return;

        Show(tm.currentSegment, authored);
    }

    void Show(TrackSegment segment, PlayableAuthoredSegment authored)
    {
        _activeSegment = segment;
        _expectedGesture = authored.Hint;
        _activeHintRoot = GetHintRoot(authored.Hint);

        if (_activeHintRoot == null)
            return;

        _inputUnlocked = true;
        _shown = true;
        Time.timeScale = hintTimeScale;
        _activeHintRoot.SetActive(true);

        Animation anim =
            _activeHintRoot.GetComponentInChildren<Animation>(true);

        if (anim != null)
        {
            anim.Rewind();
            anim.Play();

            if (anim.clip != null)
            {
                AnimationState state = anim[anim.clip.name];
                if (state != null)
                    state.speed = 1f / Mathf.Max(hintTimeScale, 0.01f);
            }
        }
    }

    void Dismiss()
    {
        if (!_shown)
            return;

        _handledSegment = _activeSegment;
        _activeSegment = null;
        _shown = false;
        Time.timeScale = 1f;

        if (_activeHintRoot != null)
            _activeHintRoot.SetActive(false);

        _activeHintRoot = null;
    }
}