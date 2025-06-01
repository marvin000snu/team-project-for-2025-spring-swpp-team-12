using UnityEngine;
using System.Collections.Generic;

public class MapLoader : MonoBehaviour
{
    [Header("Prefab Settings")]
    public GameObject chunkContainerPrefab;
    public TilePrefabEntry[] tilePrefabs;

    [Header("Path for Map json")]
    public string mapFileName = "TestMap";

    private Dictionary<int, GameObject> prefabDict;

    void Start()
    {
        LoadPrefabDict();

        TextAsset json = Resources.Load<TextAsset>($"Maps/{mapFileName}");
        ChunkMap map = JsonUtility.FromJson<ChunkMap>(json.text);

        foreach (var chunk in map.chunks)
        {
            BuildChunk(chunk);
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

    void BuildChunk(ChunkData chunk)
    {
        Vector3 chunkPos = new Vector3(chunk.x * 7f, 0f, chunk.y * 7f);
        Quaternion rotation = GetRotationFromDirection(chunk.indir, chunk.outdir);

        GameObject container = Instantiate(chunkContainerPrefab, chunkPos, rotation, transform);
        container.name = $"Chunk_{chunk.x}_{chunk.y}";

        for (int y = 0; y < 7; y++)
        {
            for (int i = 0; i < 49; i++)
            {
                int type = chunk.tiles[y][i];
                if (type == 0) continue;

                int z = i / 7;
                int x = i % 7;

                Vector3 localPos = new Vector3(x, y, z);
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
    }

    Quaternion GetRotationFromDirection(string indir, string outdir)
    {
        switch (outdir)
        {
            case "front": return Quaternion.Euler(0, 0, 0);
            case "right": return Quaternion.Euler(0, 90, 0);
            case "back": return Quaternion.Euler(0, 180, 0);
            case "left": return Quaternion.Euler(0, -90, 0);
            default: return Quaternion.identity;
        }
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
    public string indir;
    public string outdir;
    public bool is_horizontal;
    public int[][] tiles;
}