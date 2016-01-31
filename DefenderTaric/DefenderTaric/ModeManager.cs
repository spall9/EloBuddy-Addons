using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;

namespace DefenderTaric
{
    internal class ModeManager
    {
        // Clone Character Object
        public static AIHeroClient Champion = Program.Champion;

        public static void EwrComboMode()
        {
            if (MenuManager.SpellWeave && Champion.HasBuff("taricgemcraftbuff"))
            {
                var target = TargetManager.GetChampionTarget(Champion.GetAutoAttackRange(), DamageType.Physical);
                if (target != null && Orbwalker.CanAutoAttack)
                    Orbwalker.ForcedTarget = target;
            }
            else if (MenuManager.SpellWeave && !Champion.HasBuff("taricgemcraftbuff"))
            {
                if (MenuManager.ComboUseE)
                {
                    var target = TargetManager.GetChampionTarget(SpellManager.E.Range, DamageType.Magical);
                    if (target != null)
                        SpellManager.CastE(target);
                }
                if (MenuManager.ComboUseW && Champion.HealthPercent >= MenuManager.ComboWLimit)
                {
                    var target = TargetManager.GetChampionTarget(SpellManager.W.Range, DamageType.Magical);
                    if (target != null)
                        SpellManager.CastW(target);
                }
                if (MenuManager.ComboUseR)
                {
                    var target = TargetManager.GetChampionTarget(SpellManager.R.Range, DamageType.Magical);
                    if (target != null)
                        SpellManager.CastR(target);
                }
                if (MenuManager.SpellWeaveUseQ)
                {
                    var target = TargetManager.GetChampionTarget(Champion.GetAutoAttackRange(), DamageType.Physical);
                    if (target != null)
                        SpellManager.CastQ(Champion);
                }
            }
            else
            {
                if (MenuManager.ComboUseE)
                {
                    var target = TargetManager.GetChampionTarget(SpellManager.E.Range, DamageType.Magical);
                    if (target != null)
                        SpellManager.CastE(target);
                }
                if (MenuManager.ComboUseW && Champion.HealthPercent >= MenuManager.ComboWLimit)
                {
                    var target = TargetManager.GetChampionTarget(SpellManager.W.Range, DamageType.Magical);
                    if (target != null)
                        SpellManager.CastW(target);
                }
                if (MenuManager.ComboUseR)
                {
                    var target = TargetManager.GetChampionTarget(SpellManager.R.Range, DamageType.Magical);
                    if (target != null)
                        SpellManager.CastR(target);
                }
            }
        }

        public static void ErwComboMode()
        {
            if (MenuManager.SpellWeave && Champion.HasBuff("taricgemcraftbuff"))
            {
                var target = TargetManager.GetChampionTarget(Champion.GetAutoAttackRange(), DamageType.Physical);
                if (target != null && Orbwalker.CanAutoAttack)
                    Orbwalker.ForcedTarget = target;
            }
            else if (MenuManager.SpellWeave && !Champion.HasBuff("taricgemcraftbuff"))
            {
                if (MenuManager.ComboUseE)
                {
                    var target = TargetManager.GetChampionTarget(SpellManager.E.Range, DamageType.Magical);
                    if (target != null)
                        SpellManager.CastE(target);
                }
                if (MenuManager.ComboUseR)
                {
                    var target = TargetManager.GetChampionTarget(SpellManager.R.Range, DamageType.Magical);
                    if (target != null)
                        SpellManager.CastR(target);
                }
                if (MenuManager.ComboUseW && Champion.HealthPercent >= MenuManager.ComboWLimit)
                {
                    var target = TargetManager.GetChampionTarget(SpellManager.W.Range, DamageType.Magical);
                    if (target != null)
                        SpellManager.CastW(target);
                }
                if (MenuManager.SpellWeaveUseQ)
                {
                    var target = TargetManager.GetChampionTarget(Champion.GetAutoAttackRange(), DamageType.Physical);
                    if (target != null)
                        SpellManager.CastQ(Champion);
                }
            }
            else
            {
                if (MenuManager.ComboUseE)
                {
                    var target = TargetManager.GetChampionTarget(SpellManager.E.Range, DamageType.Magical);
                    if (target != null)
                        SpellManager.CastE(target);
                }
                if (MenuManager.ComboUseR)
                {
                    var target = TargetManager.GetChampionTarget(SpellManager.R.Range, DamageType.Magical);
                    if (target != null)
                        SpellManager.CastR(target);
                }
                if (MenuManager.ComboUseW && Champion.HealthPercent >= MenuManager.ComboWLimit)
                {
                    var target = TargetManager.GetChampionTarget(SpellManager.W.Range, DamageType.Magical);
                    if (target != null)
                        SpellManager.CastW(target);
                }
            }
        }

        public static void HarassMode()
        {
            if (Champion.ManaPercent < MenuManager.HarassMana) return;
            if (MenuManager.HarassUseW)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.W.Range, DamageType.Magical);
                if (target != null)
                    SpellManager.CastW(target);
            }
            if (MenuManager.HarassUseE)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.E.Range, DamageType.Magical);
                if (target != null)
                    SpellManager.CastE(target);
            }
        }

        public static void HealingMode()
        {
            if (Champion.ManaPercent < MenuManager.HealingMana) return;

            var qhealally = MenuManager.HealingAlly;
            var qhealtaric = MenuManager.HealingSelf;
            if (qhealally != 0)
            {
                var ally = TargetManager.GetChampionTarget(SpellManager.Q.Range, DamageType.Magical, true);
                if (ally != null && ally.HealthPercent <= qhealally)
                    SpellManager.CastQ(ally);
            }
            if (qhealtaric != 0 && Champion.HealthPercent <= qhealtaric)
                SpellManager.CastQ(Champion);
        }

        public static void InterruptMode(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {
            if (!MenuManager.InterrupterMode) return;
            if (sender != null && MenuManager.InterrupterUseE)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.E.Range, DamageType.Magical);
                if (target != null)
                    SpellManager.CastE(target);
            }
        }

        public static void GapCloserMode(Obj_AI_Base sender, Gapcloser.GapcloserEventArgs args)
        {
            if (!MenuManager.GapCloserMode) return;
            if (sender != null && MenuManager.GapCloserUseE)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.E.Range, DamageType.Magical);
                if (target != null)
                    SpellManager.CastE(target);
            }
        }
    }
}
