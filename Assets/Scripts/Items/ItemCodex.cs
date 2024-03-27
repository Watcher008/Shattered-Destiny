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
        public Weapon[] Weapons; 
    }

    private Dictionary<string, Item> _baseItems;
    private Dictionary<string, Weapon> _weapons;

    public void Init()
    {
        _baseItems = new Dictionary<string, Item>();
        _weapons = new Dictionary<string, Weapon>();

        var textAsset = Resources.Load("Codices/ItemCodex") as TextAsset;
        StringReader reader = new StringReader(textAsset.text);
        string json = reader.ReadToEnd();

        Codex codex = JsonUtility.FromJson<Codex>(json);

        foreach(var entry in codex.Items)
        {
            _baseItems.Add(entry.Name, entry);
        }

        foreach(var entry in codex.Weapons)
        {
            _weapons.Add(entry.Name, entry);
        }
    }

    public Weapon GetWeapon(string name)
    {
        if (_weapons.ContainsKey(name)) return _weapons[name];
        return null;
    }

    public Item GetItem(string name)
    {
        if (_weapons.ContainsKey(name)) return _weapons[name];
        else if (_baseItems.ContainsKey(name)) return _baseItems[name];
        return null;
    }
}
