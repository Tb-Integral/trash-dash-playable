using UnityEngine;
using UnityEngine.AddressableAssets;

public class PlayableSegmentQueue : MonoBehaviour
{
    public static PlayableSegmentQueue Instance { get; private set; }

    [SerializeField] AssetReference[] segments;
    [SerializeField] int authoredCount = 5;
    public static int LastConsumedIndex => Instance != null ? Instance._index - 1 : -1;

    int _index;

    void Awake()
    {
        Instance = this;
    }

    public static bool IsActive => Instance != null;

    public static int AuthoredCount => Instance != null ? Instance.authoredCount : 0;
    public static int TotalCount => Instance != null && Instance.segments != null ? Instance.segments.Length : 0;

    public static bool HasRemaining =>
        Instance != null && Instance.segments != null && Instance._index < Instance.segments.Length;

    public static bool TryGetNext(out AssetReference segment, out bool isTail)
    {
        segment = null;
        isTail = false;
        if (!HasRemaining)
            return false;

        isTail = Instance._index >= Instance.authoredCount;
        segment = Instance.segments[Instance._index];
        Instance._index++;
        return true;
    }
}