using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace ProtectorLeona
{
    internal class MenuManager
    {
        // Create Main Segments
        public static Menu ProtectorLeonaMenu, ComboMenu, HarassMenu, DrawingMenu, SettingMenu;

        public static void Initialize()
        {
            // Addon Menu
            ProtectorLeonaMenu = MainMenu.AddMenu("ProtectorLeona", "ProtectorLeona");
            ProtectorLeonaMenu.AddGroupLabel("Protector Leona");

            // Combo Menu
            ComboMenu = ProtectorLeonaMenu.AddSubMenu("Combo Features", "ComboFeatures");
            ComboMenu.AddGroupLabel("Combo Features");
            ComboMenu.AddLabel("Independent boxes for Spells:");
            ComboMenu.Add("Qcombo", new CheckBox("Use Q"));
            ComboMenu.Add("Wcombo", new CheckBox("Use W"));
            ComboMenu.Add("Ecombo", new CheckBox("Use E"));
            ComboMenu.Add("Rcombo", new CheckBox("Use R"));
            ComboMenu.AddSeparator(1);
            ComboMenu.Add("Rlimit", new Slider("Use R when Enemies in range - greater or equal to:", 2, 1, 5));

            // Harass Menu
            HarassMenu = ProtectorLeonaMenu.AddSubMenu("Harass Features", "HarassFeatures");
            HarassMenu.AddGroupLabel("Harass Features");
            HarassMenu.AddLabel("Independent boxes for Spells:");
            HarassMenu.Add("Qharass", new CheckBox("Use Q"));
            HarassMenu.Add("Eharass", new CheckBox("Use E"));
            HarassMenu.AddSeparator(1);
            HarassMenu.Add("Mharass", new Slider("Mana Limiter at Mana %", 25));

            // Drawing Menu
            DrawingMenu = ProtectorLeonaMenu.AddSubMenu("Drawing Features", "DrawingFeatures");
            DrawingMenu.AddGroupLabel("Drawing Features");
            DrawingMenu.Add("Udraw", new CheckBox("Draw Mode"));
            DrawingMenu.AddSeparator(1);
            DrawingMenu.AddLabel("Independent boxes for Spells:");
            DrawingMenu.Add("Qdraw", new CheckBox("Draw Q", false));
            DrawingMenu.Add("Wdraw", new CheckBox("Draw W", false));
            DrawingMenu.Add("Edraw", new CheckBox("Draw E"));
            DrawingMenu.Add("Rdraw", new CheckBox("Draw R"));
            DrawingMenu.AddSeparator(1);
            DrawingMenu.AddLabel("Skin Designer");
            DrawingMenu.Add("Udesign", new CheckBox("Draw Skin Design", false));
            DrawingMenu.Add("Sdesign", new Slider("Skin Designer: ", 4, 0, 8));

            // Setting Menu
            SettingMenu = ProtectorLeonaMenu.AddSubMenu("Settings", "Settings");
            SettingMenu.AddGroupLabel("Settings");
            SettingMenu.AddLabel("Automatic Leveler");
            SettingMenu.Add("Ulevel", new CheckBox("Auto Leveler"));
            SettingMenu.AddSeparator(1);
            SettingMenu.AddLabel("Use AutoAttacks in Modes");
            SettingMenu.Add("Aattack", new CheckBox("Use AA"));
            SettingMenu.AddSeparator(1);
            SettingMenu.AddLabel("Interrupter");
            SettingMenu.Add("Uinterrupt", new CheckBox("Interrupt Mode"));
            SettingMenu.Add("EQinterrupt", new CheckBox("Use E & Q to interrupt"));
            SettingMenu.Add("Rinterrupt", new CheckBox("Use R to interrupt"));
            SettingMenu.AddLabel("Gap Closer");
            SettingMenu.Add("Ugapc", new CheckBox("Gap Closer Mode"));
            SettingMenu.Add("EQgapc", new CheckBox("Use E & Q to gapclose"));
        }

        // Assign Global Checks
        public static bool ComboUseQ { get { return ComboMenu["Qcombo"].Cast<CheckBox>().CurrentValue; } }
        public static bool ComboUseW { get { return ComboMenu["Wcombo"].Cast<CheckBox>().CurrentValue; } }
        public static bool ComboUseE { get { return ComboMenu["Ecombo"].Cast<CheckBox>().CurrentValue; } }
        public static bool ComboUseR { get { return ComboMenu["Rcombo"].Cast<CheckBox>().CurrentValue; } }
        public static int ComboRLimiter { get { return ComboMenu["Rlimit"].Cast<Slider>().CurrentValue; } }

        public static bool HarassUseQ { get { return HarassMenu["Qharass"].Cast<CheckBox>().CurrentValue; } }
        public static bool HarassUseE { get { return HarassMenu["Eharass"].Cast<CheckBox>().CurrentValue; } }
        public static int HarassMana { get { return HarassMenu["Mharass"].Cast<Slider>().CurrentValue; } }

        public static bool DrawerMode { get { return DrawingMenu["Udraw"].Cast<CheckBox>().CurrentValue; } }
        public static bool DrawQ { get { return DrawingMenu["Qdraw"].Cast<CheckBox>().CurrentValue; } }
        public static bool DrawW { get { return DrawingMenu["Wdraw"].Cast<CheckBox>().CurrentValue; } }
        public static bool DrawE { get { return DrawingMenu["Edraw"].Cast<CheckBox>().CurrentValue; } }
        public static bool DrawR { get { return DrawingMenu["Rdraw"].Cast<CheckBox>().CurrentValue; } }
        public static bool DesignerMode { get { return DrawingMenu["Udesign"].Cast<CheckBox>().CurrentValue; } }
        public static int DesignerSkin { get { return DrawingMenu["Sdesign"].Cast<Slider>().CurrentValue; } }

        public static bool LevelerMode { get { return SettingMenu["Ulevel"].Cast<CheckBox>().CurrentValue; } }
        public static bool AutoAttack { get { return SettingMenu["Aattack"].Cast<CheckBox>().CurrentValue; } }

        public static bool InterrupterMode { get { return SettingMenu["Uinterrupt"].Cast<CheckBox>().CurrentValue; } }
        public static bool InterrupterUseEq { get { return SettingMenu["EQinterrupt"].Cast<CheckBox>().CurrentValue; } }
        public static bool InterrupterUseR { get { return SettingMenu["Rinterrupt"].Cast<CheckBox>().CurrentValue; } }
        public static bool GapCloserMode { get { return SettingMenu["Ugapc"].Cast<CheckBox>().CurrentValue; } }
        public static bool GapCloserUseEq { get { return SettingMenu["EQgapc"].Cast<CheckBox>().CurrentValue; } }
    }
}
