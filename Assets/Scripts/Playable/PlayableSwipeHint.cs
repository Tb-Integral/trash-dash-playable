using UnityEngine;

public class PlayableSwipeHint : MonoBehaviour
{
    public static PlayableSwipeHint Instance { get; private set; }

    [SerializeField] GameObject hintRoot;
    [SerializeField] RectTransform handRoot;
    [SerializeField] float showAtRatio = 0.35f;
    [SerializeField] float hintTimeScale = 0.4f;

    bool _shown;
    bool _done;
    TrackSegment _skippedSegment;

    void Awake()
    {
        Instance = this;
        if (hintRoot != null)
            hintRoot.SetActive(false);
    }

    public static void NotifyLaneChanged()
    {
        if (Instance != null && Instance._shown)
            Instance.Dismiss();
    }

    void LateUpdate()
    {
        if (_done || _shown || !PlayableSegmentQueue.IsActive)
            return;

        TrackManager tm = TrackManager.instance;
        if (tm == null || !tm.isLoaded || !tm.isMoving || tm.currentSegment == null)
            return;

        if (tm.currentSegment == _skippedSegment)
            return;

        SimpleBarricade[] bins = tm.currentSegment.GetComponentsInChildren<SimpleBarricade>(true);
        if (bins.Length == 0)
            return;

        float ratio = tm.currentSegmentDistance / tm.currentSegment.worldLength;
        if (ratio < showAtRatio)
            return;

        int dir;
        if (!TryGetOneSwipe(tm, bins, out dir))
        {
            _skippedSegment = tm.currentSegment;
            return;
        }

        if (handRoot != null)
        {
            Vector3 s = handRoot.localScale;
            s.x = dir < 0 ? -1f : 1f;
            handRoot.localScale = s;
        }

        _shown = true;
        Time.timeScale = hintTimeScale;
        if (hintRoot != null)
            hintRoot.SetActive(true);
        SampleHandAtStart();
    }

    void SampleHandAtStart()
    {
        if (handRoot == null)
            return;
        Animation anim = handRoot.GetComponentInChildren<Animation>(true);
        if (anim == null || anim.clip == null)
            return;
        anim.Play();
        anim[anim.clip.name].time = 0f;
        anim.Sample();
    }

    void Dismiss()
    {
        if (_done)
            return;
        _done = true;
        Time.timeScale = 1f;
        if (hintRoot != null)
            hintRoot.SetActive(false);
    }

    static bool TryGetOneSwipe(TrackManager tm, SimpleBarricade[] bins, out int dir)
    {
        dir = 0;
        int player = tm.characterController.CurrentLane;
        bool[] occupied = new bool[3];

        for (int i = 0; i < bins.Length; i++)
        {
            int lane = Mathf.RoundToInt(LateralOffset(tm.currentSegment, bins[i]) / tm.laneOffset) + 1;
            if (lane >= 0 && lane <= 2)
                occupied[lane] = true;
        }

        if (!occupied[player])
            return false;

        bool left = player > 0 && !occupied[player - 1];
        bool right = player < 2 && !occupied[player + 1];
        if (!left && !right)
            return false;

        dir = (right && !left) ? 1 : (left && !right) ? -1 : 1;
        return true;
    }

    static float LateralOffset(TrackSegment seg, SimpleBarricade bin)
    {
        Vector3 pathPos = bin.transform.position;
        Quaternion pathRot = bin.transform.rotation;
        float best = float.MaxValue;
        float[] ts = seg.obstaclePositions;
        if (ts != null)
        {
            for (int i = 0; i < ts.Length; i++)
            {
                Vector3 p;
                Quaternion r;
                seg.GetPointAt(ts[i], out p, out r);
                float d = (bin.transform.position - p).sqrMagnitude;
                if (d < best)
                {
                    best = d;
                    pathPos = p;
                    pathRot = r;
                }
            }
        }

        return Vector3.Dot(bin.transform.position - pathPos, pathRot * Vector3.right);
    }
}
