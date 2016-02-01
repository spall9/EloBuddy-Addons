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
        public static int Toggle { get { return Player.GetSpell(SpellSlot.Q).ToggleState; } } 

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
            if (GetManaRegen() > Player.GetSpell(SpellSlot.Q).SData.ManaCostArray[Player.GetSpell(SpellSlot.Q).Level]) return;

            // Disable Conditions
            if (Toggle == 2 && !Champion.IsInShopRange()
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
            var flatManaPerSecond = (Champion.CharData.BaseStaticMPRegen);
            var additionalManaPerSecond = flatManaPerSecond + (0.11f * Champion.Level);
            foreach (var item in Champion.InventoryItems)
            {
                if (item.Id == ItemId.Morellonomicon ||
                    item.Id == ItemId.Frostfang ||
                    item.Id == ItemId.Eye_of_the_Oasis ||
                    item.Id == ItemId.Ardent_Censer ||
                    item.Id == ItemId.Eye_of_the_Watchers ||
                    item.Id == ItemId.Frost_Queens_Claim ||
                    item.Id == ItemId.Talisman_of_Ascension)
                    additionalManaPerSecond += flatManaPerSecond;
                if (item.Id == ItemId.Faerie_Charm ||
                    item.Id == ItemId.Spellthiefs_Edge ||
                    item.Id == ItemId.Ancient_Coin ||
                    item.Id == ItemId.Tear_of_the_Goddess ||
                    item.Id == ItemId.Tear_of_the_Goddess_Crystal_Scar ||
                    item.Id == ItemId.Manamune ||
                    item.Id == ItemId.Manamune_Crystal_Scar)
                    additionalManaPerSecond += 0.25f * flatManaPerSecond;
                if (item.Id == ItemId.Dorans_Ring ||
                    item.Id == ItemId.Forbidden_Idol ||
                    item.Id == ItemId.Nomads_Medallion ||
                    item.Id == ItemId.Archangels_Staff ||
                    item.Id == ItemId.Archangels_Staff_Crystal_Scar)
                    additionalManaPerSecond += 0.5f * flatManaPerSecond;
                if (item.Id == ItemId.Chalice_of_Harmony)
                    additionalManaPerSecond += (float)(0.5f * flatManaPerSecond + ((0.02 * (Champion.MaxMana - Champion.Mana)) / 5));
                if (item.Id == ItemId.Athenes_Unholy_Grail ||
                    item.Id == ItemId.Mikaels_Crucible)
                    additionalManaPerSecond += (float)(flatManaPerSecond + ((0.02 * (Champion.MaxMana - Champion.Mana)) / 5));
            }

            return additionalManaPerSecond;
        }
    }
}
