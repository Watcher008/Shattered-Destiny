using UnityEngine;

/// <summary>
/// A class to handle loading sprites from file.
/// </summary>
public static class SpriteHelper
{
    public static Sprite GetSprite(string name)
    {
        var path = name.Split("/");
        var sprites = Resources.LoadAll<Sprite>($"Sprites/{path[0]}");
        foreach (var sprite in sprites)
        {
            if (!sprite.name.Equals(path[1])) continue;
            return sprite;
        }
        return null;
    }
}
