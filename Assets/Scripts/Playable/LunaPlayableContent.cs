using UnityEngine;

public class LunaPlayableContent : MonoBehaviour
{
    public static LunaPlayableContent Instance { get; private set; }

    [SerializeField] GameObject catPrefab;
    [SerializeField] ThemeData dayTheme;
    [SerializeField] GameObject[] segments;
    [SerializeField] GameObject obstacleBin;
    [SerializeField] GameObject obstacleLowBarrier;
    [SerializeField] GameObject obstacleHighBarrier;

    void Awake()
    {
        Instance = this;
    }

    public static ThemeData DayTheme => Instance != null ? Instance.dayTheme : null;

    public static GameObject InstantiateCat(Transform parent = null)
    {
        if (Instance == null || Instance.catPrefab == null)
            return null;

        GameObject cat = parent != null
            ? Instantiate(Instance.catPrefab, parent, false)
            : Instantiate(Instance.catPrefab, Vector3.zero, Quaternion.identity);
        cat.name = "PlayableCat";
        return cat;
    }

    public static GameObject InstantiateSegment(int index, Vector3 position)
    {
        if (Instance == null || Instance.segments == null)
            return null;
        if (index < 0 || index >= Instance.segments.Length || Instance.segments[index] == null)
            return null;
        return Instantiate(Instance.segments[index], position, Quaternion.identity);
    }

    public static GameObject GetObstaclePrefabForSegment(TrackSegment segment)
    {
        if (Instance == null || segment == null)
            return null;

        string n = segment.name;
        if (n.IndexOf("TutorialUp2") >= 0 || n.IndexOf("Up3") >= 0)
            return Instance.obstacleHighBarrier;
        if (n.IndexOf("WarehouseUp") >= 0)
            return Instance.obstacleLowBarrier;
        return Instance.obstacleBin;
    }
}