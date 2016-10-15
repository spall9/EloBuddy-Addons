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

namespace AlchemistSinged
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
            if (Champion.ChampionName != "Singed") return;

            // Declare base skin id
            ChampionSkin = Champion.SkinId;

            // Addon working properly, write success
            Console.WriteLine("AlchemistSinged successfully injected!");
            Console.WriteLine("Source by Counter");

            // Initialize classes
            Display.Initialize();
            Calculations.Initialize();
            Functions.Initialize();

            // Listen to events
            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnUpdate += Game_OnUpdate;
            Game.OnTick += Game_OnTick;
            Interrupter.OnInterruptableSpell += Functions.Interrupter;
            Gapcloser.OnGapcloser += Functions.GapCloser;
        }

        // Upon Game.OnUpdate list of events, add Game_OnUpdate method
        private static void Game_OnUpdate(EventArgs args)
        {
            // Designer skin
            Champion.SetSkinId(Display.GetCheckBoxValue("Designer") ? Display.GetSliderValue("Skin") : ChampionSkin);
        }

        // Upon Drawing.OnDraw list of events, add Drawing_OnDraw method
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
                    color = System.Drawing.Color.LimeGreen;
                    break;
                case 1:
                    color = System.Drawing.Color.LightGray;
                    break;
                case 2:
                    color = System.Drawing.Color.GreenYellow;
                    break;
                case 3:
                    color = System.Drawing.Color.DodgerBlue;
                    break;
                case 4:
                    color = System.Drawing.Color.Purple;
                    break;
                case 5:
                    color = System.Drawing.Color.LawnGreen;
                    break;
                case 6:
                    color = System.Drawing.Color.WhiteSmoke;
                    break;
                case 7:
                    color = System.Drawing.Color.PaleGreen;
                    break;
                case 8:
                    color = System.Drawing.Color.OrangeRed;
                    break;
            }

            // Designer
            if (!Display.GetCheckBoxValue("Drawer")) return;
            if (Display.GetCheckBoxValue("DrawA") && !Orbwalker.DrawRange)
                Drawing.DrawCircle(Champion.Position, Champion.GetAutoAttackRange(), color);
            if (Display.GetCheckBoxValue("DrawW") && Calculations.W.IsLearned)
                Drawing.DrawCircle(Champion.Position, Calculations.W.Range, color);
            if (Display.GetCheckBoxValue("DrawE") && Calculations.E.IsLearned)
                Drawing.DrawCircle(Champion.Position, Calculations.E.Range, color);
        }

        // Upon Game.OnTick list of events, add Game_OnTick method
        private static void Game_OnTick(EventArgs args)
        {         
            // No responce while dead
            if (Champion.IsDead) return;

            // Listen to Events
            if (Display.GetCheckBoxValue("DisablerQ"))
                Calculations.QDisable();

            // Initialize flag functions
            switch (Orbwalker.ActiveModesFlags)
            {
                case Orbwalker.ActiveModes.None:
                    Orbwalker.DisableAttacking = false;
                    break;
                case Orbwalker.ActiveModes.Combo:
                    Orbwalker.DisableAttacking = false;
                    Functions.Combo();
                    break;
                case Orbwalker.ActiveModes.Harass:
                    Orbwalker.DisableAttacking = false;
                    Functions.Harass();
                    break;
                case Orbwalker.ActiveModes.Flee:
                    Orbwalker.DisableAttacking = false;
                    Functions.Flee();
                    break;
                case Orbwalker.ActiveModes.LaneClear:
                    Orbwalker.DisableAttacking = Display.GetCheckBoxValue("DisablerAA");
                    Functions.LaneClear();
                    break;
                case Orbwalker.ActiveModes.LastHit:
                    Orbwalker.DisableAttacking = false;
                    Functions.LastHit();
                    break;
                case Orbwalker.ActiveModes.JungleClear:
                    Orbwalker.DisableAttacking = false;
                    Functions.JungleClear();
                    break;
            }

            // Additional functions
            if (Display.GetCheckBoxValue("Stacker"))
                Functions.Stacker();
            if (Display.GetCheckBoxValue("Kiter"))
                Functions.Kiter();
        }
    }
}