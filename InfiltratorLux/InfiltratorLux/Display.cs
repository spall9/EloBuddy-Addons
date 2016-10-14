using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InfiltratorLux
{
    class Display
    {
        // Menu instantiations
        private static Menu Infiltrator, 
            Combo, Harass, Flee, LaneClear, LastHit, JungleClear, KillSteal, Drawing, Settings;

        public static Dictionary<string, ValueBase> Menu = new Dictionary<string, ValueBase>();

        // Initialize method
        public static void Initialize()
        {
            // Main menu
            Infiltrator = MainMenu.AddMenu("InfiltratorLux", "M");
            CreateGroupLabel(Infiltrator, "Infiltrator Lux");
            CreateLabel(Infiltrator, "Source by Counter");

            // Combo menu
            CreateSubMenu(ref Combo, "Combo");
            CreateGroupLabel(Combo, "Combo Features");
            CreateLabel(Combo, "Independent boxes for Spells:");
            CreateCheckBox(Combo, "ComboQ", "Use Q");
            CreateCheckBox(Combo, "ComboE", "Use E");
            CreateCheckBox(Combo, "ComboR", "Use R");

            // Harass menu
            CreateSubMenu(ref Harass, "Harass");
            CreateGroupLabel(Harass, "Harass Features");
            CreateLabel(Harass, "Independent boxes for Spells:");
            CreateCheckBox(Harass, "HarassQ", "Use Q");
            CreateCheckBox(Harass, "HarassE", "Use E");

            // Flee menu
            CreateSubMenu(ref Flee, "Flee");
            CreateGroupLabel(Flee, "Flee Features");
            CreateLabel(Flee, "Independent boxes for Spells:");
            CreateCheckBox(Flee, "FleeQ", "Use Q");
            CreateCheckBox(Flee, "FleeE", "Use E");

            // LaneClear menu
            CreateSubMenu(ref LaneClear, "Lane Clear");
            CreateGroupLabel(LaneClear, "Lane Clear Features");
            CreateLabel(LaneClear, "Independent boxes for Spells:");
            CreateCheckBox(LaneClear, "LaneClearQ", "Use Q", false);
            CreateCheckBox(LaneClear, "LaneClearE", "Use E", false);

            // LastHit menu
            CreateSubMenu(ref LastHit, "Last Hit");
            CreateGroupLabel(LastHit, "Last Hit Features");
            CreateLabel(LastHit, "Independent boxes for Spells:");
            CreateCheckBox(LastHit, "LastHitQ", "Use Q");
            CreateCheckBox(LastHit, "LastHitE", "Use E");

            // JungleClear menu
            CreateSubMenu(ref JungleClear, "Jungle Clear");
            CreateGroupLabel(JungleClear, "Jungle Clear Features");
            CreateLabel(JungleClear, "Independent boxes for Spells:");
            CreateCheckBox(JungleClear, "JungleClearQ", "Use Q");
            CreateCheckBox(JungleClear, "JungleClearE", "Use E");
            
            // KillSteal menu
            CreateSubMenu(ref KillSteal, "Kill Steal");
            CreateGroupLabel(KillSteal, "Kill Steal Features");
            CreateCheckBox(KillSteal, "KillStealer", "KillStealer");
            CreateLabel(KillSteal, "Independent boxes for Spells:");
            CreateCheckBox(KillSteal, "KillStealQ", "Use Q");
            CreateCheckBox(KillSteal, "KillStealE", "Use E");
            CreateCheckBox(KillSteal, "KillStealR", "Use R");

            // Drawing menu
            CreateSubMenu(ref Drawing, "Drawing");
            CreateGroupLabel(Drawing, "Drawing Features");
            CreateLabel(Drawing, "(Disable to remove all drawings)");
            CreateCheckBox(Drawing, "Drawer", "All Drawings");
            CreateCheckBox(Drawing, "DrawA", "Draw Auto Attack");
            CreateLabel(Drawing, "Independent boxes for Spells:");
            CreateCheckBox(Drawing, "DrawQ", "Draw Q");
            CreateCheckBox(Drawing, "DrawW", "Draw W");
            CreateCheckBox(Drawing, "DrawE", "Draw E");
            CreateCheckBox(Drawing, "DrawR", "Draw R");
            CreateGroupLabel(Drawing, "Skin Designer");
            CreateCheckBox(Drawing, "Designer", "Skin Designer");
            CreateSlider(Drawing, "Skin", "Skin Designer: ", 0, 0, 6);

            // Settings menu
            CreateSubMenu(ref Settings, "Settings");
            CreateGroupLabel(Settings, "Automatic Leveler");
            CreateComboBox(Settings, "Leveler", "Auto Leveler", new List<string> { "Automatic", "None" });
            CreateLabel(Settings, "(Higher value means higher priority)");
            CreateSlider(Settings, "PriorityQ", "Q", 1, 0, 3);
            CreateSlider(Settings, "PriorityW", "W", 3, 0, 3);
            CreateSlider(Settings, "PriorityE", "E", 2, 0, 3);
            CreateSlider(Settings, "PriorityR", "R", 0, 0, 3);
            CreateSeperator(Settings);
            CreateGroupLabel(Settings, "Automatic Shielder");
            CreateCheckBox(Settings, "SettingsW", "Auto W");
            
            // Assign Menu list with all options
            foreach(Menu menu in Infiltrator.SubMenus)
                foreach (KeyValuePair<string, ValueBase> prompt in menu.LinkedValues)
                    Menu.Add(prompt.Key, prompt.Value);
        }

        // EloBuddy Menu options
        private static Menu CreateSubMenu(ref Menu menu, string index)
        {
            return menu = Infiltrator.AddSubMenu(index);
        }

        private static void CreateGroupLabel(Menu menu, string text)
        {
            menu.Add(menu.DisplayName + text, new GroupLabel(text));
        }
        
        private static void CreateLabel(Menu menu, string text)
        {
            menu.Add(menu.DisplayName + text, new Label(text));
        }

        private static void CreateCheckBox(Menu menu, string index, string text, bool defbool = true)
        {
            menu.Add(index, new CheckBox(text, defbool));
        }

        private static void CreateSlider(Menu menu, string index, string text, int origin, int min, int max)
        {
            menu.Add(index, new Slider(text, origin, min, max));
        }

        private static void CreateComboBox(Menu menu, string index, string text, List<string> data, int defint = 0)
        {
            menu.Add(index, new ComboBox(text, data, defint));
        }

        private static void CreateSeperator(Menu menu, int h = 25)
        {
            menu.AddSeparator(h);
        }

        // Retrieve value methods
        public static bool GetCheckBoxValue(string index)
        {
            ValueBase checkbox = Menu.Where(a => a.Key == index.ToLower()).FirstOrDefault().Value;
            if (checkbox == null)
            {
                Console.WriteLine("No value named: " + index);
                return false;
            }

            return checkbox.Cast<CheckBox>().CurrentValue;
        }

        public static int GetSliderValue(string index)
        {
            ValueBase slider = Menu.Where(a => a.Key == index.ToLower()).FirstOrDefault().Value;
            if (slider == null)
            {
                Console.WriteLine("No value named: " + index);
                return 0;
            }

            return slider.Cast<Slider>().CurrentValue;
        }

        public static string GetComboBoxValue(string index)
        {
            ValueBase combobox = Menu.Where(a => a.Key == index.ToLower()).FirstOrDefault().Value;
            if (combobox == null)
            {
                Console.WriteLine("No value named: " + index);
                return "null";
            }

            return combobox.Cast<ComboBox>().CurrentValue.ToString();
        }
    }
}