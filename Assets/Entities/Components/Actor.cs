using UnityEngine;

namespace SD.ECS
{
    public class Actor : MonoBehaviour
    {
        public delegate void OnTurnChangeCallback();
        public OnTurnChangeCallback onTurnStart;

        [SerializeField] private MovementSpeed _speed = MovementSpeed.Slow;

        public MovementSpeed Speed => _speed;

        private void Start()
        {
            GameManager.AddActor(this);
        }

        private void OnDestroy()
        {
            GameManager.RemoveActor(this);
            onTurnStart = null;
        }

        public void TakeAction()
        {
            onTurnStart?.Invoke();
        }
    }
}