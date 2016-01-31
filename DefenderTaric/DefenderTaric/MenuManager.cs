using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace DefenderTaric
{
    internal class MenuManager
    {
        // Create Main Segments
        public static Menu DefenderTaricMenu, ComboMenu, HarassMenu, HealingMenu, DrawingMenu, SettingMenu;

        public static void Initialize()
        {
            // Addon Menu
            DefenderTaricMenu = MainMenu.AddMenu("Defender Taric", "DefenderTaric");
            DefenderTaricMenu.AddGroupLabel("Defender Taric");
            DefenderTaricMenu.AddLabel("Gems? Gems are truly outrageous. They are truly, truly, truly outrageous.");
            DefenderTaricMenu.Add("Ueaster", new CheckBox("Easter Egg"));

            // Combo Menu
            ComboMenu = DefenderTaricMenu.AddSubMenu("Combo Features", "ComboFeatures");
            ComboMenu.AddGroupLabel("Combo Feautres");
            ComboMenu.Add("Ucombo", new Slider("EWR Combo", 1, 1, 2));
            ComboMenu.AddSeparator(1);
            ComboMenu.AddLabel("Independent boxes for Spells:");
            ComboMenu.Add("Wcombo", new CheckBox("Use W"));
            ComboMenu.Add("Ecombo", new CheckBox("Use E"));
            ComboMenu.Add("Rcombo", new CheckBox("Use R"));
            ComboMenu.AddSeparator(1);
            ComboMenu.AddLabel("Spell Weaving with AA");
            ComboMenu.Add("Uweave", new CheckBox("SpellWeaving"));
            ComboMenu.Add("Qweave", new CheckBox("Use Q for Spellweaving", false));
            ComboMenu.AddSeparator(1);
            ComboMenu.Add("Lcombo", new Slider("Limit W if Health % - 0 is off", 25));

            // Harass Menu
            HarassMenu = DefenderTaricMenu.AddSubMenu("Harass Features", "HarassFeatures");
            HarassMenu.AddGroupLabel("Harass Features");
            HarassMenu.AddLabel("Independent boxes for Spells:");
            HarassMenu.Add("Wharass", new CheckBox("Use W"));
            HarassMenu.Add("Eharass", new CheckBox("Use E"));
            HarassMenu.AddSeparator(1);
            HarassMenu.Add("Mharass", new Slider("Mana Limiter at Mana %", 25));

            // Healing Menu
            HealingMenu = DefenderTaricMenu.AddSubMenu("Healing Features", "HealingFeatures");
            HealingMenu.AddGroupLabel("Healing Features");
            HealingMenu.Add("Uheal", new CheckBox("Use Q to heal"));
            HealingMenu.AddSeparator(1);
            HealingMenu.AddLabel("Auto Healer with Q");
            HealingMenu.Add("Qhealally", new Slider("Heal ally at Health % - 0 is off", 35));
            HealingMenu.Add("Qhealtaric", new Slider("Heal self at Health % - 0 is off", 20));
            HealingMenu.AddSeparator(1);
            HealingMenu.AddLabel("Healer Mana Limiter");
            HealingMenu.Add("Mheal", new Slider("Mana Limiter at Mana %", 1));

            // Drawing Menu
            DrawingMenu = DefenderTaricMenu.AddSubMenu("Drawing Features", "DrawingFeatures");
            DrawingMenu.AddGroupLabel("Drawing Features");
            DrawingMenu.Add("Udraw", new CheckBox("Draw Mode"));
            DrawingMenu.AddSeparator(1);
            DrawingMenu.AddLabel("Independent boxes for Spells:");
            DrawingMenu.Add("Qdraw", new CheckBox("Draw Q"));
            DrawingMenu.Add("Edraw", new CheckBox("Draw E"));
            DrawingMenu.Add("WRdraw", new CheckBox("Draw W & R"));
            DrawingMenu.AddSeparator(1);
            DrawingMenu.AddLabel("Skin Designer");
            DrawingMenu.Add("Udesign", new CheckBox("Draw Skin Design"));
            DrawingMenu.Add("Sdesign", new Slider("Skin Designer: ", 0, 0, 3));

            // Setting Menu
            SettingMenu = DefenderTaricMenu.AddSubMenu("Setting Mode", "SettingMode");
            SettingMenu.AddGroupLabel("Setting Mode");
            SettingMenu.AddLabel("Automatic Leveler");
            SettingMenu.Add("Ulevel", new CheckBox("Auto Leveler"));
            SettingMenu.AddSeparator(1);
            SettingMenu.AddLabel("Interrupter");
            SettingMenu.Add("Uinterrupt", new CheckBox("Interrupt Mode"));
            SettingMenu.Add("Einterrupt", new CheckBox("Use E to interrupt"));
            SettingMenu.AddLabel("Gap Closer");
            SettingMenu.Add("Ugapc", new CheckBox("Gap Closer Mode"));
            SettingMenu.Add("Egapc", new CheckBox("Use E to gapclose"));
        }

        // Assign Global Checks
        public static bool EasterEgg { get { return DefenderTaricMenu["Ueaster"].Cast<CheckBox>().CurrentValue; } }

        public static int ComboMode { get { return ComboMenu["Ucombo"].Cast<Slider>().CurrentValue; } }
        public static bool ComboUseW { get { return ComboMenu["Wcombo"].Cast<CheckBox>().CurrentValue; } }
        public static bool ComboUseE { get { return ComboMenu["Ecombo"].Cast<CheckBox>().CurrentValue; } }
        public static bool ComboUseR { get { return ComboMenu["Rcombo"].Cast<CheckBox>().CurrentValue; } }
        public static int ComboWLimit { get { return ComboMenu["Lcombo"].Cast<Slider>().CurrentValue; } }
        public static bool SpellWeave { get { return ComboMenu["Uweave"].Cast<CheckBox>().CurrentValue; } }
        public static bool SpellWeaveUseQ { get { return ComboMenu["Qweave"].Cast<CheckBox>().CurrentValue; } }

        public static bool HarassUseW { get { return HarassMenu["Wharass"].Cast<CheckBox>().CurrentValue; } }
        public static bool HarassUseE { get { return HarassMenu["Eharass"].Cast<CheckBox>().CurrentValue; } }
        public static int HarassMana { get { return HarassMenu["Mharass"].Cast<Slider>().CurrentValue; } }

        public static bool HealingMode { get { return HealingMenu["Uheal"].Cast<CheckBox>().CurrentValue; } }
        public static int HealingAlly { get { return HealingMenu["Qhealally"].Cast<Slider>().CurrentValue; } }
        public static int HealingSelf { get { return HealingMenu["Qhealtaric"].Cast<Slider>().CurrentValue; } }
        public static int HealingMana { get { return HealingMenu["Mheal"].Cast<Slider>().CurrentValue; } }

        public static bool DrawerMode { get { return DrawingMenu["Udraw"].Cast<CheckBox>().CurrentValue; } }
        public static bool DrawQ { get { return DrawingMenu["Qdraw"].Cast<CheckBox>().CurrentValue; } }
        public static bool DrawE { get { return DrawingMenu["Edraw"].Cast<CheckBox>().CurrentValue; } }
        public static bool DrawWr { get { return DrawingMenu["WRdraw"].Cast<CheckBox>().CurrentValue; } }
        public static bool DesignerMode { get { return DrawingMenu["Udesign"].Cast<CheckBox>().CurrentValue; } }
        public static int DesignerSkin { get { return DrawingMenu["Sdesign"].Cast<Slider>().CurrentValue; } }

        public static bool LevelerMode { get { return SettingMenu["Ulevel"].Cast<CheckBox>().CurrentValue; } }

        public static bool InterrupterMode { get { return SettingMenu["Uinterrupt"].Cast<CheckBox>().CurrentValue; } }
        public static bool InterrupterUseE { get { return SettingMenu["Einterrupt"].Cast<CheckBox>().CurrentValue; } }
        public static bool GapCloserMode { get { return SettingMenu["Ugapc"].Cast<CheckBox>().CurrentValue; } }
        public static bool GapCloserUseE { get { return SettingMenu["Egapc"].Cast<CheckBox>().CurrentValue; } }
    }
}
