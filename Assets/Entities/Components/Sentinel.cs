using UnityEngine;
using SD.EventSystem;

namespace SD.ECS
{
    public class Sentinel : ComponentBase
    {
        [SerializeField] private GameEvent fullRoundEvent;
        public Actor Actor { get; private set; }

        protected override void Start()
        {
            base.Start();

            Actor = GetComponent<Actor>();
            Actor.onTurnChange += OnNewRound;

            GameManager.AddSentinel(this);
        }

        private void OnDestroy()
        {
            Actor.onTurnChange -= OnNewRound;
        }

        private void OnNewRound(bool isTurn)
        {
            if (isTurn)
            {
                //Debug.Log("A Full round has passed");
                fullRoundEvent?.Invoke();
                Action.SkipAction(Actor);
            }
        }
    }
}