using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DefenderTaric
{
    class Calculations
    {
        // Declare Spells
        public static Spell.Active Q { get; set; }
        public static Spell.Targeted W { get; set; }
        public static Spell.Skillshot E { get; set; }
        public static Spell.Active R { get; set; }

        // Initialize method
        public static void Initialize()
        {
            Q = new Spell.Active(SpellSlot.Q, 325);
            W = new Spell.Targeted(SpellSlot.W, 800);
            E = new Spell.Skillshot(SpellSlot.E, 800, SkillShotType.Linear, 500, 1300, 140, DamageType.Magical)
            {
                AllowedCollisionCount = int.MaxValue,
            };
            R = new Spell.Active(SpellSlot.R, 400);
        }

        // INNATE: After casting an ability, Taric's next two basic attacks within 4 seconds each gain 100% total attack speed,
        // deal 22 - 90 (based on level) (+ 15% bonus armor) bonus magic damage and reduce the cooldowns of his basic abilities
        // by 1 - 0.55 (based on Cooldown Reduction) seconds, increased to 6 - 3.3 (based on Cooldown Reduction) seconds for Starlight's Touch's recharge time.
        public static float PDamage(Obj_AI_Base target)
        {
            return Program.Champion.CalculateDamageOnUnit(target, DamageType.Magical, 
                22 + (4 * Program.Champion.Level) + (0.15f * BonusArmor()));
        }

        public static float BonusArmor()
        {
            return Program.Champion.Armor - (25 + (3.4f * Program.Champion.Level));
        }

        // PASSIVE: Taric stores a charge of Starlight's Touch periodically, up to a maximum of 3 at once. Starlight's Touch cannot be cast without charges.
        // ACTIVE: Taric heals himself and all nearby allied champions, with the amount increasing with every stored charge at the time of cast.
        public static float QHealing(Obj_AI_Base target)
        {
            return target.GetBuffCount("Starlight") * (20 + (10 * Q.Level) + (0.2f * Program.Champion.FlatMagicDamageMod) + (0.015f * Program.Champion.Health));
        }

        // PASSIVE: Taric and his Bastion-marked champion gain bonus armor.
        // ACTIVE: Taric shields himself and the target allied champion for 2.5 seconds, blessing them with Bastion
        // and causing his abilities to be replicated on them while both are near each other, though the effects do not stack.
        public static float WArmorBonus(Obj_AI_Base target)
        {
            return 0.1f + (0.025f * W.Level);
        }
        public static float WShield(Obj_AI_Base target)
        {
            return 0.08f + (0.01f * W.Level);
        }

        // ACTIVE: Taric projects a beam of starlight towards the target location, erupting after a 1 second delay, dealing magic damage
        // to all enemies hit and Stun icon stunning them.
        public static float EDamage(Obj_AI_Base target)
        {
            return Program.Champion.CalculateDamageOnUnit(target, DamageType.Magical, 
                60 + (45 * E.Level) + (0.5f * Program.Champion.FlatMagicDamageMod));
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