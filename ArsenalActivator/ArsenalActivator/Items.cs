using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

namespace ArsenalActivator
{
    internal class Items
    {
        // Clone Character Object
        public static AIHeroClient Champion = Program.Champion;

        // Grab Item[]
        public static InventorySlot[] Itemlist = Champion.InventoryItems;

        // Slider Variables
        public static float Seraphs;
        public static float HealthPot;
        public static float Biscuit;
        public static float Flask;
        public static float FlaskJungle;
        public static float FlaskDark;

        public static void Initialize()
        {
        }
        
        public static void CastItems()
        {
            CastSliders();

            foreach (var item in Itemlist)
            {
                // Banner of Command
                if (item.Id == ItemId.Banner_of_Command
                    && MenuManager.DefensiveItemMenu["Bannercall"].Cast<CheckBox>().CurrentValue
                    && item.CanUseItem())
                {
                    var minion = TargetManager.GetMinionTarget(1200, DamageType.Magical, true);
                    if (minion != null && BoCCheck(minion.BaseSkinName)
                        && minion.Health >= minion.MaxHealth * 0.5f)
                    item.Cast(minion);
                }
                // Bilgewater Cutlass
                if (item.Id == ItemId.Bilgewater_Cutlass
                    && MenuManager.OffensiveItemMenu["Cutlasscall"].Cast<CheckBox>().CurrentValue
                    && item.CanUseItem())
                {
                    var target = TargetManager.GetChampionTarget(550, DamageType.Magical);
                    if (target != null 
                        && (target.Health <= Champion.CalculateDamageOnUnit(target, DamageType.Magical, 100)
                        || (!target.IsFacing(Champion) && target.HealthPercent <= 50)))
                    item.Cast(target);
                }
                // Blade of the Ruined King -- needs test
                if (item.Id == ItemId.Blade_of_the_Ruined_King
                    && MenuManager.OffensiveItemMenu["Botrkcall"].Cast<CheckBox>().CurrentValue
                    && item.CanUseItem())
                {
                    var target = TargetManager.GetChampionTarget(550, DamageType.Physical);
                    if (target != null 
                        && (target.Health <= Champion.CalculateDamageOnUnit(target, DamageType.Physical, BotrkCheck(target))
                        || (!target.IsFacing(Champion) && target.HealthPercent <= 50)))
                    item.Cast(target);
                }
                // Corruption Potion
                if (item.Id == (ItemId) 2033
                    && Champion.HealthPercent <= FlaskDark
                    && !Champion.HasBuff("ItemDarkCrystalFlask")
                    && !Champion.IsInShopRange() && item.CanUseItem())
                    item.Cast();
                // Diet Poro-Snax
                if (item.Id == (ItemId) 2054
                    && MenuManager.TrinketItemMenu["Porosnaxcall"].Cast<CheckBox>().CurrentValue
                    && item.CanUseItem())
                    item.Cast();
                // Elixir of Iron
                if (item.Id == ItemId.Elixir_of_Iron
                    && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)
                    && MenuManager.ConsumableItemMenu["Ironcall"].Cast<CheckBox>().CurrentValue
                    && !Champion.HasBuff("ElixirOfWrath") && !Champion.HasBuff("ElixirOfSorcery")
                    && Champion.CountAlliesInRange(Champion.GetAutoAttackRange()) >= 1
                    && item.CanUseItem())
                    item.Cast();
                // Elixir of Sorcery
                if (item.Id == ItemId.Elixir_of_Sorcery
                    && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)
                    && MenuManager.ConsumableItemMenu["Sorcerycall"].Cast<CheckBox>().CurrentValue
                    && !Champion.HasBuff("ElixirOfIron") && !Champion.HasBuff("ElixirOfWrath")
                    && Champion.CountEnemiesInRange(Champion.GetAutoAttackRange()) >= 1
                    && item.CanUseItem())
                {
                    var turret = TargetManager.GetTurretTarget(Champion.GetAutoAttackRange());
                    if (turret != null)
                        item.Cast();
                }
                // Elixir of Wrath
                if (item.Id == ItemId.Elixir_of_Wrath
                    && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)
                    && MenuManager.ConsumableItemMenu["Wrathcall"].Cast<CheckBox>().CurrentValue
                    && !Champion.HasBuff("ElixirOfIron") && !Champion.HasBuff("ElixirOfSorcery")
                    && Champion.CountEnemiesInRange(Champion.GetAutoAttackRange()) >= 1
                    && item.CanUseItem())
                    item.Cast();
                // Frost Queen's Claim -- needs test
                if (item.Id == ItemId.Frost_Queens_Claim
                    && Champion.CountEnemiesInRange(5000) >= 2
                    && item.CanUseItem())
                {
                    var target = TargetManager.GetChampionTarget(5000, DamageType.Magical);
                    if (target != null && ((!target.IsFacing(Champion) && target.HealthPercent <= 50)
                                           || (Champion.CountEnemiesInRange(5000) >= 4 && target.IsFacing(Champion))))
                        item.Cast();

                }
                    item.Cast();
                // Health Potion
                if (item.Id == ItemId.Health_Potion
                    && Champion.HealthPercent <= HealthPot
                    && !Champion.HasBuff("RegenerationPotion")
                    && !Champion.IsInShopRange() && item.CanUseItem())
                    item.Cast();
                // Hextech Sweeper -- needs test
                /* if (item.Id == ItemId.Hextech_Sweeper
                    && DefensiveItemMenu["Hextechcall"].Cast<CheckBox>().CurrentValue
                    && target != null && target.HasBuffOfType(BuffType.Invisibility)
                    && target.Distance(player) <= 800
                    && item.CanUseItem())
                    item.Cast(target);*/
                // Hextech Sweeper Trinket -- needs test
                /*if (item.Name == "HextechSweeperTT"
                    && TrinketItemMenu["HextechTTcall"].Cast<CheckBox>().CurrentValue
                    && target != null && target.IsStealthed
                    && target.HasBuffOfType(BuffType.Invisibility)
                    && target.Distance(player) <= 800
                    && item.CanUseItem())
                    item.Cast(target.Position);*/
                // The Lightbringer -- needs test
                /*if (item.Id == ItemId.The_Lightbringer
                    && OffensiveItemMenu["Lightbringercall"].Cast<CheckBox>().CurrentValue
                    && target != null && target.IsStealthed
                    && target.Distance(player) <= 800
                    && item.CanUseItem())
                    item.Cast(target);*/
                // Hunter's Potion
                if (item.Id == (ItemId) 2032
                    && Champion.HealthPercent <= FlaskJungle
                    && !Champion.HasBuff("ItemCrystalFlaskJungle")
                    && !Champion.IsInShopRange() && item.CanUseItem())
                    item.Cast();
                // Muramana
                if (item.Id == (ItemId) 3042
                    && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)
                    && MenuManager.OffensiveItemMenu["Muracall"].Cast<CheckBox>().CurrentValue
                    && Champion.ManaPercent >= 3 && Champion.CountEnemiesInRange(Champion.GetAutoAttackRange()) > 0
                    && item.CanUseItem())
                    item.Cast();
                // Oracle's Extract -- needs test
                /*if (item.Id == ItemId.Oracles_Extract || item.Id == ItemId.Oracles_Extract
                    && ConsumableItemMenu["Oraclecall"].Cast<CheckBox>().CurrentValue
                    && !player.HasBuff("OracleExtractSight")
                    && target != null && target.IsStealthed
                    && item.CanUseItem())
                    item.Cast(player);*/
                // Poro-Snax
                if (item.Id == (ItemId) 2052
                    && MenuManager.TrinketItemMenu["Porosnaxcall"].Cast<CheckBox>().CurrentValue
                    && item.CanUseItem())
                    item.Cast();
                // Regeneration Potion
                if (item.Id == (ItemId) 2031
                    && Champion.HealthPercent <= Flask
                    && !Champion.HasBuff("ItemCrystalFlask")
                    && !Champion.IsInShopRange() && item.CanUseItem())
                    item.Cast();
                // Seraph's Embrace
                if ((item.Id == (ItemId) 3040 || item.Id == (ItemId) 3048)
                    && Champion.HealthPercent <= Seraphs
                    && Champion.ManaPercent > 20 && item.CanUseItem())
                    item.Cast();
                // Total Biscuit of Rejuvenation
                if (item.Id == (ItemId) 2010
                    && Champion.HealthPercent <= Biscuit
                    && !Champion.HasBuff("ItemMiniRegenPotion")
                    && !Champion.IsInShopRange() && item.CanUseItem())
                    item.Cast();
                // Youmuu's Ghostblade
                if (item.Id == ItemId.Youmuus_Ghostblade
                    && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)
                    && MenuManager.OffensiveItemMenu["Youmuucall"].Cast<CheckBox>().CurrentValue
                    && Champion.CountEnemiesInRange(Champion.GetAutoAttackRange()) >= 1
                    && item.CanUseItem())
                    item.Cast();
            }
        }

        public static void CastSliders()
        {
            Seraphs = MenuManager.DefensiveItemMenu["Seraphscall"].Cast<Slider>().CurrentValue;
            HealthPot = MenuManager.ConsumableItemMenu["Healthcall"].Cast<Slider>().CurrentValue;
            Biscuit = MenuManager.ConsumableItemMenu["Biscuitcall"].Cast<Slider>().CurrentValue;
            Flask = MenuManager.ConsumableItemMenu["Flaskcall"].Cast<Slider>().CurrentValue;
            FlaskJungle = MenuManager.ConsumableItemMenu["JungleFlaskcall"].Cast<Slider>().CurrentValue;
            FlaskDark = MenuManager.ConsumableItemMenu["DarkFlaskcall"].Cast<Slider>().CurrentValue;
        }
        public static bool BoCCheck(string sender)
        {
            switch (sender)
            {
                case "SRU_OrderMinionSiege":
                    return true;
                case "SRU_ChaosMinionSiege":
                    return true;
                case "OdinBlueSuperminion":
                    return true;
                case "OdinRedSuperminion":
                    return true;
                case "HA_OrderMinionSiege":
                    return true;
                case "HA_ChaosMinionSiege":
                    return true;
            }
            return false;
        }

        public static float BotrkCheck(Obj_AI_Base target)
        {
            if (target != null)
            {
                var health = (float)(target.MaxHealth*0.1);

                return health < 100 ? 100 : health;
            }
            return 100;
        }
    }
}