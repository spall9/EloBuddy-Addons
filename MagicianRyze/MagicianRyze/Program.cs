﻿using System;
using System.Drawing;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace MagicianRyze
{
    // Created by Counter
    internal class Program
    {
        // Enum for Spells
        public enum AttackSpell
        {
            Q,
            W,
            E
        }

        // Menus
        public static Menu MagicianRyzeMenu,
            ComboMenu,
            HarassMenu,
            JungleMenu,
            LaneClearMenu,
            LastHitMenu,
            KillStealMenu,
            DrawingMenu,
            SettingMenu;

        // Player
        public static AIHeroClient Ryze = Player.Instance;

        // Skills
        public static Spell.Skillshot Q;
        public static Spell.Targeted W;
        public static Spell.Targeted E;
        public static Spell.Active R;
        public static int Ryzeskin;
        public static InventorySlot[] Itemlist = Ryze.InventoryItems;

        // Get Entities
        public static Obj_AI_Base GetAlly(float range, GameObjectType gametype)
        {
            switch (gametype)
            {
                case GameObjectType.AIHeroClient:
                    return EntityManager.Heroes.Allies
                        .OrderByDescending(a => a.Health)
                        .FirstOrDefault(a => a.IsAlly
                                             && a.IsValidTarget(range) && a.Distance(Ryze) <= range
                                             && !a.IsInvulnerable && !a.IsZombie
                                             && !Ryze.IsRecalling());
                case GameObjectType.obj_AI_Minion:
                    return EntityManager.MinionsAndMonsters.AlliedMinions
                        .OrderByDescending(a => a.Health)
                        .FirstOrDefault(a => a.IsAlly
                                             && a.IsValidTarget(range) && a.Distance(Ryze) <= range
                                             && !Ryze.IsRecalling());
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
                                             && a.IsValidTarget(range) && a.Distance(Ryze) <= range
                                             && !a.IsInvulnerable && !a.IsZombie
                                             && !a.HasBuff("BlackShield") && !a.HasBuff("SivirE") &&
                                             !a.HasBuff("FioraW")
                                             && !a.HasBuff("ChronoShift") && !Ryze.IsRecalling());
                case GameObjectType.obj_AI_Minion:
                    return EntityManager.MinionsAndMonsters.EnemyMinions
                        .OrderByDescending(a => a.Health)
                        .FirstOrDefault(a => a.IsEnemy
                                             && a.IsValidTarget(range) && a.Distance(Ryze) <= range
                                             && !a.HasBuff("BannerOfCommand")
                                             && !Ryze.IsRecalling());
            }
            return null;
        }

        public static Obj_AI_Base GetEnemyKs(AttackSpell spell, GameObjectType gametype)
        {
            switch (spell)
            {
                case AttackSpell.Q:
                    return ObjectManager
                        .Get<Obj_AI_Base>()
                        .OrderByDescending(a => a.Health)
                        .FirstOrDefault(a => a.IsEnemy && a.Type == gametype
                                             && a.IsValidTarget(Q.Range) && a.Distance(Ryze) <= Q.Range
                                             && !a.IsInvulnerable && !a.IsZombie
                                             && !a.HasBuff("BlackShield") && !a.HasBuff("SivirE") &&
                                             !a.HasBuff("FioraW")
                                             && !a.HasBuff("ChronoShift") && !Ryze.IsRecalling()
                                             && a.Health <= QDamage(a));
                case AttackSpell.W:
                    return ObjectManager
                        .Get<Obj_AI_Base>()
                        .OrderByDescending(a => a.Health)
                        .FirstOrDefault(a => a.IsEnemy && a.Type == gametype
                                             && a.IsValidTarget(W.Range) && a.Distance(Ryze) <= W.Range
                                             && !a.IsInvulnerable && !a.IsZombie
                                             && !a.HasBuff("BlackShield") && !a.HasBuff("SivirE") &&
                                             !a.HasBuff("FioraW")
                                             && !a.HasBuff("ChronoShift") && !Ryze.IsRecalling()
                                             && a.Health <= WDamage(a));
                case AttackSpell.E:
                    return ObjectManager
                        .Get<Obj_AI_Base>()
                        .OrderByDescending(a => a.Health)
                        .FirstOrDefault(a => a.IsEnemy && a.Type == gametype
                                             && a.IsValidTarget(E.Range) && a.Distance(Ryze) <= E.Range
                                             && !a.IsInvulnerable && !a.IsZombie
                                             && !a.HasBuff("BlackShield") && !a.HasBuff("SivirE") &&
                                             !a.HasBuff("FioraW")
                                             && !a.HasBuff("ChronoShift") && !Ryze.IsRecalling()
                                             && a.Health <= EDamage(a));
                default:
                    throw new ArgumentOutOfRangeException(null, spell, null);
            }
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
                                    a.IsAlly && a.IsValidTarget(range) && a.Distance(Ryze) <= range && !a.IsInvulnerable &&
                                    !Ryze.IsRecalling());
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
                                    a.IsEnemy && a.IsValidTarget(range) && a.Distance(Ryze) <= range &&
                                    !a.IsInvulnerable && !Ryze.IsRecalling());
            }
            return null;
        }

        // Spell Calculators
        public static float QDamage(Obj_AI_Base target)
        {
            return Ryze.CalculateDamageOnUnit(target, DamageType.Magical,
                new float[] {0, 60, 85, 110, 135, 160}[Q.Level] + 0.55f*Ryze.FlatMagicDamageMod +
                new[] {0f, 0.02f, 0.025f, 0.03f, 0.035f, 0.04f}[Q.Level]*Ryze.MaxMana);
        }

        public static float WDamage(Obj_AI_Base target)
        {
            return Ryze.CalculateDamageOnUnit(target, DamageType.Magical,
                new float[] {0, 80, 100, 120, 140, 160}[W.Level] + 0.4f*Ryze.FlatMagicDamageMod + 0.025f*Ryze.MaxMana);
        }

        public static float EDamage(Obj_AI_Base target)
        {
            return Ryze.CalculateDamageOnUnit(target, DamageType.Magical,
                new float[] {0, 36, 52, 68, 84, 100}[E.Level] + 0.2f*Ryze.FlatMagicDamageMod + 0.02f*Ryze.MaxMana);
        }

        public static void Main()
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        public static void Loading_OnLoadingComplete(EventArgs args)
        {
            //Confirming Champion
            if (Ryze.ChampionName != "Ryze") return;
            Ryzeskin = Ryze.SkinId;

            Q = new Spell.Skillshot(SpellSlot.Q, 900, SkillShotType.Linear, 250, 1700, 50)
            {
                MinimumHitChance = HitChance.High,
                AllowedCollisionCount = 0
            };
            W = new Spell.Targeted(SpellSlot.W, 600);
            E = new Spell.Targeted(SpellSlot.E, 600);
            R = new Spell.Active(SpellSlot.R);

            MagicianRyzeMenu = MainMenu.AddMenu("MagicianRyze", "MagicianRyze");
            MagicianRyzeMenu.AddGroupLabel("Magician Ryze");

            ComboMenu = MagicianRyzeMenu.AddSubMenu("Combo Features", "ComboFeatures");
            ComboMenu.AddGroupLabel("Combo Features");
            ComboMenu.AddLabel("Combo Modes:");
            ComboMenu.Add("ComboM", new CheckBox("ComboMode"));
            ComboMenu.AddSeparator(1);
            ComboMenu.AddLabel("Independent boxes for Spells:");
            ComboMenu.Add("Qcombo", new CheckBox("Use Q"));
            ComboMenu.Add("Wcombo", new CheckBox("Use W"));
            ComboMenu.Add("Ecombo", new CheckBox("Use E"));
            ComboMenu.Add("Rcombo", new CheckBox("Use R"));
            ComboMenu.AddSeparator(1);
            ComboMenu.Add("Pult", new Slider("Passive stacks to Ult", 4, 0, 4));

            HarassMenu = MagicianRyzeMenu.AddSubMenu("Harass Features", "HarassFeatures");
            HarassMenu.AddGroupLabel("Harass Features");
            HarassMenu.AddLabel("Independent boxes for Spells:");
            HarassMenu.Add("Qharass", new CheckBox("Use Q"));
            HarassMenu.AddSeparator(1);
            HarassMenu.Add("Harassmana", new Slider("Mana Limiter at Mana %", 25));

            JungleMenu = MagicianRyzeMenu.AddSubMenu("Jungle Features", "JungleFeatures");
            JungleMenu.AddGroupLabel("Jungle Features");
            JungleMenu.AddLabel("Independent boxes for Spells:");
            JungleMenu.Add("Qjungle", new CheckBox("Use Q"));
            JungleMenu.Add("Wjungle", new CheckBox("Use W"));
            JungleMenu.Add("Ejungle", new CheckBox("Use E"));
            JungleMenu.AddSeparator(1);
            JungleMenu.Add("Junglemana", new Slider("Mana Limiter at Mana %", 25));

            LaneClearMenu = MagicianRyzeMenu.AddSubMenu("Lane Clear Features", "LaneClearFeatures");
            LaneClearMenu.AddGroupLabel("Lane Clear Features");
            LaneClearMenu.AddLabel("Independent boxes for Spells:");
            LaneClearMenu.Add("Qlanec", new CheckBox("Use Q"));
            LaneClearMenu.Add("Wlanec", new CheckBox("Use W", false));
            LaneClearMenu.Add("Elanec", new CheckBox("Use E", false));
            LaneClearMenu.AddSeparator(1);
            LaneClearMenu.Add("Lanecmana", new Slider("Mana Limiter at Mana %", 25));
            LaneClearMenu.AddSeparator(1);
            LaneClearMenu.AddLabel("Late Game Lane Clear Mode - QWE minions to Mana %");
            LaneClearMenu.Add("LGlanec", new CheckBox("Late Game Mode"));
            LaneClearMenu.AddSeparator(1);
            LaneClearMenu.AddLabel("Late Game Mode Activators");
            LaneClearMenu.Add("LGlevel", new Slider("Activate Late Game at Level", 14, 1, 18));
            LaneClearMenu.Add("LGmana", new Slider("Mana Limiter at Mana %", 20));
            LaneClearMenu.AddSeparator(1);
            LaneClearMenu.Add("Planec", new CheckBox("Charge Passive in Lane Clear", false));

            LastHitMenu = MagicianRyzeMenu.AddSubMenu("Last Hit Features", "LastHitFeatures");
            LastHitMenu.AddGroupLabel("Last Hit Features");
            LastHitMenu.AddLabel("Independent boxes for Spells:");
            LastHitMenu.Add("Qlasthit", new CheckBox("Use Q"));
            LastHitMenu.Add("Wlasthit", new CheckBox("Use W", false));
            LastHitMenu.Add("Elasthit", new CheckBox("Use E", false));
            LastHitMenu.AddSeparator(1);
            LastHitMenu.Add("Lasthitmana", new Slider("Mana Limiter at Mana %", 25));

            KillStealMenu = MagicianRyzeMenu.AddSubMenu("KS Features", "KSFeatures");
            KillStealMenu.AddGroupLabel("Kill Steal Features");
            KillStealMenu.Add("KSmode", new CheckBox("KS Mode"));
            KillStealMenu.AddSeparator(1);
            KillStealMenu.AddLabel("Independent boxes for Spells:");
            KillStealMenu.Add("Qks", new CheckBox("Use Q in KS"));
            KillStealMenu.Add("Wks", new CheckBox("Use W in KS"));
            KillStealMenu.Add("Eks", new CheckBox("Use E in KS"));

            DrawingMenu = MagicianRyzeMenu.AddSubMenu("Drawing Features", "DrawingFeatures");
            DrawingMenu.AddGroupLabel("Drawing Features");
            DrawingMenu.Add("DrawM", new CheckBox("Draw Mode"));
            DrawingMenu.AddSeparator(1);
            DrawingMenu.AddLabel("Independent boxes for Spells:");
            DrawingMenu.Add("Qdraw", new CheckBox("Draw Q"));
            DrawingMenu.Add("WEdraw", new CheckBox("Draw W & E"));
            DrawingMenu.AddSeparator(1);
            DrawingMenu.AddLabel("Skin Designer");
            DrawingMenu.Add("DrawS", new CheckBox("Draw Skin Design", false));
            DrawingMenu.Add("Skins", new Slider("Skin Designer: ", 9, 0, 9));

            SettingMenu = MagicianRyzeMenu.AddSubMenu("Settings", "Settings");
            SettingMenu.AddGroupLabel("Settings");
            SettingMenu.AddLabel("Automatic Leveler");
            SettingMenu.Add("Autolvl", new CheckBox("Auto Leveler"));
            SettingMenu.AddSeparator(1);
            SettingMenu.AddLabel("Automatic Tear Stacker");
            SettingMenu.Add("StackM", new CheckBox("Stack Mode"));
            SettingMenu.AddSeparator(1);
            SettingMenu.AddLabel("Interrupter");
            SettingMenu.Add("Interruptmode", new CheckBox("Interrupt Mode"));
            SettingMenu.Add("Winterrupt", new CheckBox("Use W to interrupt"));
            SettingMenu.AddLabel("Gap Closer");
            SettingMenu.Add("Gapcmode", new CheckBox("Gap Closer Mode"));
            SettingMenu.Add("Wgapc", new CheckBox("Use W to gapclose"));
            
            Interrupter.OnInterruptableSpell += InterruptMode;
            Gapcloser.OnGapcloser += GapCloserMode;
            Game.OnTick += Game_OnTick;
            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        public static void Game_OnUpdate(EventArgs args)
        {
            Ryze.SetSkinId(DrawingMenu["DrawS"].Cast<CheckBox>().CurrentValue
                ? DrawingMenu["Skins"].Cast<Slider>().CurrentValue
                : Ryzeskin);
        }

        public static void Drawing_OnDraw(EventArgs args)
        {
            Color color;

            switch (Ryze.SkinId)
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
            if (!DrawingMenu["DrawM"].Cast<CheckBox>().CurrentValue) return;
            if (DrawingMenu["Qdraw"].Cast<CheckBox>().CurrentValue && Q.IsLearned)
                Drawing.DrawCircle(Ryze.Position, Q.Range, color);
            if (DrawingMenu["WEdraw"].Cast<CheckBox>().CurrentValue && (W.IsLearned || E.IsLearned))
                Drawing.DrawCircle(Ryze.Position, W.Range, color);
        }

        public static void Game_OnTick(EventArgs args)
        {
            if (Ryze.IsDead) return;
            if (SettingMenu["Autolvl"].Cast<CheckBox>().CurrentValue && Ryze.SpellTrainingPoints >= 1)
                LevelerMode();

            if (Orbwalker.IsAutoAttacking) return;
            switch (Orbwalker.ActiveModesFlags)
            {
                case Orbwalker.ActiveModes.Combo:
                    ComboMode();
                    break;
                case Orbwalker.ActiveModes.Harass:
                    HarassMode();
                    break;
                case Orbwalker.ActiveModes.JungleClear:
                    JungleMode();
                    break;
                case Orbwalker.ActiveModes.LaneClear:
                    LaneClearMode();
                    break;
                case Orbwalker.ActiveModes.LastHit:
                    LastHitMode();
                    break;
            }
            if (KillStealMenu["KSmode"].Cast<CheckBox>().CurrentValue)
                KsMode();
            if (SettingMenu["StackM"].Cast<CheckBox>().CurrentValue)
                StackMode();
        }

        public static void LevelerMode()
        {
            int[] leveler = {1, 2, 1, 3, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3};
            var avapoints = Ryze.SpellTrainingPoints;
            while (avapoints >= 1)
            {
                var skill = leveler[Ryze.Level - avapoints];

                switch (skill)
                {
                    default:
                        Ryze.Spellbook.LevelSpell(SpellSlot.Unknown);
                        break;
                    case 1:
                        Ryze.Spellbook.LevelSpell(SpellSlot.Q);
                        break;
                    case 2:
                        Ryze.Spellbook.LevelSpell(SpellSlot.W);
                        break;
                    case 3:
                        Ryze.Spellbook.LevelSpell(SpellSlot.E);
                        break;
                    case 4:
                        Ryze.Spellbook.LevelSpell(SpellSlot.R);
                        break;
                }
                avapoints--;
            }
        }

        public static void ComboMode()
        {
            if (Ryze.HasBuff("RyzeR"))
            {
                UltimateMode(GameObjectType.AIHeroClient);
                return;
            }
            if (ComboMenu["Wcombo"].Cast<CheckBox>().CurrentValue && W.IsReady())
            {
                var target = GetEnemy(W.Range, GameObjectType.AIHeroClient);
                if (target != null)
                    W.Cast(target);
            }
            if (ComboMenu["Qcombo"].Cast<CheckBox>().CurrentValue && Q.IsReady())
            {
                var target = GetEnemy(Q.Range, GameObjectType.AIHeroClient);
                if (target != null)
                    Q.Cast(Q.GetPrediction(target).CastPosition);
            }
            if (ComboMenu["Ecombo"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                var target = GetEnemy(E.Range, GameObjectType.AIHeroClient);
                if (target != null)
                    E.Cast(target);
            }
            if (ComboMenu["Rcombo"].Cast<CheckBox>().CurrentValue && R.IsReady())
            {
                if (Ryze.HasBuff("ryzepassivecharged")
                    || Ryze.GetBuffCount("ryzepassivestack") >= ComboMenu["Pult"].Cast<Slider>().CurrentValue)
                    R.Cast();
            }
        }

        public static void HarassMode()
        {
            if (Ryze.HasBuff("RyzeR"))
            {
                UltimateMode(GameObjectType.AIHeroClient);
                return;
            }
            if (Ryze.ManaPercent < LastHitMenu["Lasthitmana"].Cast<Slider>().CurrentValue) return;
            if (HarassMenu["Qharass"].Cast<CheckBox>().CurrentValue && Q.IsReady())
            {
                var target = GetEnemy(Q.Range, GameObjectType.AIHeroClient);
                if (target != null)
                    Q.Cast(Q.GetPrediction(target).CastPosition);
            }
        }

        public static void JungleMode()
        {
            if (Ryze.HasBuff("RyzeR"))
            {
                UltimateMode(GameObjectType.obj_AI_Minion, true);
                return;
            }
            if (Ryze.ManaPercent < JungleMenu["Junglemana"].Cast<Slider>().CurrentValue) return;
            if (JungleMenu["Qjungle"].Cast<CheckBox>().CurrentValue && Q.IsReady())
            {
                var target = GetEnemy(Q.Range, GameObjectType.obj_AI_Minion);
                if (target != null && target.IsMonster)
                    Q.Cast(Q.GetPrediction(target).CastPosition);
            }
            if (JungleMenu["Wjungle"].Cast<CheckBox>().CurrentValue && W.IsReady())
            {
                var target = GetEnemy(W.Range, GameObjectType.obj_AI_Minion);
                if (target != null && target.IsMonster)
                    W.Cast(target);
            }
            if (JungleMenu["Ejungle"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                var target = GetEnemy(E.Range, GameObjectType.obj_AI_Minion);
                if (target != null && target.IsMonster)
                    E.Cast(target);
            }
        }

        public static void LaneClearMode()
        {
            if (Ryze.HasBuff("RyzeR"))
            {
                UltimateMode(GameObjectType.obj_AI_Minion);
                return;
            }
            if (Ryze.ManaPercent < LaneClearMenu["Lanecmana"].Cast<Slider>().CurrentValue) return;
            if (LaneClearMenu["Qlanec"].Cast<CheckBox>().CurrentValue && Q.IsReady())
            {
                var target = GetEnemy(Q.Range, GameObjectType.obj_AI_Minion);
                if (target != null && !target.IsMonster)
                    Q.Cast(Q.GetPrediction(target).CastPosition);
            }
            if (LaneClearMenu["Wlanec"].Cast<CheckBox>().CurrentValue && W.IsReady())
            {
                var target = GetEnemy(W.Range, GameObjectType.obj_AI_Minion);
                if (target != null && !target.IsMonster)
                    W.Cast(target);
            }
            if (LaneClearMenu["Elanec"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                var target = GetEnemy(E.Range, GameObjectType.obj_AI_Minion);
                if (target != null && !target.IsMonster)
                    E.Cast(target);
            }
        }

        public static void LastHitMode()
        {
            if (Ryze.HasBuff("RyzeR"))
            {
                UltimateMode(GameObjectType.obj_AI_Minion);
                return;
            }
            if (Ryze.ManaPercent < LastHitMenu["Lasthitmana"].Cast<Slider>().CurrentValue) return;
            if (LastHitMenu["Qlasthit"].Cast<CheckBox>().CurrentValue && Q.IsReady())
            {
                var target = GetEnemyKs(AttackSpell.Q, GameObjectType.obj_AI_Minion);
                if (target != null && !target.IsMonster)
                    Q.Cast(Q.GetPrediction(target).CastPosition);
            }
            if (LastHitMenu["Wlasthit"].Cast<CheckBox>().CurrentValue && W.IsReady())
            {
                var target = GetEnemyKs(AttackSpell.W, GameObjectType.obj_AI_Minion);
                if (target != null && !target.IsMonster)
                    W.Cast(target);
            }
            if (LastHitMenu["Elasthit"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                var target = GetEnemyKs(AttackSpell.E, GameObjectType.obj_AI_Minion);
                if (target != null && !target.IsMonster)
                    E.Cast(target);
            }
        }

        public static void UltimateMode(GameObjectType type, bool isMonster = false)
        {
            if (isMonster)
            {
                if (Q.IsReady())
                {
                    var target = GetEnemy(Q.Range, type);
                    if (target != null && target.IsMonster)
                        Q.Cast(Q.GetPrediction(target).CastPosition);
                }
                if (W.IsReady())
                {
                    var target = GetEnemy(W.Range, type);
                    if (target != null && target.IsMonster)
                        W.Cast(target);
                }
                if (E.IsReady())
                {
                    var target = GetEnemy(E.Range, type);
                    if (target != null && target.IsMonster)
                        E.Cast(target);
                }
            }
            else
            {
                if (Q.IsReady())
                {
                    var target = GetEnemy(Q.Range, type);
                    if (target != null)
                        Q.Cast(Q.GetPrediction(target).CastPosition);
                }
                if (W.IsReady())
                {
                    var target = GetEnemy(W.Range, type);
                    if (target != null)
                        W.Cast(target);
                }
                if (E.IsReady())
                {
                    var target = GetEnemy(E.Range, type);
                    if (target != null)
                        E.Cast(target);
                }
            }
        }

        public static void KsMode()
        {
            if (KillStealMenu["Qks"].Cast<CheckBox>().CurrentValue && Q.IsReady())
            {
                var target = GetEnemyKs(AttackSpell.Q, GameObjectType.AIHeroClient);
                if (target != null)
                    Q.Cast(Q.GetPrediction(target).CastPosition);
            }
            if (KillStealMenu["Wks"].Cast<CheckBox>().CurrentValue && W.IsReady())
            {
                var target = GetEnemyKs(AttackSpell.W, GameObjectType.AIHeroClient);
                if (target != null)
                    W.Cast(target);
            }
            if (KillStealMenu["Eks"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                var target = GetEnemyKs(AttackSpell.E, GameObjectType.AIHeroClient);
                if (target != null)
                    E.Cast(target);
            }
        }

        public static void StackMode()
        {
            foreach (var item in Itemlist)
            {
                if ((item.Id == ItemId.Tear_of_the_Goddess || item.Id == ItemId.Tear_of_the_Goddess_Crystal_Scar ||
                     item.Id == ItemId.Archangels_Staff || item.Id == ItemId.Archangels_Staff_Crystal_Scar ||
                     item.Id == ItemId.Manamune || item.Id == ItemId.Manamune_Crystal_Scar) && item.Stacks < 750 &&
                    Ryze.IsInShopRange() && Q.IsReady())
                    Q.Cast(Ryze);
            }
        }

        public static void InterruptMode(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {
            if (!SettingMenu["Interruptmode"].Cast<CheckBox>().CurrentValue) return;
            if (sender != null && SettingMenu["Winterrupt"].Cast<CheckBox>().CurrentValue && W.IsReady())
            {
                var target = GetEnemy(W.Range, GameObjectType.AIHeroClient);
                if (target != null)
                    W.Cast(target);
            }
        }

        public static void GapCloserMode(Obj_AI_Base sender, Gapcloser.GapcloserEventArgs args)
        {
            if (!SettingMenu["Gapcmode"].Cast<CheckBox>().CurrentValue) return;
            if (sender != null && SettingMenu["Wgapc"].Cast<CheckBox>().CurrentValue && W.IsReady())
            {
                var target = GetEnemy(W.Range, GameObjectType.AIHeroClient);
                if (target != null)
                    W.Cast(target);
            }
        }
    }
}