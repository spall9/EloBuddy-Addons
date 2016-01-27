using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace AlchemistSinged
{
    internal class MenuManager
    {
        // Create Main Segments
        public static Menu AlchemistSingedMenu, ComboMenu, HarassMenu, JungleMenu, LaneClearMenu, LastHitMenu, DrawingMenu, SettingMenu;

        public static void Initialize()
        {
            // Addon Menu
            AlchemistSingedMenu = MainMenu.AddMenu("AlchemistSinged", "AlchemistSinged");
            AlchemistSingedMenu.AddGroupLabel("Alchemist Singed");

            // Combo Menu
            ComboMenu = AlchemistSingedMenu.AddSubMenu("Combo Features", "ComboFeatures");
            ComboMenu.AddGroupLabel("Combo Features");
            ComboMenu.AddLabel("Combo Modes:");
            ComboMenu.Add("ComboM", new CheckBox("ComboMode"));
            ComboMenu.AddSeparator(1);
            ComboMenu.AddLabel("Independent boxes for Spells:");
            ComboMenu.Add("Qcombo", new CheckBox("Use Q"));
            ComboMenu.Add("Wcombo", new CheckBox("Use W"));
            ComboMenu.Add("Ecombo", new CheckBox("Use E"));
            ComboMenu.Add("Rcombo", new CheckBox("Use R"));
            ComboMenu.AddSeparator(1);
            ComboMenu.Add("Rlimiter", new Slider("Use R if Enemies in Range >= ", 3, 1, 5));

            // Harass Menu
            HarassMenu = AlchemistSingedMenu.AddSubMenu("Harass Features", "HarassFeatures");
            HarassMenu.AddGroupLabel("Harass Features");
            HarassMenu.AddLabel("Independent boxes for Spells:");
            HarassMenu.Add("Qharass", new CheckBox("Use Q"));

            // Jungle Menu
            JungleMenu = AlchemistSingedMenu.AddSubMenu("Jungle Features", "JungleFeatures");
            JungleMenu.AddGroupLabel("Jungle Features");
            JungleMenu.AddLabel("Independent boxes for Spells:");
            JungleMenu.Add("Qjungle", new CheckBox("Use Q"));
            JungleMenu.Add("Ejungle", new CheckBox("Use E"));

            // LaneClear Menu
            LaneClearMenu = AlchemistSingedMenu.AddSubMenu("Lane Clear Features", "LaneClearFeatures");
            LaneClearMenu.AddGroupLabel("Lane Clear Features");
            LaneClearMenu.AddLabel("Independent boxes for Spells:");
            LaneClearMenu.Add("Qlanec", new CheckBox("Use Q"));
            LaneClearMenu.Add("Elanec", new CheckBox("Use E", false));
            LaneClearMenu.AddSeparator(1);
            LaneClearMenu.AddLabel("AutoAttack Disabler for LaneClear - Easier for Q farm");
            LaneClearMenu.Add("AAdisable", new CheckBox("Disable AA"));

            // LastHit Menu
            LastHitMenu = AlchemistSingedMenu.AddSubMenu("Last Hit Features", "LastHitFeatures");
            LastHitMenu.AddGroupLabel("Last Hit Features");
            LastHitMenu.AddLabel("Independent boxes for Spells:");
            LastHitMenu.Add("Elasthit", new CheckBox("Use E"));

            // Drawing Menu
            DrawingMenu = AlchemistSingedMenu.AddSubMenu("Drawing Features", "DrawingFeatures");
            DrawingMenu.AddGroupLabel("Drawing Features");
            DrawingMenu.Add("DrawM", new CheckBox("Draw Mode"));
            DrawingMenu.AddSeparator(1);
            DrawingMenu.AddLabel("Independent boxes for Spells:");
            DrawingMenu.Add("Wdraw", new CheckBox("Draw W"));
            DrawingMenu.Add("Edraw", new CheckBox("Draw E"));
            DrawingMenu.AddSeparator(1);
            DrawingMenu.AddLabel("Skin Designer");
            DrawingMenu.Add("DrawS", new CheckBox("Draw Skin Design", false));
            DrawingMenu.Add("Skins", new Slider("Skin Designer: ", 0, 0, 7));

            // Setting Menu
            SettingMenu = AlchemistSingedMenu.AddSubMenu("Settings", "Settings");
            SettingMenu.AddGroupLabel("Settings");
            SettingMenu.AddLabel("Automatic Leveler");
            SettingMenu.Add("Autolvl", new CheckBox("Auto Leveler"));
            SettingMenu.AddSeparator(1);
            SettingMenu.AddLabel("Poison Trail Kiter");
            SettingMenu.Add("Kite", new CheckBox("Use Q to Kite"));
            SettingMenu.AddLabel("Automatic Tear Stacker");
            SettingMenu.Add("StackM", new CheckBox("Stack Mode"));
            SettingMenu.AddSeparator(1);
            SettingMenu.AddLabel("Interrupter");
            SettingMenu.Add("Interruptmode", new CheckBox("Interrupt Mode"));
            SettingMenu.Add("Einterrupt", new CheckBox("Use E to interrupt"));
            SettingMenu.AddLabel("Gap Closer");
            SettingMenu.Add("Gapcmode", new CheckBox("Gap Closer Mode"));
            SettingMenu.Add("Egapc", new CheckBox("Use E to gapclose"));
        }
    }
}
