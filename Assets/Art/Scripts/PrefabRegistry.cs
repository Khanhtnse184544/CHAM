using System.Collections.Generic;
using UnityEngine;

public class PrefabRegistry : MonoBehaviour
{
    [System.Serializable]
    public class PrefabEntry
    {
        public string key;           // Key như "Tree", "Rock"
        public GameObject prefab;    // Prefab tương ứng
    }

    public List<PrefabEntry> entries;

    private Dictionary<string, GameObject> prefabDict;

    public static PrefabRegistry Instance;

    void Awake()
    {
        Instance = this;
        prefabDict = new Dictionary<string, GameObject>();
        foreach (var entry in entries)
        {
            if (!prefabDict.ContainsKey(entry.key))
                prefabDict.Add(entry.key, entry.prefab);
        }
    }

    public GameObject GetPrefab(string key)
    {
        if (prefabDict.TryGetValue(key, out var prefab))
            return prefab;
        return null;
    }
}
