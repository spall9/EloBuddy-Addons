using System;
using System.Drawing;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace DefenderTaric
{
    // Created by Counter
    internal class Program
    {
        // Enum for Spells
        public enum AttackSpell
        {
            W,
            E,
            R
        }

        // Menus
        public static Menu DefenderTaricMenu,
            ComboMenu,
            HarassMenu,
            HealingMenu,
            DrawingMenu,
            SettingMenu;

        // Skills
        public static Spell.Targeted Q;
        public static Spell.Active W;
        public static Spell.Targeted E;
        public static Spell.Active R;
        public static int Taricskin;

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
                                     ((spell == AttackSpell.W && a.Health <= WDamage(a)) ||
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
        private static float WDamage(Obj_AI_Base target)
        {
            return Champion.CalculateDamageOnUnit(target, DamageType.Magical,
                new float[] {0, 40, 80, 120, 160, 200}[Q.Level] + 0.2f*Champion.FlatArmorMod);
        }

        private static float EDamage(Obj_AI_Base target)
        {
            return Champion.CalculateDamageOnUnit(target, DamageType.Magical,
                new float[] {0, 40, 70, 100, 130, 160}[E.Level] + 0.2f*Champion.FlatMagicDamageMod);
        }

        private static float RDamage(Obj_AI_Base target)
        {
            return Champion.CalculateDamageOnUnit(target, DamageType.Magical,
                new float[] {0, 150, 250, 350}[R.Level] + 0.5f*Champion.FlatMagicDamageMod);
        }

        private static void Main()
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Champion.ChampionName != "Taric") return;
            Taricskin = Champion.SkinId;

            Q = new Spell.Targeted(SpellSlot.Q, 750);
            W = new Spell.Active(SpellSlot.W, 400);
            E = new Spell.Targeted(SpellSlot.E, 625);
            R = new Spell.Active(SpellSlot.R, 400);

            DefenderTaricMenu = MainMenu.AddMenu("Defender Taric", "DefenderTaric");
            DefenderTaricMenu.AddGroupLabel("Defender Taric");
            DefenderTaricMenu.AddLabel("Gems? Gems are truly outrageous. They are truly, truly, truly outrageous.");
            DefenderTaricMenu.Add("Easter Egg", new CheckBox("Easter Egg"));

            ComboMenu = DefenderTaricMenu.AddSubMenu("Combo Features", "ComboFeatures");
            ComboMenu.AddGroupLabel("Combo Feautres");
            ComboMenu.AddLabel("Combo Modes: - Use when ready to fully engage");
            ComboMenu.Add("ComboM", new CheckBox("ComboMode"));
            ComboMenu.Add("ComboF", new Slider("EWR Combo", 1, 1, 2));
            ComboMenu.AddSeparator(1);
            ComboMenu.AddLabel("Independent boxes for Spells:");
            ComboMenu.Add("Wcombo", new Slider("Use W if Health % - 0 is off", 25));
            ComboMenu.Add("Ecombo", new CheckBox("Use E"));
            ComboMenu.Add("Rcombo", new CheckBox("Use R"));
            ComboMenu.AddSeparator(1);
            ComboMenu.AddLabel("Spell Weaving with AA");
            ComboMenu.Add("Pautos", new CheckBox("SpellWeaving", false));
            ComboMenu.AddLabel("Independent boxes for Spells:");
            ComboMenu.Add("Qpassive", new CheckBox("Use Q for Spellweaving", false));

            HarassMenu = DefenderTaricMenu.AddSubMenu("Harass Features", "HarassFeatures");
            HarassMenu.AddGroupLabel("Harass Features");
            HarassMenu.AddLabel("Independent boxes for Spells:");
            HarassMenu.Add("Wharass", new CheckBox("Use W"));
            HarassMenu.Add("Eharass", new CheckBox("Use E"));
            HarassMenu.AddSeparator(1);
            HarassMenu.Add("Harassmana", new Slider("Mana Limiter at Mana %", 25));

            HealingMenu = DefenderTaricMenu.AddSubMenu("Healing Features", "HealingFeatures");
            HealingMenu.AddGroupLabel("Healing Features");
            HealingMenu.Add("HealingM", new CheckBox("Use Q to heal"));
            HealingMenu.AddSeparator(1);
            HealingMenu.AddLabel("Auto Healer with Q");
            HealingMenu.Add("Qhealally", new Slider("Heal ally at Health % - 0 is off", 35));
            HealingMenu.Add("Qhealtaric", new Slider("Heal self at Health % - 0 is off", 20));
            HealingMenu.AddSeparator(1);
            HealingMenu.AddLabel("Healer Mana Limiter");
            HealingMenu.Add("Healingmana", new Slider("Mana Limiter at Mana %", 1));

            DrawingMenu = DefenderTaricMenu.AddSubMenu("Drawing Features", "DrawingFeatures");
            DrawingMenu.AddGroupLabel("Drawing Features");
            DrawingMenu.Add("DrawM", new CheckBox("Draw Mode"));
            DrawingMenu.AddSeparator(1);
            DrawingMenu.AddLabel("Independent boxes for Spells:");
            DrawingMenu.Add("Qdraw", new CheckBox("Draw Q"));
            DrawingMenu.Add("Edraw", new CheckBox("Draw E"));
            DrawingMenu.Add("WRdraw", new CheckBox("Draw W & R"));
            DrawingMenu.AddSeparator(1);
            DrawingMenu.AddLabel("Skin Designer");
            DrawingMenu.Add("DrawS", new CheckBox("Draw Skin Design"));
            DrawingMenu.Add("Skins", new Slider("Skin Designer: ", 0, 0, 3));

            SettingMenu = DefenderTaricMenu.AddSubMenu("Setting Mode", "SettingMode");
            SettingMenu.AddGroupLabel("Setting Mode");
            SettingMenu.AddLabel("Automatic Leveler");
            SettingMenu.Add("Autolvl", new CheckBox("Auto Leveler"));
            SettingMenu.AddSeparator(1);
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
            Champion.SetSkinId(DrawingMenu["DrawS"].Cast<CheckBox>().CurrentValue
                ? DrawingMenu["Skins"].Cast<Slider>().CurrentValue
                : Taricskin);
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

            if (!DrawingMenu["DrawM"].Cast<CheckBox>().CurrentValue) return;
            if (DrawingMenu["Qdraw"].Cast<CheckBox>().CurrentValue && Q.IsLearned)
                Drawing.DrawCircle(Champion.Position, Q.Range, color);
            if (DrawingMenu["Edraw"].Cast<CheckBox>().CurrentValue && E.IsLearned)
                Drawing.DrawCircle(Champion.Position, E.Range, color);
            if (DrawingMenu["WRdraw"].Cast<CheckBox>().CurrentValue && (W.IsLearned || R.IsLearned))
                Drawing.DrawCircle(Champion.Position, R.Range, color);
        }

        private static void Game_OnTick(EventArgs args)
        {
            if (SettingMenu["Autolvl"].Cast<CheckBox>().CurrentValue && Champion.SpellTrainingPoints >= 1)
                LevelerMode();
            ComboMenu["ComboF"].DisplayName = ComboMenu["ComboF"].Cast<Slider>().CurrentValue == 1 ? "ERW" : "EWR";
            if (Champion.IsDead) return;

            if (DefenderTaricMenu["Easter Egg"].Cast<CheckBox>().CurrentValue && Champion.HasBuff("recall"))
                Player.DoEmote(Emote.Joke);

            if (Orbwalker.IsAutoAttacking) return;
            switch (Orbwalker.ActiveModesFlags)
            {
                case Orbwalker.ActiveModes.Combo:
                {
                    if (ComboMenu["ComboF"].Cast<Slider>().DisplayName == "EWR")
                        EwrComboMode();
                    if (ComboMenu["ComboF"].Cast<Slider>().DisplayName == "ERW")
                        ErwComboMode();
                }
                    break;
                case Orbwalker.ActiveModes.Harass:
                    HarassMode();
                    break;
            }
            if (HealingMenu["HealingM"].Cast<CheckBox>().CurrentValue)
                HealingMode();
        }

        private static void LevelerMode()
        {
            int[] leveler = {1, 2, 3, 2, 2, 4, 2, 2, 1, 1, 4, 1, 1, 3, 3, 4, 3, 3};
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

        public static void EwrComboMode()
        {
            if (!ComboMenu["ComboM"].Cast<CheckBox>().CurrentValue) return;
            if (ComboMenu["Pautos"].Cast<CheckBox>().CurrentValue && Champion.HasBuff("taricgemcraftbuff"))
            {
                var target = GetEnemy(Champion.GetAutoAttackRange(), GameObjectType.AIHeroClient);
                if (target != null && Orbwalker.CanAutoAttack)
                    Orbwalker.ForcedTarget = target;
            }
            else if (ComboMenu["Pautos"].Cast<CheckBox>().CurrentValue && !Champion.HasBuff("taricgemcraftbuff"))
            {
                if (ComboMenu["Ecombo"].Cast<CheckBox>().CurrentValue && E.IsReady())
                {
                    var target = GetEnemy(E.Range, GameObjectType.AIHeroClient);
                    if (target != null)
                        E.Cast(target);
                }
                if (ComboMenu["Wcombo"].Cast<Slider>().CurrentValue > 0 && W.IsReady() &&
                    Champion.HealthPercent >= ComboMenu["Wcombo"].Cast<Slider>().CurrentValue)
                {
                    var target = GetEnemy(Champion.GetAutoAttackRange(), GameObjectType.AIHeroClient);
                    if (target != null)
                        W.Cast();
                }
                if (ComboMenu["Rcombo"].Cast<CheckBox>().CurrentValue && R.IsReady())
                {
                    var target = GetEnemy(Champion.GetAutoAttackRange(), GameObjectType.AIHeroClient);
                    if (target != null)
                        R.Cast();
                }
                if (ComboMenu["Qpassive"].Cast<CheckBox>().CurrentValue && Q.IsReady())
                {
                    var target = GetEnemy(Champion.GetAutoAttackRange(), GameObjectType.AIHeroClient);
                    if (target != null)
                        Q.Cast(Champion);
                }
            }
            else
            {
                if (ComboMenu["Ecombo"].Cast<CheckBox>().CurrentValue && E.IsReady())
                {
                    var target = GetEnemy(E.Range, GameObjectType.AIHeroClient);
                    if (target != null)
                        E.Cast(target);
                }
                if (ComboMenu["Wcombo"].Cast<Slider>().CurrentValue > 0 && W.IsReady() &&
                    Champion.HealthPercent >= ComboMenu["Wcombo"].Cast<Slider>().CurrentValue)
                {
                    var target = GetEnemy(W.Range, GameObjectType.AIHeroClient);
                    if (target != null)
                        W.Cast();
                }
                if (ComboMenu["Rcombo"].Cast<CheckBox>().CurrentValue && R.IsReady())
                {
                    var target = GetEnemy(R.Range, GameObjectType.AIHeroClient);
                    if (target != null)
                        R.Cast();
                }
            }
        }

        public static void ErwComboMode()
        {
            if (!ComboMenu["ComboM"].Cast<CheckBox>().CurrentValue) return;
            if (ComboMenu["ComboM"].Cast<CheckBox>().CurrentValue) return;
            if (ComboMenu["Pautos"].Cast<CheckBox>().CurrentValue && Champion.HasBuff("taricgemcraftbuff"))
            {
                var target = GetEnemy(Champion.GetAutoAttackRange(), GameObjectType.AIHeroClient);
                if (target != null)
                    if (Orbwalker.CanAutoAttack)
                        Orbwalker.ForcedTarget = target;
            }
            else if (ComboMenu["Pautos"].Cast<CheckBox>().CurrentValue && !Champion.HasBuff("taricgemcraftbuff"))
            {
                if (ComboMenu["Ecombo"].Cast<CheckBox>().CurrentValue && E.IsReady())
                {
                    var target = GetEnemy(E.Range, GameObjectType.AIHeroClient);
                    if (target != null)
                        E.Cast(target);
                }
                if (ComboMenu["Rcombo"].Cast<CheckBox>().CurrentValue && R.IsReady())
                {
                    var target = GetEnemy(Champion.GetAutoAttackRange(), GameObjectType.AIHeroClient);
                    if (target != null)
                        R.Cast();
                }
                if (ComboMenu["Wcombo"].Cast<Slider>().CurrentValue > 0 && W.IsReady() &&
                    Champion.HealthPercent >= ComboMenu["Wcombo"].Cast<Slider>().CurrentValue)
                {
                    var target = GetEnemy(Champion.GetAutoAttackRange(), GameObjectType.AIHeroClient);
                    if (target != null)
                        W.Cast();
                }
                if (ComboMenu["Qpassive"].Cast<CheckBox>().CurrentValue && Q.IsReady())
                {
                    var target = GetEnemy(Champion.GetAutoAttackRange(), GameObjectType.AIHeroClient);
                    if (target != null)
                        Q.Cast(Champion);
                }
            }
            else
            {
                if (ComboMenu["Ecombo"].Cast<CheckBox>().CurrentValue && E.IsReady())
                {
                    var target = GetEnemy(E.Range, GameObjectType.AIHeroClient);
                    if (target != null)
                        E.Cast(target);
                }
                if (ComboMenu["Rcombo"].Cast<CheckBox>().CurrentValue && R.IsReady())
                {
                    var target = GetEnemy(R.Range, GameObjectType.AIHeroClient);
                    if (target != null)
                        R.Cast();
                }
                if (ComboMenu["Wcombo"].Cast<Slider>().CurrentValue > 0 && W.IsReady() &&
                    Champion.HealthPercent >= ComboMenu["Wcombo"].Cast<Slider>().CurrentValue)
                {
                    var target = GetEnemy(W.Range, GameObjectType.AIHeroClient);
                    if (target != null)
                        W.Cast();
                }
            }
        }

        public static void HarassMode()
        {
            if (Champion.ManaPercent < HarassMenu["Harassmana"].Cast<Slider>().CurrentValue) return;
            if (HarassMenu["Wharass"].Cast<CheckBox>().CurrentValue && W.IsReady())
            {
                var target = GetEnemy(W.Range, GameObjectType.AIHeroClient);
                if (target != null)
                    W.Cast(target);
            }
            if (HarassMenu["Eharass"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                var target = GetEnemy(E.Range, GameObjectType.AIHeroClient);
                if (target != null)
                    E.Cast(target);
            }
        }

        public static void HealingMode()
        {
            if (Champion.ManaPercent < HealingMenu["Healingmana"].Cast<Slider>().CurrentValue) return;
            var qhealally = HealingMenu["Qhealally"].Cast<Slider>().CurrentValue;
            var qhealtaric = HealingMenu["Qhealtaric"].Cast<Slider>().CurrentValue;
            if (qhealally != 0 && Q.IsReady())
            {
                var ally = GetAlly(Q.Range, GameObjectType.AIHeroClient);
                if (ally != null && ally.HealthPercent <= qhealally)
                    Q.Cast(ally);
            }
            if (qhealtaric != 0 && Q.IsReady() && Champion.HealthPercent <= qhealtaric)
                Q.Cast(Champion);
        }

        public static void InterruptMode(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {
            if (SettingMenu["Interruptmode"].Cast<CheckBox>().CurrentValue) return;
            if (sender != null && SettingMenu["Einterrupt"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                var target = GetEnemy(E.Range, GameObjectType.AIHeroClient);
                if (target != null)
                    E.Cast(target);
            }
        }

        public static void GapCloserMode(Obj_AI_Base sender, Gapcloser.GapcloserEventArgs args)
        {
            if (SettingMenu["Gapcmode"].Cast<CheckBox>().CurrentValue) return;
            if (sender != null && SettingMenu["Egapc"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                var target = GetEnemy(E.Range, GameObjectType.AIHeroClient);
                if (target != null)
                    E.Cast(target);
            }
        }
    }
}