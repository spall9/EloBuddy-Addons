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

namespace DefenderTaric
{
    class Display
    {
        // Menu instantiations
        private static Menu Defender, 
            Combo, Harass, Flee, LaneClear, LastHit, JungleClear, Assistance, Drawing, Settings;

        public static Dictionary<string, ValueBase> Menu = new Dictionary<string, ValueBase>();

        // Initialize method
        public static void Initialize()
        {
            // Main menu
            Defender = MainMenu.AddMenu("DefenderTaric", "M");
            CreateGroupLabel(Defender, "Defender Taric");
            CreateLabel(Defender, "Source by Counter");

            // Combo menu
            CreateSubMenu(ref Combo, "Combo");
            CreateGroupLabel(Combo, "Combo Features");
            CreateLabel(Combo, "Independent boxes for Spells:");
            CreateCheckBox(Combo, "ComboE", "Use E");

            // Harass menu
            CreateSubMenu(ref Harass, "Harass");
            CreateGroupLabel(Harass, "Harass Features");
            CreateLabel(Harass, "Independent boxes for Spells:");
            CreateCheckBox(Harass, "HarassE", "Use E");
            CreateSlider(Harass, "HarassM", "Limiter for Mana at %", 25);

            // Flee menu
            CreateSubMenu(ref Flee, "Flee");
            CreateGroupLabel(Flee, "Flee Features");
            CreateLabel(Flee, "Independent boxes for Spells:");
            CreateCheckBox(Flee, "FleeE", "Use E");

            // LaneClear menu
            CreateSubMenu(ref LaneClear, "Lane Clear");
            CreateGroupLabel(LaneClear, "Lane Clear Features");
            CreateLabel(LaneClear, "Independent boxes for Spells:");
            CreateCheckBox(LaneClear, "LaneClearQ", "Spellweave Q", false);
            CreateCheckBox(LaneClear, "LaneClearW", "Spellweave W", false);
            CreateCheckBox(LaneClear, "LaneClearE", "Use E", false);
            CreateSlider(LaneClear, "LaneClearM", "Limiter for Mana at %", 25);

            // LastHit menu
            CreateSubMenu(ref LastHit, "Last Hit");
            CreateGroupLabel(LastHit, "Last Hit Features");
            CreateLabel(LastHit, "Independent boxes for Spells:");
            CreateCheckBox(LastHit, "LastHitQ", "Spellweave Q", false);
            CreateCheckBox(LastHit, "LastHitW", "Spellweave W", false);
            CreateSlider(LastHit, "LastHitM", "Limiter for Mana at %", 50);

            // JungleClear menu
            CreateSubMenu(ref JungleClear, "Jungle Clear");
            CreateGroupLabel(JungleClear, "Jungle Clear Features");
            CreateLabel(JungleClear, "Independent boxes for Spells:");
            CreateCheckBox(JungleClear, "JungleClearQ", "Spellweave Q", false);
            CreateCheckBox(JungleClear, "JungleClearW", "Spellweave W", false);
            CreateCheckBox(JungleClear, "JungleClearE", "Use E", false);
            CreateSlider(JungleClear, "JungleClearM", "Limiter for Mana at %", 25);

            // Assistance menu
            CreateSubMenu(ref Assistance, "Assistance");
            CreateGroupLabel(Assistance, "Assistance Features");
            CreateGroupLabel(Assistance, "Healing Features");
            CreateSlider(Assistance, "AssistanceHally", "Heal ally at Health % - 0 is off", 50);
            CreateSlider(Assistance, "AssistanceHself", "Heal self at Health % - 0 is off", 35);
            CreateGroupLabel(Assistance, "Stack Limiter");
            CreateSlider(Assistance, "AssistanceSally", "Auto Q ally Stacks:", 2, 1, 3);
            CreateSlider(Assistance, "AssistanceSself", "Auto Q self Stacks:", 3, 1, 3);
            CreateGroupLabel(Assistance, "Automatic Ultimate");
            CreateSlider(Assistance, "AssistanceRally", "Ult ally at Health % - 0 is off", 35);
            CreateSlider(Assistance, "AssistanceRself", "Ult self at Health % - 0 is off", 25);


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
            CreateSlider(Drawing, "Skin", "Skin Designer: ", 0, 0, 4);

            // Settings menu
            CreateSubMenu(ref Settings, "Settings");
            CreateGroupLabel(Settings, "Settings");
            CreateLabel(Settings, "Wait for passive buff to expire before next spell cast");
            CreateCheckBox(Settings, "SettingsP", "Spellweave in all modes");
            
            
            // Assign Menu list with all options
            foreach(Menu menu in Defender.SubMenus)
                foreach (KeyValuePair<string, ValueBase> prompt in menu.LinkedValues)
                    Menu.Add(prompt.Key, prompt.Value);
        }

        // EloBuddy Menu options
        private static Menu CreateSubMenu(ref Menu menu, string index)
        {
            return menu = Defender.AddSubMenu(index);
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