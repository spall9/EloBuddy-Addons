using System;
using System.Drawing;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;

namespace ProtectorLeona
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
            if (Champion.ChampionName != "Leona") return;
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

        public static void Game_OnUpdate(EventArgs args)
        {
            Champion.SetSkinId(MenuManager.DrawingMenu["DrawS"].Cast<CheckBox>().CurrentValue
                ? MenuManager.DrawingMenu["Skins"].Cast<Slider>().CurrentValue
                : ChampionSkin);
        }

        public static void Drawing_OnDraw(EventArgs args)
        {
            Color color;

            switch (Champion.SkinId)
            {
                default:
                    color = Color.Transparent;
                    break;
                case 0:
                    color = Color.Goldenrod;
                    break;
                case 1:
                    color = Color.SandyBrown;
                    break;
                case 2:
                    color = Color.IndianRed;
                    break;
                case 3:
                    color = Color.LightSteelBlue;
                    break;
                case 4:
                    color = Color.OrangeRed;
                    break;
                case 5:
                    color = Color.HotPink;
                    break;
                case 6:
                    color = Color.LightSkyBlue;
                    break;
                case 7:
                    color = Color.Orange;
                    break;
                case 8:
                    color = Color.Yellow;
                    break;
            }
            if (!MenuManager.DrawingMenu["DrawM"].Cast<CheckBox>().CurrentValue) return;
            if (MenuManager.DrawingMenu["Qdraw"].Cast<CheckBox>().CurrentValue && SpellManager.Q.IsLearned)
                Drawing.DrawCircle(Champion.Position, SpellManager.Q.Range, color);
            if (MenuManager.DrawingMenu["Wdraw"].Cast<CheckBox>().CurrentValue && SpellManager.W.IsLearned)
                Drawing.DrawCircle(Champion.Position, SpellManager.W.Range, color);
            if (MenuManager.DrawingMenu["Edraw"].Cast<CheckBox>().CurrentValue && SpellManager.E.IsLearned)
                Drawing.DrawCircle(Champion.Position, SpellManager.E.Range, color);
            if (MenuManager.DrawingMenu["Rdraw"].Cast<CheckBox>().CurrentValue && SpellManager.R.IsLearned)
                Drawing.DrawCircle(Champion.Position, SpellManager.R.Range, color);
        }

        public static void Game_OnTick(EventArgs args)
        {
            // Initialize Leveler
            if (MenuManager.SettingMenu["Autolvl"].Cast<CheckBox>().CurrentValue && Champion.SpellTrainingPoints >= 1)
                LevelerManager.Initialize();
            // No Responce While Dead
            if (Champion.IsDead) return;

            // Mode Activation
            switch (Orbwalker.ActiveModesFlags)
            {
                case Orbwalker.ActiveModes.Combo:
                    ModeManager.ComboMode();
                    break;
                case Orbwalker.ActiveModes.Harass:
                    ModeManager.HarassMode();
                    break;
            }
        }
    }
}