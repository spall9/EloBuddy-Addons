using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;

namespace ArsenalActivator
{
    // Created by Counter
    internal class Program
    {
        // Camp names: "SRU_Blue", "SRU_Red", "SRU_Murkwolf", "SRU_Razorbeak", "SRU_Krug", "SRU_Gromp", "Sru_Crab", "SRU_Dragon", "SRU_RiftHerald", "SRU_Baron", "TT_NWraith", "TT_NGolem", "TT_NWolf", "TT_Spiderboss"
        // Mini names: "SRU_BlueMini", "BlueMini2", "SRU_RedMini", "SRU_KrugMini",  "SRU_MurkwolfMini", "SRU_RazorbeakMini", "TT_NWraith2", "TT_NGolem2", "TT_NWolf2"
        // B SRU names: "SRU_OrderMinionMelee", "SRU_OrderMinionRanged", "SRU_OrderMinionSiege", "SRU_OrderMinionSuper"
        // R SRU names: "SRU_ChaosMinionMelee", "SRU_ChaosMinionRanged", "SRU_ChaosMinionSiege", "SRU_ChaosMinionSuper"
        // B Dom names: "Odin_Blue_Minion_Caster", "OdinBlueSuperminion"
        // R Dom names: "Odin_Red_Minion_Caster", "OdinRedSuperminion"
        // B TTHA names: "HA_OrderMinionMelee", "HA_OrderMinionRanged", "HA_OrderMinionSiege", "HA_OrderMinionSuper"
        // R TTHA names: "HA_ChaosMinionMelee", "HA_ChaosMinionRanged", "HA_ChaosMinionSiege", "HA_ChaosMinionSuper"

        // Menu
        public static Menu ArsenalActivatorMenu;

        // Grab Player
        public static AIHeroClient Player = ObjectManager.Player;

        public static Obj_AI_Base GetAlly(float range, GameObjectType gametype)
        {
            switch (gametype)
            {
                case GameObjectType.AIHeroClient:
                    return EntityManager.Heroes.Allies
                        .OrderByDescending(a => a.Health)
                        .FirstOrDefault(a => a.IsAlly
                                             && a.IsValidTarget(range) && a.Distance(Player) <= range
                                             && !a.IsInvulnerable && !a.IsZombie
                                             && !Player.IsRecalling());
                case GameObjectType.obj_AI_Minion:
                    return EntityManager.MinionsAndMonsters.AlliedMinions
                        .OrderByDescending(a => a.Health)
                        .FirstOrDefault(a => a.IsAlly
                                             && a.IsValidTarget(range) && a.Distance(Player) <= range
                                             && !Player.IsRecalling());
            }
            return null;
        }

        public static Obj_AI_Base GetEnemy(float range, GameObjectType gametype)
        {
            switch (gametype)
            {
                case GameObjectType.AIHeroClient:
                    return EntityManager.Heroes.Enemies
                        .OrderByDescending(a => a.Health)
                        .FirstOrDefault(a => a.IsEnemy
                                             && a.IsValidTarget(range) && a.Distance(Player) <= range
                                             && !a.IsInvulnerable && !a.IsZombie
                                             && !a.HasBuff("ChronoShift") && !Player.IsRecalling());
                case GameObjectType.obj_AI_Minion:
                    return EntityManager.MinionsAndMonsters.EnemyMinions
                        .OrderByDescending(a => a.Health)
                        .FirstOrDefault(a => a.IsEnemy
                                             && a.IsValidTarget(range) && a.Distance(Player) <= range
                                             && !a.HasBuff("BannerOfCommand")
                                             && !Player.IsRecalling());
            }
            return null;
        }

        public static Obj_AI_Base GetEnemyKs(float range, float damage, GameObjectType gametype)
        {
            return ObjectManager
                        .Get<Obj_AI_Base>()
                        .OrderByDescending(a => a.Health)
                        .FirstOrDefault(a => a.IsEnemy && a.Type == gametype
                                             && a.IsValidTarget(range) && a.Distance(Player) <= range
                                             && !a.IsInvulnerable && !a.IsZombie
                                             && !a.HasBuff("ChronoShift") && !Player.IsRecalling()
                                             && a.Health <= damage);
        }

        public static Obj_AI_Base GetAlliedObjective(float range, GameObjectType gametype)
        {
            switch (gametype)
            {
                case GameObjectType.obj_AI_Turret:
                    return
                        EntityManager.Turrets.Allies.OrderByDescending(a => a.Health)
                            .FirstOrDefault(
                                a =>
                                    a.IsAlly && a.IsValidTarget(range) && a.Distance(Player) <= range &&
                                    !a.IsInvulnerable && !Player.IsRecalling());
            }
            return null;
        }

        public static Obj_AI_Base GetEnemyObjective(float range, GameObjectType gametype)
        {
            switch (gametype)
            {
                case GameObjectType.obj_AI_Turret:
                    return
                        EntityManager.Turrets.Enemies.OrderByDescending(a => a.Health)
                            .FirstOrDefault(
                                a =>
                                    a.IsEnemy && a.IsValidTarget(range) && a.Distance(Player) <= range &&
                                    !a.IsInvulnerable && !Player.IsRecalling());
            }
            return null;
        }
        
        public static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        public static void Loading_OnLoadingComplete(EventArgs args)
        {
            ArsenalActivatorMenu = MainMenu.AddMenu("Arsenal Activator", "ArsenalActivator");
            ArsenalActivatorMenu.AddGroupLabel("Arsenal Activator");
            Items.MenuDraw();
            SummonerSpells.LoadingSummonerSpells();
            SummonerSpells.MenuDraw();

            // Activate
            Game.OnTick += Game_OnTick;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        public static void Game_OnTick(EventArgs args)
        {
            Items.CastItems();
            SummonerSpells.CastSummonerSpells();
        }

        // Draw
        public static void Drawing_OnDraw(EventArgs args)
        {
            SummonerSpells.DrawMode();
        }
    }
}