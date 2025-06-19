using UnityEngine;
using System.Collections.Generic;
using System;

public enum Direction
{
    Up,     // +Y
    Down,   // -Y
    Left,   // -X
    Right,  // +X
    Front,  // +Z
    Back    // -Z
}

public class MapLoader : MonoBehaviour
{
    [Header("Prefab Settings")]
    public GameObject chunkContainerPrefab;
    public TilePrefabEntry[] tilePrefabs;

    [Header("Path for Map json")]
    public string mapFileName = "Stage1";

    private Dictionary<int, GameObject> prefabDict;

    void Start()
    {
        LoadPrefabDict();

        TextAsset json = Resources.Load<TextAsset>($"Maps/{mapFileName}");
        ChunkMap map = JsonUtility.FromJson<ChunkMap>(json.text);

        Direction prevDir = Direction.Front;    // All maps start from Front direction
        foreach (var chunk in map.chunks)
        {
            BuildChunk(chunk, prevDir);
            prevDir = chunk.dir;
        }
    }

    void LoadPrefabDict()
    {
        prefabDict = new Dictionary<int, GameObject>();
        foreach (var entry in tilePrefabs)
        {
            prefabDict[entry.type] = entry.prefab;
        }
    }
    Vector3Int DirToVec(Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                return new Vector3Int(0, 1, 0);
            case Direction.Down:
                return new Vector3Int(0, -1, 0);
            case Direction.Left:
                return new Vector3Int(-1, 0, 0);
            case Direction.Right:
                return new Vector3Int(1, 0, 0);
            case Direction.Front:
                return new Vector3Int(0, 0, 1);
            case Direction.Back:
                return new Vector3Int(0, 0, -1);
            default:
                return Vector3Int.zero;
        }
    }

    Direction VecToDir(Vector3 vec)
    {
        if (vec == Vector3.up)     return Direction.Up;
        if (vec == Vector3.down)   return Direction.Down;
        if (vec == Vector3.left)   return Direction.Left;
        if (vec == Vector3.right)  return Direction.Right;
        if (vec == Vector3.forward)return Direction.Front;
        if (vec == Vector3.back)   return Direction.Back;

        throw new ArgumentException("Invalid direction vector: " + vec);
    }
    public static Direction GetOpposite(Direction dir)
    {
        switch (dir)
        {
            case Direction.Up: return Direction.Down;
            case Direction.Down: return Direction.Up;
            case Direction.Left: return Direction.Right;
            case Direction.Right: return Direction.Left;
            case Direction.Front: return Direction.Back;
            case Direction.Back: return Direction.Front;
            default: return dir;  // fallback
        }
    }

    bool ShouldSkipTile(int x, int y, int z, Direction prevDir)
    {
        // Debug.Log("Direction");
        // Debug.Log(prevDir);
        if (z == 8) return true;

        if (prevDir == Direction.Back && z == 0) return true;
        if (prevDir == Direction.Left && x == 0) return true;
        if (prevDir == Direction.Right && x == 8) return true;
        if (prevDir == Direction.Down && y == 0) return true;
        if (prevDir == Direction.Up && y == 8) return true;

        return false;
    }

    void BuildChunk(ChunkData chunk, Direction prevDir)
    {
        Debug.Log(prevDir);
        Vector3 chunkPos = new Vector3(chunk.x, chunk.y, chunk.z);
        chunkPos *= 70f;
        
        Quaternion rotation = GetRotationFromDirection(prevDir, chunk.dir);

        GameObject container = Instantiate(chunkContainerPrefab, chunkPos, rotation, transform);
        container.name = $"Chunk_{chunk.x}_{chunk.y}_{chunk.z}";

        if (chunk.tiles == null)
        {
            Debug.LogError("No tiles");
        }

        for (int i = 0; i < 729; i++)
        {
            int type = chunk.tiles[i];
            if (type == 0) continue;        // Air

            int y = i / 81;
            int z = i % 81 / 9;
            int x = i % 9;

            Vector3 result = -(Quaternion.Inverse(rotation) * DirToVec(prevDir));
            if (i == 0) {
                Debug.Log("result");
                Debug.Log(result);
            }
            if (ShouldSkipTile(x, y, z, VecToDir(result))) continue;

            Vector3 localPos = new Vector3(x, y, z);
            localPos *= 10f;
            localPos -= new Vector3(40f, 40f, 40f);
            Vector3 worldPos = container.transform.TransformPoint(localPos);

            if (prefabDict.TryGetValue(type, out GameObject prefab))
            {
                Instantiate(prefab, worldPos, rotation, container.transform);
            }
            else
            {
                Debug.LogWarning($"Missing prefab for type {type}");
            }
        }
    }


    Quaternion GetRotationFromDirection(Direction indir, Direction outdir)
    {
        Vector3 indirVec = DirToVec(indir);
        Vector3 outdirVec = DirToVec(outdir);

        // Step 1: rotate Vector3.forward → indirVec
        Quaternion toIndir = Quaternion.FromToRotation(Vector3.forward, indirVec);

        // Step 2: figure out how to rotate indirVec → outdirVec (in the rotated space)
        Vector3 outdirInLocal = Quaternion.Inverse(toIndir) * outdirVec;

        // Step 3: rotate forward → outdirInLocal
        Quaternion adjust = Quaternion.FromToRotation(Vector3.forward, outdirInLocal);

        // Final rotation = toIndir followed by adjust
        return toIndir * adjust;
    }
    Direction GetDirectionFromRotation(Quaternion rot)
    {
        Vector3 dir = rot * Vector3.forward;

        float maxDot = float.NegativeInfinity;
        Direction bestMatch = Direction.Front;

        foreach (Direction candidate in System.Enum.GetValues(typeof(Direction)))
        {
            Vector3 candidateVec = DirToVec(candidate);
            float dot = Vector3.Dot(dir.normalized, candidateVec.normalized);
            if (dot > maxDot)
            {
                maxDot = dot;
                bestMatch = candidate;
            }
        }

        return bestMatch;
    }
}

[System.Serializable]
public class TilePrefabEntry
{
    public int type;
    public GameObject prefab;
}

[System.Serializable]
public class ChunkMap
{
    public ChunkData[] chunks;
}

[System.Serializable]
public class ChunkData
{
    public int x;
    public int y;
    public int z;
    public Direction dir;
    public bool is_horizontal;
    public int[] tiles;
}