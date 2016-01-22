using System;
using System.Drawing;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace BallistaKogMaw
{
    // Created by Counter
    internal class Program
    {
        // Enum for Spells
        public enum AttackSpell
        {
            Q,
            W,
            E,
            R,
            P
        }

        // Menus
        public static Menu BallistaKogMawMenu,
            ComboMenu,
            HarassMenu,
            JungleMenu,
            LaneClearMenu,
            LastHitMenu,
            KillStealMenu,
            DrawingMenu,
            SettingMenu;

        // Player
        public static AIHeroClient KogMaw = Player.Instance;

        // Skills
        public static Spell.Skillshot Q;
        public static Spell.Skillshot E;
        public static int KogMawskin;
        public static InventorySlot[] Itemlist = KogMaw.InventoryItems;

        // Get Entities
        public static Obj_AI_Base Ptarget;

        public static Spell.Active W = new Spell.Active(SpellSlot.W,
            (uint) (560 + 30*KogMaw.Spellbook.GetSpell(SpellSlot.W).Level));

        public static Spell.Skillshot R = new Spell.Skillshot(SpellSlot.R,
            (uint) (1200 + 300*KogMaw.Spellbook.GetSpell(SpellSlot.R).Level),
            SkillShotType.Circular, 1200, int.MaxValue, 225)
        {
            MinimumHitChance = HitChance.High,
            AllowedCollisionCount = int.MaxValue
        };

        public static Obj_AI_Base GetAlly(float range, GameObjectType gametype)
        {
            switch (gametype)
            {
                case GameObjectType.AIHeroClient:
                    return EntityManager.Heroes.Allies
                        .OrderByDescending(a => a.Health)
                        .FirstOrDefault(a => a.IsAlly
                                             && a.IsValidTarget(range) && a.Distance(KogMaw) <= range
                                             && !a.IsInvulnerable && !a.IsZombie
                                             && !KogMaw.IsRecalling());
                case GameObjectType.obj_AI_Minion:
                    return EntityManager.MinionsAndMonsters.AlliedMinions
                        .OrderByDescending(a => a.Health)
                        .FirstOrDefault(a => a.IsAlly
                                             && a.IsValidTarget(range) && a.Distance(KogMaw) <= range
                                             && !KogMaw.IsRecalling());
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
                                             && a.IsValidTarget(range) && a.Distance(KogMaw) <= range
                                             && !a.IsInvulnerable && !a.IsZombie
                                             && !a.HasBuff("BlackShield") && !a.HasBuff("SivirE") &&
                                             !a.HasBuff("FioraW")
                                             && !a.HasBuff("ChronoShift") && !KogMaw.IsRecalling());
                case GameObjectType.obj_AI_Minion:
                    return EntityManager.MinionsAndMonsters.EnemyMinions
                        .OrderByDescending(a => a.Health)
                        .FirstOrDefault(a => a.IsEnemy
                                             && a.IsValidTarget(range) && a.Distance(KogMaw) <= range
                                             && !a.HasBuff("BannerOfCommand")
                                             && !KogMaw.IsRecalling());
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
                                             && a.IsValidTarget(Q.Range) && a.Distance(KogMaw) <= Q.Range
                                             && !a.IsInvulnerable && !a.IsZombie
                                             && !a.HasBuff("BlackShield") && !a.HasBuff("SivirE") &&
                                             !a.HasBuff("FioraW")
                                             && !a.HasBuff("ChronoShift") && !KogMaw.IsRecalling()
                                             && a.Health <= QDamage(a));
                case AttackSpell.W:
                    return ObjectManager
                        .Get<Obj_AI_Base>()
                        .OrderByDescending(a => a.Health)
                        .FirstOrDefault(a => a.IsEnemy && a.Type == gametype
                                             && a.IsValidTarget(W.Range) && a.Distance(KogMaw) <= W.Range
                                             && a.Distance(KogMaw) >= KogMaw.GetAutoAttackRange(a)
                                             && !a.IsInvulnerable && !a.IsZombie
                                             && !a.HasBuff("BlackShield") && !a.HasBuff("SivirE") &&
                                             !a.HasBuff("FioraW")
                                             && !a.HasBuff("ChronoShift") && !KogMaw.IsRecalling()
                                             && a.Health <= WDamage(a));
                case AttackSpell.E:
                    return ObjectManager
                        .Get<Obj_AI_Base>()
                        .OrderByDescending(a => a.Health)
                        .FirstOrDefault(a => a.IsEnemy && a.Type == gametype
                                             && a.IsValidTarget(E.Range) && a.Distance(KogMaw) <= E.Range
                                             && a.Distance(KogMaw) >= KogMaw.GetAutoAttackRange(a)
                                             && !a.IsInvulnerable && !a.IsZombie
                                             && !a.HasBuff("BlackShield") && !a.HasBuff("SivirE") &&
                                             !a.HasBuff("FioraW")
                                             && !a.HasBuff("ChronoShift") && !KogMaw.IsRecalling()
                                             && a.Health <= EDamage(a));
                case AttackSpell.R:
                    return ObjectManager
                        .Get<Obj_AI_Base>()
                        .OrderByDescending(a => a.Health)
                        .FirstOrDefault(a => a.IsEnemy && a.Type == gametype
                                             && a.IsValidTarget(R.Range) && a.Distance(KogMaw) <= R.Range
                                             && !a.IsInvulnerable && !a.IsZombie
                                             && !a.HasBuff("BlackShield") && !a.HasBuff("SivirE") &&
                                             !a.HasBuff("FioraW")
                                             && !a.HasBuff("ChronoShift") && !KogMaw.IsRecalling()
                                             && a.Health <= RDamage(a));
                case AttackSpell.P:
                    return ObjectManager
                        .Get<Obj_AI_Base>()
                        .OrderByDescending(a => a.Health)
                        .FirstOrDefault(a => a.IsEnemy && a.Type == gametype
                                             && a.IsValidTarget(KogMaw.GetAutoAttackRange(a)) &&
                                             a.Distance(KogMaw) <= KogMaw.GetAutoAttackRange(a)
                                             && !a.IsInvulnerable && !a.IsZombie
                                             && !a.HasBuff("BlackShield") && !a.HasBuff("SivirE") &&
                                             !a.HasBuff("FioraW")
                                             && !a.HasBuff("ChronoShift") && !KogMaw.IsRecalling()
                                             && a.Health <= PDamage(a));
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
                                    a.IsAlly && a.IsValidTarget(range) && a.Distance(KogMaw) <= range &&
                                    !a.IsInvulnerable && !KogMaw.IsRecalling());
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
                                    a.IsEnemy && a.IsValidTarget(range) && a.Distance(KogMaw) <= range &&
                                    !a.IsInvulnerable && !KogMaw.IsRecalling());
            }
            return null;
        }

        // Spell Calculators
        private static float PDamage(Obj_AI_Base target)
        {
            return KogMaw.CalculateDamageOnUnit(target, DamageType.True, 100 + 25*KogMaw.Level);
        }

        private static float QDamage(Obj_AI_Base target)
        {
            return KogMaw.CalculateDamageOnUnit(target, DamageType.Magical,
                new float[] {0, 80, 130, 180, 230, 280}[Q.Level] + 0.5f*KogMaw.FlatMagicDamageMod);
        }

        private static float WDamage(Obj_AI_Base target)
        {
            var bonusd = 0.02f*target.MaxHealth + 0.0075f*KogMaw.FlatMagicDamageMod;
            if (target.Type == GameObjectType.obj_AI_Minion && bonusd > 100)
                bonusd = 100;

            return KogMaw.CalculateDamageOnUnit(target, DamageType.Magical, KogMaw.GetAutoAttackDamage(target) + bonusd);
        }

        private static float EDamage(Obj_AI_Base target)
        {
            return KogMaw.CalculateDamageOnUnit(target, DamageType.Magical,
                new float[] {0, 60, 110, 160, 210, 260}[E.Level] + 0.7f*KogMaw.FlatMagicDamageMod);
        }

        private static float RDamage(Obj_AI_Base target)
        {
            float multiplier = 1;
            if (target.HealthPercent <= target.MaxHealth*0.25f)
                multiplier = 3;
            else if (target.HealthPercent <= target.MaxHealth*0.5f)
                multiplier = 2;

            return KogMaw.CalculateDamageOnUnit(target, DamageType.Magical,
                new float[] {0, 70, 110, 150}[R.Level] + 0.25f*KogMaw.FlatMagicDamageMod +
                0.65f*KogMaw.FlatPhysicalDamageMod*multiplier);
        }

        private static void Main()
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            // Confirming Champion
            if (Player.Instance.ChampionName != "KogMaw") return;
            KogMawskin = KogMaw.SkinId;

            Q = new Spell.Skillshot(SpellSlot.Q, 1175, SkillShotType.Linear, 250, 1650, 70)
            {
                MinimumHitChance = HitChance.High,
                AllowedCollisionCount = 0
            };
            E = new Spell.Skillshot(SpellSlot.E, 1280, SkillShotType.Linear, 250, 1400, 120)
            {
                MinimumHitChance = HitChance.High,
                AllowedCollisionCount = int.MaxValue
            };

            BallistaKogMawMenu = MainMenu.AddMenu("BallistaKogMaw", "BallistaKogMaw");
            BallistaKogMawMenu.AddGroupLabel("Ballista Kog'Maw");

            ComboMenu = BallistaKogMawMenu.AddSubMenu("Combo Features", "ComboFeatures");
            ComboMenu.AddGroupLabel("Combo Features");
            ComboMenu.AddLabel("Combo Modes:");
            ComboMenu.Add("ComboM", new CheckBox("ComboMode"));
            ComboMenu.AddSeparator(1);
            ComboMenu.AddLabel("Independent boxes for Spells:");
            ComboMenu.Add("Qcombo", new CheckBox("Use Q"));
            ComboMenu.Add("Wcombo", new CheckBox("Use W"));
            ComboMenu.Add("Ecombo", new CheckBox("Use E"));
            ComboMenu.Add("Rcombo", new CheckBox("Use R"));

            HarassMenu = BallistaKogMawMenu.AddSubMenu("Harass Features", "HarassFeatures");
            HarassMenu.AddGroupLabel("Harass Features");
            HarassMenu.AddLabel("Independent boxes for Spells:");
            HarassMenu.Add("Qharass", new CheckBox("Use Q"));
            HarassMenu.Add("Wharass", new CheckBox("Use W"));
            HarassMenu.Add("Eharass", new CheckBox("Use E", false));
            HarassMenu.Add("Rharass", new CheckBox("Use R", false));
            HarassMenu.AddSeparator(1);
            HarassMenu.Add("Harassmana", new Slider("Mana Limiter at Mana %", 25));

            JungleMenu = BallistaKogMawMenu.AddSubMenu("Jungle Features", "JungleFeatures");
            JungleMenu.AddGroupLabel("Jungle Features");
            JungleMenu.AddLabel("Independent boxes for Spells:");
            JungleMenu.Add("Qjungle", new CheckBox("Use Q"));
            JungleMenu.Add("Wjungle", new CheckBox("Use W"));
            JungleMenu.Add("Ejungle", new CheckBox("Use E", false));
            JungleMenu.Add("Rjungle", new CheckBox("Use R", false));
            JungleMenu.AddSeparator(1);
            JungleMenu.Add("Junglemana", new Slider("Mana Limiter at Mana %", 25));

            LaneClearMenu = BallistaKogMawMenu.AddSubMenu("Lane Clear Features", "LaneClearFeatures");
            LaneClearMenu.AddGroupLabel("Lane Clear Features");
            LaneClearMenu.AddLabel("Independent boxes for Spells:");
            LaneClearMenu.Add("Qlanec", new CheckBox("Use Q"));
            LaneClearMenu.Add("Wlanec", new CheckBox("Use W", false));
            LaneClearMenu.Add("Elanec", new CheckBox("Use E", false));
            LaneClearMenu.Add("Rlanec", new CheckBox("Use R", false));
            LaneClearMenu.AddSeparator(1);
            LaneClearMenu.Add("Lanecmana", new Slider("Mana Limiter at Mana %", 25));

            LastHitMenu = BallistaKogMawMenu.AddSubMenu("Last Hit Features", "LastHitFeatures");
            LastHitMenu.AddGroupLabel("Last Hit Features");
            LastHitMenu.AddLabel("Independent boxes for Spells:");
            LastHitMenu.Add("Qlasthit", new CheckBox("Use Q"));
            LastHitMenu.Add("Wlasthit", new CheckBox("Use W - if out of AA range"));
            LastHitMenu.Add("Rlasthit", new CheckBox("Use R", false));
            LastHitMenu.AddSeparator(1);
            LastHitMenu.Add("Lasthitmana", new Slider("Mana Limiter at Mana %", 25));

            KillStealMenu = BallistaKogMawMenu.AddSubMenu("KS Features", "KSFeatures");
            KillStealMenu.AddGroupLabel("Kill Steal Features");
            KillStealMenu.Add("KSmode", new CheckBox("KS Mode"));
            KillStealMenu.AddSeparator(1);
            KillStealMenu.AddLabel("Independent boxes for Spells:");
            KillStealMenu.Add("Qks", new CheckBox("Use Q in KS"));
            KillStealMenu.Add("Wks", new CheckBox("Use W in KS - if out of AA range"));
            KillStealMenu.Add("Eks", new CheckBox("Use E in KS", false));
            KillStealMenu.Add("Rks", new CheckBox("Use R in KS"));

            DrawingMenu = BallistaKogMawMenu.AddSubMenu("Drawing Features", "DrawingFeatures");
            DrawingMenu.AddGroupLabel("Drawing Features");
            DrawingMenu.Add("DrawM", new CheckBox("Draw Mode"));
            DrawingMenu.AddSeparator(1);
            DrawingMenu.AddLabel("Independent boxes for Spells:");
            DrawingMenu.Add("Qdraw", new CheckBox("Draw Q"));
            DrawingMenu.Add("Wdraw", new CheckBox("Draw W"));
            DrawingMenu.Add("Edraw", new CheckBox("Draw E"));
            DrawingMenu.Add("Rdraw", new CheckBox("Draw R"));
            DrawingMenu.AddSeparator(1);
            DrawingMenu.AddLabel("Skin Designer");
            DrawingMenu.Add("DrawS", new CheckBox("Draw Skin Design"));
            DrawingMenu.Add("Skins", new Slider("Skin Designer: ", 7, 0, 8));

            SettingMenu = BallistaKogMawMenu.AddSubMenu("Settings", "Settings");
            SettingMenu.AddGroupLabel("Settings");
            SettingMenu.AddLabel("Automatic Leveler");
            SettingMenu.Add("Autolvl", new CheckBox("Auto Leveler"));
            SettingMenu.AddSeparator(1);
            SettingMenu.AddLabel("Automatic Tear Stacker");
            SettingMenu.Add("StackM", new CheckBox("Stack Mode"));
            SettingMenu.AddSeparator(1);
            SettingMenu.Add("DeathFmode", new CheckBox("Use Passive DeathFollower"));
            SettingMenu.AddSeparator(1);
            SettingMenu.AddLabel("Gap Closer");
            SettingMenu.Add("Gapcmode", new CheckBox("Gap Closer Mode"));
            SettingMenu.Add("Egapc", new CheckBox("Use E to gapclose"));

            if (SettingMenu["Gapcmode"].Cast<CheckBox>().CurrentValue)
                Gapcloser.OnGapcloser += GapCloserMode;

            Game.OnTick += Game_OnTick;
            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            KogMaw.SetSkinId(DrawingMenu["DrawS"].Cast<CheckBox>().CurrentValue
                ? DrawingMenu["Skins"].Cast<Slider>().CurrentValue
                : KogMawskin);
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            Color color;
            Color color2;

            switch (KogMaw.SkinId)
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
                    color = Color.SlateGray;
                    color2 = Color.DarkRed;
                    break;
                case 8:
                    color = Color.Firebrick;
                    color2 = Color.Tomato;
                    break;
            }

            if (!DrawingMenu["DrawM"].Cast<CheckBox>().CurrentValue) return;
            if (DrawingMenu["Qdraw"].Cast<CheckBox>().CurrentValue && Q.IsLearned)
                Drawing.DrawCircle(KogMaw.Position, Q.Range, color);
            if (DrawingMenu["Wdraw"].Cast<CheckBox>().CurrentValue && W.IsLearned &&
                KogMaw.HasBuff("KogMawBioArcaneBarrage"))
                Drawing.DrawCircle(KogMaw.Position, W.Range, color2);
            if (DrawingMenu["Edraw"].Cast<CheckBox>().CurrentValue && E.IsLearned)
                Drawing.DrawCircle(KogMaw.Position, E.Range, color);
            if (DrawingMenu["Rdraw"].Cast<CheckBox>().CurrentValue && R.IsLearned)
                Drawing.DrawCircle(KogMaw.Position, R.Range, color);
        }

        private static void Game_OnTick(EventArgs args)
        {
            if (KogMaw.IsDead) return;
            if (SettingMenu["Autolvl"].Cast<CheckBox>().CurrentValue && KogMaw.SpellTrainingPoints >= 1)
                LevelerMode();

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                ComboMode();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass) &&
                KogMaw.ManaPercent >= HarassMenu["Harassmana"].Cast<Slider>().CurrentValue)
                HarassMode();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear) &&
                KogMaw.ManaPercent >= JungleMenu["Junglemana"].Cast<Slider>().CurrentValue)
                JungleMode();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) &&
                KogMaw.ManaPercent >= LaneClearMenu["Lanecmana"].Cast<Slider>().CurrentValue)
                LaneClearMode();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit) &&
                KogMaw.ManaPercent >= LastHitMenu["Lasthitmana"].Cast<Slider>().CurrentValue)
                LastHitMode();
            if (SettingMenu["KSmode"].Cast<CheckBox>().CurrentValue)
                KsMode();
            if (SettingMenu["StackM"].Cast<CheckBox>().CurrentValue)
                StackMode();
            if (SettingMenu["DeathFmode"].Cast<CheckBox>().CurrentValue)
            {
                if (!KogMaw.HasBuff("kogmawicathiansurprise"))
                    Ptarget = null;
                else
                    DeathFollowMode();
            }
        }

        public static void LevelerMode()
        {
            int[] leveler = {1, 3, 1, 2, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3};
            var avapoints = KogMaw.SpellTrainingPoints;
            while (avapoints >= 1)
            {
                var skill = leveler[KogMaw.Level - avapoints];

                switch (skill)
                {
                    default:
                        KogMaw.Spellbook.LevelSpell(SpellSlot.Unknown);
                        break;
                    case 1:
                        KogMaw.Spellbook.LevelSpell(SpellSlot.Q);
                        break;
                    case 2:
                        KogMaw.Spellbook.LevelSpell(SpellSlot.W);
                        break;
                    case 3:
                        KogMaw.Spellbook.LevelSpell(SpellSlot.E);
                        break;
                    case 4:
                        KogMaw.Spellbook.LevelSpell(SpellSlot.R);
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
                if (target != null)
                    Q.Cast(Q.GetPrediction(target).CastPosition);
            }
            if (ComboMenu["Wcombo"].Cast<CheckBox>().CurrentValue && W.IsReady())
            {
                var target = GetEnemy(W.Range, GameObjectType.AIHeroClient);
                if (target != null)
                    W.Cast();
            }
            if (ComboMenu["Ecombo"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                var target = GetEnemy(E.Range, GameObjectType.AIHeroClient);
                if (target != null)
                    E.Cast(E.GetPrediction(target).CastPosition);
            }
            if (ComboMenu["Rcombo"].Cast<CheckBox>().CurrentValue && R.IsReady())
            {
                var target = GetEnemy(R.Range, GameObjectType.AIHeroClient);
                if (target != null)
                    R.Cast(R.GetPrediction(target).CastPosition);
            }
        }

        public static void HarassMode()
        {
            if (HarassMenu["Qharass"].Cast<CheckBox>().CurrentValue && Q.IsReady())
            {
                var target = GetEnemy(Q.Range, GameObjectType.AIHeroClient);
                if (target != null)
                    Q.Cast(Q.GetPrediction(target).CastPosition);
            }
            if (HarassMenu["Wharass"].Cast<CheckBox>().CurrentValue && W.IsReady())
            {
                var target = GetEnemy(W.Range, GameObjectType.AIHeroClient);
                if (target != null)
                    W.Cast();
            }
            if (HarassMenu["Eharass"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                var target = GetEnemy(E.Range, GameObjectType.AIHeroClient);
                if (target != null)
                    E.Cast(E.GetPrediction(target).CastPosition);
            }
            if (HarassMenu["Rharass"].Cast<CheckBox>().CurrentValue && R.IsReady())
            {
                var target = GetEnemy(R.Range, GameObjectType.AIHeroClient);
                if (target != null)
                    R.Cast(R.GetPrediction(target).CastPosition);
            }
        }

        public static void JungleMode()
        {
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
                    W.Cast();
            }
            if (JungleMenu["Ejungle"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                var target = GetEnemy(E.Range, GameObjectType.obj_AI_Minion);
                if (target != null && target.IsMonster)
                    E.Cast(E.GetPrediction(target).CastPosition);
            }
            if (JungleMenu["Rjungle"].Cast<CheckBox>().CurrentValue && R.IsReady())
            {
                var target = GetEnemy(R.Range, GameObjectType.obj_AI_Minion);
                if (target != null && target.IsMonster)
                    R.Cast(R.GetPrediction(target).CastPosition);
            }
        }

        public static void LaneClearMode()
        {
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
                    W.Cast();
            }
            if (LaneClearMenu["Elanec"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                var target = GetEnemy(E.Range, GameObjectType.obj_AI_Minion);
                if (target != null && !target.IsMonster)
                    E.Cast(E.GetPrediction(target).CastPosition);
            }
            if (LaneClearMenu["Rlanec"].Cast<CheckBox>().CurrentValue && R.IsReady())
            {
                var target = GetEnemy(R.Range, GameObjectType.obj_AI_Minion);
                if (target != null && !target.IsMonster)
                    R.Cast(R.GetPrediction(target).CastPosition);
            }
        }

        public static void LastHitMode()
        {
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
                    W.Cast();
            }
            if (LastHitMenu["Rlasthit"].Cast<CheckBox>().CurrentValue && R.IsReady())
            {
                var target = GetEnemyKs(AttackSpell.R, GameObjectType.obj_AI_Minion);
                if (target != null && !target.IsMonster)
                    R.Cast(R.GetPrediction(target).CastPosition);
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
                    W.Cast();
            }
            if (KillStealMenu["Eks"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                var target = GetEnemyKs(AttackSpell.E, GameObjectType.AIHeroClient);
                if (target != null)
                    E.Cast(R.GetPrediction(target).CastPosition);
            }
            if (KillStealMenu["Rks"].Cast<CheckBox>().CurrentValue && R.IsReady())
            {
                var target = GetEnemyKs(AttackSpell.R, GameObjectType.AIHeroClient);
                if (target != null)
                    R.Cast(R.GetPrediction(target).CastPosition);
            }
        }

        public static void DeathFollowMode()
        {
            if (Ptarget != null)
            {
                Player.IssueOrder(GameObjectOrder.MoveTo, Ptarget.ServerPosition);
            }
            else
            {
                var kstarget = GetEnemyKs(AttackSpell.P, GameObjectType.AIHeroClient);
                var target = GetEnemy(KogMaw.GetAutoAttackRange(), GameObjectType.AIHeroClient);
                var ksminion = GetEnemyKs(AttackSpell.P, GameObjectType.obj_AI_Minion);
                var minion = GetEnemy(KogMaw.GetAutoAttackRange(), GameObjectType.obj_AI_Minion);

                if (kstarget != null)
                    Ptarget = kstarget;
                else if (target != null)
                    Ptarget = target;
                else if (ksminion != null)
                    Ptarget = ksminion;
                else if (minion != null)
                    Ptarget = minion;
            }
        }

        public static void StackMode()
        {
            foreach (var item in Itemlist)
            {
                if ((item.Id == ItemId.Tear_of_the_Goddess || item.Id == ItemId.Tear_of_the_Goddess_Crystal_Scar ||
                     item.Id == ItemId.Archangels_Staff || item.Id == ItemId.Archangels_Staff_Crystal_Scar ||
                     item.Id == ItemId.Manamune || item.Id == ItemId.Manamune_Crystal_Scar) && item.Stacks < 750 &&
                    KogMaw.IsInShopRange() && Q.IsReady())
                    Q.Cast(KogMaw);
            }
        }

        public static void GapCloserMode(Obj_AI_Base sender, Gapcloser.GapcloserEventArgs args)
        {
            if (sender != null && SettingMenu["Egapc"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                var target = GetEnemy(E.Range, GameObjectType.AIHeroClient);
                if (target != null)
                    E.Cast(target);
            }
        }
    }
}