using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace DraconicChoGath
{
    internal class SpellManager
    {
        public static Spell.Skillshot Q { get; set; }
        public static Spell.Skillshot W { get; set; }
        public static Spell.Active E { get; set; }
        public static Spell.Targeted R { get; set; }

        // Clone Character Object
        public static AIHeroClient Champion = Program.Champion;

        // Tear Timestamp
        public static float StackerStamp = 0;

        // Flash Instance
        public static SpellDataInst Sumspell { get; set; }

        public static void Initialize()
        {
            // Initialize spells
            Q = new Spell.Skillshot(SpellSlot.Q, 950, SkillShotType.Circular, 1200, int.MaxValue, 250)
            {
                MinimumHitChance = HitChance.High,
                AllowedCollisionCount = int.MaxValue
            };
            W = new Spell.Skillshot(SpellSlot.W, 575, SkillShotType.Cone, 250, int.MaxValue, 60)
            {
                MinimumHitChance = HitChance.High,
                AllowedCollisionCount = int.MaxValue
            };
            E = new Spell.Active(SpellSlot.E);
            R = new Spell.Targeted(SpellSlot.R, 175);

            // Flash Config
            if (Champion.Spellbook.GetSpell(SpellSlot.Summoner1).Name == "summonerflash")
                Sumspell = Champion.Spellbook.GetSpell(SpellSlot.Summoner1);
            if (Champion.Spellbook.GetSpell(SpellSlot.Summoner2).Name == "summonerflash")
                Sumspell = Champion.Spellbook.GetSpell(SpellSlot.Summoner2);
        }

        public static void ConfigSpells(EventArgs args)
        {
            R = new Spell.Targeted(SpellSlot.R, (uint)(Champion.GetAutoAttackRange()
                - (new[] { 0, 3.833, 6.167, 8.333 }[R.Level] * Champion.GetBuffCount("Feast"))));
        }

        // Champion Specified Abilities
        public static float QDamage()
        {
            return new float[] { 0, 80, 135, 190, 245, 305 }[Q.Level]
                + Champion.FlatMagicDamageMod;
        }

        public static float WDamage()
        {
            return new float[] { 0, 75, 125, 175, 225, 275 }[W.Level]
                   + (0.7f * Champion.FlatMagicDamageMod);
        }

        public static float RDamage()
        {
            return new float[] { 0, 300, 475, 650 }[R.Level]
                   + (0.7f * Champion.FlatMagicDamageMod);
        }

        public static float RMonsterDamage()
        {
            return 1000 + (0.7f * Champion.FlatMagicDamageMod);
        }

        public static float FlashUltRange()
        {
            return R.Range + 425;
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
                E.Cast();
        }

        public static void CastR(Obj_AI_Base target)
        {
            if (target == null) return;
            if (R.IsReady())
                R.Cast(target);
        }

        public static void CastFlash(Obj_AI_Base target)
        {
            if (target == null) return;
            Champion.Spellbook.CastSpell(Sumspell.Slot, target);
        }
    }
}