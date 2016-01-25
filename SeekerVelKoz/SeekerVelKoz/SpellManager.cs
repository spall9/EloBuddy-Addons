using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace SeekerVelKoz
{
    internal class SpellManager
    {
        public static Spell.Skillshot Q { get; set; }
        public static Spell.Skillshot W { get; set; }
        public static Spell.Skillshot E { get; set; }
        public static Spell.Skillshot R { get; set; }

        public static MissileClient Qmiss = null;

        // Clone Character Object
        public static AIHeroClient Champion = Program.Champion;

        public static void Initialize()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1050, SkillShotType.Linear, 250, 1300, 50)
            {
                MinimumHitChance = HitChance.High,
                AllowedCollisionCount = 0
            };
            W = new Spell.Skillshot(SpellSlot.W, 1050, SkillShotType.Linear, 250, 1700, 80)
            {
                MinimumHitChance = HitChance.High,
                AllowedCollisionCount = int.MaxValue
            };
            E = new Spell.Skillshot(SpellSlot.E, 850, SkillShotType.Circular, 500, 1500, 120)
            {
                MinimumHitChance = HitChance.High,
                AllowedCollisionCount = int.MaxValue
            };
            R = new Spell.Skillshot(SpellSlot.R, 1550, SkillShotType.Linear)
            {
                MinimumHitChance = HitChance.High,
                AllowedCollisionCount = int.MaxValue
            };
        }

        // Champion Specified Abilities
        public static float QDamage()
        {
            return new float[] { 0, 80, 120, 150, 200, 240 }[Q.Level] + 0.6f * Champion.FlatMagicDamageMod;
        }

        public static float WDamage()
        {
            return new float[] { 0, 75, 125, 175, 225, 275 }[W.Level] + 0.625f * Champion.FlatMagicDamageMod;
        }

        public static float EDamage()
        {
            return new float[] { 0, 70, 100, 130, 160, 190 }[E.Level] + 0.5f * Champion.FlatMagicDamageMod;
        }

        public static float RDamage()
        {
            return new float[] { 0, 500, 700, 900 }[R.Level] + 0.5f * Champion.FlatMagicDamageMod;
        }

        // Cast Methods
        public static void CastQ(Obj_AI_Base target)
        {
            if (target == null) return;
            if (Q.IsReady())
                Q.Cast(target);
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
                R.Cast(target);
        }
    }
}
