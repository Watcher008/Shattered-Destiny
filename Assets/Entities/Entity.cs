using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SD.ECS
{
    public class Entity : MonoBehaviour
    {
        public delegate void OnTurnChangeCallback(bool isTurn);
        public OnTurnChangeCallback onTurnChange;

        [field: SerializeField] public bool IsSentient { get; private set; }
        [field: SerializeField] public bool BlocksMovement { get; private set; }

        private bool isTurn = false;

        public bool IsTurn
        {
            get => isTurn;
            set
            {
                SetTurn(value);
            }
        }

        [SerializeField] private int actionPoints;

        public int ActionPoints
        {
            get => actionPoints;
            set
            {
                actionPoints = Mathf.Clamp(value, 0, int.MaxValue);
            }
        }

        public List<IComponent> components = new List<IComponent>();

        private void Start()
        {
            if (gameObject.CompareTag("Player"))
            {
                GameManager.instance.InsertEntity(this, 0);
                IsTurn = true;
            }
            else GameManager.instance.AddEntity(this);

            GetExistingComponents();
        }

        private void OnDestroy()
        {
            GameManager.instance.RemoveEntity(this);
        }

        private void GetExistingComponents()
        {
            var components = GetComponents<IComponent>();
            for (int i = 0; i < components.Length; i++)
            {
                components[i].AddComponent(this);
            }
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
            Debug.Log(gameObject.name + " spending energy: " + points);
        }

        public void RegainEnergy(int points)
        {
            ActionPoints -= points;
            Debug.Log(gameObject.name + " regaining energy: " + points);
        }

        public void Move(Vector2Int direction)
        {
            Vector3 newPos = (Vector3Int)direction;
            transform.position += newPos * SD.PathingSystem.Pathfinding.instance.GetCellSize();
        }
    }

    public interface IComponent
    {
        void AddComponent(Entity entity);

        void RemoveComponent(Entity entity);
    }
}
