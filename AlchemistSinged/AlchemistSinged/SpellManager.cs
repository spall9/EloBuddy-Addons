using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace AlchemistSinged
{
    internal class SpellManager
    {
        public static Spell.Active Q { get; set; }
        public static Spell.Skillshot W { get; set; }
        public static Spell.Targeted E { get; set; }
        public static Spell.Active R { get; set; }

        // Clone Character Object
        public static AIHeroClient Champion = Program.Champion;

        // Toggle State
        public static bool Toggle { get { return Champion.HasBuff("PoisonTrail"); } } 

        public static void Initialize()
        {
            // Initialize spells
            Q = new Spell.Active(SpellSlot.Q);
            W = new Spell.Skillshot(SpellSlot.W, 1000, SkillShotType.Circular, 500, 700, 350)
            {
                MinimumHitChance = HitChance.High,
                AllowedCollisionCount = int.MaxValue
            };
            E = new Spell.Targeted(SpellSlot.E, 125);
            R = new Spell.Active(SpellSlot.R);
        }

        // Poison Controller
        public static void QDisable()
        {
            // Mana Regen Utilizer
            if (GetManaRegen() > Champion.Spellbook.GetSpell(SpellSlot.Q).SData.ManaCostArray[Champion.Level]) return;
            
            // Disable Conditions
            if (Toggle && !Champion.IsInShopRange()
                && Champion.CountEnemiesInRange(1000) == 0
                && EntityManager.MinionsAndMonsters.Minions.Where(a => a.IsEnemy).Count(a => a.IsInRange(Champion, 1000)) == 0
                && EntityManager.MinionsAndMonsters.Monsters.Count(a => a.IsInRange(Champion, 750)) == 0)
                CastQ(Champion);
        }

        // Champion Specified Abilities
        public static float QDamage()
        {
            return new float[] { 0, 44, 68, 92, 116, 140 }[Q.Level]
                + (0.6f * Champion.FlatMagicDamageMod);
        }

        public static float EDamage()
        {
            return new float[] { 0, 50, 65, 80, 95, 110 }[E.Level]
                   + (0.75f * Champion.FlatMagicDamageMod);
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

        public static float GetManaRegen()
        {
            var flatManaPerSecond = (Champion.CharData.BaseStaticMPRegen + (0.11f * Champion.Level));
            var additionalManaPerSecond = flatManaPerSecond;
            foreach (var item in Champion.InventoryItems)
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
            if (Champion.Name == "Singed" && Champion.HasBuff("InsanityPotion"))
                additionalManaPerSecond += new float[] { 0, 7, 10, 16 }[R.Level];
            if (Champion.HasBuff("catalystheal"))
                additionalManaPerSecond += 25;
            if (Champion.HasBuff("ElixirOfSorcery"))
                additionalManaPerSecond += 3;

            return additionalManaPerSecond;
        }
    }
}
