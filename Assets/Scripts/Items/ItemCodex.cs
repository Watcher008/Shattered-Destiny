using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Item Codex")]
public class ItemCodex : ScriptableObject
{
    [System.Serializable]
    private class Codex 
    { 
        public Item[] Items;
        public Equipment[] Equipment;
    }

    private Dictionary<string, Item> _baseItems;
    private Dictionary<string, Item> _equipment;

    public void Init()
    {
        _baseItems = new Dictionary<string, Item>();
        _equipment = new Dictionary<string, Item>();

        var textAsset = Resources.Load("Codices/ItemCodex") as TextAsset;
        StringReader reader = new StringReader(textAsset.text);
        string json = reader.ReadToEnd();

        Codex codex = JsonUtility.FromJson<Codex>(json);

        foreach(var entry in codex.Items)
        {
            _baseItems.Add(entry.Name, entry);
        }

        foreach(var entry in codex.Equipment)
        {
            _equipment.Add(entry.Name, entry);
        }
    }

    public Item GetItem(string name)
    {
        if (_equipment.ContainsKey(name)) return _equipment[name];
        else if (_baseItems.ContainsKey(name)) return _baseItems[name];
        return null;
    }
}
