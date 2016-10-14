using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InfiltratorLux
{
    class TargetManager
    {
        // Assign TargetSelector with valid Champion target
        public static AIHeroClient GetChampionTarget(float range, DamageType damagetype, bool IsAlly = false, float ksdamage = -1f)
        {
            return TargetSelector.GetTarget(EntityManager.Heroes.AllHeroes
                .OrderBy(a => a.HealthPercent)
                .Where(a => WithinRange(a, range)
                    && IsTargetValid(a)
                    && IsFriendOrFoe(a, IsAlly)
                    && CalculateKs(a, damagetype, ksdamage))
            , damagetype);
        }

        public static Obj_AI_Minion GetMinionTarget(float range, DamageType damagetype, bool isAlly = false, bool isMonster = false, float ksdamage = -1)
        {
            var teamtype = EntityManager.UnitTeam.Enemy;
            if (isAlly)
                teamtype = EntityManager.UnitTeam.Ally;
            var miniontype = EntityManager.MinionsAndMonsters.GetLaneMinions(teamtype, Program.Champion.ServerPosition, range).ToArray();
            if (isMonster)
                miniontype = EntityManager.MinionsAndMonsters.GetJungleMonsters(Program.Champion.ServerPosition, range).ToArray();

            // Check list objects
            if (miniontype.Length == 0) return null;

            return miniontype
                .OrderBy(a => a.HealthPercent)
                .FirstOrDefault(a => WithinRange(a, range)
                    && IsTargetValid(a)
                    && IsFriendOrFoe(a, isAlly)
                    && CalculateKs(a, damagetype, ksdamage));
        }

        // Locate valid Turret target
        public static Obj_AI_Turret GetTurretTarget(float range, bool isAlly = false)
        {
            return EntityManager.Turrets.AllTurrets
                .OrderBy(a => a.HealthPercent)
                .FirstOrDefault(a => WithinRange(a, range)
                    && IsTargetValid(a)
                    && IsFriendOrFoe(a, isAlly));
        }

        // Reject targets with buffs that prevent damage or conditions
        public static bool BuffStatus(Obj_AI_Base target)
        {
            return !target.Buffs.Any(a => a.IsValid()
                                          && a.DisplayName == "Chrono Shift"
                                          && a.DisplayName == "FioraW"
                                          && a.Type == BuffType.SpellShield);
        }

        // Is this target alive and meet all conditions?
        public static bool IsTargetValid(Obj_AI_Base target)
        {
            return !target.IsDead && !target.IsZombie && !Program.Champion.IsRecalling() && BuffStatus(target);
        }

        // Is this traget friend or foe?
        public static bool IsFriendOrFoe(Obj_AI_Base target, bool ally)
        {
            return (ally && target.IsAlly && !target.IsMe) || (!ally && target.IsEnemy);
        }

        // Is this target within range of your spell?
        public static bool WithinRange(Obj_AI_Base target, float range)
        {
            return target.IsValidTarget(range) && target.Distance(Program.Champion) <= range;
        }

        // Does this target have low enough Health to kill?
        public static bool CalculateKs(Obj_AI_Base target, DamageType damagetype, float damage)
        {
            return (damage > -1f && target.Health <= Program.Champion.CalculateDamageOnUnit(target, damagetype, damage)) || damage == -1;
        }
    }
}