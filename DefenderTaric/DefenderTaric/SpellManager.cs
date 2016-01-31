using EloBuddy;
using EloBuddy.SDK;

namespace DefenderTaric
{
    internal class SpellManager
    {
        public static Spell.Targeted Q { get; set; }
        public static Spell.Active W { get; set; }
        public static Spell.Targeted E { get; set; }
        public static Spell.Active R { get; set; }

        // Clone Character Object
        public static AIHeroClient Champion = Program.Champion;

        public static void Initialize()
        {
            // Initialize spells
            Q = new Spell.Targeted(SpellSlot.Q, 750);
            W = new Spell.Active(SpellSlot.W, 400);
            E = new Spell.Targeted(SpellSlot.E, 625);
            R = new Spell.Active(SpellSlot.R, 400);
        }

        // Champion Specified Abilities
        public static float WDamage(Obj_AI_Base target)
        {
            return new float[] { 0, 40, 80, 120, 160, 200 }[Q.Level] + (0.2f * Champion.FlatArmorMod);
        }

        public static float EDamage(Obj_AI_Base target)
        {
            return new float[] { 0, 40, 70, 100, 130, 160 }[E.Level] + (0.2f * Champion.FlatMagicDamageMod);
        }

        public static float RDamage(Obj_AI_Base target)
        {
            return new float[] { 0, 150, 250, 350 }[R.Level] + (0.5f * Champion.FlatMagicDamageMod);
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
                W.Cast();
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
