using System;
using System.Drawing;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace SeekerVelKoz
{
    internal class VelKoz
    {
        // Enum for Spells
        public enum AttackSpell
        {
            Q,
            W,
            E,
            R
        }

        // Menus
        public static Menu SeekerVelKozMenu,
            ComboMenu,
            HarassMenu,
            JungleMenu,
            LaneClearMenu,
            LastHitMenu,
            KillStealMenu,
            DrawingMenu,
            SettingMenu;

        // Skills
        public static Spell.Skillshot Q;
        public static MissileClient Qmiss = null;
        public static Spell.Skillshot W;
        public static Spell.Skillshot E;
        public static Spell.Skillshot R;
        public static int VelKozskin;
        public static InventorySlot[] Itemlist = Champion.InventoryItems;

        // Player
        public static AIHeroClient Champion
        {
            get { return Player.Instance; }
        }

        // Get Entities
        public static Obj_AI_Base GetAlly(float range, GameObjectType gametype)
        {
            switch (gametype)
            {
                case GameObjectType.AIHeroClient:
                    return EntityManager.Heroes.Allies
                        .OrderByDescending(a => a.Health)
                        .FirstOrDefault(a => a.IsAlly
                                             && a.IsValidTarget(range) && a.Distance(Champion) <= range
                                             && !a.IsInvulnerable && !a.IsZombie
                                             && !Champion.IsRecalling());
                case GameObjectType.obj_AI_Minion:
                    return EntityManager.MinionsAndMonsters.AlliedMinions
                        .OrderByDescending(a => a.Health)
                        .FirstOrDefault(a => a.IsAlly
                                             && a.IsValidTarget(range) && a.Distance(Champion) <= range
                                             && !Champion.IsRecalling());
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
                                             && a.IsValidTarget(range) && a.Distance(Champion) <= range
                                             && !a.IsInvulnerable && !a.IsZombie
                                             && !a.HasBuff("BlackShield") && !a.HasBuff("SivirE") &&
                                             !a.HasBuff("FioraW")
                                             && !a.HasBuff("ChronoShift") && !Champion.IsRecalling());
                case GameObjectType.obj_AI_Minion:
                    return EntityManager.MinionsAndMonsters.EnemyMinions
                        .OrderByDescending(a => a.Health)
                        .FirstOrDefault(a => a.IsEnemy
                                             && a.IsValidTarget(range) && a.Distance(Champion) <= range
                                             && !a.HasBuff("BannerOfCommand")
                                             && !Champion.IsRecalling());
            }
            return null;
        }

        public static Obj_AI_Base GetEnemyKs(AttackSpell spell, GameObjectType gametype)
        {
            float range = 0;
            switch (spell)
            {
                case AttackSpell.Q:
                    range = Q.Range;
                    break;
                case AttackSpell.W:
                    range = W.Range;
                    break;
                case AttackSpell.E:
                    range = E.Range;
                    break;
                case AttackSpell.R:
                    range = R.Range;
                    break;
            }

            return ObjectManager
                .Get<Obj_AI_Base>()
                .OrderByDescending(a => a.Health)
                .FirstOrDefault(a => a.IsEnemy && a.Type == gametype
                                     && a.IsValidTarget(range) && a.Distance(Champion) <= range
                                     && !a.IsInvulnerable && !a.IsZombie
                                     && !a.HasBuff("BlackShield") && !a.HasBuff("SivirE") &&
                                     !a.HasBuff("FioraW")
                                     && !a.HasBuff("ChronoShift") && !Champion.IsRecalling()
                                     &&
                                     ((spell == AttackSpell.Q && a.Health <= QDamage(a)) ||
                                      (spell == AttackSpell.W && a.Health <= WDamage(a)) ||
                                      (spell == AttackSpell.E && a.Health <= EDamage(a)) ||
                                      (spell == AttackSpell.R && a.Health <= RDamage(a))));
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
                                    a.IsAlly && a.IsValidTarget(range) && a.Distance(Champion) <= range &&
                                    !a.IsInvulnerable && !Champion.IsRecalling());
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
                                    a.IsEnemy && a.IsValidTarget(range) && a.Distance(Champion) <= range &&
                                    !a.IsInvulnerable && !Champion.IsRecalling());
            }
            return null;
        }

        // Spell Calculators
        private static float QDamage(Obj_AI_Base target)
        {
            return Champion.CalculateDamageOnUnit(target, DamageType.Magical,
                new float[] {0, 80, 120, 150, 200, 240}[Q.Level] + 0.6f*Champion.FlatMagicDamageMod);
        }

        private static float WDamage(Obj_AI_Base target)
        {
            return Champion.CalculateDamageOnUnit(target, DamageType.Magical,
                new float[] {0, 75, 125, 175, 225, 275}[W.Level] + 0.625f*Champion.FlatMagicDamageMod);
        }

        private static float EDamage(Obj_AI_Base target)
        {
            return Champion.CalculateDamageOnUnit(target, DamageType.Magical,
                new float[] {0, 70, 100, 130, 160, 190}[E.Level] + 0.5f*Champion.FlatMagicDamageMod);
        }

        private static float RDamage(Obj_AI_Base target)
        {
            return Champion.CalculateDamageOnUnit(target, DamageType.Magical,
                new float[] {0, 500, 700, 900}[R.Level] + 0.5f*Champion.FlatMagicDamageMod);
        }

        private static void Main()
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            // Initialize classes
            //QSplit.Initialize();

            //Confirming Champion
            if (Champion.ChampionName != "Velkoz") return;
            VelKozskin = Champion.SkinId;

            Q = new Spell.Skillshot(SpellSlot.Q, 1050, SkillShotType.Linear, 250, 1300, 50)
            {
                MinimumHitChance = HitChance.High,
                AllowedCollisionCount = 0
            };
            W = new Spell.Skillshot(SpellSlot.W, 1050, SkillShotType.Linear, 250, 1700, 80)
            {
                MinimumHitChance = HitChance.High,
                AllowedCollisionCount = int.MaxValue
            };
            E = new Spell.Skillshot(SpellSlot.E, 850, SkillShotType.Circular, 500, 1500, 120)
            {
                MinimumHitChance = HitChance.High,
                AllowedCollisionCount = int.MaxValue
            };
            R = new Spell.Skillshot(SpellSlot.R, 1550, SkillShotType.Linear)
            {
                MinimumHitChance = HitChance.High,
                AllowedCollisionCount = int.MaxValue
            };

            SeekerVelKozMenu = MainMenu.AddMenu("Seeker Vel'Koz", "SeekerVelKoz");
            SeekerVelKozMenu.AddGroupLabel("Seeker Vel'Koz");

            ComboMenu = SeekerVelKozMenu.AddSubMenu("Combo Features", "ComboFeatures");
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
            ComboMenu.Add("Rlimiter", new Slider("Use R when Enemies in range - greater or equal to:", 3, 1, 5));

            HarassMenu = SeekerVelKozMenu.AddSubMenu("Harass Features", "HarassFeatures");
            HarassMenu.AddGroupLabel("Harass Features");
            HarassMenu.AddLabel("Independent boxes for Spells:");
            HarassMenu.Add("Qharass", new CheckBox("Use Q"));
            HarassMenu.Add("Wharass", new CheckBox("Use W", false));
            HarassMenu.Add("Eharass", new CheckBox("Use E", false));
            HarassMenu.AddSeparator(1);
            HarassMenu.Add("Harassmana", new Slider("Mana Limiter at Mana %", 25));

            JungleMenu = SeekerVelKozMenu.AddSubMenu("Jungle Features", "JungleFeatures");
            JungleMenu.AddGroupLabel("Jungle Features");
            JungleMenu.AddLabel("Independent boxes for Spells:");
            JungleMenu.Add("Qjungle", new CheckBox("Use Q"));
            JungleMenu.Add("Wjungle", new CheckBox("Use W"));
            JungleMenu.Add("Ejungle", new CheckBox("Use E"));
            JungleMenu.AddSeparator(1);
            JungleMenu.Add("Junglemana", new Slider("Mana Limiter at Mana %", 25));

            LaneClearMenu = SeekerVelKozMenu.AddSubMenu("Lane Clear Features", "LaneClearFeatures");
            LaneClearMenu.AddGroupLabel("Lane Clear Features");
            LaneClearMenu.AddLabel("Independent boxes for Spells:");
            LaneClearMenu.Add("Qlanec", new CheckBox("Use Q", false));
            LaneClearMenu.Add("Wlanec", new CheckBox("Use W", false));
            LaneClearMenu.Add("Elanec", new CheckBox("Use E", false));
            LaneClearMenu.AddSeparator(1);
            LaneClearMenu.Add("Lanecmana", new Slider("Mana Limiter at Mana %", 25));

            LastHitMenu = SeekerVelKozMenu.AddSubMenu("Last Hit Features", "LastHitFeatures");
            LastHitMenu.AddGroupLabel("Last Hit Features");
            LastHitMenu.AddLabel("Independent boxes for Spells:");
            LastHitMenu.Add("Qlasthit", new CheckBox("Use Q", false));
            LastHitMenu.Add("Wlasthit", new CheckBox("Use W", false));
            LastHitMenu.Add("Elasthit", new CheckBox("Use E", false));
            LastHitMenu.AddSeparator(1);
            LastHitMenu.Add("Lasthitmana", new Slider("Mana Limiter at Mana %", 25));

            KillStealMenu = SeekerVelKozMenu.AddSubMenu("KS Features", "KSFeatures");
            KillStealMenu.AddGroupLabel("Kill Steal Features");
            KillStealMenu.Add("KSmode", new CheckBox("KS Mode"));
            KillStealMenu.AddSeparator(1);
            KillStealMenu.AddLabel("Independent boxes for Spells:");
            KillStealMenu.Add("Qks", new CheckBox("Use Q in KS"));
            KillStealMenu.Add("Wks", new CheckBox("Use W in KS", false));
            KillStealMenu.Add("Eks", new CheckBox("Use E in KS", false));
            KillStealMenu.Add("Rks", new CheckBox("Use R in KS", false));

            DrawingMenu = SeekerVelKozMenu.AddSubMenu("Drawing Features", "DrawingFeatures");
            DrawingMenu.AddGroupLabel("Drawing Features");
            DrawingMenu.Add("DrawM", new CheckBox("Draw Mode"));
            DrawingMenu.AddSeparator(1);
            DrawingMenu.AddLabel("Independent boxes for Spells:");
            DrawingMenu.Add("QWdraw", new CheckBox("Draw Q & W"));
            DrawingMenu.Add("Edraw", new CheckBox("Draw E"));
            DrawingMenu.Add("Rdraw", new CheckBox("Draw R"));
            DrawingMenu.AddSeparator(1);
            DrawingMenu.AddLabel("Skin Designer");
            DrawingMenu.Add("DrawS", new CheckBox("Draw Skin Design", false));
            DrawingMenu.Add("Skins", new Slider("Skin Designer: ", 0, 0, 2));

            SettingMenu = SeekerVelKozMenu.AddSubMenu("Settings", "Settings");
            SettingMenu.AddGroupLabel("Settings");
            SettingMenu.AddLabel("Automatic Leveler");
            SettingMenu.Add("Autolvl", new CheckBox("Auto Leveler"));
            SettingMenu.AddSeparator(1);
            SettingMenu.AddLabel("Ult Follower Mode");
            SettingMenu.Add("Rfollow", new CheckBox("Using R will follow target"));
            SettingMenu.AddLabel("Automatic Tear Stacker");
            SettingMenu.Add("StackM", new CheckBox("Stack Mode"));
            SettingMenu.AddLabel("Interrupter");
            SettingMenu.Add("Interruptmode", new CheckBox("Interrupt Mode"));
            SettingMenu.Add("Einterrupt", new CheckBox("Use E to interrupt"));
            SettingMenu.AddSeparator(1);
            SettingMenu.AddLabel("Gap Closer");
            SettingMenu.Add("Gapcmode", new CheckBox("Gap Closer Mode"));
            SettingMenu.Add("Egapc", new CheckBox("Use E to gapclose"));

            Interrupter.OnInterruptableSpell += InterruptMode;
            Gapcloser.OnGapcloser += GapCloserMode;
            Game.OnTick += Game_OnTick;
            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            Player.SetSkinId(DrawingMenu["DrawS"].Cast<CheckBox>().CurrentValue
                ? DrawingMenu["Skins"].Cast<Slider>().CurrentValue
                : VelKozskin);
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            Color color;

            switch (Champion.SkinId)
            {
                default:
                    color = Color.Transparent;
                    break;
                case 0:
                    color = Color.FromArgb(168, 27, 168);
                    break;
                case 1:
                    color = Color.FromArgb(254, 35, 39);
                    break;
                case 2:
                    color = Color.FromArgb(240, 187, 55);
                    break;
            }
            if (!DrawingMenu["DrawM"].Cast<CheckBox>().CurrentValue) return;
            if (DrawingMenu["QWdraw"].Cast<CheckBox>().CurrentValue && (Q.IsLearned || W.IsLearned))
                Drawing.DrawCircle(Champion.Position, Q.Range, color);
            if (DrawingMenu["Edraw"].Cast<CheckBox>().CurrentValue && E.IsLearned)
                Drawing.DrawCircle(Champion.Position, E.Range, color);
            if (DrawingMenu["Rdraw"].Cast<CheckBox>().CurrentValue && R.IsLearned)
                Drawing.DrawCircle(Champion.Position, R.Range, color);
        }

        private static void Game_OnTick(EventArgs args)
        {
            if (Champion.IsDead) return;
            if (SettingMenu["Autolvl"].Cast<CheckBox>().CurrentValue && Champion.SpellTrainingPoints >= 1)
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
            if (SettingMenu["Rfollow"].Cast<CheckBox>().CurrentValue && Champion.HasBuff("VelkozR"))
                UltFollowMode();
        }

        private static void LevelerMode()
        {
            int[] leveler = {1, 2, 3, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3};
            var avapoints = Champion.SpellTrainingPoints;
            while (avapoints >= 1)
            {
                var skill = leveler[Champion.Level - avapoints];

                switch (skill)
                {
                    default:
                        Champion.Spellbook.LevelSpell(SpellSlot.Unknown);
                        break;
                    case 1:
                        Champion.Spellbook.LevelSpell(SpellSlot.Q);
                        break;
                    case 2:
                        Champion.Spellbook.LevelSpell(SpellSlot.W);
                        break;
                    case 3:
                        Champion.Spellbook.LevelSpell(SpellSlot.E);
                        break;
                    case 4:
                        Champion.Spellbook.LevelSpell(SpellSlot.R);
                        break;
                }
                avapoints--;
            }
        }

        public static void ComboMode()
        {
            if (ComboMenu["Qcombo"].Cast<CheckBox>().CurrentValue && Q.IsReady())
            {
                var target = GetEnemy(Q.Range, GameObjectType.AIHeroClient);
                if (target != null && Q.Name == "VelkozQ")
                {
                    Q.Cast(target);
                }
            }
            if (ComboMenu["Ecombo"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                var target = GetEnemy(E.Range, GameObjectType.AIHeroClient);
                if (target != null && (target.HasBuffOfType(BuffType.Slow) || (target.Distance(Champion) < 200)))
                    E.Cast(target);
            }
            if (ComboMenu["Wcombo"].Cast<CheckBox>().CurrentValue && W.IsReady())
            {
                var target = GetEnemy(W.Range, GameObjectType.AIHeroClient);
                if (target != null &&
                    (target.HasBuffOfType(BuffType.Knockback) || target.HasBuffOfType(BuffType.Knockup) ||
                     target.HasBuffOfType(BuffType.Snare) || target.HasBuffOfType(BuffType.Stun)))
                    W.Cast(target);
            }
            if (ComboMenu["Rcombo"].Cast<CheckBox>().CurrentValue && R.IsReady() &&
                Champion.CountEnemiesInRange(R.Range) >= ComboMenu["Rlimiter"].Cast<Slider>().CurrentValue &&
                !Champion.IsUnderTurret())
            {
                var target = GetEnemy(R.Range, GameObjectType.AIHeroClient);
                if (target != null && !Player.HasBuff("VelkozR") && target.Distance(Champion) >= R.Range*0.5f &&
                    target.Distance(Champion) <= R.Range - 100)
                    R.Cast(target);
            }
        }

        public static void HarassMode()
        {
            if (Champion.ManaPercent < HarassMenu["Harassmana"].Cast<Slider>().CurrentValue) return;
            if (HarassMenu["Qharass"].Cast<CheckBox>().CurrentValue && Q.IsReady() && Q.Name == "VelkozQ")
            {
                var target = GetEnemy(Q.Range, GameObjectType.AIHeroClient);
                if (target != null)
                    Q.Cast(target);
            }
            if (HarassMenu["Eharass"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                var target = GetEnemy(E.Range, GameObjectType.AIHeroClient);
                if (target != null)
                    E.Cast(target);
            }
            if (HarassMenu["Wharass"].Cast<CheckBox>().CurrentValue && W.IsReady())
            {
                var target = GetEnemy(W.Range, GameObjectType.AIHeroClient);
                if (target != null)
                    W.Cast(target);
            }
        }

        public static void JungleMode()
        {
            if (Champion.ManaPercent < JungleMenu["Junglemana"].Cast<Slider>().CurrentValue) return;
            if (JungleMenu["Qjungle"].Cast<CheckBox>().CurrentValue && Q.IsReady() && Q.Name == "VelkozQ")
            {
                var target = GetEnemy(Q.Range, GameObjectType.obj_AI_Minion);
                if (target != null && target.IsMonster)
                    Q.Cast(target);
            }
            if (JungleMenu["Ejungle"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                var target = GetEnemy(E.Range, GameObjectType.obj_AI_Minion);
                if (target != null && target.IsMonster)
                    E.Cast(target);
            }
            if (JungleMenu["Wjungle"].Cast<CheckBox>().CurrentValue && W.IsReady())
            {
                var target = GetEnemy(W.Range, GameObjectType.obj_AI_Minion);
                if (target != null && target.IsMonster)
                    W.Cast(target);
            }
        }

        public static void LaneClearMode()
        {
            if (Champion.ManaPercent < LaneClearMenu["Lanecmana"].Cast<Slider>().CurrentValue) return;
            if (LaneClearMenu["Qlanec"].Cast<CheckBox>().CurrentValue && Q.IsReady() && Q.Name == "VelkozQ")
            {
                var target = GetEnemy(Q.Range, GameObjectType.obj_AI_Minion);
                if (target != null && !target.IsMonster)
                    Q.Cast(target);
            }
            if (LaneClearMenu["Elanec"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                var target = GetEnemy(E.Range, GameObjectType.obj_AI_Minion);
                if (target != null && !target.IsMonster)
                    E.Cast(target);
            }
            if (LaneClearMenu["Wlanec"].Cast<CheckBox>().CurrentValue && W.IsReady())
            {
                var target = GetEnemy(W.Range, GameObjectType.obj_AI_Minion);
                if (target != null && !target.IsMonster)
                    W.Cast(target);
            }
        }

        public static void LastHitMode()
        {
            if (Champion.ManaPercent < LastHitMenu["Lasthitmana"].Cast<Slider>().CurrentValue) return;
            if (LastHitMenu["Qlasthit"].Cast<CheckBox>().CurrentValue && Q.IsReady() && Q.Name == "VelkozQ")
            {
                var target = GetEnemyKs(AttackSpell.Q, GameObjectType.obj_AI_Minion);
                if (target != null && !target.IsMonster)
                    Q.Cast(Q.GetPrediction(target).CastPosition);
            }
            if (LastHitMenu["Elasthit"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                var target = GetEnemyKs(AttackSpell.E, GameObjectType.obj_AI_Minion);
                if (target != null && !target.IsMonster)
                    E.Cast(E.GetPrediction(target).CastPosition);
            }
            if (LastHitMenu["Wlasthit"].Cast<CheckBox>().CurrentValue && W.IsReady())
            {
                var target = GetEnemyKs(AttackSpell.W, GameObjectType.obj_AI_Minion);
                if (target != null && !target.IsMonster)
                    W.Cast(W.GetPrediction(target).CastPosition);
            }
        }

        public static void KsMode()
        {
            if (KillStealMenu["Qks"].Cast<CheckBox>().CurrentValue && Q.IsReady())
            {
                var target = GetEnemyKs(AttackSpell.Q, GameObjectType.AIHeroClient);
                if (target != null)
                    Q.Cast(target);
            }
            if (KillStealMenu["Eks"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                var target = GetEnemyKs(AttackSpell.E, GameObjectType.AIHeroClient);
                if (target != null)
                    E.Cast(E.GetPrediction(target).CastPosition);
            }
            if (KillStealMenu["Wks"].Cast<CheckBox>().CurrentValue && W.IsReady())
            {
                var target = GetEnemyKs(AttackSpell.W, GameObjectType.AIHeroClient);
                if (target != null)
                    W.Cast(W.GetPrediction(target).CastPosition);
            }
            if (KillStealMenu["Rks"].Cast<CheckBox>().CurrentValue && R.IsReady() && !Champion.IsUnderTurret())
            {
                var target = GetEnemyKs(AttackSpell.R, GameObjectType.AIHeroClient);
                if (target != null && !Player.HasBuff("VelkozR"))
                    R.Cast(R.GetPrediction(target).CastPosition);
            }
        }

        public static void UltFollowMode()
        {
            var target = GetEnemy(R.Range, GameObjectType.AIHeroClient);
            var mtarget = GetEnemy(R.Range, GameObjectType.obj_AI_Minion);
            if (target != null)
                Champion.Spellbook.UpdateChargeableSpell(SpellSlot.R, target.ServerPosition, false, false);
            else if (mtarget != null)
                Champion.Spellbook.UpdateChargeableSpell(SpellSlot.R, mtarget.ServerPosition, false, false);
        }

        public static void StackMode()
        {
            foreach (var item in Itemlist)
            {
                if ((item.Id == ItemId.Tear_of_the_Goddess || item.Id == ItemId.Tear_of_the_Goddess_Crystal_Scar ||
                     item.Id == ItemId.Archangels_Staff || item.Id == ItemId.Archangels_Staff_Crystal_Scar ||
                     item.Id == ItemId.Manamune || item.Id == ItemId.Manamune_Crystal_Scar) && item.Stacks < 750 &&
                    Champion.IsInShopRange())
                {
                    if (Q.IsReady() && Q.Name == "VelkozQ")
                        Q.Cast(Champion);
                }
            }
        }

        public static void InterruptMode(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {
            if (SettingMenu["Interruptmode"].Cast<CheckBox>().CurrentValue) return;
            if (sender != null && SettingMenu["Einterrupt"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                var target = GetEnemy(E.Range, GameObjectType.AIHeroClient);
                if (target != null)
                    E.Cast(E.GetPrediction(target).CastPosition);
            }
        }

        public static void GapCloserMode(Obj_AI_Base sender, Gapcloser.GapcloserEventArgs args)
        {
            if (SettingMenu["Gapcmode"].Cast<CheckBox>().CurrentValue) return;
            if (sender != null && SettingMenu["Egapc"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                var target = GetEnemy(E.Range, GameObjectType.AIHeroClient);
                if (target != null)
                    E.Cast(E.GetPrediction(target).CastPosition);
            }
        }
    }
}