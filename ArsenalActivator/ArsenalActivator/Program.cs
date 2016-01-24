using System;
using EloBuddy;
using EloBuddy.SDK.Events;

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

        // Grab Player Attributes
        public static AIHeroClient Champion { get { return Player.Instance; } }

        public static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        public static void Loading_OnLoadingComplete(EventArgs args)
        {
            SummonerSpells.Initialize();
            MenuManager.Initialize();

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