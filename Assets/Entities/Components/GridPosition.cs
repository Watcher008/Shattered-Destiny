using SD.PathingSystem;

namespace SD.ECS
{
    public class GridPosition : ComponentBase
    {
        public delegate void OnPositionChangeCallback();
        public OnPositionChangeCallback onPositionChange;

        public int x { get; set; }
        public int y { get; set; }

        public override void Register(Entity entity)
        {
            base.Register(entity);
            SetStartingPosition();
        }

        private void SetStartingPosition()
        {
            var currentNode = Pathfinding.instance.GetNode(transform.position);
            SetPosition(currentNode.x, currentNode.y);
        }

        public void SetPosition(int x, int y)
        {
            var oldNode = Pathfinding.instance.GetNode(this.x, this.y);
            oldNode.LeaveNode(entity);

            this.x = x;
            this.y = y;

            var newNode = Pathfinding.instance.GetNode(this.x, this.y);

            transform.position = newNode.worldPosition;
            newNode.OccupyNode(entity);

            onPositionChange?.Invoke();
        }
    }
}