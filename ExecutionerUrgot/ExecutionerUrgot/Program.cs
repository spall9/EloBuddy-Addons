using System;
using System.Drawing;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace ExecutionerUrgot
{
    // Created by Counter
    internal class Program
    {
        // Enum for Spells
        public enum AttackSpell
        {
            Q,
            Q2,
            E
        }

        // Menus
        public static Menu ExecutionerUrgotMenu,
            ComboMenu,
            HarassMenu,
            JungleMenu,
            LaneClearMenu,
            LastHitMenu,
            KillStealMenu,
            DrawingMenu,
            SettingMenu;

        // Player
        public static AIHeroClient Urgot = Player.Instance;

        // Skills
        public static Spell.Skillshot Q;
        public static Spell.Targeted Q2;
        public static Spell.Active W;
        public static Spell.Skillshot E;
        public static int Urgotskin;
        public static InventorySlot[] Itemlist = Urgot.InventoryItems;

        public static Spell.Targeted R = new Spell.Targeted(SpellSlot.R,
            (uint) (400 + 150*Urgot.Spellbook.GetSpell(SpellSlot.R).Level));

        // Get Entities
        public static Obj_AI_Base GetAlly(float range, GameObjectType gametype)
        {
            switch (gametype)
            {
                case GameObjectType.AIHeroClient:
                    return EntityManager.Heroes.Allies
                        .OrderByDescending(a => a.Health)
                        .FirstOrDefault(a => a.IsAlly
                                             && a.IsValidTarget(range) && a.Distance(Urgot) <= range
                                             && !a.IsInvulnerable && !a.IsZombie
                                             && !Urgot.IsRecalling());
                case GameObjectType.obj_AI_Minion:
                    return EntityManager.MinionsAndMonsters.AlliedMinions
                        .OrderByDescending(a => a.Health)
                        .FirstOrDefault(a => a.IsAlly
                                             && a.IsValidTarget(range) && a.Distance(Urgot) <= range
                                             && !Urgot.IsRecalling());
            }
            return null;
        }

        public static Obj_AI_Base GetEnemy(float range, GameObjectType gametype, bool noBuff = true)
        {
            switch (gametype)
            {
                case GameObjectType.AIHeroClient:
                    return EntityManager.Heroes.Enemies
                        .OrderByDescending(a => a.Health)
                        .FirstOrDefault(a => a.IsEnemy
                                             && a.IsValidTarget(range) && a.Distance(Urgot) <= range
                                             && !a.IsInvulnerable && !a.IsZombie
                                             && !a.HasBuff("BlackShield") && !a.HasBuff("SivirE") &&
                                             !a.HasBuff("FioraW")
                                             && !a.HasBuff("ChronoShift")
                                             && (noBuff || a.HasBuff("UrgotPlasmaGernade"))
                                             && !Urgot.IsRecalling());
                case GameObjectType.obj_AI_Minion:
                    return EntityManager.MinionsAndMonsters.EnemyMinions
                        .OrderByDescending(a => a.Health)
                        .FirstOrDefault(a => a.IsEnemy
                                             && a.IsValidTarget(range) && a.Distance(Urgot) <= range
                                             && !a.HasBuff("BannerOfCommand")
                                             && (noBuff || a.HasBuff("UrgotPlasmaGernade"))
                                             && !Urgot.IsRecalling());
            }
            return null;
        }

        public static Obj_AI_Base GetEnemyKs(AttackSpell spell, GameObjectType gametype)
        {
            switch (spell)
            {
                case AttackSpell.Q2:
                    return ObjectManager
                        .Get<Obj_AI_Base>()
                        .OrderByDescending(a => a.Health)
                        .FirstOrDefault(a => a.IsEnemy && a.Type == gametype
                                             && a.IsValidTarget(Q.Range) && a.Distance(Urgot) <= Q.Range
                                             && !a.IsInvulnerable && !a.IsZombie
                                             && !a.HasBuff("BlackShield") && !a.HasBuff("SivirE") &&
                                             !a.HasBuff("FioraW")
                                             && !a.HasBuff("ChronoShift")
                                             && a.HasBuff("UrgotPlasmaGernade")
                                             && !Urgot.IsRecalling()
                                             && a.Health <= Q2Damage(a));
                case AttackSpell.Q:
                    return ObjectManager
                        .Get<Obj_AI_Base>()
                        .OrderByDescending(a => a.Health)
                        .FirstOrDefault(a => a.IsEnemy && a.Type == gametype
                                             && a.IsValidTarget(Q.Range) && a.Distance(Urgot) <= Q.Range
                                             && !a.IsInvulnerable && !a.IsZombie
                                             && !a.HasBuff("BlackShield") && !a.HasBuff("SivirE") &&
                                             !a.HasBuff("FioraW")
                                             && !a.HasBuff("ChronoShift")
                                             && !a.HasBuff("UrgotPlasmaGernade")
                                             && !Urgot.IsRecalling()
                                             && a.Health <= QDamage(a));
                case AttackSpell.E:
                    return ObjectManager
                        .Get<Obj_AI_Base>()
                        .OrderByDescending(a => a.Health)
                        .FirstOrDefault(a => a.IsEnemy && a.Type == gametype
                                             && a.IsValidTarget(E.Range) && a.Distance(Urgot) <= E.Range
                                             && !a.IsInvulnerable && !a.IsZombie
                                             && !a.HasBuff("BlackShield") && !a.HasBuff("SivirE") &&
                                             !a.HasBuff("FioraW")
                                             && !a.HasBuff("ChronoShift") && !Urgot.IsRecalling()
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
                                    a.IsAlly && a.IsValidTarget(range) && a.Distance(Urgot) <= range &&
                                    !a.IsInvulnerable && !Urgot.IsRecalling());
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
                                    a.IsEnemy && a.IsValidTarget(range) && a.Distance(Urgot) <= range &&
                                    !a.IsInvulnerable && !Urgot.IsRecalling());
            }
            return null;
        }

        // Spell Calculators
        private static float QDamage(Obj_AI_Base target)
        {
            return Urgot.CalculateDamageOnUnit(target, DamageType.Physical,
                new float[] {0, 10, 40, 70, 100, 130}[Q.Level] + 0.85f*Urgot.FlatPhysicalDamageMod);
        }

        private static float Q2Damage(Obj_AI_Base target)
        {
            return QDamage(target);
        }

        private static float EDamage(Obj_AI_Base target)
        {
            return Urgot.CalculateDamageOnUnit(target, DamageType.Physical,
                new float[] {0, 75, 130, 185, 240, 295}[E.Level] + 0.6f*Urgot.FlatPhysicalDamageMod);
        }

        private static void Main()
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            // Confirming Champion
            if (Urgot.ChampionName != "Urgot") return;
            Urgotskin = Urgot.SkinId;

            Q = new Spell.Skillshot(SpellSlot.Q, 1000, SkillShotType.Linear, 125, 1600, 60)
            {
                MinimumHitChance = HitChance.High,
                AllowedCollisionCount = 0
            };
            Q2 = new Spell.Targeted(SpellSlot.Q, 1200);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Skillshot(SpellSlot.E, 900, SkillShotType.Circular, 250, 1500, 210)
            {
                MinimumHitChance = HitChance.High,
                AllowedCollisionCount = int.MaxValue
            };

            ExecutionerUrgotMenu = MainMenu.AddMenu("ExecutionerUrgot", "ExecutionerUrgot");
            ExecutionerUrgotMenu.AddGroupLabel("Executioner Urgot");

            ComboMenu = ExecutionerUrgotMenu.AddSubMenu("Combo Features", "ComboFeatures");
            ComboMenu.AddGroupLabel("Combo Features");
            ComboMenu.AddLabel("Combo Modes:");
            ComboMenu.Add("ComboM", new CheckBox("ComboMode"));
            ComboMenu.AddSeparator(1);
            ComboMenu.AddLabel("Independent boxes for Spells:");
            ComboMenu.Add("Qcombo", new CheckBox("Use Q"));
            ComboMenu.Add("Wcombo", new CheckBox("Use W for slow"));
            ComboMenu.Add("Ecombo", new CheckBox("Use E"));
            ComboMenu.AddLabel("Urgot will cast R when Slider # equals # of Enemies.");
            ComboMenu.Add("Rcombo", new Slider("Use R - 0 is off", 5, 0, 5));

            HarassMenu = ExecutionerUrgotMenu.AddSubMenu("Harass Features", "HarassFeatures");
            HarassMenu.AddGroupLabel("Harass Features");
            HarassMenu.AddLabel("Independent boxes for Spells:");
            HarassMenu.Add("Qharass", new CheckBox("Use Q"));
            HarassMenu.Add("Eharass", new CheckBox("Use E"));
            HarassMenu.AddSeparator(1);
            HarassMenu.Add("Harassmana", new Slider("Mana Limiter at Mana %", 25));

            JungleMenu = ExecutionerUrgotMenu.AddSubMenu("Jungle Features", "JungleFeatures");
            JungleMenu.AddGroupLabel("Jungle Features");
            JungleMenu.AddLabel("Independent boxes for Spells:");
            JungleMenu.Add("Qjungle", new CheckBox("Use Q"));
            JungleMenu.Add("Ejungle", new CheckBox("Use E"));
            JungleMenu.AddSeparator(1);
            JungleMenu.Add("Junglemana", new Slider("Mana Limiter at Mana %", 25));

            LaneClearMenu = ExecutionerUrgotMenu.AddSubMenu("Lane Clear Features", "LaneClearFeatures");
            LaneClearMenu.AddGroupLabel("Lane Clear Features");
            LaneClearMenu.AddLabel("Independent boxes for Spells:");
            LaneClearMenu.Add("Qlanec", new CheckBox("Use Q"));
            LaneClearMenu.Add("Elanec", new CheckBox("Use E", false));
            LaneClearMenu.AddSeparator(1);
            LaneClearMenu.Add("Lanecmana", new Slider("Mana Limiter at Mana %", 25));

            LastHitMenu = ExecutionerUrgotMenu.AddSubMenu("Last Hit Features", "LastHitFeatures");
            LastHitMenu.AddGroupLabel("Last Hit Features");
            LastHitMenu.AddLabel("Independent boxes for Spells:");
            LastHitMenu.Add("Qlasthit", new CheckBox("Use Q"));
            LastHitMenu.AddSeparator(1);
            LastHitMenu.Add("Lasthitmana", new Slider("Mana Limiter at Mana %", 25));

            KillStealMenu = ExecutionerUrgotMenu.AddSubMenu("KS Features", "KSFeatures");
            KillStealMenu.AddGroupLabel("Kill Steal Features");
            KillStealMenu.Add("KSmode", new CheckBox("KS Mode"));
            KillStealMenu.AddSeparator(1);
            KillStealMenu.AddLabel("Independent boxes for Spells:");
            KillStealMenu.Add("Qks", new CheckBox("Use Q in KS"));
            KillStealMenu.Add("Eks", new CheckBox("Use E in KS", false));

            DrawingMenu = ExecutionerUrgotMenu.AddSubMenu("Drawing Features", "DrawingFeatures");
            DrawingMenu.AddGroupLabel("Drawing Features");
            DrawingMenu.Add("DrawM", new CheckBox("Draw Mode"));
            DrawingMenu.AddSeparator(1);
            DrawingMenu.AddLabel("Independent boxes for Spells:");
            DrawingMenu.Add("Qdraw", new CheckBox("Draw Q"));
            DrawingMenu.Add("Edraw", new CheckBox("Draw E"));
            DrawingMenu.Add("Rdraw", new CheckBox("Draw R"));
            DrawingMenu.AddSeparator(1);
            DrawingMenu.AddLabel("Skin Designer");
            DrawingMenu.Add("DrawS", new CheckBox("Draw Skin Design"));
            DrawingMenu.Add("Skins", new Slider("Skin Designer: ", 2, 0, 3));

            SettingMenu = ExecutionerUrgotMenu.AddSubMenu("Settings", "Settings");
            SettingMenu.AddGroupLabel("Settings");
            SettingMenu.AddLabel("Automatic Leveler");
            SettingMenu.Add("Autolvl", new CheckBox("Auto Leveler"));
            SettingMenu.AddSeparator(1);
            SettingMenu.AddLabel("Auto R - While under ally turret/base, grab enemy");
            SettingMenu.Add("Grabmode", new CheckBox("Auto R Mode"));
            SettingMenu.AddLabel("Automatic Tear Stacker");
            SettingMenu.Add("StackM", new CheckBox("Stack Mode"));
            SettingMenu.AddSeparator(1);
            SettingMenu.AddLabel("Interrupter");
            SettingMenu.Add("Interruptmode", new CheckBox("Interrupt Mode"));
            SettingMenu.Add("Rinterrupt", new CheckBox("Use R to interrupt"));
            SettingMenu.AddLabel("Gap Closer");
            SettingMenu.Add("Gapcmode", new CheckBox("Gap Closer Mode", false));
            SettingMenu.Add("Rgapc", new CheckBox("Use R to gapclose"));

            Interrupter.OnInterruptableSpell += InterruptMode;
            Gapcloser.OnGapcloser += GapCloserMode;
            Game.OnTick += Game_OnTick;
            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            Urgot.SetSkinId(DrawingMenu["DrawS"].Cast<CheckBox>().CurrentValue
                ? DrawingMenu["Skins"].Cast<Slider>().CurrentValue
                : Urgotskin);
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            Color color;
            Color color2;

            switch (Urgot.SkinId)
            {
                default:
                    color = Color.Transparent;
                    color2 = Color.Transparent;
                    break;
                case 0:
                    color = Color.SpringGreen;
                    color2 = Color.PaleVioletRed;
                    break;
                case 1:
                    color = Color.DarkOrange;
                    color2 = Color.IndianRed;
                    break;
                case 2:
                    color = Color.ForestGreen;
                    color2 = Color.Red;
                    break;
                case 3:
                    color = Color.LimeGreen;
                    color2 = Color.OrangeRed;
                    break;
            }

            if (!DrawingMenu["DrawM"].Cast<CheckBox>().CurrentValue) return;
            if (DrawingMenu["Qdraw"].Cast<CheckBox>().CurrentValue && Q.IsLearned)
            {
                Drawing.DrawCircle(Urgot.Position, Q.Range, color);
                Drawing.DrawCircle(Urgot.Position, Q2.Range, color2);
            }
            if (DrawingMenu["Edraw"].Cast<CheckBox>().CurrentValue && E.IsLearned)
                Drawing.DrawCircle(Urgot.Position, E.Range, color);
            if (DrawingMenu["Rdraw"].Cast<CheckBox>().CurrentValue && R.IsLearned)
                Drawing.DrawCircle(Urgot.Position, R.Range, color);
        }

        private static void Game_OnTick(EventArgs args)
        {
            if (Urgot.IsDead) return;
            if (SettingMenu["Autolvl"].Cast<CheckBox>().CurrentValue && Urgot.SpellTrainingPoints >= 1)
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
            if (SettingMenu["Grabmode"].Cast<CheckBox>().CurrentValue)
                GrabMode();
        }

        public static void LevelerMode()
        {
            int[] leveler = {1, 3, 1, 2, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3};
            var avapoints = Urgot.SpellTrainingPoints;
            while (avapoints >= 1)
            {
                var skill = leveler[Urgot.Level - avapoints];

                switch (skill)
                {
                    default:
                        Urgot.Spellbook.LevelSpell(SpellSlot.Unknown);
                        break;
                    case 1:
                        Urgot.Spellbook.LevelSpell(SpellSlot.Q);
                        break;
                    case 2:
                        Urgot.Spellbook.LevelSpell(SpellSlot.W);
                        break;
                    case 3:
                        Urgot.Spellbook.LevelSpell(SpellSlot.E);
                        break;
                    case 4:
                        Urgot.Spellbook.LevelSpell(SpellSlot.R);
                        break;
                }
                avapoints--;
            }
        }

        public static void ComboMode()
        {
            if (ComboMenu["Qcombo"].Cast<CheckBox>().CurrentValue && Q.IsReady())
            {
                var btarget = GetEnemy(Q2.Range, GameObjectType.AIHeroClient, false);
                if (btarget != null)
                    Q2.Cast(btarget);
                else
                {
                    var target = GetEnemy(Q.Range, GameObjectType.AIHeroClient);
                    if (target != null)
                        Q.Cast(Q.GetPrediction(target).CastPosition);
                }
            }
            if (ComboMenu["Wcombo"].Cast<CheckBox>().CurrentValue && W.IsReady())
            {
                var target = GetEnemy(Q2.Range, GameObjectType.AIHeroClient, false);
                if (target != null && !target.IsFacing(Urgot))
                    W.Cast();
            }
            if (ComboMenu["Ecombo"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                var target = GetEnemy(E.Range, GameObjectType.AIHeroClient);
                if (target != null)
                    E.Cast(E.GetPrediction(target).CastPosition);
            }
            if (ComboMenu["Rcombo"].Cast<Slider>().CurrentValue != 0 &&
                Urgot.CountEnemiesInRange(R.Range) == ComboMenu["Rcombo"].Cast<Slider>().CurrentValue && R.IsReady())
            {
                var target = GetEnemy(R.Range, GameObjectType.AIHeroClient);
                if (target != null)
                    R.Cast(target);
            }
        }

        public static void HarassMode()
        {
            if (Urgot.ManaPercent < LastHitMenu["Lasthitmana"].Cast<Slider>().CurrentValue) return;
            if (HarassMenu["Qharass"].Cast<CheckBox>().CurrentValue && Q.IsReady())
            {
                var btarget = GetEnemy(Q2.Range, GameObjectType.AIHeroClient, false);
                if (btarget != null)
                    Q2.Cast(btarget);
                else
                {
                    var target = GetEnemy(Q.Range, GameObjectType.AIHeroClient);
                    if (target != null)
                        Q.Cast(Q.GetPrediction(target).CastPosition);
                }
            }
            if (HarassMenu["Eharass"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                var target = GetEnemy(E.Range, GameObjectType.AIHeroClient);
                if (target != null)
                    E.Cast(E.GetPrediction(target).CastPosition);
            }
        }

        public static void JungleMode()
        {
            if (Urgot.ManaPercent < JungleMenu["Junglemana"].Cast<Slider>().CurrentValue) return;
            if (JungleMenu["Qjungle"].Cast<CheckBox>().CurrentValue && Q.IsReady())
            {
                var btarget = GetEnemy(Q2.Range, GameObjectType.obj_AI_Minion, false);
                if (btarget != null && btarget.IsMonster)
                    Q2.Cast(btarget);
                else
                {
                    var target = GetEnemy(Q.Range, GameObjectType.obj_AI_Minion);
                    if (target != null && target.IsMonster)
                        Q.Cast(Q.GetPrediction(target).CastPosition);
                }
            }
            if (JungleMenu["Ejungle"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                var target = GetEnemy(E.Range, GameObjectType.obj_AI_Minion);
                if (target != null && target.IsMonster)
                    E.Cast(E.GetPrediction(target).CastPosition);
            }
        }

        public static void LaneClearMode()
        {
            if (Urgot.ManaPercent < LaneClearMenu["Lanecmana"].Cast<Slider>().CurrentValue) return;
            if (LaneClearMenu["Qlanec"].Cast<CheckBox>().CurrentValue && Q.IsReady())
            {
                var btarget = GetEnemy(Q2.Range, GameObjectType.obj_AI_Minion, false);
                if (btarget != null && !btarget.IsMonster)
                    Q2.Cast(btarget);
                else
                {
                    var target = GetEnemy(Q.Range, GameObjectType.obj_AI_Minion);
                    if (target != null && !target.IsMonster)
                        Q.Cast(Q.GetPrediction(target).CastPosition);
                }
            }
            if (LaneClearMenu["Elanec"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                var target = GetEnemy(E.Range, GameObjectType.obj_AI_Minion);
                if (target != null && !target.IsMonster)
                    E.Cast(E.GetPrediction(target).CastPosition);
            }
        }

        public static void LastHitMode()
        {
            if (Urgot.ManaPercent < LastHitMenu["Lasthitmana"].Cast<Slider>().CurrentValue) return;
            if (LastHitMenu["Qlasthit"].Cast<CheckBox>().CurrentValue && Q.IsReady())
            {
                var btarget = GetEnemyKs(AttackSpell.Q2, GameObjectType.obj_AI_Minion);
                if (btarget != null && !btarget.IsMonster)
                    Q2.Cast(btarget);
                else
                {
                    var target = GetEnemyKs(AttackSpell.Q, GameObjectType.obj_AI_Minion);
                    if (target != null && !target.IsMonster)
                        Q.Cast(Q.GetPrediction(target).CastPosition);
                }
            }
        }

        public static void KsMode()
        {
            if (KillStealMenu["Qks"].Cast<CheckBox>().CurrentValue && Q.IsReady())
            {
                var btarget = GetEnemyKs(AttackSpell.Q2, GameObjectType.AIHeroClient);
                if (btarget != null)
                    Q2.Cast(btarget);
                else
                {
                    var target = GetEnemyKs(AttackSpell.Q, GameObjectType.AIHeroClient);
                    if (target != null)
                        Q.Cast(Q.GetPrediction(target).CastPosition);
                }
            }
            if (KillStealMenu["Eks"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                var target = GetEnemyKs(AttackSpell.E, GameObjectType.AIHeroClient);
                if (target != null)
                    E.Cast(E.GetPrediction(target).CastPosition);
            }
        }

        public static void GrabMode()
        {
            if (!R.IsReady()) return;
            var target = GetEnemy(R.Range, GameObjectType.AIHeroClient);
            var objective = GetAlliedObjective(R.Range, GameObjectType.obj_AI_Turret);

            if (target != null & objective != null && Urgot.IsInRange(objective, R.Range))
                R.Cast(target);
        }

        public static void StackMode()
        {
            foreach (var item in Itemlist)
            {
                if ((item.Id == ItemId.Tear_of_the_Goddess || item.Id == ItemId.Tear_of_the_Goddess_Crystal_Scar ||
                     item.Id == ItemId.Archangels_Staff || item.Id == ItemId.Archangels_Staff_Crystal_Scar ||
                     item.Id == ItemId.Manamune || item.Id == ItemId.Manamune_Crystal_Scar) && item.Stacks < 750 &&
                    Urgot.IsInShopRange() && Q.IsReady())
                    Q.Cast(Urgot);
            }
        }

        public static void InterruptMode(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {
            if (!SettingMenu["Interruptmode"].Cast<CheckBox>().CurrentValue) return;
            if (sender != null && SettingMenu["Rinterrupt"].Cast<CheckBox>().CurrentValue && R.IsReady())
            {
                var target = GetEnemy(R.Range, GameObjectType.AIHeroClient);
                if (target != null)
                    R.Cast(target);
            }
        }

        public static void GapCloserMode(Obj_AI_Base sender, Gapcloser.GapcloserEventArgs args)
        {
            if (!SettingMenu["Gapcmode"].Cast<CheckBox>().CurrentValue) return;
            if (sender != null && SettingMenu["Rgapc"].Cast<CheckBox>().CurrentValue && R.IsReady())
            {
                var target = GetEnemy(R.Range, GameObjectType.AIHeroClient);
                if (target != null)
                    R.Cast(target);
            }
        }
    }
}