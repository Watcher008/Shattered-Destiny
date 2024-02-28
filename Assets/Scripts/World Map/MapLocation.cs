using UnityEngine;
using SD.PathingSystem;

namespace SD.LocationSystem
{
    public class MapLocation : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        private PathNode _node;
        public PathNode Node => _node;

        private Color invisible = new(255, 255, 255, 0.0f);
        private Color faded = new(255, 255, 255, 0.5f);
        private Color visible = new(255, 255, 255, 1.0f);

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            _node = Pathfinding.instance.GetNode(transform.position);
            LocationManager.Register(this);
        }

        public void SetValues(Sprite sprite, int x, int y)
        {
            if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;

            _node = Pathfinding.instance.GetNode(x, y);
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
    }
}