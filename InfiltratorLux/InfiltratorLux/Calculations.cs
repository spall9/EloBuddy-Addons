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

namespace InfiltratorLux
{
    class Calculations
    {
        // Declare Spells
        public static Spell.Skillshot Q { get; set; }
        public static Spell.Skillshot W { get; set; }
        public static Spell.Skillshot E { get; set; }
        public static Spell.Skillshot R { get; set; }

        // Initialize method
        public static void Initialize()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1300, SkillShotType.Linear, 500, 1200, 80, DamageType.Magical)
            {
                AllowedCollisionCount = 2,
            };
            W = new Spell.Skillshot(SpellSlot.W, 1075, SkillShotType.Linear, 500, 1200, 150);
            E = new Spell.Skillshot(SpellSlot.E, 1100, SkillShotType.Circular, 500, 1300, 275, DamageType.Magical);
            R = new Spell.Skillshot(SpellSlot.R, 3340, SkillShotType.Linear, 1750, 3000, 190, DamageType.Magical);
        }

        // Lux's offensive abilities mark all affected enemies with light energy for 6 seconds.
        // Her basic attacks and Final Spark consume the mark, dealing 20 - 190 (based on level) (+ 20% AP) magic damage.
        public static float PDamage(Obj_AI_Base target)
        {
            return Program.Champion.CalculateDamageOnUnit(target, DamageType.Magical, 
                20 + (10 * Program.Champion.Level) + (0.2f * Program.Champion.FlatMagicDamageMod));
        }

        // ACTIVE: Lux releases a sphere of light in a line that deals magic damage to the first two enemies hit and roots them for 2 seconds.
        public static float QDamage(Obj_AI_Base target)
        {
            return Program.Champion.CalculateDamageOnUnit(target, DamageType.Magical, 
                50 + (50 * Q.Level) + (0.7f * Program.Champion.FlatMagicDamageMod));
        }

        // ACTIVE: Lux shields herself and throws out her wand in a line, shielding allied champions in its path for 3 seconds.
        public static float WShieldInitial(Obj_AI_Base target)
        {
            return 50 + (15 * W.Level) + (0.2f * Program.Champion.FlatMagicDamageMod);
        }
        // Lux's wand then returns to her, stacking the shield to all allied champions it passes through as well as herself.
        public static float WShieldTotal(Obj_AI_Base target)
        {
            return WShieldInitial(target) * 2;
        }

        // ACTIVE: Lux sends an anomaly of twisted light to the target area, slowing nearby enemies and granting sight of the area around it for up to 5 seconds.
        // At the end of the duration or if Lucent Singularity is activated again, the singularity detonates, dealing magic damage to all enemies in the area.
        public static float EDamage(Obj_AI_Base target)
        {
            return Program.Champion.CalculateDamageOnUnit(target, DamageType.Magical, 
                60 + (45 * E.Level) + (0.6f * Program.Champion.FlatMagicDamageMod));
        }

        // ACTIVE: After gathering energy for 0.5 seconds, Lux fires a giant laser in a line that deals magic damage to all enemies hit,
        // briefly Sight icon revealing them as well as the surrounding area.
        public static float RDamage(Obj_AI_Base target)
        {
            return Program.Champion.CalculateDamageOnUnit(target, DamageType.Magical, 
                300 + (100 * R.Level) + (0.75f * Program.Champion.FlatMagicDamageMod));
        }
    }
}