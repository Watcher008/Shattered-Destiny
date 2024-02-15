using UnityEngine;

namespace SD.ECS
{
    public class WanderBehavior : MonoBehaviour
    {
        [SerializeField] private int maxWanderDistance = 3;
        private MapCharacter character;

        private int startX, startY;

        private void Start()
        {
            character = GetComponent<MapCharacter>();

            if (character.Node != null )
            {
                startX = character.Node.X;
                startY = character.Node.Y;
            }

            character.onTurnStart += Wander;
        }

        private void OnDestroy()
        {
            character.onTurnStart -= Wander;
        }

        public void Wander()
        {
            int dx = Random.Range(-1, 1);
            int dy = Random.Range(-1, 1);

            if (character.Node.X > startX + maxWanderDistance) dx = -1;
            else if (character.Node.X < startX - maxWanderDistance) dx = 1;

            if (character.Node.Y > startY + maxWanderDistance) dy = -1;
            else if (character.Node.Y < startY - maxWanderDistance) dy = 1;

            character.Move(dx, dy);
        }
    }
}