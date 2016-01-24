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
            ComboMenu.AddLabel("Combo Modes:");
            ComboMenu.Add("ComboM", new CheckBox("ComboMode"));
            ComboMenu.Add("ComboF", new Slider("Counter Combo - My Personal Settings", 1, 1, 2));
            ComboMenu.AddSeparator(1);
            ComboMenu.AddLabel("Independent boxes for Spells:");
            ComboMenu.Add("Qcombo", new CheckBox("Use Q"));
            ComboMenu.Add("Wcombo", new CheckBox("Use W"));
            ComboMenu.Add("Ecombo", new CheckBox("Use E"));
            ComboMenu.Add("Rcombo", new CheckBox("Use R"));
            ComboMenu.Add("Rstun", new CheckBox("Only R if Target Rooted"));
            ComboMenu.AddSeparator(1);
            ComboMenu.Add("Pult", new Slider("Passive stacks to Ult", 4, 0, 4));

            // Harass Menu
            HarassMenu = MagicianRyzeMenu.AddSubMenu("Harass Features", "HarassFeatures");
            HarassMenu.AddGroupLabel("Harass Features");
            HarassMenu.AddLabel("Independent boxes for Spells:");
            HarassMenu.Add("Qharass", new CheckBox("Use Q"));
            HarassMenu.AddSeparator(1);
            HarassMenu.Add("Harassmana", new Slider("Mana Limiter at Mana %", 25));

            // Jungle Menu
            JungleMenu = MagicianRyzeMenu.AddSubMenu("Jungle Features", "JungleFeatures");
            JungleMenu.AddGroupLabel("Jungle Features");
            JungleMenu.AddLabel("Independent boxes for Spells:");
            JungleMenu.Add("Qjungle", new CheckBox("Use Q"));
            JungleMenu.Add("Wjungle", new CheckBox("Use W"));
            JungleMenu.Add("Ejungle", new CheckBox("Use E"));
            JungleMenu.AddSeparator(1);
            JungleMenu.Add("Junglemana", new Slider("Mana Limiter at Mana %", 25));

            // LaneClear Menu
            LaneClearMenu = MagicianRyzeMenu.AddSubMenu("Lane Clear Features", "LaneClearFeatures");
            LaneClearMenu.AddGroupLabel("Lane Clear Features");
            LaneClearMenu.AddLabel("Independent boxes for Spells:");
            LaneClearMenu.Add("Qlanec", new CheckBox("Use Q"));
            LaneClearMenu.Add("Wlanec", new CheckBox("Use W", false));
            LaneClearMenu.Add("Elanec", new CheckBox("Use E", false));
            LaneClearMenu.AddSeparator(1);
            LaneClearMenu.Add("Lanecmana", new Slider("Mana Limiter at Mana %", 25));
            LaneClearMenu.AddSeparator(1);
            LaneClearMenu.AddLabel("Late Game Lane Clear Mode - QWE minions to Mana %");
            LaneClearMenu.Add("LGlanec", new CheckBox("Late Game Mode"));
            LaneClearMenu.AddSeparator(1);
            LaneClearMenu.AddLabel("Late Game Mode Activators");
            LaneClearMenu.Add("LGlevel", new Slider("Activate Late Game at Level", 14, 1, 18));
            LaneClearMenu.Add("LGmana", new Slider("Mana Limiter at Mana %", 20));
            LaneClearMenu.AddSeparator(1);
            LaneClearMenu.Add("Planec", new CheckBox("Charge Passive in Lane Clear", false));

            // LastHit Menu
            LastHitMenu = MagicianRyzeMenu.AddSubMenu("Last Hit Features", "LastHitFeatures");
            LastHitMenu.AddGroupLabel("Last Hit Features");
            LastHitMenu.AddLabel("Independent boxes for Spells:");
            LastHitMenu.Add("Qlasthit", new CheckBox("Use Q"));
            LastHitMenu.Add("Wlasthit", new CheckBox("Use W", false));
            LastHitMenu.Add("Elasthit", new CheckBox("Use E", false));
            LastHitMenu.AddSeparator(1);
            LastHitMenu.Add("Lasthitmana", new Slider("Mana Limiter at Mana %", 25));

            // Kill Steal Menu
            KillStealMenu = MagicianRyzeMenu.AddSubMenu("KS Features", "KSFeatures");
            KillStealMenu.AddGroupLabel("Kill Steal Features");
            KillStealMenu.Add("KSmode", new CheckBox("KS Mode"));
            KillStealMenu.AddSeparator(1);
            KillStealMenu.AddLabel("Independent boxes for Spells:");
            KillStealMenu.Add("Qks", new CheckBox("Use Q in KS"));
            KillStealMenu.Add("Wks", new CheckBox("Use W in KS"));
            KillStealMenu.Add("Eks", new CheckBox("Use E in KS"));

            // Drawing Menu
            DrawingMenu = MagicianRyzeMenu.AddSubMenu("Drawing Features", "DrawingFeatures");
            DrawingMenu.AddGroupLabel("Drawing Features");
            DrawingMenu.Add("DrawM", new CheckBox("Draw Mode"));
            DrawingMenu.AddSeparator(1);
            DrawingMenu.AddLabel("Independent boxes for Spells:");
            DrawingMenu.Add("Qdraw", new CheckBox("Draw Q"));
            DrawingMenu.Add("WEdraw", new CheckBox("Draw W & E"));
            DrawingMenu.AddSeparator(1);
            DrawingMenu.AddLabel("Skin Designer");
            DrawingMenu.Add("DrawS", new CheckBox("Draw Skin Design", false));
            DrawingMenu.Add("Skins", new Slider("Skin Designer: ", 9, 0, 9));

            // Setting Menu
            SettingMenu = MagicianRyzeMenu.AddSubMenu("Settings", "Settings");
            SettingMenu.AddGroupLabel("Settings");
            SettingMenu.AddLabel("Automatic Leveler");
            SettingMenu.Add("Autolvl", new CheckBox("Auto Leveler"));
            SettingMenu.AddSeparator(1);
            SettingMenu.AddLabel("Ultimate Mode - QWE");
            SettingMenu.Add("UltM", new CheckBox("Ultimate Mode"));
            SettingMenu.AddLabel("Automatic Tear Stacker");
            SettingMenu.Add("StackM", new CheckBox("Stack Mode"));
            SettingMenu.AddSeparator(1);
            SettingMenu.AddLabel("Interrupter");
            SettingMenu.Add("Interruptmode", new CheckBox("Interrupt Mode"));
            SettingMenu.Add("Winterrupt", new CheckBox("Use W to interrupt"));
            SettingMenu.AddLabel("Gap Closer");
            SettingMenu.Add("Gapcmode", new CheckBox("Gap Closer Mode"));
            SettingMenu.Add("Wgapc", new CheckBox("Use W to gapclose"));
        }
    }
}
