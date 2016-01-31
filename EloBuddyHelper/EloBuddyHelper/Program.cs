using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;

namespace EloBuddyHelper
{
    internal class Program
    {
        public static Menu EloBuddyHelperMenu;
        public static AIHeroClient Player { get { return ObjectManager.Player; } }


        // Grab Ally
        public static Obj_AI_Base GetAlly(float range, GameObjectType gametype)
        {
            return ObjectManager.Get<Obj_AI_Base>()
                .OrderBy(a => a.Health).FirstOrDefault(a => a.IsAlly
                                                            && a.Type == gametype && !Player.IsRecalling()
                                                            && !a.IsDead && a.IsValidTarget(range) && !a.IsInvulnerable
                                                            && a.Distance(Player) <= range);
        }

        // Grab Enemy
        public static Obj_AI_Base GetEnemy(float range, GameObjectType gametype)
        {
            return ObjectManager.Get<Obj_AI_Base>()
                .OrderBy(a => a.Health).FirstOrDefault(a => a.IsEnemy
                                                            && a.Type == gametype && !Player.IsRecalling()
                                                            && !a.IsDead && a.IsValidTarget(range) && !a.IsInvulnerable
                                                            && !a.HasBuff("ChronoShift")
                                                            && a.Distance(Player) <= range);
        }


        public static void Main()
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        public static void Loading_OnLoadingComplete(EventArgs args)
        {
            Chat.Print("EloBuddyHelper Loaded!");

            Obj_AI_Base.OnBuffGain += OnBuffGain;
            Obj_AI_Base.OnBuffLose += OnBuffLose;
            Game.OnTick += GameOnTick;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        public static void GameOnTick(EventArgs args)
        {
            /*SpellDataInst sumspell1 = null;
            SpellDataInst sumspell2 = null;
            sumspell1 = Player.GetSpell(SpellSlot.Summoner1);
            sumspell2 = Player.GetSpell(SpellSlot.Summoner2);
            Chat.Print(sumspell1.Name);
            Chat.Print(sumspell2.Name);*/

            /*foreach (InventorySlot item in itemlist)
            {
                string itemidnum = itemlist[1].Id.ToString();

                Chat.Print(itemlist[1].Name);
                if (itemidnum != null)
                {
                    Chat.Print(itemidnum);
                }
            }*/
            //Chat.Print(Player.Spellbook.GetSpell(SpellSlot.Q).ToggleState);

            //SpellDataInst item4 = Player.GetSpell(SpellSlot.Trinket);
            //Chat.Print(item4);
        }

        public static void OnBuffGain(Obj_AI_Base sender, Obj_AI_BaseBuffGainEventArgs buff)
        {
            //if(sender.IsMe)
                //Chat.Print("Buff Gained: " + buff.Buff.Name);
            //if (sender.IsAlly)
            //Chat.Print("Ally Buff Gained: " + buff.Buff.Name);
            if (sender.IsEnemy)
                Chat.Print("Enemy Buff Gained: " + buff.Buff.Name);
        }

        public static void OnBuffLose(Obj_AI_Base sender, Obj_AI_BaseBuffLoseEventArgs buff)
        {
            //if (sender.IsMe)
                //Chat.Print("Buff Lost: " + buff.Buff.Name);
            //if (sender.IsAlly)
            // Chat.Print("Ally Buff Lost: " + buff.Buff.Name);
            if (sender.IsEnemy)
                Chat.Print("Enemy Buff Lost: " + buff.Buff.Name);
        }

        public static void Drawing_OnDraw(EventArgs args)
        {
        }
    }
}