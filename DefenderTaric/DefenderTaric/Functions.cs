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
    class Functions
    {
        // Initialize method
        public static void Initialize() {}

        // Combo method
        public static void Combo()
        {
            if (Display.GetCheckBoxValue("SettingsP") && Program.Champion.HasBuff("TaricPassiveAttack")) return;

            if (Display.GetCheckBoxValue("ComboE"))
            {
                var target = TargetManager.GetChampionTarget(Calculations.E.Range, Calculations.E.DamageType);
                if (target != null)
                    Calculations.CastE(target);
            }
        }

        // Harass method
        public static void Harass()
        {
            if (Display.GetCheckBoxValue("SettingsP") && Program.Champion.HasBuff("TaricPassiveAttack")) return;

            if (Program.Champion.ManaPercent <= Display.GetSliderValue("HarassM")) return;

            if (Display.GetCheckBoxValue("HarassE"))
            {
                var target = TargetManager.GetChampionTarget(Calculations.E.Range, Calculations.E.DamageType);
                if (target != null)
                    Calculations.CastE(target);
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
            if (Display.GetCheckBoxValue("SettingsP") && Program.Champion.HasBuff("TaricPassiveAttack")) return;
            if (Program.Champion.ManaPercent <= Display.GetSliderValue("LaneClearM")) return;
            
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
            if (Display.GetCheckBoxValue("SettingsP") && Program.Champion.HasBuff("TaricPassiveAttack")) return;
            if (Program.Champion.ManaPercent <= Display.GetSliderValue("LastHitM")) return;

            if (Display.GetCheckBoxValue("LaneClearQ"))
            {
                var target = TargetManager.GetMinionTarget(Calculations.Q.Range, Calculations.Q.DamageType);
                if (target != null
                    && TargetManager.CalculateKS(target, Calculations.Q.DamageType, 
                   Program.Champion.GetAutoAttackDamage(target) + Calculations.PDamage(target)))
                    Calculations.CastQ(target);
            }
        }

        // JungleClear method
        public static void JungleClear()
        {
            if (Display.GetCheckBoxValue("SettingsP") && Program.Champion.HasBuff("TaricPassiveAttack")) return;
            if (Program.Champion.ManaPercent <= Display.GetSliderValue("JungleClearM")) return;
            
            if (Display.GetCheckBoxValue("JungleClearE"))
            {
                var target = TargetManager.GetMonsterTarget(Calculations.E.Range, Calculations.E.DamageType);
                if (target != null)
                    Calculations.CastE(target);
            }
        }

        // Assistance method
        public static void Assistance()
        {
            if (Display.GetSliderValue("AssistanceHally") != 0
                && Calculations.Q.AmmoQuantity >= Display.GetSliderValue("AssistanceSally"))
            {
                var target = TargetManager.GetChampionTarget(Calculations.Q.Range, Calculations.Q.DamageType, true);
                if (target != null && target.HealthPercent <= Display.GetSliderValue("AssistanceHally"))
                    Calculations.CastQ(target);
            }
            if (Display.GetSliderValue("AssistanceHself") != 0
                && Calculations.Q.AmmoQuantity >= Display.GetSliderValue("AssistanceSself")
                && Program.Champion.HealthPercent <= Display.GetSliderValue("AssistanceHself"))
                Calculations.CastQ(Program.Champion);

            if (Display.GetSliderValue("AssistanceRally") != 0)
                {
                    var target = TargetManager.GetChampionTarget(Calculations.R.Range, Calculations.R.DamageType, true);
                    if (target != null && target.HealthPercent <= Display.GetSliderValue("AssistanceRally"))
                        Calculations.CastR(target);
                }
            if (Display.GetSliderValue("AssistanceRself") != 0 && !Program.Champion.IsRecalling()
                && Program.Champion.HealthPercent <= Display.GetSliderValue("AssistanceRself"))
                Calculations.CastR(Program.Champion);
        }
    }
}