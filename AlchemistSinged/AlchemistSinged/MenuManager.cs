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
            ComboMenu.AddLabel("Independent boxes for Spells:");
            ComboMenu.Add("Qcombo", new CheckBox("Use Q"));
            ComboMenu.Add("Wcombo", new CheckBox("Use W"));
            ComboMenu.Add("Ecombo", new CheckBox("Use E"));
            ComboMenu.Add("Rcombo", new CheckBox("Use R"));
            ComboMenu.AddSeparator(1);
            ComboMenu.Add("Lcombo", new Slider("Use R if Enemies in Range >= ", 3, 1, 5));

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
            DrawingMenu.Add("Udraw", new CheckBox("Draw Mode"));
            DrawingMenu.AddSeparator(1);
            DrawingMenu.AddLabel("Independent boxes for Spells:");
            DrawingMenu.Add("Wdraw", new CheckBox("Draw W"));
            DrawingMenu.Add("Edraw", new CheckBox("Draw E"));
            DrawingMenu.AddSeparator(1);
            DrawingMenu.AddLabel("Skin Designer");
            DrawingMenu.Add("Udesign", new CheckBox("Draw Skin Design"));
            DrawingMenu.Add("Sdesign", new Slider("Skin Designer: ", 0, 0, 7));

            // Setting Menu
            SettingMenu = AlchemistSingedMenu.AddSubMenu("Settings", "Settings");
            SettingMenu.AddGroupLabel("Settings");
            SettingMenu.AddLabel("Automatic Leveler");
            SettingMenu.Add("Ulevel", new CheckBox("Auto Leveler"));
            SettingMenu.AddSeparator(1);
            SettingMenu.AddLabel("Poison Trail Kiter");
            SettingMenu.Add("Ukite", new CheckBox("Use Q to Kite"));
            SettingMenu.AddLabel("Automatic Tear Stacker");
            SettingMenu.Add("Ustack", new CheckBox("Stack Mode"));
            SettingMenu.AddSeparator(1);
            SettingMenu.AddLabel("Interrupter");
            SettingMenu.Add("Uinterrupt", new CheckBox("Interrupt Mode"));
            SettingMenu.Add("Einterrupt", new CheckBox("Use E to interrupt"));
            SettingMenu.AddLabel("Gap Closer");
            SettingMenu.Add("Ugapc", new CheckBox("Gap Closer Mode"));
            SettingMenu.Add("Egapc", new CheckBox("Use E to gapclose"));
        }

        // Assign Global Checks+
        public static bool ComboUseQ { get { return ComboMenu["Qcombo"].Cast<CheckBox>().CurrentValue; } }
        public static bool ComboUseW { get { return ComboMenu["Wcombo"].Cast<CheckBox>().CurrentValue; } }
        public static bool ComboUseE { get { return ComboMenu["Ecombo"].Cast<CheckBox>().CurrentValue; } }
        public static bool ComboUseR { get { return ComboMenu["Rcombo"].Cast<CheckBox>().CurrentValue; } }
        public static int ComboRLimiter { get { return ComboMenu["Lcombo"].Cast<Slider>().CurrentValue; } }

        public static bool HarassUseQ { get { return HarassMenu["Qharass"].Cast<CheckBox>().CurrentValue; } }

        public static bool JungleUseQ { get { return JungleMenu["Qjungle"].Cast<CheckBox>().CurrentValue; } }
        public static bool JungleUseE { get { return JungleMenu["Ejungle"].Cast<CheckBox>().CurrentValue; } }
        
        public static bool LaneClearUseQ { get { return LaneClearMenu["Qlanec"].Cast<CheckBox>().CurrentValue; } }
        public static bool LaneClearUseE { get { return LaneClearMenu["Elanec"].Cast<CheckBox>().CurrentValue; } }
        public static bool LaneClearAaDisable { get { return LaneClearMenu["AAdisable"].Cast<CheckBox>().CurrentValue; } }
        
        public static bool LateGameMode { get { return LaneClearMenu["Ulategame"].Cast<CheckBox>().CurrentValue; } }
        public static int LateGameLevel { get { return LaneClearMenu["Llategame"].Cast<Slider>().CurrentValue; } }
        public static int LateGameMana { get { return LaneClearMenu["Mlategame"].Cast<Slider>().CurrentValue; } }

        public static bool LastHitUseE { get { return LastHitMenu["Elasthit"].Cast<CheckBox>().CurrentValue; } }
        
        public static bool DrawMode { get { return DrawingMenu["Udraw"].Cast<CheckBox>().CurrentValue; } }
        public static bool DrawW { get { return DrawingMenu["Wdraw"].Cast<CheckBox>().CurrentValue; } }
        public static bool DrawE { get { return DrawingMenu["Edraw"].Cast<CheckBox>().CurrentValue; } }
        public static bool DesignerMode { get { return DrawingMenu["Udesign"].Cast<CheckBox>().CurrentValue; } }
        public static int DesignerSkin { get { return DrawingMenu["Sdesign"].Cast<Slider>().CurrentValue; } }

        public static bool LevelerMode { get { return SettingMenu["Ulevel"].Cast<CheckBox>().CurrentValue; } }
        public static bool StackMode { get { return SettingMenu["Ustack"].Cast<CheckBox>().CurrentValue; } }
        public static bool KiteMode { get { return SettingMenu["Ukite"].Cast<CheckBox>().CurrentValue; } }

        public static bool InterrupterMode { get { return SettingMenu["Uinterrupt"].Cast<CheckBox>().CurrentValue; } }
        public static bool InterrupterUseE { get { return SettingMenu["Einterrupt"].Cast<CheckBox>().CurrentValue; } }
        public static bool GapCloserMode { get { return SettingMenu["Ugapc"].Cast<CheckBox>().CurrentValue; } }
        public static bool GapCloserUseE { get { return SettingMenu["Egapc"].Cast<CheckBox>().CurrentValue; } }
    }
}
