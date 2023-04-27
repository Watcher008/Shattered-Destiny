using UnityEngine;
using SD.PathingSystem;

namespace SD.ECS
{
    public class Position : MonoBehaviour, IComponent
    {
        public int x;
        public int y;
        private Entity entity;

        public void AddComponent(Entity entity)
        {
            this.entity = entity;
            entity.components.Add(this);
            SetStartingPosition();
        }

        private void SetStartingPosition()
        {
            var currentNode = Pathfinding.instance.GetNode(transform.position);
            SetPosition(currentNode.x, currentNode.y);
        }

        public void SetPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
            transform.position = Pathfinding.instance.GetNode(this.x, this.y).globalPosition;

            //later change this to also occupy that node
        }

        public void RemoveComponent(Entity entity)
        {
            entity.components.Remove(this);
        }
    }
}