using SD.Grids;
using System.Collections.Generic;

namespace SD.Combat.Effects
{
    public abstract class AreaEffects { }

    public class ActiveAreaEffect
    {
        public AreaEffects Effect { get; private set; }
        public int Duration;
        public List<PathNode> Area { get; private set; }

        public ActiveAreaEffect(AreaEffects effect, int duration, List<PathNode> nodes)
        {
            Effect = effect;
            Duration = duration;
            Area = nodes;
        }
    }

    /// <summary>
    /// Creates a 3x3 area that blocks incoming projectiles and increases damage of outgoing projectiles by 50%.
    /// Lasts 3 turns.
    /// </summary>
    public class Effect_Barrier : AreaEffects { }

    /// <summary>
    /// Creates a 3x3 area that applies the Slowed effect to units within it. 
    /// Lasts 4 turns.
    /// </summary>
    public class Effect_Mulch : AreaEffects { }
}