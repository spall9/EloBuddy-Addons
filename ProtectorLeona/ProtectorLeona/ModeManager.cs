using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;

namespace ProtectorLeona
{
    internal class ModeManager
    {
        // Clone Character Object
        public static AIHeroClient Champion = Program.Champion;

        public static void ComboMode()
        {
            if (MenuManager.ComboUseE)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.E.Range, DamageType.Magical);
                if (target != null && !target.IsUnderHisturret())
                    SpellManager.CastE(target);
            }
            if (MenuManager.ComboUseW)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.W.Range, DamageType.Magical);
                if (target != null)
                    SpellManager.CastW(target);
            }
            if (MenuManager.ComboUseQ)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.Q.Range, DamageType.Magical);
                if (target != null && !target.HasBuff("leonazenithbladeroot"))
                {
                    AutoAttackMode(target);
                    SpellManager.CastQ(target);
                    Orbwalker.ResetAutoAttack();
                    AutoAttackMode(target);
                }
            }
            if (MenuManager.ComboUseR && Champion.CountEnemiesInRange(SpellManager.R.Range) >= MenuManager.ComboRLimiter)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.R.Range, DamageType.Magical);
                if (target != null && !target.IsUnderHisturret()
                    && !target.HasBuff("leonazenithbladeroot"))
                    SpellManager.CastR(target);
            }
        }

        public static void HarassMode()
        {
            if (Champion.ManaPercent < MenuManager.HarassMana) return;
            if (MenuManager.HarassUseE)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.E.Range, DamageType.Magical);
                if (target != null && !target.IsUnderHisturret())
                    SpellManager.CastE(target);
            }
            if (MenuManager.HarassUseQ)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.Q.Range, DamageType.Magical);
                if (target != null && !target.HasBuff("leonazenithbladeroot"))
                {
                    AutoAttackMode(target);
                    SpellManager.CastQ(target);
                    Orbwalker.ResetAutoAttack();
                    AutoAttackMode(target);
                }
            }
        }

        public static void InterruptMode(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {
            if (!MenuManager.InterrupterMode) return;
            if (sender != null && !sender.IsUnderEnemyturret())
            {
                if (MenuManager.InterrupterUseEq && args.DangerLevel < DangerLevel.High)
                {
                    var target = TargetManager.GetChampionTarget(SpellManager.E.Range, DamageType.Magical);
                    if (target != null && !target.IsUnderHisturret())
                    {
                        if (target.Distance(Champion) > Champion.GetAutoAttackRange(target))
                            SpellManager.CastE(target);
                        if (Champion.IsInAutoAttackRange(target))
                        {
                            AutoAttackMode(target);
                            SpellManager.CastQ(target);
                            Orbwalker.ResetAutoAttack();
                            AutoAttackMode(target);
                        }
                    }
                }
                if (MenuManager.InterrupterUseR && args.DangerLevel >= DangerLevel.High)
                {
                    var target = TargetManager.GetChampionTarget(SpellManager.E.Range, DamageType.Magical);
                    if (target != null)
                        SpellManager.CastR(target);
                }
            }
        }

        public static void GapCloserMode(Obj_AI_Base sender, Gapcloser.GapcloserEventArgs args)
        {
            if (!MenuManager.GapCloserMode) return;
            if (sender != null && !sender.IsUnderEnemyturret() && MenuManager.GapCloserUseEq)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.E.Range, DamageType.Magical);
                if (target != null && !target.IsUnderHisturret())
                {
                    if (target.Distance(Champion) > Champion.GetAutoAttackRange(target))
                        SpellManager.CastE(target);
                    if (Champion.IsInAutoAttackRange(target))
                    {
                        AutoAttackMode(target);
                        SpellManager.CastQ(target);
                        Orbwalker.ResetAutoAttack();
                        AutoAttackMode(target);
                    }
                }
            }
        }

        public static void AutoAttackMode(Obj_AI_Base target)
        {
            if (MenuManager.AutoAttack && Orbwalker.CanAutoAttack && Champion.IsInAutoAttackRange(target))
                Player.IssueOrder(GameObjectOrder.AutoAttack, target);
        }
    }
}
