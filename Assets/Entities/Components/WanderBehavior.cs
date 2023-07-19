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

        protected override void Start()
        {
            base.Start();

            Entity.HasBehavior = true;

            actor = GetComponent<Actor>();
            position = GetComponent<GridPosition>();
            locomotion = GetComponent<Locomotion>();

            startX = position.x;
            startY = position.y;

            actor.onTurnChange += Wander;
        }

        private void OnDestroy()
        {
            actor.onTurnChange -= Wander;
        }

        public void Wander(bool isTurn)
        {
            if (!isTurn) return;

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