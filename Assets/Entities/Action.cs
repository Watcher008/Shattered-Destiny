using UnityEngine;

namespace SD.ECS
{
    public static class Action
    {
        public static void GetItem(Actor actor, Entity item)
        {
            if (!actor.IsTurn) return;
            //Pick up item in same node as actor
            GameManager.EndTurn();
        }

        public static void EquipItem(Actor actor)
        {
            if (!actor.IsTurn) return;
            //Equip item from inventory - likely a mostly player action
            GameManager.EndTurn();
        }

        public static void EquipFromGround(Actor actor, Entity item)
        {
            if (!actor.IsTurn) return;
            //Pick up item in same node as actor and equip it
            GameManager.EndTurn();
        }

        public static void DropItem(Actor actor)
        {
            if (!actor.IsTurn) return;
            //Drop item from inventory into same node being occupied
            GameManager.EndTurn();
        }

        public static void Unequip(Actor actor)
        {
            if (!actor.IsTurn) return;
            //Unequip item
            GameManager.EndTurn();
        }

        public static void MovementAction(Locomotion locomotion, Vector2Int direction)
        {
            locomotion.MoveEntity(direction);
            GameManager.EndTurn();
        }

        public static void SkipAction(Actor actor)
        {
            if (!actor.IsTurn) return;
            actor.SpendActionPoints(GameManager.pointsToAct);
            GameManager.EndTurn();
        }
    }
}