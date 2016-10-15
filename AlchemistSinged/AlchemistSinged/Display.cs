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


namespace AlchemistSinged
{
    class Display
    {
        // Menu instantiations
        private static Menu Alchemist, 
            Combo, Harass, Flee, LaneClear, LastHit, JungleClear, Assistance, Drawing, Settings;

        public static Dictionary<string, ValueBase> Menu = new Dictionary<string, ValueBase>();

        // Initialize method
        public static void Initialize()
        {
            // Main menu
            Alchemist = MainMenu.AddMenu("AlchemistSinged", "M");
            CreateGroupLabel(Alchemist, "Alchemist Singed");
            CreateLabel(Alchemist, "Source by Counter");

            // Combo menu
            CreateSubMenu(ref Combo, "Combo");
            CreateGroupLabel(Combo, "Combo Features");
            CreateLabel(Combo, "Independent boxes for Spells:");
            CreateCheckBox(Combo, "ComboQ", "Use Q");
            CreateCheckBox(Combo, "ComboW", "Use W");
            CreateCheckBox(Combo, "ComboE", "Use E");
            CreateCheckBox(Combo, "ComboR", "Use R");
            CreateSlider(Combo, "ComboL", "Use R if Enemies in Range >= ", 3, 1, 5);

            // Harass menu
            CreateSubMenu(ref Harass, "Harass");
            CreateGroupLabel(Harass, "Harass Features");
            CreateLabel(Harass, "Independent boxes for Spells:");
            CreateCheckBox(Harass, "HarassQ", "Use Q");

            // Flee menu
            CreateSubMenu(ref Flee, "Flee");
            CreateGroupLabel(Flee, "Flee Features");
            CreateLabel(Flee, "Independent boxes for Spells:");
            CreateCheckBox(Flee, "FleeE", "Use E");

            // LaneClear menu
            CreateSubMenu(ref LaneClear, "Lane Clear");
            CreateGroupLabel(LaneClear, "Lane Clear Features");
            CreateLabel(LaneClear, "Independent boxes for Spells:");
            CreateCheckBox(LaneClear, "LaneClearQ", "Use Q");
            CreateCheckBox(LaneClear, "LaneClearE", "Use E", false);
            CreateLabel(LaneClear, "AutoAttack Disabler for LaneClear - Easier for Q farm");
            CreateCheckBox(LaneClear, "DisablerAA", "Disable AA");

            // LastHit menu
            CreateSubMenu(ref LastHit, "Last Hit");
            CreateGroupLabel(LastHit, "Last Hit Features");
            CreateLabel(LastHit, "Independent boxes for Spells:");
            CreateCheckBox(LastHit, "LastHitE", "Use E");

            // JungleClear menu
            CreateSubMenu(ref JungleClear, "Jungle Clear");
            CreateGroupLabel(JungleClear, "Jungle Clear Features");
            CreateLabel(JungleClear, "Independent boxes for Spells:");
            CreateCheckBox(JungleClear, "JungleClearQ", "Use Q");
            CreateCheckBox(JungleClear, "JungleClearE", "Use E");

            // Drawing menu
            CreateSubMenu(ref Drawing, "Drawing");
            CreateGroupLabel(Drawing, "Drawing Features");
            CreateLabel(Drawing, "(Disable to remove all drawings)");
            CreateCheckBox(Drawing, "Drawer", "All Drawings");
            CreateCheckBox(Drawing, "DrawA", "Draw Auto Attack");
            CreateLabel(Drawing, "Independent boxes for Spells:");
            CreateCheckBox(Drawing, "DrawW", "Draw W");
            CreateCheckBox(Drawing, "DrawE", "Draw E");
            CreateGroupLabel(Drawing, "Skin Designer");
            CreateCheckBox(Drawing, "Designer", "Skin Designer");
            CreateSlider(Drawing, "Skin", "Skin Designer: ", 0, 0, 8);

            // Settings menu
            CreateSubMenu(ref Settings, "Settings");
            CreateGroupLabel(Settings, "Automatic Tear Stacker");
            CreateCheckBox(Settings, "Stacker", "Stack Tear");
            CreateGroupLabel(Settings, "Q Disabler");
            CreateLabel(Settings, "(Disable if no enemies around or not at base)");
            CreateCheckBox(Settings, "DisablerQ", "Disable Q");
            CreateGroupLabel(Settings, "Poison Trail Kiter");
            CreateLabel(Settings, "(If enemy facing you, but you facing away)");
            CreateCheckBox(Settings, "Kiter", "Use Q to Kite");
            CreateGroupLabel(Settings, "Interrupter");
            CreateCheckBox(Settings, "InterrupterE", "Use E to interrupt");
            CreateGroupLabel(Settings, "Gap Closer");
            CreateCheckBox(Settings, "GapCloserE", "Use E to gapclose");
            
            // Assign Menu list with all options
            foreach(Menu menu in Alchemist.SubMenus)
                foreach (KeyValuePair<string, ValueBase> prompt in menu.LinkedValues)
                    Menu.Add(prompt.Key, prompt.Value);
        }

        // EloBuddy Menu options
        private static Menu CreateSubMenu(ref Menu menu, string index)
        {
            return menu = Alchemist.AddSubMenu(index);
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

        private static void CreateSlider(Menu menu, string index, string text, int origin, int min = 0, int max = 100)
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