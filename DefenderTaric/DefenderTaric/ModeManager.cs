using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;

namespace DefenderTaric
{
    internal class ModeManager
    {
        // Clone Character Object
        public static AIHeroClient Champion = Program.Champion;

        public static void Initialize()
        {
        }

        public static void EwrComboMode()
        {
            if (!MenuManager.ComboMenu["ComboM"].Cast<CheckBox>().CurrentValue) return;
            if (MenuManager.ComboMenu["Pautos"].Cast<CheckBox>().CurrentValue && Champion.HasBuff("taricgemcraftbuff"))
            {
                var target = TargetManager.GetChampionTarget(Champion.GetAutoAttackRange(), DamageType.Physical);
                if (target != null && Orbwalker.CanAutoAttack)
                    Orbwalker.ForcedTarget = target;
            }
            else if (MenuManager.ComboMenu["Pautos"].Cast<CheckBox>().CurrentValue && !Champion.HasBuff("taricgemcraftbuff"))
            {
                if (MenuManager.ComboMenu["Ecombo"].Cast<CheckBox>().CurrentValue)
                {
                    var target = TargetManager.GetChampionTarget(SpellManager.E.Range, DamageType.Magical);
                    if (target != null)
                        SpellManager.CastE(target);
                }
                if (MenuManager.ComboMenu["Wcombo"].Cast<CheckBox>().CurrentValue
                    && Champion.HealthPercent >= MenuManager.ComboMenu["Wlimit"].Cast<Slider>().CurrentValue)
                {
                    var target = TargetManager.GetChampionTarget(SpellManager.W.Range, DamageType.Magical);
                    if (target != null)
                        SpellManager.CastW(target);
                }
                if (MenuManager.ComboMenu["Rcombo"].Cast<CheckBox>().CurrentValue)
                {
                    var target = TargetManager.GetChampionTarget(SpellManager.R.Range, DamageType.Magical);
                    if (target != null)
                        SpellManager.CastR(target);
                }
                if (MenuManager.ComboMenu["Qpassive"].Cast<CheckBox>().CurrentValue)
                {
                    var target = TargetManager.GetChampionTarget(Champion.GetAutoAttackRange(), DamageType.Physical);
                    if (target != null)
                        SpellManager.CastQ(Champion);
                }
            }
            else
            {
                if (MenuManager.ComboMenu["Ecombo"].Cast<CheckBox>().CurrentValue)
                {
                    var target = TargetManager.GetChampionTarget(SpellManager.E.Range, DamageType.Magical);
                    if (target != null)
                        SpellManager.CastE(target);
                }
                if (MenuManager.ComboMenu["Wcombo"].Cast<Slider>().CurrentValue > 0
                    && Champion.HealthPercent >= MenuManager.ComboMenu["Wcombo"].Cast<Slider>().CurrentValue)
                {
                    var target = TargetManager.GetChampionTarget(SpellManager.W.Range, DamageType.Magical);
                    if (target != null)
                        SpellManager.CastW(target);
                }
                if (MenuManager.ComboMenu["Rcombo"].Cast<CheckBox>().CurrentValue)
                {
                    var target = TargetManager.GetChampionTarget(SpellManager.R.Range, DamageType.Magical);
                    if (target != null)
                        SpellManager.CastR(target);
                }
            }
        }

