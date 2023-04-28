using UnityEngine;

namespace SD.ECS
{
    public class Actor : ComponentBase
    {
        public delegate void OnTurnChangeCallback(bool isTurn);
        public OnTurnChangeCallback onTurnChange;

        private bool isTurn = false;
        private int actionPoints;

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

        public override void Register(Entity entity)
        {
            base.Register(entity);
            GameManager.AddActor(this);
        }

        public override void Unregister()
        {
            GameManager.RemoveActor(this);
            base.Unregister();
        }

        private void SetTurn(bool isTurn)
        {
            if (this.isTurn == isTurn) return;

            this.isTurn = isTurn;
            onTurnChange?.Invoke(isTurn);
        }

        public void SpendEnergy(int points)
        {
            ActionPoints += points;
            //Debug.Log(gameObject.name + " spending energy: " + points);
        }

        public void RegainEnergy(int points)
        {
            ActionPoints -= points;
            //Debug.Log(gameObject.name + " regaining energy: " + points);
        }
    }
}