using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;

namespace AlchemistSinged
{
    internal class ModeManager
    {
        // Clone Character Object
        public static AIHeroClient Champion = Program.Champion;
        public static bool isStackingTear = false;

        public static void Initialize()
        {
        }

        public static void ComboMode()
        {
            if (MenuManager.ComboMenu["Qcombo"].Cast<CheckBox>().CurrentValue
                && Player.GetSpell(SpellSlot.Q).ToggleState == 1)
            {
                var target = TargetManager.GetChampionTarget(250, DamageType.Magical);
                if (target != null)
                    SpellManager.CastQ(target);
            }
            if (MenuManager.ComboMenu["Ecombo"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.E.Range, DamageType.Magical);
                if (target != null)
                {
                    if (MenuManager.ComboMenu["Wcombo"].Cast<CheckBox>().CurrentValue)
                    {
                        var pos = Prediction.Position.PredictUnitPosition(target, SpellManager.W.CastDelay/2)
                                .Extend(Champion, 515 + Champion.Distance(target.Position)).To3D();
                        if (SpellManager.W.IsReady())
                            SpellManager.W.Cast(pos);
                    }
                    SpellManager.CastE(target);
                }
            }
            if ((MenuManager.ComboMenu["Rcombo"].Cast<CheckBox>().CurrentValue
                 && Champion.CountEnemiesInRange(SpellManager.W.Range) >= MenuManager.ComboMenu["Rlimiter"].Cast<Slider>().CurrentValue))
            {
                var target = TargetManager.GetChampionTarget(1000, DamageType.Magical);
                if (target != null && Champion.IsMoving && target.HasBuff("poisontrailtarget"))
                    SpellManager.CastR(Champion);
            }
        }

        public static void HarassMode()
        {
            if (MenuManager.HarassMenu["Qharass"].Cast<CheckBox>().CurrentValue
                && Player.GetSpell(SpellSlot.Q).ToggleState == 1)
            {
                var target = TargetManager.GetChampionTarget(250, DamageType.Magical);
                if (target != null)
                    SpellManager.CastQ(target);
            }
            if (MenuManager.HarassMenu["Eharass"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.E.Range, DamageType.Magical);
                if (target != null)
                    SpellManager.CastE(target);
            }
        }

        public static void JungleMode()
        {
            if (MenuManager.JungleMenu["Qjungle"].Cast<CheckBox>().CurrentValue
                && Player.GetSpell(SpellSlot.Q).ToggleState == 1)
            {
                var target = TargetManager.GetMinionTarget(250, DamageType.Magical, false, true);
                if (target != null)
                    SpellManager.CastQ(target);
            }
            if (MenuManager.JungleMenu["Ejungle"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.E.Range, DamageType.Magical, false, true);
                if (target != null)
                    SpellManager.CastE(target);
                if (target != null)
                {
                    if (MenuManager.JungleMenu["Wjungle"].Cast<CheckBox>().CurrentValue)
                    {
                        var pos = Prediction.Position.PredictUnitPosition(target, SpellManager.W.CastDelay / 2)
                                .Extend(Champion, 515 + Champion.Distance(target.Position)).To3D();
                        if (SpellManager.W.IsReady())
                            SpellManager.W.Cast(pos);
                    }
                    SpellManager.CastE(target);
                }
            }
        }

        public static void LaneClearMode()
        {
            Orbwalker.DisableAttacking = MenuManager.LaneClearMenu["AAdisable"].Cast<CheckBox>().CurrentValue;
            if (MenuManager.LaneClearMenu["Qlanec"].Cast<CheckBox>().CurrentValue
                && Player.GetSpell(SpellSlot.Q).ToggleState == 1)
            {
                var target = TargetManager.GetMinionTarget(250, DamageType.Magical);
                if (target != null)
                    SpellManager.CastQ(target);
            }
            if (MenuManager.LaneClearMenu["Elanec"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.E.Range, DamageType.Magical);
                if (target != null)
                    SpellManager.CastE(target);
            }
        }

        public static void LastHitMode()
        {
            if (MenuManager.LaneClearMenu["Elanec"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.E.Range, DamageType.Magical, false, false, SpellManager.EDamage());
                if (target != null)
                    SpellManager.CastE(target);
            }
        }

        public static void KiteMode()
        {
            var target = TargetManager.GetChampionTarget(1000, DamageType.Magical);
            if (target != null && !Champion.IsFacing(target) && target.IsFacing(Champion)
                && Player.GetSpell(SpellSlot.Q).ToggleState == 1)
                SpellManager.CastQ(target);
        }

        public static void StackMode()
        {
            foreach (var item in Champion.InventoryItems)
            {
                if ((item.Id == ItemId.Tear_of_the_Goddess || item.Id == ItemId.Tear_of_the_Goddess_Crystal_Scar
                    || item.Id == ItemId.Archangels_Staff || item.Id == ItemId.Archangels_Staff_Crystal_Scar
                    || item.Id == ItemId.Manamune || item.Id == ItemId.Manamune_Crystal_Scar)
                    && item.Stacks < 750 && Champion.IsInShopRange()
                    && Player.GetSpell(SpellSlot.Q).ToggleState == 1)
                {
                    isStackingTear = true;
                    SpellManager.CastQ(Champion);
                }
            }
        }

        public static void InterruptMode(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {
            if (!MenuManager.SettingMenu["Interruptmode"].Cast<CheckBox>().CurrentValue) return;
            if (sender != null && MenuManager.SettingMenu["Einterrupt"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(Champion.GetAutoAttackRange(), DamageType.Magical);
                if (target != null)
                    SpellManager.CastE(target);
            }
        }

        public static void GapCloserMode(Obj_AI_Base sender, Gapcloser.GapcloserEventArgs args)
        {
            if (!MenuManager.SettingMenu["Gapcmode"].Cast<CheckBox>().CurrentValue) return;
            if (sender != null && MenuManager.SettingMenu["Egapc"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(Champion.GetAutoAttackRange(), DamageType.Magical);
                if (target != null)
                    SpellManager.CastE(target);
            }
        }
    }
}