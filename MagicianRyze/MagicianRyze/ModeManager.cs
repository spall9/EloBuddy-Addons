using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;

namespace MagicianRyze
{
    internal class ModeManager
    {
        // Clone Character Object
        public static AIHeroClient Champion = Program.Champion;

        public static void Initialize()
        {
        }

        public static void CounterCombo()
        {
            if (Champion.HasBuff("RyzeR")
                && MenuManager.SettingMenu["UltM"].Cast<CheckBox>().CurrentValue)
            {
                UltimateMode(GameObjectType.AIHeroClient);
                return;
            }
            if (MenuManager.ComboMenu["Wcombo"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.W, DamageType.Magical);
                if (target != null)
                    SpellManager.CastW(target);
            }
            if (MenuManager.ComboMenu["Qcombo"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.Q, DamageType.Magical);
                if (target != null)
                    SpellManager.CastQ(target);
            }
            if (MenuManager.ComboMenu["Ecombo"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.E, DamageType.Magical);
                if (target != null)
                    SpellManager.CastE(target);
            }
            if (MenuManager.ComboMenu["Rcombo"].Cast<CheckBox>().CurrentValue)
            {
                if (Champion.HasBuff("ryzepassivecharged")
                    || Champion.GetBuffCount("ryzepassivestack") >= MenuManager.ComboMenu["Pult"].Cast<Slider>().CurrentValue)
                {
                    var target = TargetManager.GetChampionTarget(SpellManager.Q, DamageType.Magical);
                    if (target != null)
                        SpellManager.CastR(target);
                }
            }
        }

        public static void SluttyCombo()
        {
            if (Champion.HasBuff("RyzeR")
                && MenuManager.SettingMenu["UltM"].Cast<CheckBox>().CurrentValue)
            {
                UltimateMode(GameObjectType.AIHeroClient);
                return;
            }

            var target = TargetManager.GetChampionTarget(SpellManager.Q, DamageType.Magical);
            if (target != null)
            {
                var bcount = Champion.GetBuffCount("ryzepassivestack");

                if (!Champion.HasBuff("ryzepassivecharged") && bcount > 0)
                {
                    switch (bcount)
                    {
                        case 1:
                            SpellManager.CastQ(target);
                            SpellManager.CastE(target);
                            SpellManager.CastW(target);
                            SpellManager.CastR(target);
                            break;
                        case 2:
                            SpellManager.CastQ(target);
                            SpellManager.CastW(target);
                            SpellManager.CastE(target);
                            SpellManager.CastR(target);
                            break;
                        case 3:
                            SpellManager.CastQ(target);
                            SpellManager.CastE(target);
                            SpellManager.CastW(target);
                            SpellManager.CastR(target);
                            break;
                        case 4:
                            SpellManager.CastW(target);
                            SpellManager.CastQ(target);
                            SpellManager.CastE(target);
                            SpellManager.CastR(target);
                            break;
                    }
                }
                else
                {
                    SpellManager.CastW(target);
                    SpellManager.CastQ(target);
                    SpellManager.CastE(target);
                    SpellManager.CastR(target);
                }
            }

            if (target != null
                && (Champion.GetBuffCount("ryzepassivestack") == 4
                    || Champion.HasBuff("ryzepassivecharged")))
            {
                if (!SpellManager.Q.IsReady()
                    && !SpellManager.W.IsReady()
                    && !SpellManager.E.IsReady())
                {
                    SpellManager.CastR(target);
                }
            }
        }

        public static void HarassMode()
        {
            if (Champion.HasBuff("RyzeR")
                && MenuManager.SettingMenu["UltM"].Cast<CheckBox>().CurrentValue)
            {
                UltimateMode(GameObjectType.AIHeroClient);
                return;
            }
            if (Champion.ManaPercent < MenuManager.HarassMenu["Harassmana"].Cast<Slider>().CurrentValue) return;
            if (MenuManager.HarassMenu["Qharass"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.Q, DamageType.Magical);
                if (target != null)
                    SpellManager.CastQ(target);
            }
        }

        public static void JungleMode()
        {
            if (Champion.HasBuff("RyzeR")
                && MenuManager.SettingMenu["UltM"].Cast<CheckBox>().CurrentValue)
            {
                UltimateMode(GameObjectType.obj_AI_Minion, true);
                return;
            }
            if (Champion.ManaPercent < MenuManager.JungleMenu["Junglemana"].Cast<Slider>().CurrentValue) return;
            if (MenuManager.JungleMenu["Qjungle"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.Q, DamageType.Magical, false, true);
                if (target != null)
                    SpellManager.CastQ(target);
            }
            if (MenuManager.JungleMenu["Wjungle"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.W, DamageType.Magical, false, true);
                if (target != null)
                    SpellManager.CastW(target);
            }
            if (MenuManager.JungleMenu["Ejungle"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.E, DamageType.Magical, false, true);
                if (target != null)
                    SpellManager.CastE(target);
            }
        }

        public static void LaneClearMode()
        {
            if (Champion.HasBuff("RyzeR")
                && MenuManager.SettingMenu["UltM"].Cast<CheckBox>().CurrentValue)
            {
                UltimateMode(GameObjectType.obj_AI_Minion);
                return;
            }
            if (Champion.ManaPercent < MenuManager.LaneClearMenu["Lanecmana"].Cast<Slider>().CurrentValue) return;
            if (MenuManager.LaneClearMenu["Qlanec"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.Q, DamageType.Magical);
                if (target != null)
                    SpellManager.CastQ(target);
            }
            if (MenuManager.LaneClearMenu["Wlanec"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.W, DamageType.Magical);
                if (target != null)
                    SpellManager.CastW(target);
            }
            if (MenuManager.LaneClearMenu["Elanec"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.E, DamageType.Magical);
                if (target != null)
                    SpellManager.CastE(target);
            }
        }

