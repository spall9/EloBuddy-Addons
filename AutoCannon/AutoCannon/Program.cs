using System;
using System.Drawing;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace AutoCannon
{
    /* Created by Counter */

    internal class Program
    {
        // Menus
        public static Menu AutoCannonMenu, SettingsMenu;
        // Skills
        public static Spell.Skillshot Throw;

        // Grab Player
        public static AIHeroClient Player = ObjectManager.Player;

        // Grab Enemy
        public static Obj_AI_Base GetEnemy(float range, GameObjectType gametype)
        {
            return ObjectManager.Get<Obj_AI_Base>()
                .OrderBy(a => a.Health).FirstOrDefault(a => a.IsEnemy
                                                            && a.Type == gametype && !Player.IsRecalling()
                                                            && !a.IsDead && a.IsValidTarget(range) && !a.IsInvulnerable
                                                            && a.Distance(Player) <= range);
        }

        // Grab KSable Enemy
        public static Obj_AI_Base GetKs(float range, float damage, GameObjectType gametype)
        {
            return ObjectManager.Get<Obj_AI_Base>()
                .OrderBy(a => a.Health).FirstOrDefault(a => a.IsEnemy
                                                            && a.Type == gametype
                                                            && !a.IsDead && a.IsValidTarget(range) && !a.IsInvulnerable
                                                            && !a.HasBuff("ChronoShift")
                                                            && a.Health <= damage
                                                            && a.Distance(Player) <= range);
        }

        private static void Main()
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Game.MapId != GameMapId.HowlingAbyss) return;

            // Draw Menus
            AutoCannonMenu = MainMenu.AddMenu("AutoCannon", "AutoCannon");
            AutoCannonMenu.AddGroupLabel("AutoCannon");
            AutoCannonMenu.AddLabel("Created by Counter");
            AutoCannonMenu.AddLabel("This addon will shoot Mark/Poros. :) <3");
            AutoCannonMenu.AddLabel("Also has a KS feature for priority kills!! ;) xD");

            SettingsMenu = AutoCannonMenu.AddSubMenu("Settings", "Settings");
            SettingsMenu.AddGroupLabel("Settings");
            SettingsMenu.AddLabel("Keep in mind that the script won't dash to target. This must be done manually.");
            SettingsMenu.AddSeparator();
            SettingsMenu.AddLabel("Press F5 to update these numbers for Drawing.");

            // Setting variable
            SpellDataInst sumspell = null;
            if (EloBuddy.Player.GetSpell(SpellSlot.Summoner1).Name == "summonersnowball")
                sumspell = EloBuddy.Player.GetSpell(SpellSlot.Summoner1);
            if (EloBuddy.Player.GetSpell(SpellSlot.Summoner2).Name == "summonersnowball")
                sumspell = EloBuddy.Player.GetSpell(SpellSlot.Summoner2);
            if (EloBuddy.Player.GetSpell(SpellSlot.Summoner1).Name == "summonerporothrow")
                sumspell = EloBuddy.Player.GetSpell(SpellSlot.Summoner1);
            else if (EloBuddy.Player.GetSpell(SpellSlot.Summoner2).Name == "summonerporothrow")
                sumspell = EloBuddy.Player.GetSpell(SpellSlot.Summoner2);

            if (sumspell != null)
            {
                switch (sumspell.Name)
                {
                    case "summonersnowball":
                        SettingsMenu.Add("markrange", new Slider("SS: Mark range - 0 is off", 1600, 0, 1600));
                        break;
                    case "summonerporothrow":
                        SettingsMenu.Add("pororange", new Slider("SS: Poro Throw range - 0 is off", 2500, 0, 2500));
                        break;
                }

                var range = sumspell.Name == "summonersnowball"
                    ? SettingsMenu["markrange"].Cast<Slider>().CurrentValue
                    : SettingsMenu["pororange"].Cast<Slider>().CurrentValue;
                Throw = new Spell.Skillshot(sumspell.Slot, (uint) range, SkillShotType.Linear)
                {
                    MinimumHitChance = HitChance.High,
                    AllowedCollisionCount = 0
                };

                Game.OnTick += Game_OnTick;
                Drawing.OnDraw += Drawing_OnDraw;
            }
        }

        private static void Game_OnTick(EventArgs args)
        {
            // Mark Calculations
            if (!Throw.IsOnCooldown && Throw.Name != "snowballfollowupcast" && Throw.Name != "porothrowfollowupcast")
            {
                // calculate damage
                var damage = Throw.Name == "summonersnowball" ? 10 + 5*Player.Level : 20 + 10*Player.Level;

                var kstarget = GetKs(Throw.Range, damage, GameObjectType.AIHeroClient);
                if (kstarget != null)
                    Throw.Cast(Throw.GetPrediction(kstarget).CastPosition);
                else
                {
                    var target = GetEnemy(Throw.Range, GameObjectType.AIHeroClient);
                    if (target != null)
                        Throw.Cast(Throw.GetPrediction(target).CastPosition);
                }
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            Drawing.DrawCircle(Player.Position, Throw.Range, Color.CadetBlue);
        }
    }
}