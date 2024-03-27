using SD.Characters;
using SD.Grids;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace SD.Combat
{
    public abstract class WeaponArt : ScriptableObject
    {
        [SerializeField] protected int _actionPointCost;
        [SerializeField] private string _description;

        //public abstract void OnUse(Combatant combatant);
        public abstract void OnUse(Combatant combatant, Combatant target);
    }

    public static class WeaponArt_New
    {
        public static void OnUse(Arts art, Combatant combatant, PathNode node)
        {
            switch (art)
            {
                case Arts.Cleave:
                    OnCleave(combatant, node);
                    break;
                case Arts.Pierce:
                    OnPierce(combatant, node);
                    break;
            }
        }
        #region - Sword Arts -
        public static void OnCleave(Combatant combatant, PathNode targetNode)
        {
            int dmg = 3 * combatant.GetAttributeBonus(Attributes.Physicality);

            var nodes = Pathfinding.instance.GetArea(combatant.Node, 1);
            foreach (var node in nodes)
            {
                if (CombatManager.Instance.CheckNode(node, out var nextTarget))
                {
                    if (nextTarget.IsPlayer != combatant.IsPlayer)
                    {
                        Debug.Log($"Dealing {dmg} Cleave dmg to {nextTarget.gameObject.name}");
                        combatant.DealDamage(dmg, nextTarget);
                    }
                }
            }

            combatant.SpendActionPoints(2);
        }

        public static void OnPierce(Combatant combatant, PathNode targetNode)
        {
            Debug.LogWarning("NOT YET IMPLEMENTED.");
            if (CombatManager.Instance.CheckNode(targetNode, out var target))
            {

            }
        }

        public static void OnQuickFeet(Combatant combatant, PathNode targetNode)
        {
            Debug.LogWarning("NOT YET IMPLEMENTED.");

        }
        #endregion

        #region - Shield Arts -
        public static void OnBlock(Combatant combatant, PathNode targetNode)
        {
            combatant.Block = combatant.GetAttributeBonus(Characters.Attributes.Physicality) * 5;
            combatant.SpendActionPoints(1);
        }

        public static void OnBash(Combatant combatant, PathNode targetNode)
        {
            if (CombatManager.Instance.CheckNode(targetNode, out var target))
            {

            }
            int dmg = combatant.GetAttributeBonus(Attributes.Physicality);

            combatant.DealDamage(dmg, target);
            target.AddEffect(new Stun());

            combatant.SpendActionPoints(2);
        }

        public static void OnStandBehindMe(Combatant combatant, PathNode targetNode)
        {
            Debug.LogWarning("NOT YET IMPLEMENTED.");

        }

        public static void OnStandYourGround(Combatant combatant, PathNode targetNode)
        {
            Debug.LogWarning("NOT YET IMPLEMENTED.");

        }

        public static void OnUseDestroy(Combatant combatant, PathNode targetNode)
        {
            Debug.LogWarning("NOT YET IMPLEMENTED.");

        }
        #endregion

        #region - Warhammer Arts -
        public static void OnSmash(Combatant combatant, PathNode targetNode)
        {
            Debug.LogWarning("NOT YET IMPLEMENTED.");

        }

        public static void OnCoupDeGrace(Combatant combatant, PathNode targetNode)
        {
            if (CombatManager.Instance.CheckNode(targetNode, out var target))
            {
                if ((float)target.Health / target.MaxHealth <= 0.35f)
                {
                    combatant.DealDamage(int.MaxValue, target);
                }
                combatant.SpendActionPoints(3);
            }
        }

        public static void OnPush(Combatant combatant, PathNode targetNode)
        {
            Debug.LogWarning("NOT YET IMPLEMENTED.");

        }

        public static void OnShatter(Combatant combatant, PathNode targetNode)
        {
            Debug.LogWarning("NOT YET IMPLEMENTED.");

        }

        public static void OnWarcry(Combatant combatant, PathNode targetNode)
        {
            Debug.LogWarning("NOT YET IMPLEMENTED.");

        }
        #endregion

        #region - Bow Arts -
        public static void OnPiercingShot(Combatant combatant, PathNode targetNode)
        {
            Debug.LogWarning("NOT YET IMPLEMENTED.");

            if (CombatManager.Instance.CheckNode(targetNode, out var target))
            {
                if (CombatManager.Instance.AttackHits(combatant, target))
                {

                }
                combatant.SpendActionPoints(2);
            }
        }

        public static void OnMarkingShot(Combatant combatant, PathNode targetNode)
        {
            Debug.LogWarning("NOT YET IMPLEMENTED.");

            if (CombatManager.Instance.CheckNode(targetNode, out var target))
            {
                if (CombatManager.Instance.AttackHits(combatant, target))
                {

                }
                combatant.SpendActionPoints(2);
            }
        }

        private const int HEADSHOT_MOD = 6;
        public static void OnHeadshot(Combatant combatant, PathNode targetNode)
        {
            if (CombatManager.Instance.CheckNode(targetNode, out var target))
            {
                if (CombatManager.Instance.AttackHits(combatant, target))
                {
                    int dmg = combatant.GetAttributeBonus(Attributes.Physicality) * HEADSHOT_MOD;
                    combatant.DealDamage(dmg, target);
                }
                combatant.SpendActionPoints(2);
            }
        }

        public static void OnAnkleShot(Combatant combatant, PathNode targetNode)
        {
            if (CombatManager.Instance.CheckNode(targetNode, out var target))
            {
                if (CombatManager.Instance.AttackHits(combatant, target))
                {
                    int dmg = combatant.GetAttributeBonus(Attributes.Physicality) * 3;
                    combatant.DealDamage(dmg, target);
                    target.AddEffect(new Slow(), 2);
                }
                combatant.SpendActionPoints(2);
            }
        }

        public static void OnArrowShower(Combatant combatant, PathNode targetNode)
        {
            Debug.LogWarning("NOT YET IMPLEMENTED.");

        }

        public static void OnFireArrow(Combatant combatant, PathNode targetNode)
        {
            Debug.LogWarning("NOT YET IMPLEMENTED.");

        }
        #endregion

        #region - Staff Arts -
        public static void OnBarrier(Combatant combatant, PathNode targetNode)
        {
            Debug.LogWarning("NOT YET IMPLEMENTED.");

        }

        public static void OnConfuse(Combatant combatant, PathNode targetNode)
        {
            Debug.LogWarning("NOT YET IMPLEMENTED.");

        }

        public static void OnMagicBolt(Combatant combatant, PathNode targetNode)
        {
            Debug.LogWarning("NOT YET IMPLEMENTED.");

        }

        public static void OnFieldOfMulch(Combatant combatant, PathNode targetNode)
        {
            Debug.LogWarning("NOT YET IMPLEMENTED.");

        }

        public static void OnWindOfCourage(Combatant combatant, PathNode targetNode)
        {
            Debug.LogWarning("NOT YET IMPLEMENTED.");

        }

        public static void OnChainLightning(Combatant combatant, PathNode targetNode)
        {
            Debug.LogWarning("NOT YET IMPLEMENTED.");

        }

        public static void OnSummonElemental(Combatant combatant, PathNode targetNode)
        {
            Debug.LogWarning("NOT YET IMPLEMENTED.");

        }
        #endregion

        #region - Book Arts -
        public static void OnEmpower(Combatant combatant, PathNode targetNode)
        {
            Debug.LogWarning("NOT YET IMPLEMENTED.");

        }
        #endregion
    }

    public enum Arts
    {
        Cleave,
        Pierce,
        Bash,
        Smash,
        AnkleShot,
    }
}