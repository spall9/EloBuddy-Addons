using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AlchemistSinged
{
    class Calculations
    {
        // Declare Spells
        public static Spell.Active Q { get; set; }
        public static Spell.Skillshot W { get; set; }
        public static Spell.Targeted E { get; set; }
        public static Spell.Active R { get; set; }

        public static float poisontime = 0;

        // Initialize method
        public static void Initialize()
        {
            Q = new Spell.Active(SpellSlot.Q);
            W = new Spell.Skillshot(SpellSlot.W, 1000, SkillShotType.Circular, 500, 700, 350)
            {
                AllowedCollisionCount = int.MaxValue
            };
            E = new Spell.Targeted(SpellSlot.E, 125);
            R = new Spell.Active(SpellSlot.R);
        }

        // TOGGLE: Singed leaves a trail of poison behind him that lasts for 3.25 seconds.
        // Enemies caught in the path are poisoned for 2 seconds and dealt magic damage
        // each 0.25 seconds for the duration. Continual exposure renews the poison.
        public static float QDamage()
        {
            return (20 + (24 * Q.Level)) + (0.6f * Program.Champion.FlatMagicDamageMod);
        }

        // ACTIVE: Singed flings the target enemy over his shoulder, dealing them magic damage that is capped against minions and monsters.
        // If the target lands onto his Mega Adhesive, they are temporarily rooted.
        public static float EDamage(Obj_AI_Base target)
        {
            return (35 + (15 * E.Level)) + (target.MaxHealth * (5.5f + (0.5f * E.Level))) + (0.75f * Program.Champion.FlatMagicDamageMod);
        }

        // Cast Methods
        public static void ToggleQ_On(Obj_AI_Base target)
        {
            if (target == null) return;
            if (Q.IsReady() && Q.ToggleState == 1 && (Game.Time - poisontime >= 2))
            {
                poisontime = Game.Time;
                Q.Cast();
            }
        }

        public static void ToggleQ_Off(Obj_AI_Base target)
        {
            if (target == null) return;
            if (Q.IsReady() && Q.ToggleState == 2)
            {
                poisontime = Game.Time;
                Q.Cast();
            }
        }

        public static void CastW(Obj_AI_Base target)
        {
            if (target == null) return;
            if (W.IsReady())
                W.Cast(target);
        }

        public static void CastE(Obj_AI_Base target)
        {
            if (target == null) return;
            if (E.IsReady())
                E.Cast(target);
        }

        public static void CastR(Obj_AI_Base target)
        {
            if (target == null) return;
            if (R.IsReady())
                R.Cast();
        }

        // Poison Controller
        public static void QDisable()
        {
            if (GetManaRegen() > Program.Champion.Spellbook.GetSpell(SpellSlot.Q).SData.ManaCostArray[Program.Champion.Level]) return;
            
            // Disable Conditions
            if (!Program.Champion.IsInShopRange()
                && Program.Champion.CountEnemiesInRange(1000) == 0
                && EntityManager.MinionsAndMonsters.EnemyMinions.Count(a => a.IsInRange(Program.Champion, 1000)) == 0
                && EntityManager.MinionsAndMonsters.Monsters.Count(a => a.IsInRange(Program.Champion, 750)) == 0)
                ToggleQ_Off(Program.Champion);
        }

        public static float GetManaRegen()
        {
            var flatManaPerSecond = (7.52f + (0.55f * Program.Champion.Level)) / 5;
            var additionalManaPerSecond = flatManaPerSecond;

            foreach (var item in Program.Champion.InventoryItems)
            {
                switch (item.Id)
                {
                    // %25
                    case ItemId.Ancient_Coin:
                    case ItemId.Faerie_Charm:
                    case ItemId.Spellthiefs_Edge:
                        additionalManaPerSecond += 0.25f * flatManaPerSecond;
                        break;
                    // %50
                    case ItemId.Ardent_Censer:
                    case ItemId.Chalice_of_Harmony:
                    case ItemId.Dorans_Ring:
                    case ItemId.Forbidden_Idol:
                        additionalManaPerSecond += 0.5f * flatManaPerSecond;
                        break;
                    // %75
                    case ItemId.Athenes_Unholy_Grail:
                    case ItemId.Frost_Queens_Claim:
                    case ItemId.Frostfang:
                    case ItemId.Nomads_Medallion:
                    case ItemId.Talisman_of_Ascension:
                        additionalManaPerSecond += 0.75f * flatManaPerSecond;
                        break;
                    // %100
                    case ItemId.Eye_of_the_Oasis:
                    case ItemId.Eye_of_the_Watchers:
                        additionalManaPerSecond += flatManaPerSecond;
                        break;
                    // %150
                    case ItemId.Mikaels_Crucible:
                        additionalManaPerSecond += 1.5f * flatManaPerSecond;
                        break;
                    // Flats
                    case ItemId.Guardians_Orb:
                        additionalManaPerSecond += 2;
                        break;
                }
            }

            if (Program.Champion.HasBuff("InsanityPotion"))
                additionalManaPerSecond += (20 + (15 * R.Level)) / 25;
            if (Program.Champion.HasBuff("ElixirOfSorcery"))
                additionalManaPerSecond += 3;
            //if (Program.Champion.HasBuff("treelinemasterbufft0"))
                //additionalManaPerSecond += 0.01f * (Program.Champion.ManaPercent);

            return additionalManaPerSecond;
        }
    }
}