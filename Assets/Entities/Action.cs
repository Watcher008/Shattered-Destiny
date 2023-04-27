using UnityEngine;

namespace SD.ECS
{
    public static class Action
    {
        public static void GetItem(Entity actor, Entity item)
        {
            if (!actor.IsTurn) return;
            //Pick up item in same node as actor
            GameManager.instance.EndTurn();
        }

        public static void EquipItem(Entity entity)
        {
            if (!entity.IsTurn) return;
            //Equip item from inventory - likely a mostly player action
            GameManager.instance.EndTurn();
        }

        public static void EquipFromGround(Entity actor, Entity item)
        {
            if (!actor.IsTurn) return;
            //Pick up item in same node as actor and equip it
            GameManager.instance.EndTurn();
        }

        public static void DropItem(Entity entity)
        {
            if (!entity.IsTurn) return;
            //Drop item from inventory into same node being occupied
            GameManager.instance.EndTurn();
        }

        public static void Unequip(Entity entity)
        {
            if (!entity.IsTurn) return;
            //Unequip item
            GameManager.instance.EndTurn();
        }

        public static void MovementAction(Locomotion locomotion, Vector2Int direction)
        {
            locomotion.MoveEntity(direction);
            GameManager.instance.EndTurn();
        }

        public static void SkipAction(Entity entity)
        {
            if (!entity.IsTurn) return;
            entity.SpendEnergy(100);
            GameManager.instance.EndTurn();
        }
    }
}