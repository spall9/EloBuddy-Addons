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

namespace AlchemistSinged
{
    class TargetManager
    {
        // Assign TargetSelector with valid Champion target
        public static AIHeroClient GetChampionTarget(float range, DamageType damagetype, bool IsAlly = false)
        {
            return TargetSelector.GetTarget(EntityManager.Heroes.AllHeroes
                .OrderBy(a => a.HealthPercent)
                .Where(a => IsTargetValid(a)
                    && IsFriendOrFoe(a, IsAlly)
                    && a.IsInRange(Program.Champion, range))
                , damagetype);
        }

        public static Obj_AI_Minion GetMinionTarget(float range, DamageType damagetype, bool isAlly = false)
        {
            EntityManager.UnitTeam team = isAlly ? EntityManager.UnitTeam.Ally : EntityManager.UnitTeam.Enemy;
            var miniontype = EntityManager.MinionsAndMonsters.GetLaneMinions(team, Program.Champion.Position, range).ToArray();

            // Check list objects
            if (miniontype.Length == 0) return null;

            return miniontype
                .OrderBy(a => a.HealthPercent)
                .FirstOrDefault(a => IsTargetValid(a)
                    && a.IsInRange(a, range));
        }
        
        public static Obj_AI_Minion GetMonsterTarget(float range, DamageType damagetype)
        {
            var miniontype = EntityManager.MinionsAndMonsters.GetJungleMonsters(Program.Champion.Position, range).ToArray();

            // Check list objects
            if (miniontype.Length == 0) return null;

            return miniontype
                .OrderBy(a => a.HealthPercent)
                .FirstOrDefault(a => IsTargetValid(a));
        }

        // Locate valid Turret target
        public static Obj_AI_Turret GetTurretTarget(float range, bool isAlly = false)
        {
            return EntityManager.Turrets.AllTurrets
                .OrderBy(a => a.HealthPercent)
                .FirstOrDefault(a => IsTargetValid(a)
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

        // Does this target have low enough Health to kill?
        public static bool CalculateKS(Obj_AI_Base target, DamageType damagetype, float damage)
        {
            return target.Health <= Program.Champion.CalculateDamageOnUnit(target, damagetype, damage);
        }
    }
}