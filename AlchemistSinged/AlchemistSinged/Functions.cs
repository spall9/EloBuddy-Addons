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

namespace AlchemistSinged
{
    class Functions
    {
        // Initialize method
        public static void Initialize()
        {

        }

        // Combo method
        public static void Combo()
        {
            if (Display.GetCheckBoxValue("ComboQ"))
            {
                if (Calculations.Toggle) return;
                var target = TargetManager.GetChampionTarget(250, Calculations.Q.DamageType);
                if (target != null)
                    Calculations.CastQ(target);
            }

            if (Display.GetCheckBoxValue("ComboE"))
            {
                var target = TargetManager.GetChampionTarget(Calculations.E.Range,Calculations.E.DamageType);
                if (target != null && Calculations.E.IsReady())
                {
                    if (Display.GetCheckBoxValue("ComboW") && Calculations.W.IsReady())
                    {
                        var pos = Prediction.Position.PredictUnitPosition(target, Calculations.W.CastDelay/2).Extend(Program.Champion, 550).To3D();
                            Calculations.W.Cast(pos);
                    }
                    Calculations.CastE(target);
                }
            }

            if (Display.GetCheckBoxValue("ComboR")
                && Program.Champion.CountEnemiesInRange(Calculations.W.Range) >= Display.GetSliderValue("ComboL"))
            {
                var target = TargetManager.GetChampionTarget(1000, Calculations.R.DamageType);
                if (target != null && target.HasBuff("poisontrailtarget"))
                    Calculations.CastR(Program.Champion);
            }
        }

        // Harass method
        public static void Harass()
        {
            if (Display.GetCheckBoxValue("HarassQ"))
            {
                if (Calculations.Toggle) return;
                var target = TargetManager.GetChampionTarget(250, Calculations.Q.DamageType);
                if (target != null)
                    Calculations.CastQ(target);
            }
        }

        // Flee method
        public static void Flee()
        {
            var target = TargetManager.GetChampionTarget(1000, Calculations.Q.DamageType);
            if (target != null
                && !Program.Champion.IsUnderEnemyturret() && !target.IsUnderEnemyturret()
                && !Program.Champion.IsFacing(target) && target.IsFacing(Program.Champion)
                && target.Distance(Program.Champion) <= 750)
            {
                if (Calculations.Toggle) return;
                Calculations.CastQ(target);
            }
        }

        // LaneClear method
        public static void LaneClear()
        {
            Orbwalker.DisableAttacking = Display.GetCheckBoxValue("DisablerAA");
            if (Display.GetCheckBoxValue("LaneClearQ"))
            {
                if (Calculations.Toggle) return;
                var target = TargetManager.GetMinionTarget(250, Calculations.Q.DamageType);
                if (target != null)
                    Calculations.CastQ(target);
            }
            if (Display.GetCheckBoxValue("LaneClearE"))
            {
                var target = TargetManager.GetMinionTarget(Calculations.E.Range, Calculations.E.DamageType);
                if (target != null)
                    Calculations.CastE(target);
            }
        }

        // LastHit method
        public static void LastHit()
        {
            if (Display.GetCheckBoxValue("LastHitE"))
            {
                var target = TargetManager.GetMinionTarget(Calculations.E.Range, Calculations.E.DamageType);
                if (target != null && TargetManager.CalculateKS(target, Calculations.E.DamageType, Calculations.EDamage(target)))
                    Calculations.CastE(target);
            }
        }

        // JungleClear method
        public static void JungleClear()
        {
            if (Display.GetCheckBoxValue("JungleClearQ"))
            {
                if (Calculations.Toggle) return;
                var target = TargetManager.GetMonsterTarget(250, Calculations.Q.DamageType);
                if (target != null)
                    Calculations.CastQ(target);
            }
            if (Display.GetCheckBoxValue("JungleClearE"))
            {
                var target = TargetManager.GetMonsterTarget(Calculations.E.Range, Calculations.E.DamageType);
                if (target != null)
                    Calculations.CastE(target);
            }
        }

        // Stacker method
        public static void Stacker()
        {
            foreach (var item in Program.Champion.InventoryItems)
            {
                if ((item.Id == ItemId.Tear_of_the_Goddess || item.Id == ItemId.Archangels_Staff 
                    || item.Id == ItemId.Manamune) && item.Stacks < 750 && Program.Champion.IsInShopRange())
                {
                    if (Calculations.Toggle) return;
                    Calculations.CastQ(Program.Champion);
                }
            }
        }

        // Interrupter method
        public static void Interrupter(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {
            if (sender != null && Display.GetCheckBoxValue("InterrupterE"))
            {
                var target = TargetManager.GetChampionTarget(Calculations.E.Range, Calculations.E.DamageType);
                if (target != null)
                    Calculations.CastE(target);
            }
        }

        // GapCloser method
        public static void GapCloser(Obj_AI_Base sender, Gapcloser.GapcloserEventArgs args)
        {
            if (sender != null && args != null)
            {
                var spell = args.SpellName;
                if (spell == "TristanaR" || spell == "BlindMonkRKick" || spell == "MalazaharNetherGrasp"
                    || spell == "GalioIdolOfDurand" || spell == "VayneCondemn" || spell == "JayceThunderingBlow" || spell == "Headbutt")
                    if (args.Target.IsMe)
                        Calculations.CastE(sender);
            }
            if (sender != null && Display.GetCheckBoxValue("GapCloserE"))
            {
                var target = TargetManager.GetChampionTarget(Calculations.E.Range, Calculations.E.DamageType);
                if (target != null)
                    Calculations.CastE(target);
            }
        }
    }
}