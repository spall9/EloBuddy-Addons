using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace ArsenalActivator
{
    internal class Items
    {
        // Menu
        public static Menu OffensiveItemMenu, DefensiveItemMenu, ConsumableItemMenu, TrinketItemMenu;

        // Grab Objects
        public static AIHeroClient Player = Program.Player;

        // Grab Item[]
        public static InventorySlot[] Itemlist = Player.InventoryItems;

        // Sliders
        public static float Seraphs;
        public static float HealthPot;
        public static float Biscuit;
        public static float Flask;
        public static float FlaskJungle;
        public static float FlaskDark;
        // Create
        public static void MenuDraw()
        {
            OffensiveItemMenu = Program.ArsenalActivatorMenu.AddSubMenu("Offensive Items", "OffensiveItems");
            OffensiveItemMenu.AddGroupLabel("Offensive Items");
            OffensiveItemMenu.AddLabel("Damaging Weapons");
            OffensiveItemMenu.Add("Botrkcall", new CheckBox("Use Blade of the Ruined King"));
            OffensiveItemMenu.Add("Cutlasscall", new CheckBox("Use Bilgewater Cutlass"));
            OffensiveItemMenu.AddLabel("Boosting Weapons");
            OffensiveItemMenu.Add("Muracall", new CheckBox("Use Muramana"));
            OffensiveItemMenu.Add("Youmuucall", new CheckBox("Use Youmouu's Ghostblade"));
            //OffensiveItemMenu.AddLabel("Revealer Weapons");
            //OffensiveItemMenu.Add("Lightbringercall", new CheckBox("Use The Lightbringer"));

            DefensiveItemMenu = Program.ArsenalActivatorMenu.AddSubMenu("Defensive Items", "DefensiveItems");
            DefensiveItemMenu.AddGroupLabel("Defensive Items");
            DefensiveItemMenu.AddLabel("Assisting Boosts");
            DefensiveItemMenu.Add("Bannercall", new CheckBox("Use Banner of Command"));
            DefensiveItemMenu.AddLabel("Protective Shields");
            DefensiveItemMenu.Add("Seraphscall", new Slider("Use Seraph's Embrace at Health % - 0 is off", 10));
            //DefensiveItemMenu.AddLabel("Revealer Shields");
            //DefensiveItemMenu.Add("Hextechcall", new CheckBox("Use Hextech Sweeper"));

            ConsumableItemMenu = Program.ArsenalActivatorMenu.AddSubMenu("Consumable Items", "ConsumableItems");
            ConsumableItemMenu.AddGroupLabel("Consumable Items");
            ConsumableItemMenu.AddLabel("Consumable Potions");
            ConsumableItemMenu.Add("Healthcall", new Slider("Use Health Potion at Health % - 0 is off", 25));
            ConsumableItemMenu.Add("Biscuitcall",
                new Slider("Use Total Biscuit of Rejuvenation at Health % - 0 is off", 25));
            ConsumableItemMenu.Add("Flaskcall", new Slider("Use Regeneration Potion at Health % - 0 is off", 25));
            ConsumableItemMenu.Add("JungleFlaskcall",
                new Slider("Use Hunter's Potion at Health % - 0 is off", 25));
            ConsumableItemMenu.Add("DarkFlaskcall",
                new Slider("Use Corruption Potion at Health % - 0 is off", 25));
            ConsumableItemMenu.AddSeparator(1);
            ConsumableItemMenu.AddLabel("Consumable Elixirs");
            ConsumableItemMenu.Add("Ironcall", new CheckBox("Use Elixir of Iron in Combo"));
            ConsumableItemMenu.Add("Sorcerycall", new CheckBox("Use Elixir of Sorcery in Combo"));
            ConsumableItemMenu.Add("Wrathcall", new CheckBox("Use Elixir of Wrath in Combo"));
            //ConsumableItemMenu.AddSeparator(1);
            //ConsumableItemMenu.AddLabel("Consumable Revealer");
            //ConsumableItemMenu.Add("Oraclecall", new CheckBox("Use Oracle's Extract on Stealthed Enemies"));

            TrinketItemMenu = Program.ArsenalActivatorMenu.AddSubMenu("Trinket Items", "TrinketItems");
            TrinketItemMenu.AddGroupLabel("Trinket Items");
            //TrinketItemMenu.AddLabel("Revealer Trinket");
            //TrinketItemMenu.Add("HextechTTcall", new CheckBox("Use Hextech Sweeper Trinket"));
            TrinketItemMenu.AddLabel("Trinket Food");
            TrinketItemMenu.Add("Porosnaxcall", new CheckBox("Feed Poro-snax"));
        }

        // Calculate
        public static void CastItems()
        {
            CastSliders();
            var turret = Program.GetAlliedObjective(2500, GameObjectType.obj_AI_Turret);
            var target = Program.GetEnemy(2500, GameObjectType.AIHeroClient);
            var allyminion = Program.GetAlly(2500, GameObjectType.obj_AI_Minion);
            foreach (var item in Itemlist)
            {
                // Banner of Command
                if (item.Id == ItemId.Banner_of_Command
                    && DefensiveItemMenu["Bannercall"].Cast<CheckBox>().CurrentValue
                    && allyminion != null && allyminion.IsAlly
                    && BoCCheck(allyminion.BaseSkinName)
                    && allyminion.Distance(Player) <= 1200
                    && allyminion.Health >= allyminion.MaxHealth*0.5f
                    && item.CanUseItem())
                    item.Cast(allyminion);
                // Bilgewater Cutlass
                if (item.Id == ItemId.Bilgewater_Cutlass
                    && OffensiveItemMenu["Cutlasscall"].Cast<CheckBox>().CurrentValue
                    && target != null
                    && (target.Health <= Player.CalculateDamageOnUnit(target, DamageType.Magical, 100)
                        || (!target.IsFacing(Player) && target.HealthPercent <= 50))
                    && target.Distance(Player) <= 550
                    && item.CanUseItem())
                    item.Cast(target);
                // Blade of the Ruined King -- needs test
                if (item.Id == ItemId.Blade_of_the_Ruined_King
                    && OffensiveItemMenu["Botrkcall"].Cast<CheckBox>().CurrentValue
                    && target != null
                    && (target.Health <= Player.CalculateDamageOnUnit(target, DamageType.Physical, BotrkCheck(target))
                        || (!target.IsFacing(Player) && target.HealthPercent <= 50))
                    && target.Distance(Player) <= 550
                    && item.CanUseItem())
                    item.Cast(target);
                // Corruption Potion
                if (item.Id == (ItemId) 2033
                    && Player.HealthPercent <= FlaskDark
                    && !Player.HasBuff("ItemDarkCrystalFlask")
                    && !Player.IsInShopRange() && item.CanUseItem())
                    item.Cast();
                // Diet Poro-Snax
                if (item.Id == (ItemId) 2054
                    && TrinketItemMenu["Porosnaxcall"].Cast<CheckBox>().CurrentValue
                    && item.CanUseItem())
                    item.Cast();
                // Elixir of Iron
                if (item.Id == ItemId.Elixir_of_Iron
                    && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)
                    && ConsumableItemMenu["Ironcall"].Cast<CheckBox>().CurrentValue
                    && !Player.HasBuff("ElixirOfWrath") && !Player.HasBuff("ElixirOfSorcery")
                    && Player.CountAlliesInRange(Player.GetAutoAttackRange()) >= 1
                    && item.CanUseItem())
                    item.Cast();
                // Elixir of Sorcery
                if (item.Id == ItemId.Elixir_of_Sorcery
                    && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)
                    && ConsumableItemMenu["Sorcerycall"].Cast<CheckBox>().CurrentValue
                    && !Player.HasBuff("ElixirOfIron") && !Player.HasBuff("ElixirOfWrath")
                    && (Player.CountEnemiesInRange(Player.GetAutoAttackRange()) >= 1
                        || (turret != null && turret.IsEnemy))
                    && item.CanUseItem())
                    item.Cast();
                // Elixir of Wrath
                if (item.Id == ItemId.Elixir_of_Wrath
                    && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)
                    && ConsumableItemMenu["Wrathcall"].Cast<CheckBox>().CurrentValue
                    && !Player.HasBuff("ElixirOfIron") && !Player.HasBuff("ElixirOfSorcery")
                    && Player.CountEnemiesInRange(Player.GetAutoAttackRange()) >= 1
                    && item.CanUseItem())
                    item.Cast();
                // Frost Queen's Claim -- needs test
                if (item.Id == ItemId.Frost_Queens_Claim
                    && Player.CountEnemiesInRange(5000) >= 2
                    && target != null
                    && target.Distance(Player) <= 5000
                    && ((!target.IsFacing(Player) && target.HealthPercent <= 50)
                    || (Player.CountEnemiesInRange(5000) >= 4 && target.IsFacing(Player)))
                    && item.CanUseItem())
                    item.Cast();
                // Health Potion
                if (item.Id == ItemId.Health_Potion
                    && Player.HealthPercent <= HealthPot
                    && !Player.HasBuff("RegenerationPotion")
                    && !Player.IsInShopRange() && item.CanUseItem())
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
                    && Player.HealthPercent <= FlaskJungle
                    && !Player.HasBuff("ItemCrystalFlaskJungle")
                    && !Player.IsInShopRange() && item.CanUseItem())
                    item.Cast();
                // Muramana
                if (item.Id == (ItemId) 3042
                    && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)
                    && OffensiveItemMenu["Muracall"].Cast<CheckBox>().CurrentValue
                    && Player.ManaPercent >= 3 && Player.CountEnemiesInRange(Player.GetAutoAttackRange()) > 0
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
                    && TrinketItemMenu["Porosnaxcall"].Cast<CheckBox>().CurrentValue
                    && item.CanUseItem())
                    item.Cast();
                // Regeneration Potion
                if (item.Id == (ItemId) 2031
                    && Player.HealthPercent <= Flask
                    && !Player.HasBuff("ItemCrystalFlask")
                    && !Player.IsInShopRange() && item.CanUseItem())
                    item.Cast();
                // Seraph's Embrace
                if ((item.Id == (ItemId) 3040 || item.Id == (ItemId) 3048)
                    && Player.HealthPercent <= Seraphs
                    && Player.ManaPercent > 20 && item.CanUseItem())
                    item.Cast();
                // Total Biscuit of Rejuvenation
                if (item.Id == (ItemId) 2010
                    && Player.HealthPercent <= Biscuit
                    && !Player.HasBuff("ItemMiniRegenPotion")
                    && !Player.IsInShopRange() && item.CanUseItem())
                    item.Cast();
                // Youmuu's Ghostblade
                if (item.Id == ItemId.Youmuus_Ghostblade
                    && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)
                    && OffensiveItemMenu["Youmuucall"].Cast<CheckBox>().CurrentValue
                    && Player.CountEnemiesInRange(Player.GetAutoAttackRange()) >= 1
                    && item.CanUseItem())
                    item.Cast();
            }
        }

        public static void CastSliders()
        {
            Seraphs = DefensiveItemMenu["Seraphscall"].Cast<Slider>().CurrentValue;
            HealthPot = ConsumableItemMenu["Healthcall"].Cast<Slider>().CurrentValue;
            Biscuit = ConsumableItemMenu["Biscuitcall"].Cast<Slider>().CurrentValue;
            Flask = ConsumableItemMenu["Flaskcall"].Cast<Slider>().CurrentValue;
            FlaskJungle = ConsumableItemMenu["JungleFlaskcall"].Cast<Slider>().CurrentValue;
            FlaskDark = ConsumableItemMenu["DarkFlaskcall"].Cast<Slider>().CurrentValue;
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