        public static void ErwComboMode()
        {
            if (!MenuManager.ComboMenu["ComboM"].Cast<CheckBox>().CurrentValue) return;
            if (MenuManager.ComboMenu["ComboM"].Cast<CheckBox>().CurrentValue) return;
            if (MenuManager.ComboMenu["Pautos"].Cast<CheckBox>().CurrentValue && Champion.HasBuff("taricgemcraftbuff"))
            {
                var target = TargetManager.GetChampionTarget(Champion.GetAutoAttackRange(), DamageType.Physical);
                if (target != null)
                    if (Orbwalker.CanAutoAttack)
                        Orbwalker.ForcedTarget = target;
            }
            else if (MenuManager.ComboMenu["Pautos"].Cast<CheckBox>().CurrentValue && !Champion.HasBuff("taricgemcraftbuff"))
            {
                if (MenuManager.ComboMenu["Ecombo"].Cast<CheckBox>().CurrentValue)
                {
                    var target = TargetManager.GetChampionTarget(SpellManager.E.Range, DamageType.Magical);
                    if (target != null)
                        SpellManager.CastE(target);
                }
                if (MenuManager.ComboMenu["Rcombo"].Cast<CheckBox>().CurrentValue)
                {
                    var target = TargetManager.GetChampionTarget(SpellManager.R.Range, DamageType.Magical);
                    if (target != null)
                        SpellManager.CastR(target);
                }
                if (MenuManager.ComboMenu["Wcombo"].Cast<Slider>().CurrentValue > 0
                    && Champion.HealthPercent >= MenuManager.ComboMenu["Wcombo"].Cast<Slider>().CurrentValue)
                {
                    var target = TargetManager.GetChampionTarget(SpellManager.W.Range, DamageType.Magical);
                    if (target != null)
                        SpellManager.CastW(target);
                }
                if (MenuManager.ComboMenu["Qpassive"].Cast<CheckBox>().CurrentValue)
                {
                    var target = TargetManager.GetChampionTarget(Champion.GetAutoAttackRange(), DamageType.Physical);
                    if (target != null)
                        SpellManager.CastQ(Champion);
                }
            }
            else
            {
                if (MenuManager.ComboMenu["Ecombo"].Cast<CheckBox>().CurrentValue)
                {
                    var target = TargetManager.GetChampionTarget(SpellManager.E.Range, DamageType.Magical);
                    if (target != null)
                        SpellManager.CastE(target);
                }
                if (MenuManager.ComboMenu["Rcombo"].Cast<CheckBox>().CurrentValue)
                {
                    var target = TargetManager.GetChampionTarget(SpellManager.R.Range, DamageType.Magical);
                    if (target != null)
                        SpellManager.CastR(target);
                }
                if (MenuManager.ComboMenu["Wcombo"].Cast<Slider>().CurrentValue > 0
                    && Champion.HealthPercent >= MenuManager.ComboMenu["Wcombo"].Cast<Slider>().CurrentValue)
                {
                    var target = TargetManager.GetChampionTarget(SpellManager.W.Range, DamageType.Magical);
                    if (target != null)
                        SpellManager.CastW(target);
                }
            }
        }

        public static void HarassMode()
        {
            if (Champion.ManaPercent < MenuManager.HarassMenu["Harassmana"].Cast<Slider>().CurrentValue) return;
            if (MenuManager.HarassMenu["Wharass"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.W.Range, DamageType.Magical);
                if (target != null)
                    SpellManager.CastW(target);
            }
            if (MenuManager.HarassMenu["Eharass"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.E.Range, DamageType.Magical);
                if (target != null)
                    SpellManager.CastE(target);
            }
        }

        public static void HealingMode()
        {
            if (Champion.ManaPercent < MenuManager.HealingMenu["Healingmana"].Cast<Slider>().CurrentValue) return;
            var qhealally = MenuManager.HealingMenu["Qhealally"].Cast<Slider>().CurrentValue;
            var qhealtaric = MenuManager.HealingMenu["Qhealtaric"].Cast<Slider>().CurrentValue;
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
            if (MenuManager.SettingMenu["Interruptmode"].Cast<CheckBox>().CurrentValue) return;
            if (sender != null && MenuManager.SettingMenu["Einterrupt"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.E.Range, DamageType.Magical);
                if (target != null)
                    SpellManager.CastE(target);
            }
        }

        public static void GapCloserMode(Obj_AI_Base sender, Gapcloser.GapcloserEventArgs args)
        {
            if (MenuManager.SettingMenu["Gapcmode"].Cast<CheckBox>().CurrentValue) return;
            if (sender != null && MenuManager.SettingMenu["Egapc"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.E.Range, DamageType.Magical);
                if (target != null)
                    SpellManager.CastE(target);
            }
        }
    }
}
