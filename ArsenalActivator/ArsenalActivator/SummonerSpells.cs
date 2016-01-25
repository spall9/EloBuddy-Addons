using System.Drawing;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
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

        // Clone Character Object
        public static AIHeroClient Champion = Program.Champion;

        // Skills
        public static SpellDataInst Sumspell1;
        public static SpellDataInst Sumspell2;

        // Variables
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

        public static void Initialize()
        {
            FindSummonerSpells();
            ConfigSummonerSpells();
        }

        // Find Both SummonerSpells
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

        // Config Both SummonerSpells
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

        // Calculate SummonerSpell Damage & Shields
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
                            new float[] { 0, 390, 410, 430, 450, 480, 510, 540, 570, 600, 640, 680, 720, 760, 800, 850, 900, 950,  1000 }[Champion.Level];
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
                var ally = TargetManager.GetChampionTarget(Clarity.Range, DamageType.Magical, true);

                // Restore self
                if (MenuManager.SummonerSpellMenu["clarityself"].Cast<Slider>().CurrentValue > 0
                    && (Champion.ManaPercent <= MenuManager.SummonerSpellMenu["clarityself"].Cast<Slider>().CurrentValue))
                    Clarity.Cast();
                // Restore ally
                if (ally != null
                    && MenuManager.SummonerSpellMenu["clarityally"].Cast<Slider>().CurrentValue > 0
                    && ally.ManaPercent <= MenuManager.SummonerSpellMenu["clarityally"].Cast<Slider>().CurrentValue)
                    Clarity.Cast();
            }
            // CLEANSE
            if (Cleanse != null && !Cleanse.IsOnCooldown 
                &&
                ((Champion.HasBuffOfType(BuffType.Blind) && MenuManager.SummonerSpellMenu["Cblind"].Cast<CheckBox>().CurrentValue)
                 || (Champion.HasBuffOfType(BuffType.Charm) && MenuManager.SummonerSpellMenu["Ccharm"].Cast<CheckBox>().CurrentValue)
                 || (Champion.HasBuffOfType(BuffType.Fear) && MenuManager.SummonerSpellMenu["Cfear"].Cast<CheckBox>().CurrentValue)
                 || (Champion.HasBuffOfType(BuffType.Flee) && MenuManager.SummonerSpellMenu["Cflee"].Cast<CheckBox>().CurrentValue)
                 || (Champion.HasBuffOfType(BuffType.Polymorph) && MenuManager.SummonerSpellMenu["Cpolymorph"].Cast<CheckBox>().CurrentValue)
                 || (Champion.HasBuffOfType(BuffType.Silence) && MenuManager.SummonerSpellMenu["Csilence"].Cast<CheckBox>().CurrentValue)
                 || (Champion.HasBuffOfType(BuffType.Sleep) && MenuManager.SummonerSpellMenu["Csleep"].Cast<CheckBox>().CurrentValue)
                 || (Champion.HasBuffOfType(BuffType.Slow) && MenuManager.SummonerSpellMenu["Cslow"].Cast<CheckBox>().CurrentValue)
                 || (Champion.HasBuffOfType(BuffType.Snare) && MenuManager.SummonerSpellMenu["Csnare"].Cast<CheckBox>().CurrentValue)
                 || (Champion.HasBuffOfType(BuffType.Stun) && MenuManager.SummonerSpellMenu["Cstun"].Cast<CheckBox>().CurrentValue)
                 || (Champion.HasBuffOfType(BuffType.Taunt) && MenuManager.SummonerSpellMenu["Ctaunt"].Cast<CheckBox>().CurrentValue)
                 | (Champion.HasBuff("summonerexhaust") && MenuManager.SummonerSpellMenu["Cexhaust"].Cast<CheckBox>().CurrentValue)
                 || (Champion.HasBuff("summonerdot") && Champion.Health <= 50 + 20*Champion.Level &&
                  MenuManager.SummonerSpellMenu["Cignite"].Cast<CheckBox>().CurrentValue)))
                Cleanse.Cast();
            // EXHAUST
            if (Exhaust != null && !Exhaust.IsOnCooldown)
            {
                // Targets
                var target = TargetManager.GetChampionTarget(Exhaust.Range, DamageType.Magical);
                var ally = TargetManager.GetChampionTarget(Exhaust.Range, DamageType.Magical, true);

                // Exhaust for self
                if (target != null
                    && Champion.HealthPercent <= MenuManager.SummonerSpellMenu["exhaustself"].Cast<Slider>().CurrentValue
                    && target.IsAttackingPlayer)
                    Exhaust.Cast(target);
                // Exhaust for ally
                if (target != null && ally != null
                    && ally.HealthPercent <= MenuManager.SummonerSpellMenu["exhaustally"].Cast<Slider>().CurrentValue
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
                var ally = TargetManager.GetChampionTarget(Heal.Range, DamageType.Magical);

                // Heal self
                if (MenuManager.SummonerSpellMenu["healself"].Cast<Slider>().CurrentValue > 0
                    && (Champion.HealthPercent <= MenuManager.SummonerSpellMenu["healself"].Cast<Slider>().CurrentValue)
                    && !Champion.HasBuff("summonerheal"))
                    Heal.Cast();
                // Heal ally
                if (ally != null
                    && MenuManager.SummonerSpellMenu["healally"].Cast<Slider>().CurrentValue > 0
                    && ally.HealthPercent <= MenuManager.SummonerSpellMenu["healally"].Cast<Slider>().CurrentValue
                    && !ally.HasBuff("summonerheal"))
                    Heal.Cast();
            }
            // IGNITE
            if (Ignite != null && !Ignite.IsOnCooldown
                && MenuManager.SummonerSpellMenu["ignite"].Cast<Slider>().CurrentValue > 0)
            {
                var target = TargetManager.GetChampionTarget(Ignite.Range, DamageType.True);

                if (target != null
                    && target.Health <= MenuManager.SummonerSpellMenu["ignite"].Cast<Slider>().CurrentValue)
                    Ignite.Cast(target);
            }
            // MARK
            if (Mark != null && !Mark.IsOnCooldown
                && MenuManager.SummonerSpellMenu["mark"].Cast<CheckBox>().CurrentValue
                && Mark.Name != "snowballfollowupcast")
            {
                var ks = TargetManager.GetChampionTarget(Mark.Range, DamageType.True, false, Markdamage);

                if (ks != null)
                    Mark.Cast(ks);
                else
                {
                    var target = TargetManager.GetChampionTarget(Mark.Range, DamageType.True, false, Markdamage);
                    if (target != null)
                        Mark.Cast(target);
                }
            }
            // PORO THROW
            if (Porothrow != null && !Porothrow.IsOnCooldown
                && MenuManager.SummonerSpellMenu["porothrow"].Cast<CheckBox>().CurrentValue
                && Porothrow.Name != "porothrowfollowupcast")
            {
                var ks = TargetManager.GetChampionTarget(Porothrow.Range, DamageType.True, false, Porothrowdamage);

                if (ks != null)
                    Porothrow.Cast(ks);
                else
                {
                    var target = TargetManager.GetChampionTarget(Porothrow.Range, DamageType.True, false, Porothrowdamage);
                    if (target != null)
                        Porothrow.Cast(target);
                }
            }
            // SMITE
            if (Smite != null && !Smite.IsOnCooldown
                && MenuManager.SummonerSpellMenu["smite"].Cast<CheckBox>().CurrentValue)
            {
                var kstarget = TargetManager.GetChampionTarget(Smite.Range, DamageType.True, false, Smitedamage);
                if (kstarget != null && Smitename == SmiteName.Chilling
                    && MenuManager.SummonerSpellMenu["smitechill"].Cast<CheckBox>().CurrentValue)
                    Smite.Cast(kstarget);

                var target = TargetManager.GetChampionTarget(Champion.GetAutoAttackRange(), DamageType.True);
                if (target != null && Smitename == SmiteName.Challenging
                    && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)
                    && MenuManager.SummonerSpellMenu["smiteduel"].Cast<CheckBox>().CurrentValue)
                    Smite.Cast(target);

                var ksminion = TargetManager.GetMinionTarget(Smite.Range, DamageType.True, false, true, Smitedamage);
                if (ksminion != null)
                {
                    switch (target.Name)
                    {
                        case "SRU_Blue":
                            if (MenuManager.SummonerSpellMenu["smiteblue"].Cast<CheckBox>().CurrentValue)
                                Smite.Cast(ksminion);
                            break;
                        case "SRU_Red":
                            if (MenuManager.SummonerSpellMenu["smitered"].Cast<CheckBox>().CurrentValue)
                                Smite.Cast(ksminion);
                            break;
                        case "SRU_Murkwolf":
                            if (MenuManager.SummonerSpellMenu["smitemurk"].Cast<CheckBox>().CurrentValue)
                                Smite.Cast(ksminion);
                            break;
                        case "SRU_Razorbeak":
                            if (MenuManager.SummonerSpellMenu["smiteraptor"].Cast<CheckBox>().CurrentValue)
                                Smite.Cast(ksminion);
                            break;
                        case "SRU_Krug":
                            if (MenuManager.SummonerSpellMenu["smitekrug"].Cast<CheckBox>().CurrentValue)
                                Smite.Cast(ksminion);
                            break;
                        case "SRU_Gromp":
                            if (MenuManager.SummonerSpellMenu["smitegromp"].Cast<CheckBox>().CurrentValue)
                                Smite.Cast(ksminion);
                            break;
                        case "Sru_Crab":
                            if (MenuManager.SummonerSpellMenu["smitecrab"].Cast<CheckBox>().CurrentValue)
                                Smite.Cast(ksminion);
                            break;
                        case "SRU_Dragon":
                            if (MenuManager.SummonerSpellMenu["smitedragon"].Cast<CheckBox>().CurrentValue)
                                Smite.Cast(ksminion);
                            break;
                        case "SRU_RiftHerald":
                            if (MenuManager.SummonerSpellMenu["smiteherald"].Cast<CheckBox>().CurrentValue)
                                Smite.Cast(ksminion);
                            break;
                        case "SRU_Baron":
                            if (MenuManager.SummonerSpellMenu["smitenashor"].Cast<CheckBox>().CurrentValue)
                                Smite.Cast(ksminion);
                            break;
                        case "TT_NWolf":
                            if (MenuManager.SummonerSpellMenu["smitewolf"].Cast<CheckBox>().CurrentValue)
                                Smite.Cast(ksminion);
                            break;
                        case "TT_NGolem":
                            if (MenuManager.SummonerSpellMenu["smitegolem"].Cast<CheckBox>().CurrentValue)
                                Smite.Cast(ksminion);
                            break;
                        case "TT_NWraith":
                            if (MenuManager.SummonerSpellMenu["smitewraith"].Cast<CheckBox>().CurrentValue)
                                Smite.Cast(ksminion);
                            break;
                        case "TT_Spiderboss":
                            if (MenuManager.SummonerSpellMenu["smitespider"].Cast<CheckBox>().CurrentValue)
                                Smite.Cast(ksminion);
                            break;
                    }
                }
            }
        }

        // Draw
        public static void DrawMode()
        {
            if (!MenuManager.SummonerSpellMenu["DrawSS"].Cast<CheckBox>().CurrentValue) return;

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