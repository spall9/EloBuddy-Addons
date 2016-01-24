using System;
using System.Drawing;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;

namespace ExecutionerUrgot
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

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            // Validate Player.Instace is Addon Champion
            if (Champion.ChampionName != "Urgot") return;
            ChampionSkin = Champion.SkinId;

            // Initialize classes
            SpellManager.Initialize();
            MenuManager.Initialize();
            TargetManager.Initialize();
            ModeManager.Initialize();

            // Listen to Events
            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnUpdate += Game_OnUpdate;
            Game.OnTick += SpellManager.ConfigSpells;
            Game.OnTick += Game_OnTick;
            Interrupter.OnInterruptableSpell += ModeManager.InterruptMode;
            Gapcloser.OnGapcloser += ModeManager.GapCloserMode;
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            Champion.SetSkinId(MenuManager.DrawingMenu["DrawS"].Cast<CheckBox>().CurrentValue
                ? MenuManager.DrawingMenu["Skins"].Cast<Slider>().CurrentValue
                : ChampionSkin);
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            Color color;
            Color color2;

            // Setup Designer Coloration
            switch (Champion.SkinId)
            {
                default:
                    color = Color.Transparent;
                    color2 = Color.Transparent;
                    break;
                case 0:
                    color = Color.SpringGreen;
                    color2 = Color.PaleVioletRed;
                    break;
                case 1:
                    color = Color.DarkOrange;
                    color2 = Color.IndianRed;
                    break;
                case 2:
                    color = Color.ForestGreen;
                    color2 = Color.Red;
                    break;
                case 3:
                    color = Color.LimeGreen;
                    color2 = Color.OrangeRed;
                    break;
            }

            // Apply Designer Color into Circle
            if (!MenuManager.DrawingMenu["DrawM"].Cast<CheckBox>().CurrentValue) return;
            if (MenuManager.DrawingMenu["Qdraw"].Cast<CheckBox>().CurrentValue && SpellManager.Q.IsLearned)
            {
                Drawing.DrawCircle(Champion.Position, SpellManager.Q.Range, color);
                Drawing.DrawCircle(Champion.Position, SpellManager.Q2.Range, color2);
            }
            if (MenuManager.DrawingMenu["Edraw"].Cast<CheckBox>().CurrentValue && SpellManager.E.IsLearned)
                Drawing.DrawCircle(Champion.Position, SpellManager.E.Range, color);
            if (MenuManager.DrawingMenu["Rdraw"].Cast<CheckBox>().CurrentValue && SpellManager.R.IsLearned)
                Drawing.DrawCircle(Champion.Position, SpellManager.R.Range, color);
        }

        private static void Game_OnTick(EventArgs args)
        {
            // Initialize Leveler
            if (MenuManager.SettingMenu["Autolvl"].Cast<CheckBox>().CurrentValue && Champion.SpellTrainingPoints >= 1)
                LevelerManager.Initialize();
            // No Responce While Dead
            if (Champion.IsDead) return;

            // Mode Activation
            if (Orbwalker.IsAutoAttacking) return;
            switch (Orbwalker.ActiveModesFlags)
            {
                case Orbwalker.ActiveModes.Combo:
                    ModeManager.ComboMode();
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
            if (MenuManager.SettingMenu["Grabmode"].Cast<CheckBox>().CurrentValue)
                ModeManager.GrabMode();
        }
    }
}