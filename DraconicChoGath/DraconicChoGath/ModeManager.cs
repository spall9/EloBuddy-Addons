using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;

namespace DraconicChoGath
{
    internal class ModeManager
    {
        // Clone Character Object
        public static AIHeroClient Champion = Program.Champion;

        public static void ComboMode()
        {
            if (MenuManager.ComboUseQ)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.Q.Range, DamageType.Magical);
                if (target != null)
                    SpellManager.CastQ(target);
            }
            if (MenuManager.ComboUseW)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.W.Range, DamageType.Magical);
                if (target != null)
                    SpellManager.CastW(target);
            }
            if (MenuManager.ComboUseR)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.R.Range, DamageType.True);
                if (target != null)
                    SpellManager.CastR(target);
            }
        }

        public static void HarassMode()
        {
            if (MenuManager.HarassUseQ)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.Q.Range, DamageType.Magical);
                if (target != null)
                    SpellManager.CastQ(target);
            }
            if (MenuManager.HarassUseW)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.W.Range, DamageType.Magical);
                if (target != null)
                    SpellManager.CastW(target);
            }
        }

        public static void JungleMode()
        {
            if (MenuManager.JungleUseQ)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.Q.Range, DamageType.Magical, false, true);
                if (target != null)
                    SpellManager.CastQ(target);
            }
            if (MenuManager.JungleUseW)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.W.Range, DamageType.Magical, false, true);
                if (target != null)
                    SpellManager.CastW(target);
            }
            if (MenuManager.JungleUseR)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.R.Range, DamageType.True, false, true, false, SpellManager.RMonsterDamage());
                if (target != null && CheckLargeMonster(target))
                    SpellManager.CastR(target);
            }
            if (MenuManager.JungleUseSteal)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.FlashUltRange(), DamageType.True, false, true, false, SpellManager.RMonsterDamage());
                if (target != null && SpellManager.R.IsReady() && CheckLargeMonster(target)
                    && Champion.CountEnemiesInRange(SpellManager.FlashUltRange()) > 0)
                {
                    SpellManager.CastFlash(target);
                    SpellManager.CastR(target);
                }
            }
        }

        public static void LaneClearMode()
        {
            if (MenuManager.LaneClearUseQ)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.Q.Range, DamageType.Magical);
                if (target != null)
                    SpellManager.CastQ(target);
            }
            if (MenuManager.LaneClearUseW)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.W.Range, DamageType.Magical);
                if (target != null)
                    SpellManager.CastW(target);
            }
        }

        public static void LastHitMode()
        {
            if (MenuManager.LastHitUseQ)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.Q.Range, DamageType.Magical, false, false, false, SpellManager.QDamage());
                if (target != null)
                    SpellManager.CastQ(target);
            }
            if (MenuManager.LastHitUseW)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.W.Range, DamageType.Magical, false, false, false, SpellManager.WDamage());
                if (target != null)
                    SpellManager.CastW(target);
            }
            if (MenuManager.LastHitUseR)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.R.Range, DamageType.True, false, false, false, SpellManager.RMonsterDamage());
                if (target != null)
                    SpellManager.CastR(target);
            }
        }

        public static void KsMode()
        {
            if (MenuManager.KsUseQ)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.Q.Range, DamageType.Magical, false, true, SpellManager.QDamage());
                if (target != null)
                    SpellManager.CastQ(target);
            }
            if (MenuManager.KsUseW)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.W.Range, DamageType.Magical, false, false, SpellManager.WDamage());
                if (target != null)
                    SpellManager.CastW(target);
            }
            if (MenuManager.KsUseR)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.R.Range, DamageType.True, false, false, SpellManager.RDamage());
                if (target != null)
                    SpellManager.CastR(target);
            }
            if (MenuManager.KsUseFlashR)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.FlashUltRange(), DamageType.True, false, false, SpellManager.RDamage());
                if (target != null && SpellManager.R.IsReady()
                    && target.Distance(Champion) > SpellManager.R.Range)
                {
                    SpellManager.CastFlash(target);
                    SpellManager.CastR(target);
                }
            }
        }

        public static void StackMode()
        {
            foreach (var item in Champion.InventoryItems)
            {
                if ((item.Id == ItemId.Tear_of_the_Goddess || item.Id == ItemId.Archangels_Staff
                    || item.Id == ItemId.Manamune) && Champion.IsInShopRange())
                {
                    if ((int)(Game.Time - SpellManager.StackerStamp) >= 2)
                    {
                        SpellManager.CastQ(Champion);
                        SpellManager.StackerStamp = Game.Time;
                    }
                }
            }
        }

        public static void InterruptMode(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {
            if (!MenuManager.InterrupterMode) return;
            if (sender != null && MenuManager.InterrupterUseQ)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.Q.Range, DamageType.Magical);
                if (target != null)
                    SpellManager.CastQ(target);
            }
        }

        public static void GapCloserMode(Obj_AI_Base sender, Gapcloser.GapcloserEventArgs args)
        {
            if (!MenuManager.GapCloserMode) return;
            if (sender != null && MenuManager.GapCloserUseQ)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.Q.Range, DamageType.Magical);
                if (target != null)
                    SpellManager.CastQ(target);
            }
        }

        public static bool CheckLargeMonster(Obj_AI_Base target)
        {
            return (target.BaseSkinName.Contains("SRU_Blue") && !target.BaseSkinName.Contains("Mini"))
                || (target.BaseSkinName.Contains("SRU_Red") && !target.BaseSkinName.Contains("Mini"))
                || target.BaseSkinName.Contains("SRU_Dragon") || target.BaseSkinName.Contains("TT_Spiderboss")
                || target.BaseSkinName.Contains("SRU_RiftHerald") || target.BaseSkinName.Contains("SRU_Baron");
        }
    }
}