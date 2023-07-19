using UnityEngine;

namespace SD.ECS
{
    public class Entity : MonoBehaviour
    {
        [field: SerializeField] public bool IsSentient { get; private set; }
        [field: SerializeField] public bool BlocksMovement { get; private set; }
        public bool HasBehavior { get; set; }
    }
}