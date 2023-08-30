using SD.PathingSystem;

namespace SD.ECS
{
    public class NodeCollisionListener : ComponentBase
    {
        public delegate void OnEntityCollisionCallback(Entity entityA, Entity entityB);
        public OnEntityCollisionCallback onEntityCollision;

        private GridPosition position;
        private PathNode currentNode;

        protected override void Start()
        {
            base.Start();

            position = GetComponent<GridPosition>();
            position.onPositionChange += ListenToNode;
            ListenToNode();
        }

        private void OnDestroy()
        {
            position.onPositionChange -= ListenToNode;
            onEntityCollision = null;
        }

        private void ListenToNode()
        {
            if (currentNode != null) currentNode.onNodeEntered -= OnNodeCollision;

            currentNode = Pathfinding.instance.GetNode(position.x, position.y);
            currentNode.onNodeEntered += OnNodeCollision;
        }

        private void OnNodeCollision(Entity newEntity)
        {
            if (newEntity == Entity) return;
            onEntityCollision?.Invoke(Entity, newEntity);
        }
    }
}