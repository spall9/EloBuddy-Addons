using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace ArsenalActivator
{
    internal class MenuManager
    {
        // Create Main Segments
        public static Menu ArsenalActivatorMenu,
            OffensiveItemMenu,
            DefensiveItemMenu,
            ConsumableItemMenu,
            TrinketItemMenu,
            SummonerSpellMenu;

        public static void Initialize()
        {
            // Main Menu
            ArsenalActivatorMenu = MainMenu.AddMenu("Arsenal Activator", "ArsenalActivator");
            ArsenalActivatorMenu.AddGroupLabel("Arsenal Activator");
            
            // Item Menu
            ItemMenuDraw();
            // Summoner Spell Menu
            SummonerSpellMenuDraw();
        }

        public static void ItemMenuDraw()
        {
            OffensiveItemMenu = ArsenalActivatorMenu.AddSubMenu("Offensive Items", "OffensiveItems");
            OffensiveItemMenu.AddGroupLabel("Offensive Items");
            OffensiveItemMenu.AddLabel("Damaging Weapons");
            OffensiveItemMenu.Add("Botrkcall", new CheckBox("Use Blade of the Ruined King"));
            OffensiveItemMenu.Add("Cutlasscall", new CheckBox("Use Bilgewater Cutlass"));
            OffensiveItemMenu.AddLabel("Boosting Weapons");
            OffensiveItemMenu.Add("Muracall", new CheckBox("Use Muramana"));
            OffensiveItemMenu.Add("Youmuucall", new CheckBox("Use Youmouu's Ghostblade"));
            //OffensiveItemMenu.AddLabel("Revealer Weapons");
            //OffensiveItemMenu.Add("Lightbringercall", new CheckBox("Use The Lightbringer"));

            DefensiveItemMenu = ArsenalActivatorMenu.AddSubMenu("Defensive Items", "DefensiveItems");
            DefensiveItemMenu.AddGroupLabel("Defensive Items");
            DefensiveItemMenu.AddLabel("Assisting Boosts");
            DefensiveItemMenu.Add("Bannercall", new CheckBox("Use Banner of Command"));
            DefensiveItemMenu.AddLabel("Protective Shields");
            DefensiveItemMenu.Add("Seraphscall", new Slider("Use Seraph's Embrace at Health % - 0 is off", 10));
            //DefensiveItemMenu.AddLabel("Revealer Shields");
            //DefensiveItemMenu.Add("Hextechcall", new CheckBox("Use Hextech Sweeper"));

            ConsumableItemMenu = ArsenalActivatorMenu.AddSubMenu("Consumable Items", "ConsumableItems");
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

            TrinketItemMenu = ArsenalActivatorMenu.AddSubMenu("Trinket Items", "TrinketItems");
            TrinketItemMenu.AddGroupLabel("Trinket Items");
            //TrinketItemMenu.AddLabel("Revealer Trinket");
            //TrinketItemMenu.Add("HextechTTcall", new CheckBox("Use Hextech Sweeper Trinket"));
            TrinketItemMenu.AddLabel("Trinket Food");
            TrinketItemMenu.Add("Porosnaxcall", new CheckBox("Feed Poro-snax"));
        }

        public static void SummonerSpellMenuDraw()
        {
            SummonerSpellMenu = ArsenalActivatorMenu.AddSubMenu("Summoner Spells", "Summoner Spells");
            SummonerSpellMenu.AddGroupLabel("Summoner Spells");
            SummonerSpellMenu.AddLabel("Offensive Summoner Spells:");
            if (SummonerSpells.Ignite != null)
                SummonerSpellMenu.Add("ignite", new Slider("Use Ignite at Health % - 0 is off", 15));
            if (SummonerSpells.Smite != null)
            {
                SummonerSpellMenu.Add("smite", new CheckBox("Use Smite"));
                SummonerSpellMenu.AddSeparator(1);
                SummonerSpellMenu.AddLabel("Smite Upgrades");
                SummonerSpellMenu.Add("smitechill", new CheckBox("KS with Chilling"));
                SummonerSpellMenu.Add("smiteduel", new CheckBox("Combo with Challenging"));
                SummonerSpellMenu.AddSeparator(1);
                SummonerSpellMenu.AddLabel("Summoner Rift Jungle Camps:");
                SummonerSpellMenu.Add("smiteblue", new CheckBox("Blue Sentinel", false));
                SummonerSpellMenu.Add("smitered", new CheckBox("Red Brambleback", false));
                SummonerSpellMenu.Add("smitemurk", new CheckBox("Greater Murk Wolf", false));
                SummonerSpellMenu.Add("smiteraptor", new CheckBox("Crimson Raptor", false));
                SummonerSpellMenu.Add("smitekrug", new CheckBox("Ancient Krug"));
                SummonerSpellMenu.Add("smitegromp", new CheckBox("Gromp"));
                SummonerSpellMenu.Add("smitecrab", new CheckBox("Rift Scuttler", false));
                SummonerSpellMenu.Add("smitedragon", new CheckBox("Dragon"));
                SummonerSpellMenu.Add("smiteherald", new CheckBox("Rift Herald"));
                SummonerSpellMenu.Add("smitenashor", new CheckBox("Baron Nashor"));
                SummonerSpellMenu.AddSeparator(1);
                SummonerSpellMenu.AddLabel("Twisted Treeline Jungle Camps:");
                SummonerSpellMenu.Add("smitewolf", new CheckBox("Giant Wolf"));
                SummonerSpellMenu.Add("smitegolem", new CheckBox("Big Golem"));
                SummonerSpellMenu.Add("smitewraith", new CheckBox("Wraith"));
                SummonerSpellMenu.Add("smitespider", new CheckBox("Vilemaw"));
                SummonerSpellMenu.AddSeparator(1);
                SummonerSpellMenu.AddLabel("Jungling Options");
                SummonerSpellMenu.Add("smitecheck", new CheckBox("KS Smite Camps if Enemies Near"));
            }
            SummonerSpellMenu.AddSeparator(1);
            SummonerSpellMenu.AddLabel("Defensive Summoner Spells");
            if (SummonerSpells.Clarity != null)
            {
                SummonerSpellMenu.Add("clarityself",
                    new Slider("Use Clairty on Self at Mana % - 0 is off", 25));
                SummonerSpellMenu.Add("clarityally",
                    new Slider("Use Clairty on Ally at Mana % - 0 is off"));
            }
            if (SummonerSpells.Cleanse != null)
            {
                SummonerSpellMenu.Add("Cblind", new CheckBox("Use Cleanse on Blind", false));
                SummonerSpellMenu.Add("Ccharm", new CheckBox("Use Cleanse on Charm"));
                SummonerSpellMenu.Add("Cfear", new CheckBox("Use Cleanse on Fear", false));
                SummonerSpellMenu.Add("Cflee", new CheckBox("Use Cleanse on Flee", false));
                SummonerSpellMenu.Add("Cpolymorph", new CheckBox("Use Cleanse on Polymorph"));
                SummonerSpellMenu.Add("Csilence", new CheckBox("Use Cleanse on Silence", false));
                SummonerSpellMenu.Add("Csleep", new CheckBox("Use Cleanse on Sleep"));
                SummonerSpellMenu.Add("Cslow", new CheckBox("Use Cleanse on Slow", false));
                SummonerSpellMenu.Add("Csnare", new CheckBox("Use Cleanse on Snare"));
                SummonerSpellMenu.Add("Cstun", new CheckBox("Use Cleanse on Stun"));
                SummonerSpellMenu.Add("Ctaunt", new CheckBox("Use Cleanse on Taunt", false));
                SummonerSpellMenu.Add("Cexhaust", new CheckBox("Use Cleanse on Exhaust"));
                SummonerSpellMenu.Add("Cignite", new CheckBox("Use Cleanse on Ignite"));
            }
            if (SummonerSpells.Exhaust != null)
            {
                SummonerSpellMenu.Add("exhaustself",
                    new Slider("Use Exhaust when Self at Health % - 0 is off", 40));
                SummonerSpellMenu.Add("exhaustally",
                    new Slider("Use Exhaust when Ally at Health % - 0 is off", 25));
            }
            if (SummonerSpells.Heal != null)
            {
                SummonerSpellMenu.Add("healself", new Slider("Use Heal on Self at Health % - 0 is off", 25));
                SummonerSpellMenu.Add("healally", new Slider("Use Heal on Ally at Health % - 0 is off", 15));
            }
            SummonerSpellMenu.AddSeparator(1);
            SummonerSpellMenu.AddLabel("Misc Summoner Spells");
            if (SummonerSpells.Clairvoyance != null)
            {
                //SummonerSpellMenu.Add("clairvoyance", new CheckBox("Use Clairvoyance"));
            }
            if (SummonerSpells.Mark != null)
                SummonerSpellMenu.Add("mark", new CheckBox("Use Mark"));
            if (SummonerSpells.Porothrow != null)
                SummonerSpellMenu.Add("porothrow", new CheckBox("Use Poro Throw"));
            if (SummonerSpells.Garrison != null)
            {
                //SummonerSpellMenu.Add("odininterrupt", new CheckBox("Use Garrison to Interrupt Neutralization"));
                //SummonerSpellMenu.Add("odinreduce", new CheckBox("Use Garrison to Reduce Turret's Damage"));
            }
            SummonerSpellMenu.AddSeparator(1);
            SummonerSpellMenu.AddLabel("Draw Summoner Spells");
            SummonerSpellMenu.Add("DrawSS", new CheckBox("Draw Summoner Spells"));
        }
    }
}
