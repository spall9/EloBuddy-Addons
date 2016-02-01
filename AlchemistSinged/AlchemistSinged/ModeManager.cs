using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;

namespace AlchemistSinged
{
    internal class ModeManager
    {
        // Clone Character Object
        public static AIHeroClient Champion = Program.Champion;

        public static void ComboMode()
        {
            if (MenuManager.ComboUseQ)
            {
                if (SpellManager.Toggle == 2) return;
                var target = TargetManager.GetChampionTarget(250, DamageType.Magical);
                if (target != null)
                    SpellManager.CastQ(target);
            }
            if (MenuManager.ComboUseE)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.E.Range, DamageType.Magical);
                if (target != null && SpellManager.E.IsReady())
                {
                    if (MenuManager.ComboUseW && SpellManager.W.IsReady())
                    {
                        var pos = Prediction.Position.PredictUnitPosition(target, SpellManager.W.CastDelay/2).Extend(Champion, 550).To3D();
                            SpellManager.W.Cast(pos);
                    }
                    SpellManager.CastE(target);
                }
            }
            if (MenuManager.ComboUseR && Champion.CountEnemiesInRange(SpellManager.W.Range) >= MenuManager.ComboRLimiter)
            {
                var target = TargetManager.GetChampionTarget(1000, DamageType.Magical);
                if (target != null && target.HasBuff("poisontrailtarget"))
                    SpellManager.CastR(Champion);
            }
        }

        public static void HarassMode()
        {
            if (MenuManager.HarassUseQ)
            {
                if (SpellManager.Toggle == 2) return;
                var target = TargetManager.GetChampionTarget(250, DamageType.Magical);
                if (target != null)
                    SpellManager.CastQ(target);
            }
        }

        public static void JungleMode()
        {
            if (MenuManager.JungleUseQ)
            {
                if (SpellManager.Toggle == 2) return;
                var target = TargetManager.GetMinionTarget(250, DamageType.Magical, false, true);
                if (target != null)
                    SpellManager.CastQ(target);
            }
            if (MenuManager.JungleUseE)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.E.Range, DamageType.Magical, false, true);
                if (target != null)
                    SpellManager.CastE(target);
            }
        }

        public static void LaneClearMode()
        {
            Orbwalker.DisableAttacking = MenuManager.LaneClearAaDisable;
            if (MenuManager.LaneClearUseQ)
            {
                if (SpellManager.Toggle == 2) return;
                var target = TargetManager.GetMinionTarget(250, DamageType.Magical);
                if (target != null)
                    SpellManager.CastQ(target);
            }
            if (MenuManager.LaneClearUseE)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.E.Range, DamageType.Magical);
                if (target != null)
                    SpellManager.CastE(target);
            }
        }

        public static void LastHitMode()
        {
            if (MenuManager.LastHitUseE)
            {
                var target = TargetManager.GetMinionTarget(SpellManager.E.Range, DamageType.Magical, false, false, false, SpellManager.EDamage());
                if (target != null)
                    SpellManager.CastE(target);
            }
        }

        public static void KiteMode()
        {
            var target = TargetManager.GetChampionTarget(1000, DamageType.Magical);
            if (target != null
                && !Champion.IsUnderEnemyturret() && !target.IsUnderEnemyturret()
                && !Champion.IsFacing(target) && target.IsFacing(Champion)
                && target.Distance(Champion) <= 750)
            {
                if (SpellManager.Toggle == 2) return;
                SpellManager.CastQ(target);
            }
        }

        public static void StackMode()
        {
            foreach (var item in Champion.InventoryItems)
            {
                if ((item.Id == ItemId.Tear_of_the_Goddess || item.Id == ItemId.Tear_of_the_Goddess_Crystal_Scar
                     || item.Id == ItemId.Archangels_Staff || item.Id == ItemId.Archangels_Staff_Crystal_Scar
                     || item.Id == ItemId.Manamune || item.Id == ItemId.Manamune_Crystal_Scar)
                    && item.Stacks < 750 && Champion.IsInShopRange())
                {
                    if (SpellManager.Toggle == 2) return;
                    SpellManager.CastQ(Champion);
                }
            }
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
            if (sender != null && args != null)
            {
                var spell = args.SpellName;
                if (spell == "TristanaR" || spell == "BlindMonkRKick" || spell == "AlZaharNetherGrasp" || spell == "GalioIdolOfDurand" || spell == "VayneCondemn" || spell == "JayceThunderingBlow" || spell == "Headbutt")
                    if (args.Target.IsMe)
                        SpellManager.CastE(sender);
            }
            if (sender != null && MenuManager.GapCloserUseE)
            {
                var target = TargetManager.GetChampionTarget(SpellManager.E.Range, DamageType.Magical);
                if (target != null)
                    SpellManager.CastE(target);
            }
        }
    }
}