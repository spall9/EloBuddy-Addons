using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace BallistaKogMaw
{
    internal class TargetManager
    {
        // Clone Character Object
        public static AIHeroClient Champion = Program.Champion;

        public static void Initialize()
        {
        }

        public static AIHeroClient GetChampionTarget(Spell.SpellBase spell, DamageType damagetype, bool isAlly = false, float ksdamage = -1f)
        {
            var herotype = EntityManager.Heroes.AllHeroes;
            var targets = herotype.OrderBy(a => a.HealthPercent)
                .Where(a => a.IsValidTarget(spell.Range) && ((isAlly && a.IsAlly) || (!isAlly && a.IsEnemy))
                            && !a.IsDead && !a.IsZombie
                            && TargetStatus(a)
                            && ((spell.Slot == SpellSlot.W
                            && ksdamage > -1f && a.Health <= Champion.CalculateDamageOnUnit(a, damagetype, SpellManager.WBonus(a))) || ksdamage == -1)
                            && ((spell.Slot == SpellSlot.R
                            && ksdamage > -1f && a.Health <= Champion.CalculateDamageOnUnit(a, damagetype, SpellManager.RDamage() * SpellManager.RMultiplier(a))) || ksdamage == -1)
                            && ((spell.Slot != SpellSlot.W && spell.Slot != SpellSlot.R
                            && ksdamage > -1f && a.Health <= Champion.CalculateDamageOnUnit(a, damagetype, ksdamage)) || ksdamage == -1)
                            && !Champion.IsRecalling()
                            && a.Distance(Champion) <= spell.Range);
            return TargetSelector.GetTarget(targets, damagetype);
        }

        public static Obj_AI_Minion GetMinionTarget(Spell.SpellBase spell, DamageType damagetype, bool isAlly = false, bool isMonster = false, float ksdamage = -1)
        {
            var teamtype = EntityManager.UnitTeam.Enemy;
            if (isAlly)
                teamtype = EntityManager.UnitTeam.Ally;
            var miniontype = EntityManager.MinionsAndMonsters.GetLaneMinions(teamtype, Champion.ServerPosition, spell.Range).ToArray();
            if (isMonster)
                miniontype = EntityManager.MinionsAndMonsters.GetJungleMonsters(Champion.ServerPosition, spell.Range).ToArray();

            // Check list objects
            if (miniontype.Length == 0) return null;

            var target = miniontype
                .OrderBy(a => a.HealthPercent)
                .FirstOrDefault(a => a.IsValidTarget(spell.Range) && ((isAlly && a.IsAlly) || (!isAlly && a.IsEnemy))
                                     && ((isMonster && a.IsMonster) || (!isMonster && !a.IsMonster))
                                     && !a.IsDead && !a.IsZombie
                                     && TargetStatus(a)
                                     && ((spell.Slot == SpellSlot.W
                                     && ksdamage > -1f && a.Health <= Champion.CalculateDamageOnUnit(a, damagetype, SpellManager.WBonus(a))) || ksdamage == -1)
                                     && ((spell.Slot == SpellSlot.R
                                     && ksdamage > -1f && a.Health <= Champion.CalculateDamageOnUnit(a, damagetype, SpellManager.RDamage() * SpellManager.RMultiplier(a))) || ksdamage == -1)
                                     && ((spell.Slot != SpellSlot.W && spell.Slot != SpellSlot.R
                                     && ksdamage > -1f && a.Health <= Champion.CalculateDamageOnUnit(a, damagetype, ksdamage)) || ksdamage == -1)
                                     && !Champion.IsRecalling()
                                     && a.Distance(Champion) <= spell.Range);
            return target;
        }

        public static Obj_AI_Turret GetTurretTarget(Spell.SpellBase spell, bool isAlly = false)
        {
            var turrettype = EntityManager.Turrets.AllTurrets;
            var target = turrettype.OrderByDescending(a => a.HealthPercent)
                .FirstOrDefault(a => a.IsValidTarget(spell.Range) && ((isAlly && a.IsAlly) || (!isAlly && a.IsEnemy))
                                     && !a.IsDead && !isAlly
                                     && !Champion.IsRecalling()
                                     && a.Distance(Champion) <= spell.Range);
            return target;
        }

        public static bool TargetStatus(Obj_AI_Base target)
        {
            return !target.Buffs.Any(a => a.IsValid()
                                          && a.DisplayName == "Chrono Shift"
                                          && a.DisplayName == "FioraW"
                                          && a.Type == BuffType.SpellShield);
        }
    }
}