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

        // Toggle State
        public static bool Toggle { get { return Program.Champion.HasBuff("PoisonTrail"); } } 

        // Initialize method
        public static void Initialize()
        {
            Q = new Spell.Active(SpellSlot.Q);
            W = new Spell.Skillshot(SpellSlot.W, 1000, SkillShotType.Circular, 500, 700, 350)
            {
                MinimumHitChance = HitChance.High,
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
            return (44 + (24 * Q.Level)) + (0.6f * Program.Champion.FlatMagicDamageMod);
        }

        // ACTIVE: Singed flings the target enemy over his shoulder, dealing them magic damage that is capped against minions and monsters.
        // If the target lands onto his Mega Adhesive, they are temporarily rooted.
        public static float EDamage(Obj_AI_Base target)
        {
            return (50 + (15 * E.Level)) + (target.MaxHealth * (6 + (0.5f * E.Level))) + (0.75f * Program.Champion.FlatMagicDamageMod);
        }

        // Cast Methods
        public static void CastQ(Obj_AI_Base target)
        {
            if (target == null) return;
            if (Q.IsReady())
                Q.Cast();
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
            // Mana Regen Utilizer
            if (GetManaRegen() > Program.Champion.Spellbook.GetSpell(SpellSlot.Q).SData.ManaCostArray[Program.Champion.Level]) return;
            
            // Disable Conditions
            if (Toggle && !Program.Champion.IsInShopRange()
                && Program.Champion.CountEnemiesInRange(1000) == 0
                && EntityManager.MinionsAndMonsters.Minions.Where(a => a.IsEnemy).Count(a => a.IsInRange(Program.Champion, 1000)) == 0
                && EntityManager.MinionsAndMonsters.Monsters.Count(a => a.IsInRange(Program.Champion, 750)) == 0)
                CastQ(Program.Champion);
        }

        public static float GetManaRegen()
        {
            var flatManaPerSecond = (Program.Champion.CharData.BaseStaticMPRegen + (0.11f * Program.Champion.Level));
            var additionalManaPerSecond = flatManaPerSecond;
            foreach (var item in Program.Champion.InventoryItems)
            {
                switch (item.Id)
                {
                    // %25
                    case ItemId.Faerie_Charm:
                    case ItemId.Spellthiefs_Edge:
                    case ItemId.Ancient_Coin:
                    case ItemId.Tear_of_the_Goddess:
                    case ItemId.Manamune:
                        additionalManaPerSecond += 0.25f * flatManaPerSecond;
                        break;
                    // %50
                    case ItemId.Dorans_Ring:
                    case ItemId.Forbidden_Idol:
                    case ItemId.Nomads_Medallion:
                    case ItemId.Chalice_of_Harmony:
                    case ItemId.Archangels_Staff:
                        additionalManaPerSecond += 0.5f * flatManaPerSecond;
                        break;
                    // %100
                    case ItemId.Morellonomicon:
                    case ItemId.Frostfang:
                    case ItemId.Eye_of_the_Oasis:
                    case ItemId.Ardent_Censer:
                    case ItemId.Eye_of_the_Watchers:
                    case ItemId.Frost_Queens_Claim:
                    case ItemId.Talisman_of_Ascension:
                    case ItemId.Athenes_Unholy_Grail:
                    case ItemId.Mikaels_Crucible:
                        additionalManaPerSecond += flatManaPerSecond;
                        break;
                    // Jungle items while in Jungle
                    case ItemId.Hunters_Talisman:
                    case ItemId.Stalkers_Blade:
                    case ItemId.Trackers_Knife:
                    case ItemId.Skirmishers_Sabre:
                        break;
                }
            }
            if (Program.Champion.Name == "Singed" && Program.Champion.HasBuff("InsanityPotion"))
                additionalManaPerSecond += new float[] { 0, 7, 10, 16 }[R.Level];
            if (Program.Champion.HasBuff("catalystheal"))
                additionalManaPerSecond += 25;
            if (Program.Champion.HasBuff("ElixirOfSorcery"))
                additionalManaPerSecond += 3;

            return additionalManaPerSecond;
        }
    }
}