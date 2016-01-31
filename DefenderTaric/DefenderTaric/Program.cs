using System;
using System.Drawing;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;

namespace DefenderTaric
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
            if (Champion.ChampionName != "Taric") return;
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

        private static void Game_OnUpdate(EventArgs args)
        {
            Champion.SetSkinId(MenuManager.DesignerMode
                ? MenuManager.DesignerSkin
                : ChampionSkin);
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            // Wait for Game Load
            if (Game.Time < 10) return;

            // No Responce While Dead
            if (Champion.IsDead) return;

            Color color;

            switch (Champion.SkinId)
            {
                default:
                    color = Color.Transparent;
                    break;
                case 0:
                    color = Color.LightSkyBlue;
                    break;
                case 1:
                    color = Color.Green;
                    break;
                case 2:
                    color = Color.HotPink;
                    break;
                case 3:
                    color = Color.DarkRed;
                    break;
            }

            if (!MenuManager.DrawerMode) return;
            if (MenuManager.DrawQ && SpellManager.Q.IsLearned)
                Drawing.DrawCircle(Champion.Position, SpellManager.Q.Range, color);
            if (MenuManager.DrawE && SpellManager.E.IsLearned)
                Drawing.DrawCircle(Champion.Position, SpellManager.E.Range, color);
            if (MenuManager.DrawWr && (SpellManager.W.IsLearned || SpellManager.R.IsLearned))
                Drawing.DrawCircle(Champion.Position, SpellManager.R.Range, color);
        }

        private static void Game_OnTick(EventArgs args)
        {
            // Initialize Leveler
            if (MenuManager.LevelerMode && Champion.SpellTrainingPoints >= 1)
                LevelerManager.Initialize();

            MenuManager.ComboMenu["Ucombo"].DisplayName = MenuManager.ComboMode == 1 ? "ERW" : "EWR";
            
            // No Responce While Dead
            if (Champion.IsDead) return;

            if (MenuManager.EasterEgg && Champion.HasBuff("recall") && !Champion.IsInShopRange())
                Player.DoEmote(Emote.Joke);

            // Mode Activation
            switch (Orbwalker.ActiveModesFlags)
            {
                case Orbwalker.ActiveModes.Combo:
                {
                    if (MenuManager.ComboMode == 1)
                            ModeManager.EwrComboMode();
                    if (MenuManager.ComboMode == 2)
                            ModeManager.ErwComboMode();
                }
                    break;
                case Orbwalker.ActiveModes.Harass:
                    ModeManager.HarassMode();
                    break;
            }
            if (MenuManager.HealingMode)
                ModeManager.HealingMode();
        }
    }
}