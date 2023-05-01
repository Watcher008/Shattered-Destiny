using SD.PathingSystem;

namespace SD.ECS
{
    public class NodeCollisionListener : ComponentBase
    {
        public delegate void OnEntityCollisionCallback(Entity entityA, Entity entityB);
        public OnEntityCollisionCallback onEntityCollision;

        private GridPosition position;
        private WorldNode currentNode;

        public override void Register(Entity entity)
        {
            base.Register(entity);
            position = entity.GetComponentBase<GridPosition>();
            position.onPositionChange += ListenToNode;
            ListenToNode();
        }

        public override void Unregister()
        {
            position.onPositionChange -= ListenToNode;
            base.Unregister();
        }

        private void ListenToNode()
        {
            if (currentNode != null) currentNode.onNodeEntered -= OnNodeCollision;

            currentNode = Pathfinding.instance.GetNode(position.x, position.y);
            currentNode.onNodeEntered += OnNodeCollision;
        }

        private void OnNodeCollision(Entity newEntity)
        {
            if (newEntity == entity) return;
            onEntityCollision?.Invoke(entity, newEntity);
        }
    }
}