using System;
using System.Drawing;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;

namespace MagicianRyze
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
            if (Champion.ChampionName != "Ryze") return;
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
            Champion.SetSkinId(MenuManager.DrawingMenu["DrawS"].Cast<CheckBox>().CurrentValue
                ? MenuManager.DrawingMenu["Skins"].Cast<Slider>().CurrentValue
                : ChampionSkin);
        }

        public static void Drawing_OnDraw(EventArgs args)
        {
            Color color;

            // Setup Designer Coloration
            switch (Champion.SkinId)
            {
                default:
                    color = Color.Transparent;
                    break;
                case 0:
                    color = Color.SkyBlue;
                    break;
                case 1:
                    color = Color.Green;
                    break;
                case 2:
                    color = Color.DeepSkyBlue;
                    break;
                case 3:
                    color = Color.LightSkyBlue;
                    break;
                case 4:
                    color = Color.RoyalBlue;
                    break;
                case 5:
                    color = Color.MediumPurple;
                    break;
                case 6:
                    color = Color.ForestGreen;
                    break;
                case 7:
                    color = Color.DarkMagenta;
                    break;
                case 8:
                    color = Color.Firebrick;
                    break;
                case 9:
                    color = Color.SlateBlue;
                    break;
            }

            // Apply Designer Color into Circle
            if (!MenuManager.DrawingMenu["DrawM"].Cast<CheckBox>().CurrentValue) return;
            if (MenuManager.DrawingMenu["Qdraw"].Cast<CheckBox>().CurrentValue && SpellManager.Q.IsLearned)
                Drawing.DrawCircle(Champion.Position, SpellManager.Q.Range, color);
            if (MenuManager.DrawingMenu["WEdraw"].Cast<CheckBox>().CurrentValue && (SpellManager.W.IsLearned || SpellManager.E.IsLearned))
                Drawing.DrawCircle(Champion.Position, SpellManager.W.Range, color);
        }

        public static void Game_OnTick(EventArgs args)
        {
            // Initialize Leveler
            if (MenuManager.SettingMenu["Autolvl"].Cast<CheckBox>().CurrentValue && Champion.SpellTrainingPoints >= 1)
                LevelerManager.Initialize();
            MenuManager.ComboMenu["ComboF"].DisplayName = MenuManager.ComboMenu["ComboF"].Cast<Slider>().CurrentValue == 1
                ? "Counter Combo - My Personal Settings"
                : "Slutty Combo - Fastest Ryze Combo";
            // No Responce While Dead
            if (Champion.IsDead) return;

            // Mode Activation
            if (Orbwalker.IsAutoAttacking) return;
            switch (Orbwalker.ActiveModesFlags)
            {
                case Orbwalker.ActiveModes.Combo:
                    if (MenuManager.ComboMenu["ComboF"].Cast<Slider>().DisplayName == "Counter Combo - My Personal Settings")
                        ModeManager.CounterCombo();
                    if (MenuManager.ComboMenu["ComboF"].Cast<Slider>().DisplayName == "Slutty Combo - Fastest Ryze Combo")
                        ModeManager.SluttyCombo();
                    break;
                case Orbwalker.ActiveModes.Harass:
                    ModeManager.HarassMode();
                    break;
                case Orbwalker.ActiveModes.JungleClear:
                    ModeManager.JungleMode();
                    break;
                case Orbwalker.ActiveModes.LaneClear:
                    ModeManager.LaneClearMode();
                    break;
                case Orbwalker.ActiveModes.LastHit:
                    ModeManager.LastHitMode();
                    break;
            }
            if (MenuManager.KillStealMenu["KSmode"].Cast<CheckBox>().CurrentValue)
                ModeManager.KsMode();
            if (MenuManager.SettingMenu["StackM"].Cast<CheckBox>().CurrentValue)
                ModeManager.StackMode();
        }
    }
}