        public static void LastHitMode()
        {
            if (Champion.HasBuff("RyzeR")
                && MenuManager.SettingMenu["UltM"].Cast<CheckBox>().CurrentValue)
            {
                UltimateMode(GameObjectType.obj_AI_Minion);
                return;
            }
            if (Champion.ManaPercent < MenuManager.LastHitMenu["Lasthitmana"].Cast<Slider>().CurrentValue) return;
            if (MenuManager.LastHitMenu["Qlasthit"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.Q, DamageType.Magical, false, false, SpellManager.QDamage());
                if (target != null)
                    SpellManager.CastQ(target);
            }
            if (MenuManager.LastHitMenu["Wlasthit"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.W, DamageType.Magical, false, false, SpellManager.QDamage());
                if (target != null)
                    SpellManager.CastW(target);
            }
            if (MenuManager.LastHitMenu["Elasthit"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.E, DamageType.Magical, false, false, SpellManager.QDamage());
                if (target != null)
                    SpellManager.CastE(target);
            }
        }

        public static void UltimateMode(GameObjectType type, bool isMonster = false)
        {
            switch (type)
            {
                case GameObjectType.AIHeroClient:
                {
                    if (SpellManager.Q.IsReady())
                    {
                        var target = TargetManager.GetChampionTarget(SpellManager.Q, DamageType.Magical);
                        if (target != null)
                            SpellManager.CastQ(target);
                    }
                    if (SpellManager.W.IsReady())
                    {
                        var target = TargetManager.GetChampionTarget(SpellManager.W, DamageType.Magical);
                        if (target != null)
                            SpellManager.CastW(target);
                    }
                    if (SpellManager.E.IsReady())
                    {
                        var target = TargetManager.GetChampionTarget(SpellManager.E, DamageType.Magical);
                        if (target != null)
                            SpellManager.CastE(target);
                    }
                    break;
                }
                case GameObjectType.obj_AI_Minion:
                {
                    if (isMonster)
                    {
                        if (SpellManager.Q.IsReady())
                        {
                            var target = TargetManager.GetMinionTarget(SpellManager.Q, DamageType.Magical, false, true);
                            if (target != null)
                                SpellManager.CastQ(target);
                        }
                        if (SpellManager.W.IsReady())
                        {
                            var target = TargetManager.GetMinionTarget(SpellManager.W, DamageType.Magical, false, true);
                            if (target != null)
                                SpellManager.CastW(target);
                        }
                        if (SpellManager.E.IsReady())
                        {
                            var target = TargetManager.GetMinionTarget(SpellManager.E, DamageType.Magical, false, true);
                            if (target != null)
                                SpellManager.CastE(target);
                        }
                    }
                    else
                    {
                        if (SpellManager.Q.IsReady())
                        {
                            var target = TargetManager.GetMinionTarget(SpellManager.Q, DamageType.Magical);
                            if (target != null)
                                SpellManager.CastQ(target);
                        }
                        if (SpellManager.W.IsReady())
                        {
                            var target = TargetManager.GetMinionTarget(SpellManager.W, DamageType.Magical);
                            if (target != null)
                                SpellManager.CastW(target);
                        }
                        if (SpellManager.E.IsReady())
                        {
                            var target = TargetManager.GetMinionTarget(SpellManager.E, DamageType.Magical);
                            if (target != null)
                                SpellManager.CastE(target);
                        }
                    }
                    break;
                }
            }
        }

        public static void KsMode()
        {
            if (MenuManager.KillStealMenu["Qks"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.Q, DamageType.Magical, false, SpellManager.QDamage());
                if (target != null)
                    SpellManager.CastQ(target);
            }
            if (MenuManager.KillStealMenu["Wks"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.W, DamageType.Magical, false, SpellManager.QDamage());
                if (target != null)
                    SpellManager.CastW(target);
            }
            if (MenuManager.KillStealMenu["Eks"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.E, DamageType.Magical, false, SpellManager.QDamage());
                if (target != null)
                    SpellManager.CastE(target);
            }
        }

        public static void StackMode()
        {
            foreach (var item in Champion.InventoryItems)
            {
                if ((item.Id == ItemId.Tear_of_the_Goddess || item.Id == ItemId.Tear_of_the_Goddess_Crystal_Scar ||
                     item.Id == ItemId.Archangels_Staff || item.Id == ItemId.Archangels_Staff_Crystal_Scar ||
                     item.Id == ItemId.Manamune || item.Id == ItemId.Manamune_Crystal_Scar) && item.Stacks < 750 &&
                    Champion.IsInShopRange() && SpellManager.Q.IsReady())
                    SpellManager.Q.Cast(Champion);
            }
        }

        public static void InterruptMode(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {
            if (!MenuManager.SettingMenu["Interruptmode"].Cast<CheckBox>().CurrentValue) return;
            if (sender != null && MenuManager.SettingMenu["Winterrupt"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.W, DamageType.Magical);
                if (target != null)
                    SpellManager.CastW(target);
            }
        }

        public static void GapCloserMode(Obj_AI_Base sender, Gapcloser.GapcloserEventArgs args)
        {
            if (!MenuManager.SettingMenu["Gapcmode"].Cast<CheckBox>().CurrentValue) return;
            if (sender != null && MenuManager.SettingMenu["Wgapc"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.W, DamageType.Magical);
                if (target != null)
                    SpellManager.CastW(target);
            }
        }
    }
}
