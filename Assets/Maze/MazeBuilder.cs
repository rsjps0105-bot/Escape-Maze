using UnityEngine;

public class MazeBuilder : MonoBehaviour
{
    [System.Serializable]
    public struct WallSegment
    {
        public Vector2 start;
        public Vector2 end;

        public WallSegment(Vector2 start, Vector2 end)
        {
            this.start = start;
            this.end = end;
        }
    }

    [Header("Prefab")]
    public GameObject wallPrefab;

    [Header("Wall Settings")]
    public float wallHeight = 2.5f;
    public float wallThickness = 1f;
    public float unitScale = 1f;

    [Header("Build")]
    public bool buildOnStart = true;
    public bool clearBeforeBuild = true;

    private WallSegment[] walls =
    {
        new WallSegment(new Vector2(0, 0),  new Vector2(20, 0)),
        new WallSegment(new Vector2(20, 0), new Vector2(20, 20)),
        new WallSegment(new Vector2(20, 20),new Vector2(0, 20)),
        new WallSegment(new Vector2(0, 20), new Vector2(0, 0)),

        new WallSegment(new Vector2(2, 20), new Vector2(6, 20)),
        new WallSegment(new Vector2(6, 20), new Vector2(6, 16)),

        new WallSegment(new Vector2(8, 20), new Vector2(12, 20)),
        new WallSegment(new Vector2(12, 20), new Vector2(12, 18)),
        new WallSegment(new Vector2(8, 18), new Vector2(12, 18)),
        new WallSegment(new Vector2(10, 18), new Vector2(10, 16)),
        new WallSegment(new Vector2(10, 16), new Vector2(12, 16)),

        new WallSegment(new Vector2(14, 20), new Vector2(20, 20)),
        new WallSegment(new Vector2(14, 18), new Vector2(18, 18)),
        new WallSegment(new Vector2(16, 18), new Vector2(16, 16)),
        new WallSegment(new Vector2(14, 16), new Vector2(18, 16)),

        new WallSegment(new Vector2(0, 16), new Vector2(4, 16)),
        new WallSegment(new Vector2(4, 16), new Vector2(4, 18)),
        new WallSegment(new Vector2(2, 16), new Vector2(2, 14)),

        new WallSegment(new Vector2(8, 16), new Vector2(8, 14)),
        new WallSegment(new Vector2(8, 14), new Vector2(10, 14)),

        new WallSegment(new Vector2(12, 16), new Vector2(12, 12)),
        new WallSegment(new Vector2(12, 12), new Vector2(16, 12)),
        new WallSegment(new Vector2(16, 12), new Vector2(16, 14)),

        new WallSegment(new Vector2(14, 16), new Vector2(14, 14)),
        new WallSegment(new Vector2(14, 14), new Vector2(16, 14)),

        new WallSegment(new Vector2(4, 14), new Vector2(6, 14)),
        new WallSegment(new Vector2(4, 14), new Vector2(4, 12)),
        new WallSegment(new Vector2(4, 12), new Vector2(6, 12)),

        new WallSegment(new Vector2(2, 12), new Vector2(2, 6)),
        new WallSegment(new Vector2(4, 10), new Vector2(8, 10)),
        new WallSegment(new Vector2(8, 10), new Vector2(8, 12)),
        new WallSegment(new Vector2(6, 12), new Vector2(6, 8)),
        new WallSegment(new Vector2(6, 8),  new Vector2(8, 8)),

        new WallSegment(new Vector2(4, 10), new Vector2(4, 6)),
        new WallSegment(new Vector2(4, 6),  new Vector2(2, 6)),

        new WallSegment(new Vector2(10, 14), new Vector2(10, 2)),
        new WallSegment(new Vector2(8, 8),   new Vector2(8, 0)),
        new WallSegment(new Vector2(8, 4),   new Vector2(10, 4)),

        new WallSegment(new Vector2(10, 10), new Vector2(14, 10)),
        new WallSegment(new Vector2(14, 10), new Vector2(14, 8)),
        new WallSegment(new Vector2(12, 8),  new Vector2(16, 8)),
        new WallSegment(new Vector2(16, 8),  new Vector2(16, 10)),

        new WallSegment(new Vector2(12, 8),  new Vector2(12, 6)),
        new WallSegment(new Vector2(12, 6),  new Vector2(14, 6)),
        new WallSegment(new Vector2(14, 6),  new Vector2(14, 4)),

        new WallSegment(new Vector2(18, 10), new Vector2(20, 10)),
        new WallSegment(new Vector2(18, 8),  new Vector2(18, 6)),
        new WallSegment(new Vector2(16, 6),  new Vector2(20, 6)),

        new WallSegment(new Vector2(2, 4), new Vector2(6, 4)),
        new WallSegment(new Vector2(6, 4), new Vector2(6, 6)),
        new WallSegment(new Vector2(2, 4), new Vector2(2, 0)),
        new WallSegment(new Vector2(4, 2), new Vector2(4, 0)),
        new WallSegment(new Vector2(6, 2), new Vector2(6, 0)),

        new WallSegment(new Vector2(8, 4),  new Vector2(8, 0)),
        new WallSegment(new Vector2(10, 4), new Vector2(10, 2)),

        new WallSegment(new Vector2(12, 4), new Vector2(14, 4)),
        new WallSegment(new Vector2(12, 4), new Vector2(12, 2)),
        new WallSegment(new Vector2(12, 2), new Vector2(14, 2)),

        new WallSegment(new Vector2(16, 4), new Vector2(18, 4)),
        new WallSegment(new Vector2(18, 4), new Vector2(18, 2)),
        new WallSegment(new Vector2(16, 2), new Vector2(20, 2)),
    };

    private void Start()
    {
        if (buildOnStart)
        {
            BuildMaze();
        }
    }

    [ContextMenu("Build Maze")]
    public void BuildMaze()
    {
        if (wallPrefab == null)
        {
            Debug.LogError("wallPrefab が設定されていません。");
            return;
        }

        if (clearBeforeBuild)
        {
            ClearChildren();
        }

        for (int i = 0; i < walls.Length; i++)
        {
            CreateWall(walls[i]);
        }
    }

    [ContextMenu("Clear Maze")]
    public void ClearChildren()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }

    private void CreateWall(WallSegment segment)
    {
        Vector2 start2 = segment.start * unitScale;
        Vector2 end2 = segment.end * unitScale;

        Vector3 start = new Vector3(start2.x, wallHeight * 0.5f, start2.y);
        Vector3 end = new Vector3(end2.x, wallHeight * 0.5f, end2.y);

        Vector3 center = (start + end) * 0.5f;
        Vector3 dir = end - start;

        if (dir.magnitude <= 0.001f) return;

        float length = dir.magnitude + wallThickness;

        GameObject wall = Instantiate(wallPrefab, center, Quaternion.identity, transform);
        wall.name = $"Wall_{segment.start}_{segment.end}";

        bool isHorizontal = Mathf.Abs(dir.x) >= Mathf.Abs(dir.z);

        if (isHorizontal)
        {
            wall.transform.localScale = new Vector3(length, wallHeight, wallThickness);
        }
        else
        {
            wall.transform.localScale = new Vector3(wallThickness, wallHeight, length);
        }
    }
}
