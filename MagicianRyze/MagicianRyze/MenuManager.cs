using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace MagicianRyze
{
    internal class MenuManager
    {
        // Create Main Segments
        public static Menu MagicianRyzeMenu, ComboMenu, HarassMenu, JungleMenu, LaneClearMenu, LastHitMenu, KillStealMenu, DrawingMenu, SettingMenu;

        public static void Initialize()
        {
            // Addon Menu
            MagicianRyzeMenu = MainMenu.AddMenu("MagicianRyze", "MagicianRyze");
            MagicianRyzeMenu.AddGroupLabel("Magician Ryze");

            // Combo Menu
            ComboMenu = MagicianRyzeMenu.AddSubMenu("Combo Features", "ComboFeatures");
            ComboMenu.AddGroupLabel("Combo Features");
            ComboMenu.Add("Ucombo", new Slider("Counter Combo - My Personal Settings", 1, 1, 2));
            ComboMenu.AddSeparator(1);
            ComboMenu.AddLabel("Independent boxes for Spells:");
            ComboMenu.Add("Qcombo", new CheckBox("Use Q"));
            ComboMenu.Add("Wcombo", new CheckBox("Use W"));
            ComboMenu.Add("Ecombo", new CheckBox("Use E"));
            ComboMenu.Add("Rcombo", new CheckBox("Use R"));
            ComboMenu.Add("Dcombo", new CheckBox("Only R if Target Rooted"));
            ComboMenu.AddSeparator(1);
            ComboMenu.Add("Scombo", new Slider("Passive stacks to Ult", 4, 1, 4));

            // Harass Menu
            HarassMenu = MagicianRyzeMenu.AddSubMenu("Harass Features", "HarassFeatures");
            HarassMenu.AddGroupLabel("Harass Features");
            HarassMenu.AddLabel("Independent boxes for Spells:");
            HarassMenu.Add("Qharass", new CheckBox("Use Q"));
            HarassMenu.AddSeparator(1);
            HarassMenu.Add("Mharass", new Slider("Mana Limiter at Mana %", 25));

            // Jungle Menu
            JungleMenu = MagicianRyzeMenu.AddSubMenu("Jungle Features", "JungleFeatures");
            JungleMenu.AddGroupLabel("Jungle Features");
            JungleMenu.AddLabel("Independent boxes for Spells:");
            JungleMenu.Add("Qjungle", new CheckBox("Use Q"));
            JungleMenu.Add("Wjungle", new CheckBox("Use W"));
            JungleMenu.Add("Ejungle", new CheckBox("Use E"));
            JungleMenu.AddSeparator(1);
            JungleMenu.Add("Mjungle", new Slider("Mana Limiter at Mana %", 25));

            // LaneClear Menu
            LaneClearMenu = MagicianRyzeMenu.AddSubMenu("Lane Clear Features", "LaneClearFeatures");
            LaneClearMenu.AddGroupLabel("Lane Clear Features");
            LaneClearMenu.AddLabel("Independent boxes for Spells:");
            LaneClearMenu.Add("Qlanec", new CheckBox("Use Q"));
            LaneClearMenu.Add("Wlanec", new CheckBox("Use W", false));
            LaneClearMenu.Add("Elanec", new CheckBox("Use E", false));
            LaneClearMenu.AddSeparator(1);
            LaneClearMenu.Add("Mlanec", new Slider("Mana Limiter at Mana %", 25));
            LaneClearMenu.AddSeparator(1);
            LaneClearMenu.Add("Planec", new CheckBox("Charge Passive in Lane Clear"));
            LaneClearMenu.AddSeparator(1);
            LaneClearMenu.AddLabel("Late Game Lane Clear Mode - QWE minions to Mana %");
            LaneClearMenu.Add("Ulategame", new CheckBox("Late Game Mode", false));
            LaneClearMenu.AddSeparator(1);
            LaneClearMenu.AddLabel("Late Game Mode Activators");
            LaneClearMenu.Add("Llategame", new Slider("Activate Late Game at Level", 14, 1, 18));
            LaneClearMenu.Add("Mlategame", new Slider("Mana Limiter at Mana %", 15));

            // LastHit Menu
            LastHitMenu = MagicianRyzeMenu.AddSubMenu("Last Hit Features", "LastHitFeatures");
            LastHitMenu.AddGroupLabel("Last Hit Features");
            LastHitMenu.AddLabel("Independent boxes for Spells:");
            LastHitMenu.Add("Qlasthit", new CheckBox("Use Q"));
            LastHitMenu.Add("Wlasthit", new CheckBox("Use W", false));
            LastHitMenu.Add("Elasthit", new CheckBox("Use E", false));
            LastHitMenu.AddSeparator(1);
            LastHitMenu.Add("Mlasthit", new Slider("Mana Limiter at Mana %", 25));

            // Kill Steal Menu
            KillStealMenu = MagicianRyzeMenu.AddSubMenu("KS Features", "KSFeatures");
            KillStealMenu.AddGroupLabel("Kill Steal Features");
            KillStealMenu.Add("Uks", new CheckBox("KS Mode"));
            KillStealMenu.AddSeparator(1);
            KillStealMenu.AddLabel("Independent boxes for Spells:");
            KillStealMenu.Add("Qks", new CheckBox("Use Q in KS"));
            KillStealMenu.Add("Wks", new CheckBox("Use W in KS"));
            KillStealMenu.Add("Eks", new CheckBox("Use E in KS"));

            // Drawing Menu
            DrawingMenu = MagicianRyzeMenu.AddSubMenu("Drawing Features", "DrawingFeatures");
            DrawingMenu.AddGroupLabel("Drawing Features");
            DrawingMenu.Add("Udraw", new CheckBox("Draw Mode"));
            DrawingMenu.AddSeparator(1);
            DrawingMenu.AddLabel("Independent boxes for Spells:");
            DrawingMenu.Add("Qdraw", new CheckBox("Draw Q"));
            DrawingMenu.Add("WEdraw", new CheckBox("Draw W & E"));
            DrawingMenu.AddSeparator(1);
            DrawingMenu.AddLabel("Skin Designer");
            DrawingMenu.Add("Udesign", new CheckBox("Draw Skin Design"));
            DrawingMenu.Add("Sdesign", new Slider("Skin Designer: ", 9, 0, 9));

            // Setting Menu
            SettingMenu = MagicianRyzeMenu.AddSubMenu("Settings", "Settings");
            SettingMenu.AddGroupLabel("Settings");
            SettingMenu.AddLabel("Automatic Leveler");
            SettingMenu.Add("Ulevel", new CheckBox("Auto Leveler"));
            SettingMenu.AddSeparator(1);
            SettingMenu.AddLabel("Automatic Tear Stacker");
            SettingMenu.Add("Ustack", new CheckBox("Stack Mode"));
            SettingMenu.AddSeparator(1);
            SettingMenu.AddLabel("Ultimate Mode - If Ult is active, spam QWE");
            SettingMenu.Add("Uultimate", new CheckBox("Ultimate Mode", false));
            SettingMenu.AddSeparator(1);
            SettingMenu.AddLabel("Interrupter");
            SettingMenu.Add("Uinterrupt", new CheckBox("Interrupt Mode"));
            SettingMenu.Add("Winterrupt", new CheckBox("Use W to interrupt"));
            SettingMenu.AddLabel("Gap Closer");
            SettingMenu.Add("Ugapc", new CheckBox("Gap Closer Mode"));
            SettingMenu.Add("Wgapc", new CheckBox("Use W to gapclose"));
        }

        // Assign Global Checks+
        public static int ComboMode { get { return ComboMenu["Ucombo"].Cast<Slider>().CurrentValue; } }
        public static bool ComboUseQ { get { return ComboMenu["Qcombo"].Cast<CheckBox>().CurrentValue; } }
        public static bool ComboUseW { get { return ComboMenu["Wcombo"].Cast<CheckBox>().CurrentValue; } }
        public static bool ComboUseE { get { return ComboMenu["Ecombo"].Cast<CheckBox>().CurrentValue; } }
        public static bool ComboUseR { get { return ComboMenu["Rcombo"].Cast<CheckBox>().CurrentValue; } }
        public static bool ComboStun { get { return ComboMenu["Dcombo"].Cast<CheckBox>().CurrentValue; } }
        public static int ComboStacks { get { return ComboMenu["Scombo"].Cast<Slider>().CurrentValue; } }

        public static bool HarassUseQ { get { return HarassMenu["Qharass"].Cast<CheckBox>().CurrentValue; } }
        public static int HarassMana { get { return HarassMenu["Mharass"].Cast<Slider>().CurrentValue; } }

        public static bool JungleUseQ { get { return JungleMenu["Qjungle"].Cast<CheckBox>().CurrentValue; } }
        public static bool JungleUseW { get { return JungleMenu["Wjungle"].Cast<CheckBox>().CurrentValue; } }
        public static bool JungleUseE { get { return JungleMenu["Ejungle"].Cast<CheckBox>().CurrentValue; } }
        public static int JungleMana { get { return JungleMenu["Mjungle"].Cast<Slider>().CurrentValue; } }

        public static bool LaneClearUseQ { get { return LaneClearMenu["Qlanec"].Cast<CheckBox>().CurrentValue; } }
        public static bool LaneClearUseW { get { return LaneClearMenu["Wlanec"].Cast<CheckBox>().CurrentValue; } }
        public static bool LaneClearUseE { get { return LaneClearMenu["Elanec"].Cast<CheckBox>().CurrentValue; } }
        public static bool LaneClearPassive { get { return LaneClearMenu["Planec"].Cast<CheckBox>().CurrentValue; } }
        public static int LaneClearMana { get { return LaneClearMenu["Mlanec"].Cast<Slider>().CurrentValue; } }

        public static bool LateGameMode { get { return LaneClearMenu["Ulategame"].Cast<CheckBox>().CurrentValue; } }
        public static int LateGameLevel { get { return LaneClearMenu["Llategame"].Cast<Slider>().CurrentValue; } }
        public static int LateGameMana { get { return LaneClearMenu["Mlategame"].Cast<Slider>().CurrentValue; } }
        
        public static bool LastHitUseQ { get { return LastHitMenu["Qlasthit"].Cast<CheckBox>().CurrentValue; } }
        public static bool LastHitUseW { get { return LastHitMenu["Wlasthit"].Cast<CheckBox>().CurrentValue; } }
        public static bool LastHitUseE { get { return LastHitMenu["Elasthit"].Cast<CheckBox>().CurrentValue; } }
        public static int LastHitMana { get { return LastHitMenu["Mlasthit"].Cast<Slider>().CurrentValue; } }

        public static bool KsMode { get { return KillStealMenu["Uks"].Cast<CheckBox>().CurrentValue; } }
        public static bool KsUseQ { get { return KillStealMenu["Qks"].Cast<CheckBox>().CurrentValue; } }
        public static bool KsUseW { get { return KillStealMenu["Wks"].Cast<CheckBox>().CurrentValue; } }
        public static bool KsUseE { get { return KillStealMenu["Eks"].Cast<CheckBox>().CurrentValue; } }

        public static bool DrawMode { get { return DrawingMenu["Udraw"].Cast<CheckBox>().CurrentValue; } }
        public static bool DrawQ { get { return DrawingMenu["Qdraw"].Cast<CheckBox>().CurrentValue; } }
        public static bool DrawWe { get { return DrawingMenu["WEdraw"].Cast<CheckBox>().CurrentValue; } }
        public static bool DesignerMode { get { return DrawingMenu["Udesign"].Cast<CheckBox>().CurrentValue; } }
        public static int DesignerSkin { get { return DrawingMenu["Sdesign"].Cast<Slider>().CurrentValue; } }

        public static bool LevelerMode { get { return SettingMenu["Ulevel"].Cast<CheckBox>().CurrentValue; } }
        public static bool StackMode { get { return SettingMenu["Ustack"].Cast<CheckBox>().CurrentValue; } }
        public static bool UltimateMode { get { return SettingMenu["Uultimate"].Cast<CheckBox>().CurrentValue; } }

        public static bool InterrupterMode { get { return SettingMenu["Uinterrupt"].Cast<CheckBox>().CurrentValue; } }
        public static bool InterrupterUseW { get { return SettingMenu["Winterrupt"].Cast<CheckBox>().CurrentValue; } }
        public static bool GapCloserMode { get { return SettingMenu["Ugapc"].Cast<CheckBox>().CurrentValue; } }
        public static bool GapCloserUseW { get { return SettingMenu["Wgapc"].Cast<CheckBox>().CurrentValue; } }
    }
}
