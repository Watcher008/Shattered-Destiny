using UnityEngine;

namespace SD.ECS
{
    public class Actor : ComponentBase
    {
        public delegate void OnTurnChangeCallback(bool isTurn);
        public OnTurnChangeCallback onTurnChange;

        private bool isTurn = false;
        [SerializeField] private int actionPoints;
        [SerializeField] private int speed = 100;

        public bool IsTurn
        {
            get => isTurn;
            set
            {
                SetTurn(value);
            }
        }

        public int ActionPoints
        {
            get => actionPoints;
            set
            {
                actionPoints = Mathf.Clamp(value, 0, int.MaxValue);
            }
        }

        public int Speed => speed;

        protected override void Start()
        {
            base.Start();
            GameManager.AddActor(this);
        }

        private void OnDestroy()
        {
            GameManager.RemoveActor(this);
            onTurnChange = null;
        }

        private void SetTurn(bool isTurn)
        {
            if (this.isTurn == isTurn) return;

            this.isTurn = isTurn;
            onTurnChange?.Invoke(isTurn);
        }

        public void SpendActionPoints(int points)
        {
            ActionPoints -= points;
        }

        public void RegainActionPoints()
        {
            ActionPoints += speed;
        }
    }
}