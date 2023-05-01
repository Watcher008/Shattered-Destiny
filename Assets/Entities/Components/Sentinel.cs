using UnityEngine;
using SD.EventSystem;

namespace SD.ECS
{
    public class Sentinel : ComponentBase
    {
        [SerializeField] private GameEvent fullRoundEvent;
        private Actor actor;

        public override void Register(Entity entity)
        {
            base.Register(entity);
            actor = entity.GetComponentBase<Actor>();
            actor.onTurnChange += OnNewRound;

            GameManager.RemoveActor(actor);
            GameManager.AddSentinel(actor, 0);
        }

        public override void Unregister()
        {
            actor.onTurnChange -= OnNewRound;
            base.Unregister();
        }

        private void OnNewRound(bool isTurn)
        {
            if (isTurn)
            {
                Debug.Log("A Full round has passed");
                fullRoundEvent?.Invoke();
                Action.SkipAction(actor);
            }
        }
    }
}