using System;
using System.Drawing;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;

namespace BallistaKogMaw
{
    // Created by Counter
    internal class Program
    {
        // Grab Player Attributes
        public static AIHeroClient Champion { get { return Player.Instance; } }
        public static int ChampionSkin;

        // Global Passive Target
        public static Obj_AI_Base Ptarget;


        public static void Main()
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        public static void Loading_OnLoadingComplete(EventArgs args)
        {
            // Validate Player.Instace is Addon Champion
            if (Champion.ChampionName != "KogMaw") return;
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
            Color color2;

            // Setup Designer Coloration
            switch (Champion.SkinId)
            {
                default:
                    color = Color.Transparent;
                    color2 = Color.Transparent;
                    break;
                case 0:
                    color = Color.SeaGreen;
                    color2 = Color.Goldenrod;
                    break;
                case 1:
                    color = Color.Lime;
                    color2 = Color.GreenYellow;
                    break;
                case 2:
                    color = Color.DarkOrange;
                    color2 = Color.PaleGreen;
                    break;
                case 3:
                    color = Color.FloralWhite;
                    color2 = Color.Wheat;
                    break;
                case 4:
                    color = Color.Tan;
                    color2 = Color.RosyBrown;
                    break;
                case 5:
                    color = Color.MediumVioletRed;
                    color2 = Color.LightYellow;
                    break;
                case 6:
                    color = Color.SeaGreen;
                    color2 = Color.Goldenrod;
                    break;
                case 7:
                    color = Color.SaddleBrown;
                    color2 = Color.DarkRed;
                    break;
                case 8:
                    color = Color.Firebrick;
                    color2 = Color.Tomato;
                    break;
            }

            // Apply Designer Color into Circle
            if (!MenuManager.DrawingMenu["DrawM"].Cast<CheckBox>().CurrentValue) return;
            if (!Champion.HasBuff("kogmawicathiansurprise"))
            {
                if (MenuManager.DrawingMenu["Qdraw"].Cast<CheckBox>().CurrentValue && SpellManager.Q.IsLearned)
                    Drawing.DrawCircle(Champion.Position, SpellManager.Q.Range, color);
                if (MenuManager.DrawingMenu["Wdraw"].Cast<CheckBox>().CurrentValue && SpellManager.W.IsLearned
                    && Champion.HasBuff("KogMawBioArcaneBarrage"))
                    Drawing.DrawCircle(Champion.Position, SpellManager.W.Range, color2);
                if (MenuManager.DrawingMenu["Edraw"].Cast<CheckBox>().CurrentValue && SpellManager.E.IsLearned)
                    Drawing.DrawCircle(Champion.Position, SpellManager.E.Range, color);
                if (MenuManager.DrawingMenu["Rdraw"].Cast<CheckBox>().CurrentValue && SpellManager.R.IsLearned)
                    Drawing.DrawCircle(Champion.Position, SpellManager.R.Range, color);
            }
            else
            {
                Drawing.DrawCircle(Champion.Position, SpellManager.P.Range, color);
            }
        }

        public static void Game_OnTick(EventArgs args)
        {
            // Initialize Leveler
            if (MenuManager.SettingMenu["Autolvl"].Cast<CheckBox>().CurrentValue && Champion.SpellTrainingPoints >= 1)
                LevelerManager.Initialize();
            // No Responce While Dead
            if (Champion.IsDead) return;

            // Mode Activation
            if (MenuManager.SettingMenu["DeathFmode"].Cast<CheckBox>().CurrentValue)
            {
                if (!Champion.HasBuff("kogmawicathiansurprise"))
                    Ptarget = null;
                else
                    ModeManager.DeathFollowMode();
            }
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
        }
    }
}