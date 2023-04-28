using UnityEngine;

namespace SD.ECS
{
    public class EntityRenderer : ComponentBase
    {
        private SpriteRenderer spriteRenderer;

        public override void Register(Entity entity)
        {
            base.Register(entity);
            spriteRenderer = entity.GetComponent<SpriteRenderer>();
        }

        public void ToggleRenderer(bool isVisible)
        {
            if (spriteRenderer == null) spriteRenderer = entity.GetComponent<SpriteRenderer>();

            spriteRenderer.enabled = isVisible;
        }

        public void SetSprite(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
        }
    }
}