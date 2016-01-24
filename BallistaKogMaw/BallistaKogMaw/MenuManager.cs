using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace BallistaKogMaw
{
    internal class MenuManager
    {
        // Create Main Segments
        public static Menu BallistaKogMawMenu, ComboMenu, HarassMenu, JungleMenu, LaneClearMenu, LastHitMenu, KillStealMenu, DrawingMenu, SettingMenu;

        public static void Initialize()
        {
            // Addon Menu
            BallistaKogMawMenu = MainMenu.AddMenu("BallistaKogMaw", "BallistaKogMaw");
            BallistaKogMawMenu.AddGroupLabel("Ballista Kog'Maw");

            // Combo Menu
            ComboMenu = BallistaKogMawMenu.AddSubMenu("Combo Features", "ComboFeatures");
            ComboMenu.AddGroupLabel("Combo Features");
            ComboMenu.AddLabel("Combo Modes:");
            ComboMenu.Add("ComboM", new CheckBox("ComboMode"));
            ComboMenu.AddSeparator(1);
            ComboMenu.AddLabel("Independent boxes for Spells:");
            ComboMenu.Add("Qcombo", new CheckBox("Use Q"));
            ComboMenu.Add("Wcombo", new CheckBox("Use W"));
            ComboMenu.Add("Ecombo", new CheckBox("Use E"));
            ComboMenu.Add("Rcombo", new CheckBox("Use R"));
            ComboMenu.Add("Ultcombo", new Slider("Max R Stacks", 2, 1, 10));

            // Harass Menu
            HarassMenu = BallistaKogMawMenu.AddSubMenu("Harass Features", "HarassFeatures");
            HarassMenu.AddGroupLabel("Harass Features");
            HarassMenu.AddLabel("Independent boxes for Spells:");
            HarassMenu.Add("Qharass", new CheckBox("Use Q"));
            HarassMenu.Add("Wharass", new CheckBox("Use W", false));
            HarassMenu.Add("Eharass", new CheckBox("Use E", false));
            HarassMenu.Add("Rharass", new CheckBox("Use R", false));
            HarassMenu.Add("Ultharass", new Slider("Max R Stacks", 1, 1, 10));
            HarassMenu.AddSeparator(1);
            HarassMenu.Add("Harassmana", new Slider("Mana Limiter at Mana %", 25));

            // Jungle Menu
            JungleMenu = BallistaKogMawMenu.AddSubMenu("Jungle Features", "JungleFeatures");
            JungleMenu.AddGroupLabel("Jungle Features");
            JungleMenu.AddLabel("Independent boxes for Spells:");
            JungleMenu.Add("Qjungle", new CheckBox("Use Q"));
            JungleMenu.Add("Wjungle", new CheckBox("Use W"));
            JungleMenu.Add("Ejungle", new CheckBox("Use E", false));
            JungleMenu.Add("Rjungle", new CheckBox("Use R", false));
            JungleMenu.Add("Ultjungle", new Slider("Max R Stacks", 1, 1, 10));
            JungleMenu.AddSeparator(1);
            JungleMenu.Add("Junglemana", new Slider("Mana Limiter at Mana %", 25));

            // LaneClear Menu
            LaneClearMenu = BallistaKogMawMenu.AddSubMenu("Lane Clear Features", "LaneClearFeatures");
            LaneClearMenu.AddGroupLabel("Lane Clear Features");
            LaneClearMenu.AddLabel("Independent boxes for Spells:");
            LaneClearMenu.Add("Qlanec", new CheckBox("Use Q", false));
            LaneClearMenu.Add("Wlanec", new CheckBox("Use W", false));
            LaneClearMenu.Add("Elanec", new CheckBox("Use E", false));
            LaneClearMenu.Add("Rlanec", new CheckBox("Use R", false));
            LaneClearMenu.Add("Ultlanec", new Slider("Max R Stacks", 1, 1, 10));
            LaneClearMenu.AddSeparator(1);
            LaneClearMenu.Add("Lanecmana", new Slider("Mana Limiter at Mana %", 25));

            // LastHit Menu
            LastHitMenu = BallistaKogMawMenu.AddSubMenu("Last Hit Features", "LastHitFeatures");
            LastHitMenu.AddGroupLabel("Last Hit Features");
            LastHitMenu.AddLabel("Independent boxes for Spells:");
            LastHitMenu.Add("Qlasthit", new CheckBox("Use Q"));
            LastHitMenu.Add("Wlasthit", new CheckBox("Use W"));
            LastHitMenu.Add("Rlasthit", new CheckBox("Use R", false));
            LastHitMenu.Add("Ultlasthit", new Slider("Max R Stacks", 1, 1, 10));
            LastHitMenu.AddSeparator(1);
            LastHitMenu.Add("Lasthitmana", new Slider("Mana Limiter at Mana %", 25));

            // Kill Steal Menu
            KillStealMenu = BallistaKogMawMenu.AddSubMenu("KS Features", "KSFeatures");
            KillStealMenu.AddGroupLabel("Kill Steal Features");
            KillStealMenu.Add("KSmode", new CheckBox("KS Mode"));
            KillStealMenu.AddSeparator(1);
            KillStealMenu.AddLabel("Independent boxes for Spells:");
            KillStealMenu.Add("Qks", new CheckBox("Use Q in KS"));
            KillStealMenu.Add("Wks", new CheckBox("Use W in KS"));
            KillStealMenu.Add("Eks", new CheckBox("Use E in KS", false));
            KillStealMenu.Add("Rks", new CheckBox("Use R in KS"));

            // Drawing Menu
            DrawingMenu = BallistaKogMawMenu.AddSubMenu("Drawing Features", "DrawingFeatures");
            DrawingMenu.AddGroupLabel("Drawing Features");
            DrawingMenu.Add("DrawM", new CheckBox("Draw Mode"));
            DrawingMenu.AddSeparator(1);
            DrawingMenu.AddLabel("Independent boxes for Spells:");
            DrawingMenu.Add("Qdraw", new CheckBox("Draw Q"));
            DrawingMenu.Add("Wdraw", new CheckBox("Draw W"));
            DrawingMenu.Add("Edraw", new CheckBox("Draw E"));
            DrawingMenu.Add("Rdraw", new CheckBox("Draw R"));
            DrawingMenu.AddSeparator(1);
            DrawingMenu.AddLabel("Skin Designer");
            DrawingMenu.Add("DrawS", new CheckBox("Draw Skin Design"));
            DrawingMenu.Add("Skins", new Slider("Skin Designer: ", 7, 0, 8));

            // Setting Menu
            SettingMenu = BallistaKogMawMenu.AddSubMenu("Settings", "Settings");
            SettingMenu.AddGroupLabel("Settings");
            SettingMenu.AddLabel("Automatic Leveler");
            SettingMenu.Add("Autolvl", new CheckBox("Auto Leveler"));
            SettingMenu.AddSeparator(1);
            SettingMenu.AddLabel("Automatic Tear Stacker");
            SettingMenu.Add("StackM", new CheckBox("Stack Mode"));
            SettingMenu.AddSeparator(1);
            SettingMenu.Add("DeathFmode", new CheckBox("Use Passive DeathFollower"));
            SettingMenu.AddSeparator(1);
            SettingMenu.AddLabel("Gap Closer");
            SettingMenu.Add("Gapcmode", new CheckBox("Gap Closer Mode"));
            SettingMenu.Add("Egapc", new CheckBox("Use E to gapclose"));
        }
    }
}
