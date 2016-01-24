using System;
using System.Drawing;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace ProtectorLeona
{
    // Created by Counter
    internal class Program
    {
        // Enum for Spells
        public enum AttackSpell
        {
            Q,
            E,
            R
        }

        // Menus
        public static Menu ProtectorLeonaMenu,
            ComboMenu,
            HarassMenu,
            DrawingMenu,
            SettingMenu;

        // Player
        public static AIHeroClient Champion
        {
            get { return ObjectManager.Player; }
        }

        // Skills
        public static Spell.Active W;
        public static Spell.Skillshot E;
        public static Spell.Skillshot R;
        public static int Leonaskin;
        public static InventorySlot[] Itemlist;

        public static Spell.Active Q = new Spell.Active(SpellSlot.Q, (uint) (Champion.GetAutoAttackRange() + 30));

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
            switch (spell)
            {
                case AttackSpell.Q:
                    return ObjectManager
                        .Get<Obj_AI_Base>()
                        .OrderByDescending(a => a.Health)
                        .FirstOrDefault(a => a.IsEnemy && a.Type == gametype
                                             && a.IsValidTarget(Q.Range) && a.Distance(Champion) <= Q.Range
                                             && !a.IsInvulnerable && !a.IsZombie
                                             && !a.HasBuff("BlackShield") && !a.HasBuff("SivirE") &&
                                             !a.HasBuff("FioraW")
                                             && !a.HasBuff("ChronoShift") && !Champion.IsRecalling()
                                             && a.Health <= QDamage(a));
                case AttackSpell.E:
                    return ObjectManager
                        .Get<Obj_AI_Base>()
                        .OrderByDescending(a => a.Health)
                        .FirstOrDefault(a => a.IsEnemy && a.Type == gametype
                                             && a.IsValidTarget(E.Range) && a.Distance(Champion) <= E.Range
                                             && !a.IsInvulnerable && !a.IsZombie
                                             && !a.HasBuff("BlackShield") && !a.HasBuff("SivirE") &&
                                             !a.HasBuff("FioraW")
                                             && !a.HasBuff("ChronoShift") && !Champion.IsRecalling()
                                             && a.Health <= EDamage(a));
                case AttackSpell.R:
                    return ObjectManager
                        .Get<Obj_AI_Base>()
                        .OrderByDescending(a => a.Health)
                        .FirstOrDefault(a => a.IsEnemy && a.Type == gametype
                                             && a.IsValidTarget(R.Range) && a.Distance(Champion) <= R.Range
                                             && !a.IsInvulnerable && !a.IsZombie
                                             && !a.HasBuff("BlackShield") && !a.HasBuff("SivirE") &&
                                             !a.HasBuff("FioraW")
                                             && !a.HasBuff("ChronoShift") && !Champion.IsRecalling()
                                             && a.Health <= RDamage(a));
            }
            return null;
        }

        public static Obj_AI_Base GetAlliedObjective(float range, GameObjectType gametype)
        {
            switch (gametype)
            {
                case GameObjectType.obj_AI_Turret:
                    return EntityManager.Turrets.Allies
                        .OrderByDescending(a => a.Health)
                        .FirstOrDefault(a => a.IsAlly
                                             && a.IsValidTarget(range) && a.Distance(Champion) <= range
                                             && !a.IsInvulnerable
                                             && !Champion.IsRecalling());
            }
            return null;
        }

        public static Obj_AI_Base GetEnemyObjective(float range, GameObjectType gametype)
        {
            switch (gametype)
            {
                case GameObjectType.obj_AI_Turret:
                    return EntityManager.Turrets.Enemies
                        .OrderByDescending(a => a.Health)
                        .FirstOrDefault(a => a.IsEnemy
                                             && a.IsValidTarget(range) && a.Distance(Champion) <= range
                                             && !a.IsInvulnerable
                                             && !Champion.IsRecalling());
            }
            return null;
        }

        // Spell Calculators
        public static float QDamage(Obj_AI_Base target)
        {
            return Champion.CalculateDamageOnUnit(target, DamageType.Magical,
                new float[] {0, 40, 70, 100, 130, 160}[Q.Level]
                + 0.3f*Champion.FlatMagicDamageMod
                + Champion.GetAutoAttackDamage(target));
        }

        public static float WDamage(Obj_AI_Base target)
        {
            return Champion.CalculateDamageOnUnit(target, DamageType.Magical,
                new float[] {0, 60, 110, 160, 210, 260}[W.Level]
                + 0.4f*Champion.FlatMagicDamageMod);
        }

        public static float EDamage(Obj_AI_Base target)
        {
            return Champion.CalculateDamageOnUnit(target, DamageType.Magical,
                new float[] {0, 60, 100, 140, 180, 220}[E.Level]
                + 0.4f*Champion.FlatMagicDamageMod);
        }

        public static float RDamage(Obj_AI_Base target)
        {
            return Champion.CalculateDamageOnUnit(target, DamageType.Magical,
                new float[] {0, 150, 250, 350}[E.Level]
                + 0.8f*Champion.FlatMagicDamageMod);
        }

        public static void Main()
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        public static void Loading_OnLoadingComplete(EventArgs args)
        {
            //Confirming Champion
            if (Champion.ChampionName != "Leona") return;
            Leonaskin = Champion.SkinId;

            W = new Spell.Active(SpellSlot.W, 275);
            E = new Spell.Skillshot(SpellSlot.E, 875, SkillShotType.Linear, 250, 2000, 70)
            {
                MinimumHitChance = HitChance.High,
                AllowedCollisionCount = int.MaxValue
            };
            R = new Spell.Skillshot(SpellSlot.R, 1200, SkillShotType.Circular, 1000, int.MaxValue, 250)
            {
                MinimumHitChance = HitChance.High,
                AllowedCollisionCount = int.MaxValue
            };

            ProtectorLeonaMenu = MainMenu.AddMenu("ProtectorLeona", "ProtectorLeona");
            ProtectorLeonaMenu.AddGroupLabel("Protector Leona");

            ComboMenu = ProtectorLeonaMenu.AddSubMenu("Combo Features", "ComboFeatures");
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
            ComboMenu.Add("Rlimiter", new Slider("Use R when Enemies in range - greater or equal to:", 2, 1, 5));

            HarassMenu = ProtectorLeonaMenu.AddSubMenu("Harass Features", "HarassFeatures");
            HarassMenu.AddGroupLabel("Harass Features");
            HarassMenu.AddLabel("Independent boxes for Spells:");
            HarassMenu.Add("Qharass", new CheckBox("Use Q"));
            HarassMenu.Add("Eharass", new CheckBox("Use E"));
            HarassMenu.AddSeparator(1);
            HarassMenu.Add("Harassmana", new Slider("Mana Limiter at Mana %", 25));

            DrawingMenu = ProtectorLeonaMenu.AddSubMenu("Drawing Features", "DrawingFeatures");
            DrawingMenu.AddGroupLabel("Drawing Features");
            DrawingMenu.Add("DrawM", new CheckBox("Draw Mode"));
            DrawingMenu.AddSeparator(1);
            DrawingMenu.AddLabel("Independent boxes for Spells:");
            DrawingMenu.Add("Qdraw", new CheckBox("Draw Q", false));
            DrawingMenu.Add("Wdraw", new CheckBox("Draw W", false));
            DrawingMenu.Add("Edraw", new CheckBox("Draw E"));
            DrawingMenu.Add("Rdraw", new CheckBox("Draw R"));
            DrawingMenu.AddSeparator(1);
            DrawingMenu.AddLabel("Skin Designer");
            DrawingMenu.Add("DrawS", new CheckBox("Draw Skin Design", false));
            DrawingMenu.Add("Skins", new Slider("Skin Designer: ", 4, 0, 8));

            SettingMenu = ProtectorLeonaMenu.AddSubMenu("Settings", "Settings");
            SettingMenu.AddGroupLabel("Settings");
            SettingMenu.AddLabel("Automatic Leveler");
            SettingMenu.Add("Autolvl", new CheckBox("Auto Leveler"));
            SettingMenu.AddSeparator(1);
            SettingMenu.AddLabel("Use AutoAttacks in Modes");
            SettingMenu.Add("Aattack", new CheckBox("Use AA"));
            SettingMenu.AddSeparator(1);
            SettingMenu.AddLabel("Interrupter");
            SettingMenu.Add("Interruptmode", new CheckBox("Interrupt Mode"));
            SettingMenu.Add("EQinterrupt", new CheckBox("Use E & Q to interrupt"));
            SettingMenu.Add("Rinterrupt", new CheckBox("Use R to interrupt"));
            SettingMenu.AddLabel("Gap Closer");
            SettingMenu.Add("Gapcmode", new CheckBox("Gap Closer Mode"));
            SettingMenu.Add("EQgapc", new CheckBox("Use E & Q to gapclose"));

            Interrupter.OnInterruptableSpell += InterruptMode;
            Gapcloser.OnGapcloser += GapCloserMode;
            Game.OnTick += Game_OnTick;
            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        public static void Game_OnUpdate(EventArgs args)
        {
            Champion.SetSkinId(DrawingMenu["DrawS"].Cast<CheckBox>().CurrentValue
                ? DrawingMenu["Skins"].Cast<Slider>().CurrentValue
                : Leonaskin);
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
            if (!DrawingMenu["DrawM"].Cast<CheckBox>().CurrentValue) return;
            if (DrawingMenu["Qdraw"].Cast<CheckBox>().CurrentValue && Q.IsLearned)
                Drawing.DrawCircle(Champion.Position, Q.Range, color);
            if (DrawingMenu["Wdraw"].Cast<CheckBox>().CurrentValue && W.IsLearned)
                Drawing.DrawCircle(Champion.Position, W.Range, color);
            if (DrawingMenu["Edraw"].Cast<CheckBox>().CurrentValue && E.IsLearned)
                Drawing.DrawCircle(Champion.Position, E.Range, color);
            if (DrawingMenu["Rdraw"].Cast<CheckBox>().CurrentValue && R.IsLearned)
                Drawing.DrawCircle(Champion.Position, R.Range, color);
        }

        public static void Game_OnTick(EventArgs args)
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
            }
        }

        public static void LevelerMode()
        {
            int[] leveler = {1, 3, 2, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3};
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
            if (SettingMenu["Aattack"].Cast<CheckBox>().CurrentValue)
            {
                var target = GetEnemy(Champion.GetAutoAttackRange(), GameObjectType.AIHeroClient);
                if (target != null && Orbwalker.CanAutoAttack)
                    Player.IssueOrder(GameObjectOrder.AttackTo, target);
            }
            if (ComboMenu["Ecombo"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                var target = GetEnemy(E.Range, GameObjectType.AIHeroClient);
                if (target != null && !target.IsUnderHisturret())
                    E.Cast(E.GetPrediction(target).CastPosition);
            }
            if (ComboMenu["Wcombo"].Cast<CheckBox>().CurrentValue && W.IsReady())
            {
                var target = GetEnemy(W.Range, GameObjectType.AIHeroClient);
                if (target != null)
                    W.Cast();
            }
            if (ComboMenu["Qcombo"].Cast<CheckBox>().CurrentValue && Q.IsReady())
            {
                var target = GetEnemy(Q.Range, GameObjectType.AIHeroClient);
                if (target != null && !target.HasBuff("leonazenithbladeroot"))
                    Q.Cast();
                Orbwalker.ResetAutoAttack();
            }
            if (ComboMenu["Rcombo"].Cast<CheckBox>().CurrentValue && R.IsReady()
                && Champion.CountEnemiesInRange(R.Range) >= ComboMenu["Rlimiter"].Cast<Slider>().CurrentValue)
            {
                var target = GetEnemy(R.Range, GameObjectType.AIHeroClient);
                if (target != null && !target.IsUnderEnemyturret())
                    R.Cast(R.GetPrediction(target).CastPosition);
            }
        }

        public static void HarassMode()
        {
            if (Champion.ManaPercent < HarassMenu["Harassmana"].Cast<Slider>().CurrentValue) return;
            if (SettingMenu["Aattack"].Cast<CheckBox>().CurrentValue)
            {
                var target = GetEnemy(Champion.GetAutoAttackRange(), GameObjectType.AIHeroClient);
                if (target != null && Orbwalker.CanAutoAttack)
                    Player.IssueOrder(GameObjectOrder.AttackTo, target);
            }
            if (HarassMenu["Ecombo"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                var target = GetEnemy(E.Range, GameObjectType.AIHeroClient);
                if (target != null && !target.IsUnderHisturret())
                    E.Cast(E.GetPrediction(target).CastPosition);
            }
            if (HarassMenu["Qharass"].Cast<CheckBox>().CurrentValue && Q.IsReady())
            {
                var target = GetEnemy(Q.Range, GameObjectType.AIHeroClient);
                if (target != null && !target.HasBuff("leonazenithbladeroot"))
                    Q.Cast();
                Orbwalker.ResetAutoAttack();
            }
        }

        public static void InterruptMode(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {
            if (!SettingMenu["Interruptmode"].Cast<CheckBox>().CurrentValue) return;
            if (sender != null && !sender.IsUnderEnemyturret())
            {
                if (SettingMenu["EQinterrupt"].Cast<CheckBox>().CurrentValue
                    && args.DangerLevel < DangerLevel.High
                    && Q.IsReady() && E.IsReady())
                {
                    var target = GetEnemy(E.Range, GameObjectType.AIHeroClient);
                    if (target != null)
                    {
                        E.Cast(E.GetPrediction(target).CastPosition);
                        if (Orbwalker.CanAutoAttack)
                            Orbwalker.ForcedTarget = target;
                        if (!target.HasBuff("leonazenithbladeroot")
                            && Champion.IsInAutoAttackRange(target))
                        {
                            Q.Cast();
                            Orbwalker.ResetAutoAttack();
                            if (Orbwalker.CanAutoAttack)
                                Player.IssueOrder(GameObjectOrder.AttackTo, target);
                        }
                    }
                }
                if (SettingMenu["Rinterrupt"].Cast<CheckBox>().CurrentValue
                    && args.DangerLevel >= DangerLevel.High
                    && R.IsReady())
                {
                    var target = GetEnemy(R.Range, GameObjectType.AIHeroClient);
                    if (target != null)
                    {
                        R.Cast(R.GetPrediction(target).CastPosition);
                    }
                }
            }
        }

        public static void GapCloserMode(Obj_AI_Base sender, Gapcloser.GapcloserEventArgs args)
        {
            if (!SettingMenu["Gapcmode"].Cast<CheckBox>().CurrentValue) return;
            if (sender != null && !sender.IsUnderEnemyturret()
                && SettingMenu["EQgapc"].Cast<CheckBox>().CurrentValue
                && Q.IsReady() && E.IsReady())
            {
                var target = GetEnemy(E.Range, GameObjectType.AIHeroClient);
                if (target != null && !target.IsUnderHisturret())
                {
                    E.Cast(E.GetPrediction(target).CastPosition);
                    if (Orbwalker.CanAutoAttack)
                        Player.IssueOrder(GameObjectOrder.AttackTo, target);
                    if (!target.HasBuff("leonazenithbladeroot")
                        && Champion.IsInAutoAttackRange(target))
                    {
                        Q.Cast();
                        if (Orbwalker.CanAutoAttack)
                        {
                            Orbwalker.ResetAutoAttack();
                            Player.IssueOrder(GameObjectOrder.AttackTo, target);
                        }
                    }
                }
            }
        }
    }
}