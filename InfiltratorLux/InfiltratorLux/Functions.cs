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
    class Functions
    {

        // Initialize method
        public static void Initialize()
        {
            
        }

        // Combo method
        public static void Combo()
        {
            
        }

        // Harass method
        public static void Harass()
        {

        }

        // Flee method
        public static void Flee()
        {

        }

        // LaneClear method
        public static void LaneClear()
        {

        }

        // LastHit method
        public static void LastHit()
        {

        }

        // JungleClear method
        public static void JungleClear()
        {

        }

        // KillSteal method
        public static void KillSteal()
        {
            if (Display.GetCheckBoxValue("KillStealQ") && Calculations.Q.IsReady()
                && TargetSelector.SelectedTarget.IsInRange(Program.Champion, Calculations.Q.Range)
                && TargetSelector.SelectedTarget.Health <= Calculations.QDamage(TargetSelector.SelectedTarget))
            {
                Calculations.Q.Cast(TargetSelector.SelectedTarget);
            }
            if (Display.GetCheckBoxValue("KillStealE") && Calculations.E.IsReady()
                && TargetSelector.SelectedTarget.IsInRange(Program.Champion, Calculations.E.Range)
                && TargetSelector.SelectedTarget.Health <= Calculations.EDamage(TargetSelector.SelectedTarget))
            {
                Calculations.E.Cast(TargetSelector.SelectedTarget);
            }
            if (Display.GetCheckBoxValue("KillStealR") && Calculations.R.IsReady()
                && TargetSelector.SelectedTarget.IsInRange(Program.Champion, Calculations.R.Range)
                && TargetSelector.SelectedTarget.Health <= Calculations.RDamage(TargetSelector.SelectedTarget))
            {
                Calculations.R.Cast(TargetSelector.SelectedTarget);
            }
        }

        // Leveler method
        public static void Leveler()
        {
            // Array of 18 levels
            int[] leveler = { 1, 3, 3, 2, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };

            var avapoints = Program.Champion.SpellTrainingPoints;
            while (avapoints >= 1)
            {
                // Calculate Skill For Next LevelUp
                var skill = leveler[Program.Champion.Level - avapoints];

                switch (skill)
                {
                    case 1:
                        Program.Champion.Spellbook.LevelSpell(SpellSlot.Q);
                        break;
                    case 2:
                        Program.Champion.Spellbook.LevelSpell(SpellSlot.W);
                        break;
                    case 3:
                        Program.Champion.Spellbook.LevelSpell(SpellSlot.E);
                        break;
                    case 4:
                        Program.Champion.Spellbook.LevelSpell(SpellSlot.R);
                        break;
                }
                avapoints--;
            }
        }
    }
}