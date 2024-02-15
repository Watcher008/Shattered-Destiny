using SD.PathingSystem;
using UnityEngine;

public class MapCharacter : MonoBehaviour
{
    private Color invisible = new(255, 255, 255, 0.0f);
    private Color faded = new(255, 255, 255, 0.5f);
    private Color visible = new(255, 255, 255, 1.0f);

    private SpriteRenderer spriteRenderer;
    private PathNode _node;
    public PathNode Node => _node;

    private void Awake()
    {
        var node = Pathfinding.instance.GetNode(transform.position);
        if (node != null) _node = node;
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Sets the visibility of the locaiton on the map.
    /// </summary>
    /// <param name="visibility">0 = invisible, 1 = faded, 2 = visible</param>
    public void SetVisibility(int visibility)
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

        if (visibility == 1) spriteRenderer.color = faded;
        else if (visibility == 2) spriteRenderer.color = visible;
        else spriteRenderer.color = invisible;
    }

    /// <summary>
    /// Moves the character in the given direction.
    /// </summary>
    /// <param name="dx">delta X</param>
    /// <param name="dy">delta Y</param>
    /// <returns>Returns true if able to move to the position.</returns>
    public bool Move(int dx, int dy)
    {
        // Make sure that the new position is valid
        if (!Pathfinding.PositionIsValid(_node.X + dx, _node.Y + dy)) return false;

        var newNode = Pathfinding.instance.GetNode(_node.X + dx, _node.Y + dy);

        transform.position = newNode.WorldPosition;

        _node = newNode;
        return true;
    }
}
