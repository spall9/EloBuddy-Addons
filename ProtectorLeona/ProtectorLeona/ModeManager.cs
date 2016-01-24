using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;

namespace ProtectorLeona
{
    internal class ModeManager
    {
        // Clone Character Object
        public static AIHeroClient Champion = Program.Champion;

        public static void Initialize()
        {
        }

        public static void ComboMode()
        {
            if (MenuManager.SettingMenu["Aattack"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(Champion.GetAutoAttackRange(), DamageType.Physical);
                if (target != null && Orbwalker.CanAutoAttack)
                    Player.IssueOrder(GameObjectOrder.AttackTo, target);
            }
            if (MenuManager.ComboMenu["Ecombo"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.E.Range, DamageType.Magical);
                if (target != null && !target.IsUnderHisturret())
                    SpellManager.CastE(target);
            }
            if (MenuManager.ComboMenu["Wcombo"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.W.Range, DamageType.Magical);
                if (target != null)
                    SpellManager.CastW(target);
            }
            if (MenuManager.ComboMenu["Qcombo"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.Q.Range, DamageType.Magical);
                if (target != null && !target.HasBuff("leonazenithbladeroot"))
                    SpellManager.CastQ(target);
                Orbwalker.ResetAutoAttack();
            }
            if (MenuManager.ComboMenu["Rcombo"].Cast<CheckBox>().CurrentValue
                && Champion.CountEnemiesInRange(SpellManager.R.Range) >= MenuManager.ComboMenu["Rlimiter"].Cast<Slider>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.R.Range, DamageType.Magical);
                if (target != null && !target.IsUnderEnemyturret())
                    SpellManager.CastR(target);
            }
        }

        public static void HarassMode()
        {
            if (Champion.ManaPercent < MenuManager.HarassMenu["Harassmana"].Cast<Slider>().CurrentValue) return;
            if (MenuManager.SettingMenu["Aattack"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(Champion.GetAutoAttackRange(), DamageType.Physical);
                if (target != null && Orbwalker.CanAutoAttack)
                    Player.IssueOrder(GameObjectOrder.AttackTo, target);
            }
            if (MenuManager.HarassMenu["Ecombo"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.E.Range, DamageType.Magical);
                if (target != null && !target.IsUnderHisturret())
                    SpellManager.CastE(target);
            }
            if (MenuManager.HarassMenu["Qharass"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.Q.Range, DamageType.Magical);
                if (target != null && !target.HasBuff("leonazenithbladeroot"))
                    SpellManager.CastQ(target);
                Orbwalker.ResetAutoAttack();
            }
        }

        public static void InterruptMode(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {
            if (!MenuManager.SettingMenu["Interruptmode"].Cast<CheckBox>().CurrentValue) return;
            if (sender != null && !sender.IsUnderEnemyturret())
            {
                if (MenuManager.SettingMenu["EQinterrupt"].Cast<CheckBox>().CurrentValue
                    && args.DangerLevel < DangerLevel.High)
                {
                    var target = TargetManager.GetChampionTarget(SpellManager.E.Range, DamageType.Magical);
                    if (target != null)
                    {
                        SpellManager.CastE(target);
                        if (Orbwalker.CanAutoAttack)
                            Orbwalker.ForcedTarget = target;
                        if (!target.HasBuff("leonazenithbladeroot")
                            && Champion.IsInAutoAttackRange(target))
                        {
                            SpellManager.CastQ(target);
                            Orbwalker.ResetAutoAttack();
                            if (Orbwalker.CanAutoAttack)
                                Player.IssueOrder(GameObjectOrder.AttackTo, target);
                        }
                    }
                }
                if (MenuManager.SettingMenu["Rinterrupt"].Cast<CheckBox>().CurrentValue
                    && args.DangerLevel >= DangerLevel.High)
                {
                    var target = TargetManager.GetChampionTarget(SpellManager.E.Range, DamageType.Magical);
                    if (target != null)
                    {
                        SpellManager.CastR(target);
                    }
                }
            }
        }

        public static void GapCloserMode(Obj_AI_Base sender, Gapcloser.GapcloserEventArgs args)
        {
            if (!MenuManager.SettingMenu["Gapcmode"].Cast<CheckBox>().CurrentValue) return;
            if (sender != null && !sender.IsUnderEnemyturret()
                && MenuManager.SettingMenu["EQgapc"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.E.Range, DamageType.Magical);
                if (target != null && !target.IsUnderHisturret())
                {
                    SpellManager.CastE(target);
                    if (Orbwalker.CanAutoAttack)
                        Player.IssueOrder(GameObjectOrder.AttackTo, target);
                    if (!target.HasBuff("leonazenithbladeroot")
                        && Champion.IsInAutoAttackRange(target))
                    {
                        SpellManager.CastQ(target);
                        if (Orbwalker.CanAutoAttack)
                        {
                            Orbwalker.ResetAutoAttack();
                            Player.IssueOrder(GameObjectOrder.AttackTo, target);
                        }
                    }
                }
            }
        }
    }
}
