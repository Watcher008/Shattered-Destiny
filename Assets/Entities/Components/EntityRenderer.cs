using UnityEngine;

namespace SD.ECS
{
    public class EntityRenderer : ComponentBase
    {
        private SpriteRenderer spriteRenderer;

        protected override void Start()
        {
            base.Start();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void ToggleRenderer(bool isVisible)
        {
            if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

            spriteRenderer.enabled = isVisible;
        }

        public void SetSprite(Sprite sprite)
        {
            if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

            spriteRenderer.sprite = sprite;
        }
    }
}