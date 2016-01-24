using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;

namespace BallistaKogMaw
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
            if (MenuManager.ComboMenu["Qcombo"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.Q.Range, DamageType.Magical);
                if (target != null)
                    SpellManager.CastQ(target);
            }
            if (MenuManager.ComboMenu["Wcombo"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.W.Range, DamageType.Mixed);
                if (target != null)
                    SpellManager.CastW(target);
            }
            if (MenuManager.ComboMenu["Ecombo"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.E.Range, DamageType.Magical);
                if (target != null)
                    SpellManager.CastE(target);
            }
            if (MenuManager.ComboMenu["Rcombo"].Cast<CheckBox>().CurrentValue
                && Champion.GetBuffCount("kogmawlivingartillerycost") <= MenuManager.ComboMenu["Ultcombo"].Cast<Slider>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.R.Range, DamageType.Mixed);
                if (target != null)
                    SpellManager.CastR(target);
            }
        }

        public static void HarassMode()
        {
            if (Program.Champion.ManaPercent < MenuManager.HarassMenu["Harassmana"].Cast<Slider>().CurrentValue) return;
            if (MenuManager.HarassMenu["Qharass"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.Q.Range, DamageType.Magical);
                if (target != null)
                    SpellManager.CastQ(target);
            }
            if (MenuManager.HarassMenu["Wharass"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.W.Range, DamageType.Mixed);
                if (target != null)
                    SpellManager.CastW(target);
            }
            if (MenuManager.HarassMenu["Eharass"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.E.Range, DamageType.Magical);
                if (target != null)
                    SpellManager.CastE(target);
            }
            if (MenuManager.HarassMenu["Rharass"].Cast<CheckBox>().CurrentValue
                && Champion.GetBuffCount("kogmawlivingartillerycost") <= MenuManager.HarassMenu["Ultharass"].Cast<Slider>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.R.Range, DamageType.Mixed);
                if (target != null)
                    SpellManager.CastR(target);
            }
        }

        public static void JungleMode()
        {
            if (Program.Champion.ManaPercent < MenuManager.JungleMenu["Junglemana"].Cast<Slider>().CurrentValue) return;
            if (MenuManager.JungleMenu["Qjungle"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.Q.Range, DamageType.Magical, false, true);
                if (target != null)
                    SpellManager.CastQ(target);
            }
            if (MenuManager.JungleMenu["Wjungle"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.W.Range, DamageType.Magical, false, true);
                if (target != null)
                    SpellManager.CastW(target);
            }
            if (MenuManager.JungleMenu["Ejungle"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.E.Range, DamageType.Magical, false, true);
                if (target != null)
                    SpellManager.CastE(target);
            }
            if (MenuManager.JungleMenu["Rjungle"].Cast<CheckBox>().CurrentValue
                && Champion.GetBuffCount("kogmawlivingartillerycost") <= MenuManager.JungleMenu["Ultjungle"].Cast<Slider>().CurrentValue)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.R.Range, DamageType.Mixed, false, true);
                if (target != null)
                    SpellManager.CastR(target);
            }
        }

        public static void LaneClearMode()
        {
            if (Program.Champion.ManaPercent < MenuManager.LaneClearMenu["Lanecmana"].Cast<Slider>().CurrentValue) return;
            if (MenuManager.LaneClearMenu["Qlanec"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.Q.Range, DamageType.Magical);
                if (target != null)
                    SpellManager.CastQ(target);
            }
            if (MenuManager.LaneClearMenu["Wlanec"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.W.Range, DamageType.Magical);
                if (target != null)
                    SpellManager.CastW(target);
            }
            if (MenuManager.LaneClearMenu["Elanec"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.E.Range, DamageType.Magical);
                if (target != null)
                    SpellManager.CastE(target);
            }
            if (MenuManager.LaneClearMenu["Rlanec"].Cast<CheckBox>().CurrentValue
                && Champion.GetBuffCount("kogmawlivingartillerycost") <= MenuManager.LaneClearMenu["Ultlanec"].Cast<Slider>().CurrentValue)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.R.Range, DamageType.Mixed);
                if (target != null)
                    SpellManager.CastR(target);
            }
        }

        public static void LastHitMode()
        {
            if (Program.Champion.ManaPercent < MenuManager.LastHitMenu["Lasthitmana"].Cast<Slider>().CurrentValue) return;
            if (MenuManager.LastHitMenu["Qlasthit"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.Q.Range, DamageType.Magical, false, false, SpellManager.QDamage());
                if (target != null)
                    SpellManager.CastQ(target);
            }
            if (MenuManager.LastHitMenu["Wlasthit"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.W.Range, DamageType.Magical, false, false, SpellManager.WDamage());
                if (target != null)
                    SpellManager.CastW(target);
            }
            if (MenuManager.LastHitMenu["Rlasthit"].Cast<CheckBox>().CurrentValue
                && Champion.GetBuffCount("kogmawlivingartillerycost") <= MenuManager.LastHitMenu["Ultlasthit"].Cast<Slider>().CurrentValue)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.R.Range, DamageType.Mixed, false, false, SpellManager.RDamage());
                if (target != null)
                    SpellManager.CastR(target);
            }
        }

        public static void KsMode()
        {
            if (MenuManager.KillStealMenu["Qks"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.Q.Range, DamageType.Magical, false, SpellManager.QDamage());
                if (target != null)
                    SpellManager.CastQ(target);
            }
            if (MenuManager.KillStealMenu["Wks"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.W.Range, DamageType.Mixed, false, SpellManager.WDamage());
                if (target != null)
                    SpellManager.CastW(target);
            }
            if (MenuManager.KillStealMenu["Eks"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.E.Range, DamageType.Magical, false, SpellManager.EDamage());
                if (target != null)
                    SpellManager.CastE(target);
            }
            if (MenuManager.KillStealMenu["Rks"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.R.Range, DamageType.Mixed, false, SpellManager.RDamage());
                if (target != null)
                    SpellManager.CastR(target);
            }
        }

        public static void DeathFollowMode()
        {
            if (Program.Ptarget != null)
            {
                Player.IssueOrder(GameObjectOrder.MoveTo, Program.Ptarget.ServerPosition - 100);
            }
            else
            {
                var kstarget = TargetManager.GetChampionTarget(SpellManager.Q.Range, DamageType.True, false, SpellManager.PDamage());

                if (kstarget != null)
                    Program.Ptarget = kstarget;
                else
                {
                    var ksminion = TargetManager.GetMinionTarget(SpellManager.Q.Range, DamageType.True, false, false, SpellManager.PDamage());
                    if (ksminion != null)
                        Program.Ptarget = ksminion;
                    else
                    {
                        var minion = TargetManager.GetMinionTarget(SpellManager.Q.Range, DamageType.True);
                        if (minion != null)
                            Program.Ptarget = minion;
                        else
                        {
                            var target = TargetManager.GetChampionTarget(SpellManager.Q.Range, DamageType.True);
                            if (target != null)
                                Program.Ptarget = target;
                        }
                    }
                }
            }
        }

        public static void StackMode()
        {
            foreach (var item in Champion.InventoryItems)
            {
                if ((item.Id == ItemId.Tear_of_the_Goddess || item.Id == ItemId.Tear_of_the_Goddess_Crystal_Scar ||
                     item.Id == ItemId.Archangels_Staff || item.Id == ItemId.Archangels_Staff_Crystal_Scar ||
                     item.Id == ItemId.Manamune || item.Id == ItemId.Manamune_Crystal_Scar) && item.Stacks < 750 &&
                    Champion.IsInShopRange())
                    SpellManager.CastQ(Champion);
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