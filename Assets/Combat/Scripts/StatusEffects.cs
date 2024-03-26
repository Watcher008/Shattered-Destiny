using UnityEngine;

namespace SD.Combat
{
    public abstract class StatusEffects { }

    #region - Debuffs - 
    /// <summary>
    /// The unit's control switches to the opposing side.
    /// </summary>
    public class Charm : StatusEffects { }

    /// <summary>
    /// The unit does not regenerate action points at the start of their turn.
    /// </summary>
    public class Stun : StatusEffects { }

    /// <summary>
    /// Movement costs 2x per 1 tile moved.
    /// </summary>
    public class Slow : StatusEffects { }

    /// <summary>
    /// Unit uses all movement randomly.
    /// Unit uses 1/3 of their action points on random weapon arts towards random targets.
    /// </summary>
    public class Confuse : StatusEffects { }

    /// <summary>
    /// Unit does 25% less damage.
    /// </summary>
    public class Weaken : StatusEffects { }

    /// <summary>
    /// Unit loses all available action points.
    /// </summary>
    public class Daze : StatusEffects { }

    /// <summary>
    /// Unit takes 25% more damage.
    /// </summary>
    public class Vulnerable : StatusEffects { }

    /// <summary>
    /// Applies a random debuff every turn.
    /// </summary>
    public class Cursed : StatusEffects { }
    #endregion

    #region - Buffs -
    /// <summary>
    /// Unit does not have miss chance.
    /// </summary>
    public class Focused : StatusEffects { }

    /// <summary>
    /// Unit does 25% more damage.
    /// </summary>
    public class Empowered : StatusEffects { }

    /// <summary>
    /// Unit takes 25% less damage. 
    /// </summary>
    public class Reinforced : StatusEffects { }

    /// <summary>
    /// Doubles the movement of the unit.
    /// </summary>
    public class Hurry : StatusEffects { }
    #endregion
}

