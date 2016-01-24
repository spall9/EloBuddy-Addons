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
            DefenderTaricMenu.Add("Easter Egg", new CheckBox("Easter Egg"));

            // Combo Menu
            ComboMenu = DefenderTaricMenu.AddSubMenu("Combo Features", "ComboFeatures");
            ComboMenu.AddGroupLabel("Combo Feautres");
            ComboMenu.AddLabel("Combo Modes: - Use when ready to fully engage");
            ComboMenu.Add("ComboM", new CheckBox("ComboMode"));
            ComboMenu.Add("ComboF", new Slider("EWR Combo", 1, 1, 2));
            ComboMenu.AddSeparator(1);
            ComboMenu.AddLabel("Independent boxes for Spells:");
            ComboMenu.Add("Wcombo", new CheckBox("Use W"));
            ComboMenu.Add("Ecombo", new CheckBox("Use E"));
            ComboMenu.Add("Rcombo", new CheckBox("Use R"));
            ComboMenu.Add("Wlimit", new Slider("Limit W if Health % - 0 is off", 25));
            ComboMenu.AddSeparator(1);
            ComboMenu.AddLabel("Spell Weaving with AA");
            ComboMenu.Add("Pautos", new CheckBox("SpellWeaving"));
            ComboMenu.Add("Qpassive", new CheckBox("Use Q for Spellweaving", false));

            // Harass Menu
            HarassMenu = DefenderTaricMenu.AddSubMenu("Harass Features", "HarassFeatures");
            HarassMenu.AddGroupLabel("Harass Features");
            HarassMenu.AddLabel("Independent boxes for Spells:");
            HarassMenu.Add("Wharass", new CheckBox("Use W"));
            HarassMenu.Add("Eharass", new CheckBox("Use E"));
            HarassMenu.AddSeparator(1);
            HarassMenu.Add("Harassmana", new Slider("Mana Limiter at Mana %", 25));

            // Healing Menu
            HealingMenu = DefenderTaricMenu.AddSubMenu("Healing Features", "HealingFeatures");
            HealingMenu.AddGroupLabel("Healing Features");
            HealingMenu.Add("HealingM", new CheckBox("Use Q to heal"));
            HealingMenu.AddSeparator(1);
            HealingMenu.AddLabel("Auto Healer with Q");
            HealingMenu.Add("Qhealally", new Slider("Heal ally at Health % - 0 is off", 35));
            HealingMenu.Add("Qhealtaric", new Slider("Heal self at Health % - 0 is off", 20));
            HealingMenu.AddSeparator(1);
            HealingMenu.AddLabel("Healer Mana Limiter");
            HealingMenu.Add("Healingmana", new Slider("Mana Limiter at Mana %", 1));

            // Drawing Menu
            DrawingMenu = DefenderTaricMenu.AddSubMenu("Drawing Features", "DrawingFeatures");
            DrawingMenu.AddGroupLabel("Drawing Features");
            DrawingMenu.Add("DrawM", new CheckBox("Draw Mode"));
            DrawingMenu.AddSeparator(1);
            DrawingMenu.AddLabel("Independent boxes for Spells:");
            DrawingMenu.Add("Qdraw", new CheckBox("Draw Q"));
            DrawingMenu.Add("Edraw", new CheckBox("Draw E"));
            DrawingMenu.Add("WRdraw", new CheckBox("Draw W & R"));
            DrawingMenu.AddSeparator(1);
            DrawingMenu.AddLabel("Skin Designer");
            DrawingMenu.Add("DrawS", new CheckBox("Draw Skin Design"));
            DrawingMenu.Add("Skins", new Slider("Skin Designer: ", 0, 0, 3));

            // Setting Menu
            SettingMenu = DefenderTaricMenu.AddSubMenu("Setting Mode", "SettingMode");
            SettingMenu.AddGroupLabel("Setting Mode");
            SettingMenu.AddLabel("Automatic Leveler");
            SettingMenu.Add("Autolvl", new CheckBox("Auto Leveler"));
            SettingMenu.AddSeparator(1);
            SettingMenu.AddLabel("Interrupter");
            SettingMenu.Add("Interruptmode", new CheckBox("Interrupt Mode"));
            SettingMenu.Add("Einterrupt", new CheckBox("Use E to interrupt"));
            SettingMenu.AddSeparator(1);
            SettingMenu.AddLabel("Gap Closer");
            SettingMenu.Add("Gapcmode", new CheckBox("Gap Closer Mode"));
            SettingMenu.Add("Egapc", new CheckBox("Use E to gapclose"));
        }
    }
}
