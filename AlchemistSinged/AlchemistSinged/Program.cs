using System;
using System.Drawing;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;

namespace AlchemistSinged
{
    // Created by Counter
    internal class Program
    {
        // Grab Player Attributes
        public static AIHeroClient Champion { get { return Player.Instance; } }
        public static int ChampionSkin;

        public static void Main()
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        public static void Loading_OnLoadingComplete(EventArgs args)
        {
            // Validate Player.Instace is Addon Champion
            if (Champion.ChampionName != "Singed") return;
            ChampionSkin = Champion.SkinId;

            // Initialize classes
            SpellManager.Initialize();
            MenuManager.Initialize();

            // Listen to Events
            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnUpdate += Game_OnUpdate;
            Game.OnTick += Game_OnTick;
            Interrupter.OnInterruptableSpell += ModeManager.InterruptMode;
            Gapcloser.OnGapcloser += ModeManager.GapCloserMode;
        }

        public static void Game_OnUpdate(EventArgs args)
        {
            // Initialize Skin Designer
            Champion.SetSkinId(MenuManager.DesignerMode
                ? MenuManager.DesignerSkin
                : ChampionSkin);
        }

        public static void Drawing_OnDraw(EventArgs args)
        {
            // Wait for Game Load
            if (Game.Time < 10) return;

            // No Responce While Dead
            if (Champion.IsDead) return;

            Color color;

            // Setup Designer Coloration
            switch (Champion.SkinId)
            {
                default:
                    color = Color.Transparent;
                    break;
                case 0:
                    color = Color.LimeGreen;
                    break;
                case 1:
                    color = Color.Red;
                    break;
                case 2:
                    color = Color.GreenYellow;
                    break;
                case 3:
                    color = Color.DodgerBlue;
                    break;
                case 4:
                    color = Color.Purple;
                    break;
                case 5:
                    color = Color.LawnGreen;
                    break;
                case 6:
                    color = Color.WhiteSmoke;
                    break;
                case 7:
                    color = Color.PaleGreen;
                    break;
            }

            // Apply Designer Color into Circle
            if (!MenuManager.DrawMode) return;
            if (MenuManager.DrawW && SpellManager.W.IsLearned)
                Drawing.DrawCircle(Champion.Position, SpellManager.W.Range, color);
            if (MenuManager.DrawE && SpellManager.E.IsLearned)
                Drawing.DrawCircle(Champion.Position, SpellManager.E.Range, color);
        }

        public static void Game_OnTick(EventArgs args)
        {
            // Initialize Leveler
            if (MenuManager.LevelerMode
                && Champion.SpellTrainingPoints >= 1)
                LevelerManager.Initialize();

            // No Responce While Dead
            if (Champion.IsDead) return;

            // Listen to Events
            SpellManager.QDisable();

            // Mode Activation
            switch (Orbwalker.ActiveModesFlags)
            {
                case Orbwalker.ActiveModes.Combo:
                    Orbwalker.DisableAttacking = false;
                    ModeManager.ComboMode();
                    break;
                case Orbwalker.ActiveModes.Harass:
                    Orbwalker.DisableAttacking = false;
                    ModeManager.HarassMode();
                    break;
                case Orbwalker.ActiveModes.JungleClear:
                    Orbwalker.DisableAttacking = false;
                    ModeManager.JungleMode();
                    break;
                case Orbwalker.ActiveModes.LaneClear:
                    Orbwalker.DisableAttacking = false;
                    ModeManager.LaneClearMode();
                    break;
                case Orbwalker.ActiveModes.LastHit:
                    Orbwalker.DisableAttacking = false;
                    ModeManager.LastHitMode();
                    break;
                case Orbwalker.ActiveModes.Flee:
                    Orbwalker.DisableAttacking = false;
                    ModeManager.KiteMode();
                    break;
            }
            if (MenuManager.StackMode)
                ModeManager.StackMode();
        }
    }
}