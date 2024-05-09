using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Location Codex")]
public class LocationCodex : ScriptableObject
{
    private List<string[]> _locationBlueprints;

    [System.Serializable]
    private class LocationBlueprint
    {
        public string Name;
        // Maybe additional parameters for type, ground, etc.
        public string[] Layout;
    }

    [System.Serializable]
    private class Codex
    {
        public LocationBlueprint[] Items;
    }

    public void Init()
    {
        _locationBlueprints = new List<string[]>();

        var textAsset = Resources.Load("Codices/LocationCodex") as TextAsset;
        StringReader reader = new StringReader(textAsset.text);
        string json = reader.ReadToEnd();

        Codex codex = JsonUtility.FromJson<Codex>(json);

        foreach(var item in codex.Items)
        {
            _locationBlueprints.Add(item.Layout);
        }
    }

    public string[] GetBlueprint()
    {
        return _locationBlueprints[Random.Range(0, _locationBlueprints.Count)];
    }
}