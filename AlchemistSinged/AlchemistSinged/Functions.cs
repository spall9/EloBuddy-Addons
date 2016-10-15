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
                var target = TargetManager.GetChampionTarget(250, Calculations.Q.DamageType);
                if (target != null)
                    Calculations.ToggleQ_On(target);
            }

            if (Display.GetCheckBoxValue("ComboE"))
            {
                var target = TargetManager.GetChampionTarget(Calculations.E.Range,Calculations.E.DamageType);
                if (target != null && Calculations.E.IsReady())
                {
                    if (Display.GetCheckBoxValue("ComboW") && Calculations.W.IsReady())
                        Calculations.W.Cast(Prediction.Position.PredictUnitPosition(target, Calculations.W.CastDelay/2).Extend(Program.Champion, 550).To3D());
                    Calculations.CastE(target);
                }
            }

            if (Display.GetCheckBoxValue("ComboR")
                && Program.Champion.CountEnemiesInRange(Calculations.W.Range) >= Display.GetSliderValue("ComboL"))
            {
                var target = TargetManager.GetChampionTarget(1000, Calculations.R.DamageType);
                if (target != null)
                    Calculations.CastR(Program.Champion);
            }
        }

        // Harass method
        public static void Harass()
        {
            if (Display.GetCheckBoxValue("HarassQ"))
            {
                var target = TargetManager.GetChampionTarget(250, Calculations.Q.DamageType);
                if (target != null)
                    Calculations.ToggleQ_On(target);
            }
        }

        // Flee method
        public static void Flee()
        {
            if (Display.GetCheckBoxValue("FleeE"))
            {
                var target = TargetManager.GetChampionTarget(Calculations.E.Range, Calculations.E.DamageType);
                if (target != null)
                    Calculations.CastE(target);
            }
        }

        // LaneClear method
        public static void LaneClear()
        {
            if (Display.GetCheckBoxValue("LaneClearQ"))
            {
                var target = TargetManager.GetMinionTarget(250, Calculations.Q.DamageType);
                if (target != null)
                    Calculations.ToggleQ_On(target);
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
                var target = TargetManager.GetMonsterTarget(250, Calculations.Q.DamageType);
                if (target != null)
                    Calculations.ToggleQ_On(target);
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
                if (Program.Champion.IsInShopRange()
                    && (item.Id == ItemId.Tear_of_the_Goddess || item.Id == ItemId.Archangels_Staff || item.Id == ItemId.Manamune))
                    Calculations.ToggleQ_On(Program.Champion);
            }
        }

        // Kiter method
        public static void Kiter()
        {
            var target = TargetManager.GetMonsterTarget(250, Calculations.Q.DamageType);
            if (target != null &&  !Program.Champion.IsUnderEnemyturret()
                && !Program.Champion.IsFacing(target) && target.IsFacing(Program.Champion)
                && target.Distance(Program.Champion) <= 750)
                Calculations.ToggleQ_On(target);
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
            if (sender != null && Display.GetCheckBoxValue("GapCloserE"))
            {
                var target = TargetManager.GetChampionTarget(Calculations.E.Range, Calculations.E.DamageType);
                if (target != null)
                    Calculations.CastE(target);
            }
        }
    }
}