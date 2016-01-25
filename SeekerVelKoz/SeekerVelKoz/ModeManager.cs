using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;

namespace SeekerVelKoz
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
                if (target != null && SpellManager.Q.Name == "VelkozQ")
                {
                    SpellManager.CastQ(target);
                }
            }
            if (MenuManager.ComboMenu["Ecombo"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.E.Range, DamageType.Magical);
                if (target != null && (target.HasBuffOfType(BuffType.Slow) || (target.Distance(Champion) < 200)))
                    SpellManager.CastE(target);
            }
            if (MenuManager.ComboMenu["Wcombo"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.W.Range, DamageType.Magical);
                if (target != null &&
                    (target.HasBuffOfType(BuffType.Knockback) || target.HasBuffOfType(BuffType.Knockup)
                    || target.HasBuffOfType(BuffType.Snare) || target.HasBuffOfType(BuffType.Stun)))
                    SpellManager.CastW(target);
            }
            if (MenuManager.ComboMenu["Rcombo"].Cast<CheckBox>().CurrentValue 
                && Champion.CountEnemiesInRange(SpellManager.R.Range) >= MenuManager.ComboMenu["Rlimiter"].Cast<Slider>().CurrentValue
                && !Champion.IsUnderTurret())
            {
                var target = TargetManager.GetChampionTarget(SpellManager.R.Range, DamageType.Magical);
                if (target != null && !Player.HasBuff("VelkozR") && target.Distance(Champion) >= SpellManager.R.Range * 0.5f
                    && target.Distance(Champion) <= SpellManager.R.Range - 100)
                    SpellManager.CastR(target);
            }
        }

        public static void HarassMode()
        {
            if (Champion.ManaPercent < MenuManager.HarassMenu["Harassmana"].Cast<Slider>().CurrentValue) return;
            if (MenuManager.HarassMenu["Qharass"].Cast<CheckBox>().CurrentValue && SpellManager.Q.Name == "VelkozQ")
            {
                var target = TargetManager.GetChampionTarget(SpellManager.Q.Range, DamageType.Magical);
                if (target != null)
                    SpellManager.CastQ(target);
            }
            if (MenuManager.HarassMenu["Eharass"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.E.Range, DamageType.Magical);
                if (target != null)
                    SpellManager.CastE(target);
            }
            if (MenuManager.HarassMenu["Wharass"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.W.Range, DamageType.Magical);
                if (target != null)
                    SpellManager.CastW(target);
            }
        }

        public static void JungleMode()
        {
            if (Champion.ManaPercent < MenuManager.JungleMenu["Junglemana"].Cast<Slider>().CurrentValue) return;
            if (MenuManager.JungleMenu["Qjungle"].Cast<CheckBox>().CurrentValue && SpellManager.Q.Name == "VelkozQ")
            {
                var target = TargetManager.GetMinionTarget(SpellManager.Q.Range, DamageType.Magical, false, true);
                if (target != null)
                    SpellManager.CastQ(target);
            }
            if (MenuManager.JungleMenu["Ejungle"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.E.Range, DamageType.Magical, false, true);
                if (target != null)
                    SpellManager.CastE(target);
            }
            if (MenuManager.JungleMenu["Wjungle"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.W.Range, DamageType.Magical, false, true);
                if (target != null)
                    SpellManager.CastW(target);
            }
        }

        public static void LaneClearMode()
        {
            if (Champion.ManaPercent < MenuManager.LaneClearMenu["Lanecmana"].Cast<Slider>().CurrentValue) return;
            if (MenuManager.LaneClearMenu["Qlanec"].Cast<CheckBox>().CurrentValue && SpellManager.Q.Name == "VelkozQ")
            {
                var target = TargetManager.GetMinionTarget(SpellManager.Q.Range, DamageType.Magical);
                if (target != null)
                    SpellManager.CastQ(target);
            }
            if (MenuManager.LaneClearMenu["Elanec"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.E.Range, DamageType.Magical);
                if (target != null)
                    SpellManager.CastE(target);
            }
            if (MenuManager.LaneClearMenu["Wlanec"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.W.Range, DamageType.Magical);
                if (target != null)
                    SpellManager.CastW(target);
            }
        }

        public static void LastHitMode()
        {
            if (Champion.ManaPercent < MenuManager.LastHitMenu["Lasthitmana"].Cast<Slider>().CurrentValue) return;
            if (MenuManager.LastHitMenu["Qlasthit"].Cast<CheckBox>().CurrentValue && SpellManager.Q.Name == "VelkozQ")
            {
                var target = TargetManager.GetMinionTarget(SpellManager.Q.Range, DamageType.Magical, false, false, SpellManager.QDamage());
                if (target != null)
                    SpellManager.CastQ(target);
            }
            if (MenuManager.LastHitMenu["Elasthit"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.E.Range, DamageType.Magical, false, false, SpellManager.EDamage());
                if (target != null)
                    SpellManager.CastE(target);
            }
            if (MenuManager.LastHitMenu["Wlasthit"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.W.Range, DamageType.Magical, false, false, SpellManager.WDamage());
                if (target != null)
                    SpellManager.CastW(target);
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
            if (MenuManager.KillStealMenu["Eks"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.E.Range, DamageType.Magical, false, SpellManager.EDamage());
                if (target != null)
                    SpellManager.CastE(target);
            }
            if (MenuManager.KillStealMenu["Wks"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.W.Range, DamageType.Magical, false, SpellManager.WDamage());
                if (target != null)
                    SpellManager.CastW(target);
            }
            if (MenuManager.KillStealMenu["Rks"].Cast<CheckBox>().CurrentValue && !Champion.IsUnderTurret())
            {
                var target = TargetManager.GetChampionTarget(SpellManager.R.Range, DamageType.Magical, false, SpellManager.RDamage());
                if (target != null && !Player.HasBuff("VelkozR"))
                    SpellManager.CastR(target);
            }
        }

        public static void UltFollowMode()
        {
            var target = TargetManager.GetChampionTarget(SpellManager.R.Range, DamageType.Magical);
            if (target != null)
            {
                Champion.Spellbook.UpdateChargeableSpell(SpellSlot.R, target.ServerPosition, false, false);
            }
            else
            {
                var mtarget = TargetManager.GetMinionTarget(SpellManager.R.Range, DamageType.Magical);
                if (mtarget != null)
                    Champion.Spellbook.UpdateChargeableSpell(SpellSlot.R, mtarget.ServerPosition, false, false);
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
                {
                    if (SpellManager.Q.Name == "VelkozQ")
                        SpellManager.CastQ(Champion);
                }
            }
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
