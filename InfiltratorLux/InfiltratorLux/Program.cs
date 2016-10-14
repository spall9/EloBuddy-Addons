using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace InfiltratorLux
{
    class Program
    {
        // Global player Champion instance variable
        public static AIHeroClient Champion { get { return Player.Instance; } }

        // Base Champion skin value
        public static int ChampionSkin;
        
        // Main method for Loading.OnLoadingComplete call
        public static void Main(string[] args)
        {
            // Listen to Loading events
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        // Upon Loading list of events, preform Loading_OnLoadingComplete events
        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            // Check Champion has correct Champion.Name
            if (Champion.ChampionName != "Lux") return;

            // Declare base skin id
            ChampionSkin = Champion.SkinId;

            // Addon working properly, write success
            Console.WriteLine("InfiltratorLux successfully injected!");
            Console.WriteLine("Source by Counter");

            // Initialize classes
            Display.Initialize();
            Calculations.Initialize();
            Functions.Initialize();

            // Listen to events
            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnUpdate += Game_OnUpdate;
            Game.OnTick += Game_OnTick;
        }

        // Upon Game list of events, preform Game_OnUpdate events
        private static void Game_OnUpdate(EventArgs args)
        {
            // Designer skin
            Champion.SetSkinId(Display.GetCheckBoxValue("Designer") ? Display.GetSliderValue("Skin") : ChampionSkin);
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            // Wait for Game Load <-- Prevents grahpical problems
            if (Game.Time < 10) return;

            // No responce while dead
            if (Champion.IsDead) return;

            // Designer colors
            System.Drawing.Color color;
            switch (Champion.SkinId)
            {
                default:
                    color = System.Drawing.Color.Goldenrod;
                    break;
                case 1:
                    color = System.Drawing.Color.LightBlue;
                    break;
                case 2:
                    color = System.Drawing.Color.LightCoral;
                    break;
                case 3:
                    color = System.Drawing.Color.LightGreen;
                    break;
                case 4:
                    color = System.Drawing.Color.OrangeRed;
                    break;
                case 5:
                    color = System.Drawing.Color.DimGray;
                    break;
                case 6:
                    color = System.Drawing.Color.HotPink;
                    break;
            }

            // Designer
            if (!Display.GetCheckBoxValue("Drawer")) return;
            if (Display.GetCheckBoxValue("DrawA") && !Orbwalker.DrawRange)
                Drawing.DrawCircle(Champion.Position, Champion.GetAutoAttackRange(), color);
            if (Display.GetCheckBoxValue("DrawQ") && Calculations.Q.IsLearned)
                Drawing.DrawCircle(Champion.Position, Calculations.Q.Range, color);
            if (Display.GetCheckBoxValue("DrawW") && Calculations.W.IsLearned)
                Drawing.DrawCircle(Champion.Position, Calculations.W.Range, color);
            if (Display.GetCheckBoxValue("DrawE") && Calculations.E.IsLearned)
                Drawing.DrawCircle(Champion.Position, Calculations.E.Range, color);
            if (Display.GetCheckBoxValue("DrawR") && Calculations.R.IsLearned)
                Drawing.DrawCircle(Champion.Position, Calculations.R.Range, color);
        }

        // Upon Game list of events, preform Game_OnTick events
        private static void Game_OnTick(EventArgs args)
        {
            // Initialize Leveler functions
            if (Display.GetComboBoxValue("Leveler") != "None")
                Functions.Leveler();
            
            // No responce while dead
            if (Champion.IsDead) return;

            // Initialize flag functions
            switch (Orbwalker.ActiveModesFlags)
            {
                case Orbwalker.ActiveModes.Combo:
                    Functions.Combo();
                    break;
                case Orbwalker.ActiveModes.Harass:
                    Functions.Harass();
                    break;
                case Orbwalker.ActiveModes.Flee:
                    Functions.Flee();
                    break;
                case Orbwalker.ActiveModes.LaneClear:
                    Functions.LaneClear();
                    break;
                case Orbwalker.ActiveModes.LastHit:
                    Functions.LastHit();
                    break;
                case Orbwalker.ActiveModes.JungleClear:
                    Functions.JungleClear();
                    break;
            }

            // Initalize KillSteal functions
            if (Display.GetCheckBoxValue("KillStealer"))
                Functions.KillSteal();
        }
    }
}
