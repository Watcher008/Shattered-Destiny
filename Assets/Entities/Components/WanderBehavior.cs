using UnityEngine;

namespace SD.ECS
{
    public class WanderBehavior : ComponentBase
    {
        [SerializeField] private int maxWanderDistance = 3;

        private Actor actor;
        private GridPosition position;
        private Locomotion locomotion;
        private int startX, startY;

        public override void Register(Entity entity)
        {
            base.Register(entity);
            entity.HasBehavior = true;

            actor = entity.GetComponentBase<Actor>();
            position = entity.GetComponentBase<GridPosition>();
            locomotion = entity.GetComponentBase<Locomotion>();

            startX = position.x;
            startY = position.y;

            actor.onTurnChange += i => Wander();
        }

        public override void Unregister()
        {
            actor.onTurnChange -= i => Wander();
            base.Unregister();
        }

        public void Wander()
        {
            if (!actor.IsTurn) return;

            int x = Random.Range(-1, 1);
            int y = Random.Range(-1, 1);

            if (position.x > startX + maxWanderDistance) x = -1;
            else if (position.x < startX - maxWanderDistance) x = 1;

            if (position.y > startY + maxWanderDistance) y = -1;
            else if (position.y < startY - maxWanderDistance) y = 1;

            var direction = new Vector2Int(x, y);
            if (locomotion.CanMoveToPosition(direction)) Action.MovementAction(locomotion, direction);
            else Action.SkipAction(actor);
        }
    }
}