using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;

namespace ExecutionerUrgot
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
                var target = TargetManager.GetChampionTarget(SpellManager.Q.Range, DamageType.Physical);
                if (target != null)
                    SpellManager.CastQ(target);
            }
            if (MenuManager.ComboMenu["Wcombo"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.Q.Range, DamageType.Magical);
                if (target != null && !target.IsFacing(Champion))
                    SpellManager.CastW(target);
            }
            if (MenuManager.ComboMenu["Ecombo"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.E.Range, DamageType.Physical);
                if (target != null)
                    SpellManager.CastE(target);
            }
            if (MenuManager.ComboMenu["Rcombo"].Cast<Slider>().CurrentValue != 0 &&
                Champion.CountEnemiesInRange(SpellManager.R.Range) == MenuManager.ComboMenu["Rcombo"].Cast<Slider>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.R.Range, DamageType.Magical);
                if (target != null)
                    SpellManager.CastR(target);
            }
        }

        public static void HarassMode()
        {
            if (Champion.ManaPercent < MenuManager.LastHitMenu["Lasthitmana"].Cast<Slider>().CurrentValue) return;
            if (MenuManager.HarassMenu["Qharass"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.Q.Range, DamageType.Physical);
                if (target != null)
                    SpellManager.CastQ(target);
            }
            if (MenuManager.HarassMenu["Eharass"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.E.Range, DamageType.Physical);
                if (target != null)
                    SpellManager.CastE(target);
            }
        }

        public static void JungleMode()
        {
            if (Champion.ManaPercent < MenuManager.JungleMenu["Junglemana"].Cast<Slider>().CurrentValue) return;
            if (MenuManager.JungleMenu["Qjungle"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.Q.Range, DamageType.Physical, false, true);
                if (target != null)
                    SpellManager.CastQ(target);
            }
            if (MenuManager.JungleMenu["Ejungle"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.E.Range, DamageType.Physical, false, true);
                if (target != null)
                    SpellManager.CastE(target);
            }
        }

        public static void LaneClearMode()
        {
            if (Champion.ManaPercent < MenuManager.LaneClearMenu["Lanecmana"].Cast<Slider>().CurrentValue) return;
            if (MenuManager.LaneClearMenu["Qlanec"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.Q.Range, DamageType.Physical);
                if (target != null)
                    SpellManager.CastQ(target);
            }
            if (MenuManager.LaneClearMenu["Elanec"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.E.Range, DamageType.Physical);
                if (target != null)
                    SpellManager.CastE(target);
            }
        }

        public static void LastHitMode()
        {
            if (Champion.ManaPercent < MenuManager.LastHitMenu["Lasthitmana"].Cast<Slider>().CurrentValue) return;
            if (MenuManager.LastHitMenu["Qlasthit"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.Q.Range, DamageType.Physical, false, false, SpellManager.QDamage());
                if (target != null)
                    SpellManager.CastQ(target);
            }
        }

        public static void KsMode()
        {
            if (MenuManager.KillStealMenu["Qks"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.Q.Range, DamageType.Physical, false, SpellManager.QDamage());
                if (target != null)
                    SpellManager.CastQ(target);
            }
            if (MenuManager.KillStealMenu["Eks"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.E.Range, DamageType.Physical, false, SpellManager.QDamage());
                if (target != null)
                    SpellManager.CastE(target);
            }
        }

        public static void GrabMode()
        {
            if (!Champion.IsUnderHisturret()) return;
            var target = TargetManager.GetChampionTarget(SpellManager.R.Range, DamageType.Magical);
            if (target != null)
            {
                var turret = TargetManager.GetTurretTarget(Champion.GetAutoAttackRange(target), true);
                if (turret != null)
                {
                    Chat.Print("Found turret");
                    SpellManager.CastR(target);
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

        public static void InterruptMode(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {
            if (!MenuManager.SettingMenu["Interruptmode"].Cast<CheckBox>().CurrentValue) return;
            if (sender != null && MenuManager.SettingMenu["Rinterrupt"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.R.Range, DamageType.Magical);
                if (target != null)
                    SpellManager.CastR(target);
            }
        }

        public static void GapCloserMode(Obj_AI_Base sender, Gapcloser.GapcloserEventArgs args)
        {
            if (!MenuManager.SettingMenu["Gapcmode"].Cast<CheckBox>().CurrentValue) return;
            if (sender != null && MenuManager.SettingMenu["Rgapc"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.R.Range, DamageType.Magical);
                if (target != null)
                    SpellManager.CastR(target);
            }
        }
    }
}
