using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace AlchemistSinged
{
    internal class SpellManager
    {
        public static Spell.Active Q { get; set; }
        public static Spell.Skillshot W { get; set; }
        public static Spell.Targeted E { get; set; }
        public static Spell.Active R { get; set; }

        // Global Poison Control
        public static bool IsStackingTear;

        // Clone Character Object
        public static AIHeroClient Champion = Program.Champion;

        public static void Initialize()
        {
            // Initialize spells
            Q = new Spell.Active(SpellSlot.Q);
            W = new Spell.Skillshot(SpellSlot.W, 1000, SkillShotType.Circular, 500, 700, 350)
            {
                MinimumHitChance = HitChance.High,
                AllowedCollisionCount = int.MaxValue
            };
            E = new Spell.Targeted(SpellSlot.E, 125);
            R = new Spell.Active(SpellSlot.R);
        }

        // Poison Controller
        public static void QDisable()
        {
            // Disable Bools
            if (IsStackingTear && Champion.IsInShopRange()) return;
            if (IsStackingTear)
                IsStackingTear = false;

            // Disable Conditions
            if (Player.GetSpell(SpellSlot.Q).ToggleState == 2
                && Champion.CountEnemiesInRange(1000) < 1
                && !(EntityManager.MinionsAndMonsters.EnemyMinions.Any(a => a.IsInRange(Champion, 1000))))
                CastQ(Champion);
        }

        // Champion Specified Abilities
        public static float QDamage()
        {
            return new float[] { 0, 44, 68, 92, 116, 140 }[Q.Level]
                + (0.6f * Champion.FlatMagicDamageMod);
        }

        public static float EDamage()
        {
            return new float[] { 0, 50, 65, 80, 95, 110 }[E.Level]
                   + (0.75f * Champion.FlatMagicDamageMod);
        }

        // Cast Methods
        public static void CastQ(Obj_AI_Base target)
        {
            if (target == null) return;
            if (Q.IsReady())
                Q.Cast();
        }

        public static void CastW(Obj_AI_Base target)
        {
            if (target == null) return;
            if (W.IsReady())
                W.Cast(target);
        }

        public static void CastE(Obj_AI_Base target)
        {
            if (target == null) return;
            if (E.IsReady())
                E.Cast(target);
        }

        public static void CastR(Obj_AI_Base target)
        {
            if (target == null) return;
            if (R.IsReady())
                R.Cast();
        }
    }
}
