using System.Drawing;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace ArsenalActivator
{
    internal class SummonerSpells
    {
        public enum SmiteName
        {
            Default,
            Challenging,
            Chilling
        }

        // Menu
        public static Menu SummonerSpellMenu;

        // Player
        public static AIHeroClient Champion = Program.Champion;

        // Skills
        public static SpellDataInst Sumspell1;
        public static SpellDataInst Sumspell2;

        public static Spell.Active Barrier;
        public static float Barriershield;
        public static Spell.Active Clarity;
        public static Spell.Active Cleanse;
        public static Spell.Targeted Clairvoyance;
        public static Spell.Targeted Exhaust;
        public static Spell.Active Garrison;
        public static Spell.Active Heal;
        public static Spell.Targeted Ignite;
        public static Spell.Skillshot Mark;
        public static float Markdamage;
        public static Spell.Skillshot Porothrow;
        public static float Porothrowdamage;
        public static Spell.Targeted Smite;
        public static SmiteName Smitename = SmiteName.Default;
        public static float Smitedamage;

        public static void LoadingSummonerSpells()
        {
            // Locate
            FindSummonerSpells();
            ConfigSummonerSpells();
        }

        // Locate
        public static void FindSummonerSpells()
        {
            switch (Player.GetSpell(SpellSlot.Summoner1).Name)
            {
                case "summonerbarrier":
                    Sumspell1 = Player.GetSpell(SpellSlot.Summoner1);
                    break;
                case "summonermana":
                    Sumspell1 = Player.GetSpell(SpellSlot.Summoner1);
                    break;
                case "summonerclairvoyance":
                    Sumspell1 = Player.GetSpell(SpellSlot.Summoner1);
                    break;
                case "summonerboost":
                    Sumspell1 = Player.GetSpell(SpellSlot.Summoner1);
                    break;
                case "summonerexhaust":
                    Sumspell1 = Player.GetSpell(SpellSlot.Summoner1);
                    break;
                case "summonerflash":
                    Sumspell1 = Player.GetSpell(SpellSlot.Summoner1);
                    break;
                case "summonerodingarrison":
                    Sumspell1 = Player.GetSpell(SpellSlot.Summoner1);
                    break;
                case "summonerhaste":
                    Sumspell1 = Player.GetSpell(SpellSlot.Summoner1);
                    break;
                case "summonerheal":
                    Sumspell1 = Player.GetSpell(SpellSlot.Summoner1);
                    break;
                case "summonerdot":
                    Sumspell1 = Player.GetSpell(SpellSlot.Summoner1);
                    break;
                case "summonersnowball":
                    Sumspell1 = Player.GetSpell(SpellSlot.Summoner1);
                    break;
                case "summonerporothrow":
                    Sumspell1 = Player.GetSpell(SpellSlot.Summoner1);
                    break;
                case "summonersmite":
                    Sumspell1 = Player.GetSpell(SpellSlot.Summoner1);
                    break;
                case "summonerteleport":
                    Sumspell1 = Player.GetSpell(SpellSlot.Summoner1);
                    break;
                case "summonerpororecall":
                    Sumspell1 = Player.GetSpell(SpellSlot.Summoner1);
                    break;
            }
            switch (Player.GetSpell(SpellSlot.Summoner2).Name)
            {
                case "summonerbarrier":
                    Sumspell2 = Player.GetSpell(SpellSlot.Summoner2);
                    break;
                case "summonermana":
                    Sumspell2 = Player.GetSpell(SpellSlot.Summoner2);
                    break;
                case "summonerclairvoyance":
                    Sumspell2 = Player.GetSpell(SpellSlot.Summoner2);
                    break;
                case "summonerboost":
                    Sumspell2 = Player.GetSpell(SpellSlot.Summoner2);
                    break;
                case "summonerexhaust":
                    Sumspell2 = Player.GetSpell(SpellSlot.Summoner2);
                    break;
                case "summonerflash":
                    Sumspell2 = Player.GetSpell(SpellSlot.Summoner2);
                    break;
                case "summonerodingarrison":
                    Sumspell2 = Player.GetSpell(SpellSlot.Summoner2);
                    break;
                case "summonerhaste":
                    Sumspell2 = Player.GetSpell(SpellSlot.Summoner2);
                    break;
                case "summonerheal":
                    Sumspell2 = Player.GetSpell(SpellSlot.Summoner2);
                    break;
                case "summonerdot":
                    Sumspell2 = Player.GetSpell(SpellSlot.Summoner2);
                    break;
                case "summonersnowball":
                    Sumspell2 = Player.GetSpell(SpellSlot.Summoner2);
                    break;
                case "summonerporothrow":
                    Sumspell2 = Player.GetSpell(SpellSlot.Summoner2);
                    break;
                case "summonersmite":
                    Sumspell2 = Player.GetSpell(SpellSlot.Summoner2);
                    break;
                case "summonerteleport":
                    Sumspell2 = Player.GetSpell(SpellSlot.Summoner2);
                    break;
                case "summonerpororecall":
                    Sumspell2 = Player.GetSpell(SpellSlot.Summoner2);
                    break;
            }
        }

        public static void ConfigSummonerSpells()
        {
            // Barrier
            switch (Sumspell1.Name)
            {
                case "summonerbarrier":
                    Barrier = new Spell.Active(Sumspell1.Slot);
                    break;
                case "summonermana":
                    Clarity = new Spell.Active(Sumspell1.Slot, 600);
                    break;
                case "summonerclairvoyance":
                    Clairvoyance = new Spell.Targeted(Sumspell1.Slot, 4294967295);
                    break;
                case "summonerboost":
                    Cleanse = new Spell.Active(Sumspell1.Slot);
                    break;
                case "summonerexhaust":
                    Exhaust = new Spell.Targeted(Sumspell1.Slot, 650);
                    break;
                case "summonerodingarrison":
                    Garrison = new Spell.Active(Sumspell1.Slot, 1250);
                    break;
                case "summonerheal":
                    Heal = new Spell.Active(Sumspell1.Slot, 850);
                    break;
                case "summonerdot":
                    Ignite = new Spell.Targeted(Sumspell1.Slot, 600);
                    break;
                case "summonersnowball":
                    Mark = new Spell.Skillshot(Sumspell1.Slot, 1600, SkillShotType.Linear)
                    {
                        MinimumHitChance = HitChance.High,
                        AllowedCollisionCount = 0
                    };
                    break;
                case "summonerporothrow":
                    Porothrow = new Spell.Skillshot(Sumspell1.Slot, 1600, SkillShotType.Linear)
                    {
                        MinimumHitChance = HitChance.High,
                        AllowedCollisionCount = 0
                    };
                    break;
                case "summonersmite":
                    Smite = new Spell.Targeted(Sumspell1.Slot, 565);
                    break;
            }
            switch (Sumspell2.Name)
            {
                case "summonerbarrier":
                    Barrier = new Spell.Active(Sumspell2.Slot);
                    break;
                case "summonermana":
                    Clarity = new Spell.Active(Sumspell2.Slot, 600);
                    break;
                case "summonerclairvoyance":
                    Clairvoyance = new Spell.Targeted(Sumspell2.Slot, 4294967295);
                    break;
                case "summonerboost":
                    Cleanse = new Spell.Active(Sumspell2.Slot);
                    break;
                case "summonerexhaust":
                    Exhaust = new Spell.Targeted(Sumspell2.Slot, 650);
                    break;
                case "summonerodingarrison":
                    Garrison = new Spell.Active(Sumspell2.Slot, 1250);
                    break;
                case "summonerheal":
                    Heal = new Spell.Active(Sumspell2.Slot, 850);
                    break;
                case "summonerdot":
                    Ignite = new Spell.Targeted(Sumspell2.Slot, 600);
                    break;
                case "summonersnowball":
                    Mark = new Spell.Skillshot(Sumspell2.Slot, 1600, SkillShotType.Linear)
                    {
                        MinimumHitChance = HitChance.High,
                        AllowedCollisionCount = 0
                    };
                    break;
                case "summonerporothrow":
                    Porothrow = new Spell.Skillshot(Sumspell2.Slot, 1600, SkillShotType.Linear)
                    {
                        MinimumHitChance = HitChance.High,
                        AllowedCollisionCount = 0
                    };
                    break;
                case "summonersmite":
                    Smite = new Spell.Targeted(Sumspell2.Slot, 565);
                    break;
            }
        }

        // Create
        public static void MenuDraw()
        {
            SummonerSpellMenu = Program.ArsenalActivatorMenu.AddSubMenu("Summoner Spells", "Summoner Spells");
            SummonerSpellMenu.AddGroupLabel("Summoner Spells");
            SummonerSpellMenu.AddLabel("Offensive Summoner Spells:");
            if (Ignite != null)
                SummonerSpellMenu.Add("ignite", new Slider("Use Ignite at Health % - 0 is off", 15));
            if (Smite != null)
            {
                SummonerSpellMenu.Add("smite", new CheckBox("Use Smite"));
                SummonerSpellMenu.AddSeparator(1);
                SummonerSpellMenu.AddLabel("Smite Upgrades");
                SummonerSpellMenu.Add("smitechill", new CheckBox("KS with Chilling"));
                SummonerSpellMenu.Add("smiteduel", new CheckBox("Combo with Challenging"));
                SummonerSpellMenu.AddSeparator(1);
                SummonerSpellMenu.AddLabel("Summoner Rift Jungle Camps:");
                SummonerSpellMenu.Add("smiteblue", new CheckBox("Blue Sentinel", false));
                SummonerSpellMenu.Add("smitered", new CheckBox("Red Brambleback", false));
                SummonerSpellMenu.Add("smitemurk", new CheckBox("Greater Murk Wolf", false));
                SummonerSpellMenu.Add("smiteraptor", new CheckBox("Crimson Raptor", false));
                SummonerSpellMenu.Add("smitekrug", new CheckBox("Ancient Krug"));
                SummonerSpellMenu.Add("smitegromp", new CheckBox("Gromp"));
                SummonerSpellMenu.Add("smitecrab", new CheckBox("Rift Scuttler", false));
                SummonerSpellMenu.Add("smitedragon", new CheckBox("Dragon"));
                SummonerSpellMenu.Add("smiteherald", new CheckBox("Rift Herald"));
                SummonerSpellMenu.Add("smitenashor", new CheckBox("Baron Nashor"));
                SummonerSpellMenu.AddSeparator(1);
                SummonerSpellMenu.AddLabel("Twisted Treeline Jungle Camps:");
                SummonerSpellMenu.Add("smitewolf", new CheckBox("Giant Wolf"));
                SummonerSpellMenu.Add("smitegolem", new CheckBox("Big Golem"));
                SummonerSpellMenu.Add("smitewraith", new CheckBox("Wraith"));
                SummonerSpellMenu.Add("smitespider", new CheckBox("Vilemaw"));
                SummonerSpellMenu.AddSeparator(1);
                SummonerSpellMenu.AddLabel("Jungling Options");
                SummonerSpellMenu.Add("smitecheck", new CheckBox("KS Smite Camps if Enemies Near"));
            }
            SummonerSpellMenu.AddSeparator(1);
            SummonerSpellMenu.AddLabel("Defensive Summoner Spells");
            if (Clarity != null)
            {
                SummonerSpellMenu.Add("clarityself",
                    new Slider("Use Clairty on Self at Mana % - 0 is off", 25));
                SummonerSpellMenu.Add("clarityally",
                    new Slider("Use Clairty on Ally at Mana % - 0 is off"));
            }
            if (Cleanse != null)
            {
                SummonerSpellMenu.Add("Cblind", new CheckBox("Use Cleanse on Blind", false));
                SummonerSpellMenu.Add("Ccharm", new CheckBox("Use Cleanse on Charm"));
                SummonerSpellMenu.Add("Cfear", new CheckBox("Use Cleanse on Fear", false));
                SummonerSpellMenu.Add("Cflee", new CheckBox("Use Cleanse on Flee", false));
                SummonerSpellMenu.Add("Cpolymorph", new CheckBox("Use Cleanse on Polymorph"));
                SummonerSpellMenu.Add("Csilence", new CheckBox("Use Cleanse on Silence", false));
                SummonerSpellMenu.Add("Csleep", new CheckBox("Use Cleanse on Sleep"));
                SummonerSpellMenu.Add("Cslow", new CheckBox("Use Cleanse on Slow", false));
                SummonerSpellMenu.Add("Csnare", new CheckBox("Use Cleanse on Snare"));
                SummonerSpellMenu.Add("Cstun", new CheckBox("Use Cleanse on Stun"));
                SummonerSpellMenu.Add("Ctaunt", new CheckBox("Use Cleanse on Taunt", false));
                SummonerSpellMenu.Add("Cexhaust", new CheckBox("Use Cleanse on Exhaust"));
                SummonerSpellMenu.Add("Cignite", new CheckBox("Use Cleanse on Ignite"));
            }
            if (Exhaust != null)
            {
                SummonerSpellMenu.Add("exhaustself",
                    new Slider("Use Exhaust when Self at Health % - 0 is off", 40));
                SummonerSpellMenu.Add("exhaustally",
                    new Slider("Use Exhaust when Ally at Health % - 0 is off", 25));
            }
            if (Heal != null)
            {
                SummonerSpellMenu.Add("healself", new Slider("Use Heal on Self at Health % - 0 is off", 25));
                SummonerSpellMenu.Add("healally", new Slider("Use Heal on Ally at Health % - 0 is off", 15));
            }
            SummonerSpellMenu.AddSeparator(1);
            SummonerSpellMenu.AddLabel("Misc Summoner Spells");
            if (Clairvoyance != null)
            {
                //SummonerSpellMenu.Add("clairvoyance", new CheckBox("Use Clairvoyance"));
            }
            if (Mark != null)
                SummonerSpellMenu.Add("mark", new CheckBox("Use Mark"));
            if (Porothrow != null)
                SummonerSpellMenu.Add("porothrow", new CheckBox("Use Poro Throw"));
            if (Garrison != null)
            {
                //SummonerSpellMenu.Add("odininterrupt", new CheckBox("Use Garrison to Interrupt Neutralization"));
                //SummonerSpellMenu.Add("odinreduce", new CheckBox("Use Garrison to Reduce Turret's Damage"));
            }
            SummonerSpellMenu.AddSeparator(1);
            SummonerSpellMenu.AddLabel("Draw Summoner Spells");
            SummonerSpellMenu.Add("DrawSS", new CheckBox("Draw Summoner Spells"));
        }

        // Calculate
        public static void CalculateSummonerSpells()
        {
            // Barrier
            if (Barrier != null)
                Barriershield = 95 + 20*Champion.Level;
            // Mark
            if (Mark != null)
                Markdamage = 10 + 5*Champion.Level;
            // Poro Throw
            if (Porothrow != null)
                Porothrowdamage = 20 + 10*Champion.Level;
            // Smite
            if (Smite != null)
            {
                // Smite Check
                switch (Sumspell1.Name)
                {
                    case "summonersmite":
                        Smitename = SmiteName.Default;
                        break;
                    case "s5_summonersmiteplayerganker":
                        Smitename = SmiteName.Chilling;
                        break;
                    case "s5_summonersmiteduel":
                        Smitename = SmiteName.Challenging;
                        break;
                }
                switch (Sumspell2.Name)
                {
                    case "summonersmite":
                        Smitename = SmiteName.Default;
                        break;
                    case "s5_summonersmiteplayerganker":
                        Smitename = SmiteName.Chilling;
                        break;
                    case "s5_summonersmiteduel":
                        Smitename = SmiteName.Challenging;
                        break;
                }

                // Smite Damage
                switch (Smitename)
                {
                    case SmiteName.Default:
                        Smitedamage =
                            new float[]
                            {
                                0, 390, 410, 430, 450, 480, 510, 540, 570, 600, 640, 680, 720, 760, 800, 850, 900, 950,
                                1000
                            }[Champion.Level];
                        break;
                    case SmiteName.Challenging:
                        Smitedamage = 54 + 6*Champion.Level;
                        break;
                    case SmiteName.Chilling:
                        Smitedamage = 20 + 8*Champion.Level;
                        break;
                }
            }
        }

        public static void CastSummonerSpells()
        {
            // Calculate Spell Damage
            CalculateSummonerSpells();

            // BARRIER
            if (Barrier != null && !Barrier.IsOnCooldown)
            {
                // Needs Damage Calculator to calculate
            }
            // CLARITY
            if (Clarity != null && !Clarity.IsOnCooldown)
            {
                // Targets
                var ally = Program.GetAlly(Clarity.Range, GameObjectType.AIHeroClient);

                // Restore self
                if (SummonerSpellMenu["clarityself"].Cast<Slider>().CurrentValue > 0
                    && (Champion.ManaPercent <= SummonerSpellMenu["clarityself"].Cast<Slider>().CurrentValue))
                    Clarity.Cast();
                // Restore ally
                if (ally != null
                    && SummonerSpellMenu["clarityally"].Cast<Slider>().CurrentValue > 0
                    && ally.ManaPercent <= SummonerSpellMenu["clarityally"].Cast<Slider>().CurrentValue)
                    Clarity.Cast();
            }
            // CLEANSE
            if (Cleanse != null && !Cleanse.IsOnCooldown &&
                ((Champion.HasBuffOfType(BuffType.Blind) && SummonerSpellMenu["Cblind"].Cast<CheckBox>().CurrentValue)
                 ||
                 (Champion.HasBuffOfType(BuffType.Charm) && SummonerSpellMenu["Ccharm"].Cast<CheckBox>().CurrentValue)
                 || (Champion.HasBuffOfType(BuffType.Fear) && SummonerSpellMenu["Cfear"].Cast<CheckBox>().CurrentValue)
                 || (Champion.HasBuffOfType(BuffType.Flee) && SummonerSpellMenu["Cflee"].Cast<CheckBox>().CurrentValue)
                 ||
                 (Champion.HasBuffOfType(BuffType.Polymorph) &&
                  SummonerSpellMenu["Cpolymorph"].Cast<CheckBox>().CurrentValue)
                 ||
                 (Champion.HasBuffOfType(BuffType.Silence) &&
                  SummonerSpellMenu["Csilence"].Cast<CheckBox>().CurrentValue)
                 ||
                 (Champion.HasBuffOfType(BuffType.Sleep) && SummonerSpellMenu["Csleep"].Cast<CheckBox>().CurrentValue)
                 || (Champion.HasBuffOfType(BuffType.Slow) && SummonerSpellMenu["Cslow"].Cast<CheckBox>().CurrentValue)
                 ||
                 (Champion.HasBuffOfType(BuffType.Snare) && SummonerSpellMenu["Csnare"].Cast<CheckBox>().CurrentValue)
                 || (Champion.HasBuffOfType(BuffType.Stun) && SummonerSpellMenu["Cstun"].Cast<CheckBox>().CurrentValue)
                 ||
                 (Champion.HasBuffOfType(BuffType.Taunt) && SummonerSpellMenu["Ctaunt"].Cast<CheckBox>().CurrentValue)
                 ||
                 (Champion.HasBuff("summonerexhaust") && SummonerSpellMenu["Cexhaust"].Cast<CheckBox>().CurrentValue)
                 ||
                 (Champion.HasBuff("summonerdot") && Champion.Health <= 50 + 20*Champion.Level &&
                  SummonerSpellMenu["Cignite"].Cast<CheckBox>().CurrentValue))) Cleanse.Cast();
            // EXHAUST
            if (Exhaust != null && !Exhaust.IsOnCooldown)
            {
                // Targets
                var target = Program.GetEnemy(Exhaust.Range, GameObjectType.AIHeroClient);
                var ally = Program.GetAlly(Exhaust.Range, GameObjectType.AIHeroClient);

                // Exhaust for self
                if (target != null
                    && Champion.HealthPercent <= SummonerSpellMenu["exhaustself"].Cast<Slider>().CurrentValue
                    && target.IsAttackingPlayer)
                    Exhaust.Cast(target);
                // Exhaust for ally
                if (target != null && ally != null
                    && ally.HealthPercent <= SummonerSpellMenu["exhaustally"].Cast<Slider>().CurrentValue
                    && target.IsFacing(ally))
                    Exhaust.Cast(target);
            }
            // GARRISON
            if (Garrison != null && !Garrison.IsOnCooldown)
            {
                // Need Object name for CP before I can finish coding
            }
            // HEAL
            if (Heal != null && !Heal.IsOnCooldown)
            {
                // Targets
                var ally = Program.GetAlly(Heal.Range, GameObjectType.AIHeroClient);

                // Heal self
                if (SummonerSpellMenu["healself"].Cast<Slider>().CurrentValue > 0
                    && (Champion.HealthPercent <= SummonerSpellMenu["healself"].Cast<Slider>().CurrentValue)
                    && !Champion.HasBuff("summonerheal"))
                    Heal.Cast();
                // Heal ally
                if (ally != null
                    && SummonerSpellMenu["healally"].Cast<Slider>().CurrentValue > 0
                    && ally.HealthPercent <= SummonerSpellMenu["healally"].Cast<Slider>().CurrentValue
                    && !ally.HasBuff("summonerheal"))
                    Heal.Cast();
            }
            // IGNITE
            if (Ignite != null && !Ignite.IsOnCooldown
                && SummonerSpellMenu["ignite"].Cast<Slider>().CurrentValue > 0)
            {
                // Targets
                var target = Program.GetEnemy(Ignite.Range, GameObjectType.AIHeroClient);

                if (target != null
                    && target.Health <= SummonerSpellMenu["ignite"].Cast<Slider>().CurrentValue)
                    Ignite.Cast(target);
            }
            // MARK
            if (Mark != null && !Mark.IsOnCooldown
                && SummonerSpellMenu["mark"].Cast<CheckBox>().CurrentValue
                && Mark.Name != "snowballfollowupcast")
            {
                // Targets
                var ks = Program.GetEnemyKs(Mark.Range, Markdamage, GameObjectType.AIHeroClient);
                var target = Program.GetEnemy(Mark.Range, GameObjectType.AIHeroClient);

                if (ks != null
                    && Mark.GetPrediction(ks).HitChance >= HitChance.High)
                    Mark.Cast(Mark.GetPrediction(ks).CastPosition);
                else if (target != null
                         && Mark.GetPrediction(target).HitChance >= HitChance.High)
                    Mark.Cast(Mark.GetPrediction(target).CastPosition);
            }
            // PORO THROW
            if (Porothrow != null && !Porothrow.IsOnCooldown
                && SummonerSpellMenu["porothrow"].Cast<CheckBox>().CurrentValue
                && Porothrow.Name != "porothrowfollowupcast")
            {
                // Targets
                var ks = Program.GetEnemyKs(Porothrow.Range, Porothrowdamage, GameObjectType.AIHeroClient);
                var target = Program.GetEnemy(Porothrow.Range, GameObjectType.AIHeroClient);

                if (ks != null
                    && Porothrow.GetPrediction(ks).HitChance >= HitChance.High)
                    Porothrow.Cast(Porothrow.GetPrediction(ks).CastPosition);
                else if (target != null
                         && Porothrow.GetPrediction(target).HitChance >= HitChance.High)
                    Porothrow.Cast(Porothrow.GetPrediction(target).CastPosition);
            }
            // SMITE
            if (Smite != null && !Smite.IsOnCooldown
                && SummonerSpellMenu["smite"].Cast<CheckBox>().CurrentValue)
            {
                var ks = Program.GetEnemyKs(Smite.Range, Smitedamage, GameObjectType.AIHeroClient);
                if (ks != null && Smitename == SmiteName.Chilling
                    && SummonerSpellMenu["smitechill"].Cast<CheckBox>().CurrentValue)
                    Smite.Cast(ks);

                var target = Program.GetEnemy(Champion.GetAutoAttackRange(), GameObjectType.AIHeroClient);
                if (target != null && Smitename == SmiteName.Challenging
                    && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)
                    && SummonerSpellMenu["smiteduel"].Cast<CheckBox>().CurrentValue)
                    Smite.Cast(target);

                var ksmonster = Program.GetEnemyKs(Smite.Range, Smitedamage, GameObjectType.obj_AI_Minion);
                if (ksmonster != null && ksmonster.IsMonster)
                {
                    switch (ksmonster.Name)
                    {
                        case "SRU_Blue":
                            if (SummonerSpellMenu["smiteblue"].Cast<CheckBox>().CurrentValue)
                                Smite.Cast(ksmonster);
                            break;
                        case "SRU_Red":
                            if (SummonerSpellMenu["smitered"].Cast<CheckBox>().CurrentValue)
                                Smite.Cast(ksmonster);
                            break;
                        case "SRU_Murkwolf":
                            if (SummonerSpellMenu["smitemurk"].Cast<CheckBox>().CurrentValue)
                                Smite.Cast(ksmonster);
                            break;
                        case "SRU_Razorbeak":
                            if (SummonerSpellMenu["smiteraptor"].Cast<CheckBox>().CurrentValue)
                                Smite.Cast(ksmonster);
                            break;
                        case "SRU_Krug":
                            if (SummonerSpellMenu["smitekrug"].Cast<CheckBox>().CurrentValue)
                                Smite.Cast(ksmonster);
                            break;
                        case "SRU_Gromp":
                            if (SummonerSpellMenu["smitegromp"].Cast<CheckBox>().CurrentValue)
                                Smite.Cast(ksmonster);
                            break;
                        case "Sru_Crab":
                            if (SummonerSpellMenu["smitecrab"].Cast<CheckBox>().CurrentValue)
                                Smite.Cast(ksmonster);
                            break;
                        case "SRU_Dragon":
                            if (SummonerSpellMenu["smitedragon"].Cast<CheckBox>().CurrentValue)
                                Smite.Cast(ksmonster);
                            break;
                        case "SRU_RiftHerald":
                            if (SummonerSpellMenu["smiteherald"].Cast<CheckBox>().CurrentValue)
                                Smite.Cast(ksmonster);
                            break;
                        case "SRU_Baron":
                            if (SummonerSpellMenu["smitenashor"].Cast<CheckBox>().CurrentValue)
                                Smite.Cast(ksmonster);
                            break;
                        case "TT_NWolf":
                            if (SummonerSpellMenu["smitewolf"].Cast<CheckBox>().CurrentValue)
                                Smite.Cast(ksmonster);
                            break;
                        case "TT_NGolem":
                            if (SummonerSpellMenu["smitegolem"].Cast<CheckBox>().CurrentValue)
                                Smite.Cast(ksmonster);
                            break;
                        case "TT_NWraith":
                            if (SummonerSpellMenu["smitewraith"].Cast<CheckBox>().CurrentValue)
                                Smite.Cast(ksmonster);
                            break;
                        case "TT_Spiderboss":
                            if (SummonerSpellMenu["smitespider"].Cast<CheckBox>().CurrentValue)
                                Smite.Cast(ksmonster);
                            break;
                    }
                }
            }
        }

        // Draw
        public static void DrawMode()
        {
            if (!SummonerSpellMenu["DrawSS"].Cast<CheckBox>().CurrentValue) return;

            if (Clarity != null)
                Drawing.DrawCircle(Champion.Position, Clarity.Range, Color.FromArgb(41, 221, 225));
            if (Exhaust != null)
                Drawing.DrawCircle(Champion.Position, Exhaust.Range, Color.FromArgb(233, 200, 80));
            if (Garrison != null)
                Drawing.DrawCircle(Champion.Position, Garrison.Range, Color.FromArgb(156, 255, 140));
            if (Heal != null)
                Drawing.DrawCircle(Champion.Position, Heal.Range, Color.FromArgb(145, 243, 104));
            if (Ignite != null)
                Drawing.DrawCircle(Champion.Position, Ignite.Range, Color.FromArgb(232, 59, 11));
            if (Mark != null)
                Drawing.DrawCircle(Champion.Position, Mark.Range, Color.FromArgb(228, 236, 255));
            if (Porothrow != null)
                Drawing.DrawCircle(Champion.Position, Porothrow.Range, Color.FromArgb(114, 218, 213));
            if (Smite != null)
                Drawing.DrawCircle(Champion.Position, Smite.Range, Color.FromArgb(255, 182, 16));
        }
    }
}