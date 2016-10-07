using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using SharpDX;

namespace SeekerVelKoz
{
    internal class SpellManager
    {
        public static Spell.Skillshot Q { get; set; }
        public static Spell.Skillshot W { get; set; }
        public static Spell.Skillshot E { get; set; }
        public static Spell.Skillshot R { get; set; }

        public const int MissileSpeed = 2100;
        public const int CastDelay = 250;
        public const int SpellWidth = 45;
        public const int SpellRange = 1100;

        // Clone Character Object
        public static AIHeroClient Champion = Program.Champion;

        public static void Initialize()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1050, SkillShotType.Linear, 250, 1300, 50)
            {
                MinimumHitChance = HitChance.High,
                AllowedCollisionCount = 1
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
        public static float PDamage()
        {
            return (25 + (8 * Champion.Level))
                + (0.5f * Champion.FlatMagicDamageMod);
        }

        public static float QDamage()
        {
            return new float[] { 0, 80, 120, 160, 200, 240 }[Q.Level] 
               + (0.6f * Champion.FlatMagicDamageMod);
        }

        public static float WDamage()
        {
            return new float[] { 0, 30, 50, 70, 90, 110 }[W.Level]
                + (0.15f * Champion.FlatMagicDamageMod);
        }

        public static float EDamage()
        {
            return new float[] { 0, 70, 100, 130, 160, 190 }[E.Level]
                + (0.3f * Champion.FlatMagicDamageMod);
        }

        public static float RPerTickDamage()
        {
            return new float[] { 0, 45, 62.5f, 80 }[R.Level]
                + (0.125f * Champion.FlatMagicDamageMod);
        }

        public static float RTotalDamage()
        {
            return RPerTickDamage() * 10;
        }

        // Cast Methods
        public static void CastQ(Obj_AI_Base target)
        {
            if (target == null) return;
            if (Q.IsReady() && Q.Name == "VelkozQ")
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
            if (R.IsReady() && !Champion.HasBuff("VelkozR"))
                R.Cast(target);
        }
    }
}
