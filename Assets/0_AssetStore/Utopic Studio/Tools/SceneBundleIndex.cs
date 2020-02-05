using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An object used to create an index for the scene's path and their related scene bundle
/// </summary>
[CreateAssetMenu(fileName = "SceneBundleIndex", menuName = "Utopic/SceneBundleIndex", order = 1)]
public class SceneBundleIndex : ScriptableObject, ISerializationCallbackReceiver
{
    [System.Serializable]
    public struct SceneBundleIndexEntry
    {
        public SceneBundleIndexEntry(string SceneId, string BundleName)
        {
            Scene = SceneId;
            AssetBundleName = BundleName;
        }

        public string Scene;
        public string AssetBundleName;
    }

    //Serialization
    [SerializeField, HideInInspector]
    private List<SceneBundleIndexEntry> _SerializableEntries = new List<SceneBundleIndexEntry>();
    
    //Internal storage
    [SerializeField]
    private Dictionary<string, string> IndexMap = new Dictionary<string, string>();
    
    //overloaded operator
    public string this[string SceneId]
    {
        get { return IndexMap[SceneId]; }
        set { IndexMap[SceneId] = value; }
    }

    public IEnumerable<SceneBundleIndexEntry> GetEntries()
    {
        return _SerializableEntries;
    }

    public int Num()
    {
        return _SerializableEntries.Count;
    }

    public void OnBeforeSerialize()
    {
        _SerializableEntries.Clear();

        foreach (var kvp in IndexMap)
        {
            _SerializableEntries.Add(new SceneBundleIndexEntry(kvp.Key, kvp.Value));
        }
    }

    public void OnAfterDeserialize()
    {
        IndexMap = new Dictionary<string, string>();
        foreach (SceneBundleIndexEntry Entry in _SerializableEntries)
        {
            IndexMap.Add(Entry.Scene, Entry.AssetBundleName);
        }
    }

    public void Clear()
    {
        IndexMap.Clear();
    }

    public void Add(string SceneId, string AssetBundleName)
    {
        IndexMap.Add(SceneId, AssetBundleName);
    }

    public string Get(string SceneId)
    {
        if(IndexMap.ContainsKey(SceneId))
        {
            return IndexMap[SceneId];
        }

        return null;
    }

    public bool Contains(string SceneId)
    {
        return IndexMap.ContainsKey(SceneId);
    }
}
