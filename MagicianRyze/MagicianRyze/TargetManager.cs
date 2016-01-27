using System.Linq;
using SharpDX;
using EloBuddy;
using EloBuddy.SDK;

namespace MagicianRyze
{
    internal class TargetManager
    {
        // Clone Character Object
        public static AIHeroClient Champion = Program.Champion;

        public static void Initialize()
        {
        }

        public static AIHeroClient GetChampionTarget(float range, DamageType damagetype, bool isAlly = false, float ksdamage = -1f)
        {
            var herotype = EntityManager.Heroes.AllHeroes;
            var targets = herotype
                .OrderByDescending(a => a.HealthPercent)
                .Where(a => a.IsValidTarget(range) && ((isAlly && a.IsAlly) || (!isAlly && a.IsEnemy))
                            && !a.IsDead && !a.IsZombie
                            && TargetStatus(a)
                            && ((ksdamage > -1f && a.Health <= Champion.CalculateDamageOnUnit(a, damagetype, ksdamage)) || ksdamage == -1)
                            && !Champion.IsRecalling()
                            && a.Distance(Champion) <= range);
            return TargetSelector.GetTarget(targets, damagetype);
        }

        public static Obj_AI_Minion GetMinionTarget(float range, DamageType damagetype, bool isAlly = false, bool isMonster = false, float ksdamage = -1)
        {

            var teamtype = EntityManager.UnitTeam.Enemy;
            if (isAlly)
                teamtype = EntityManager.UnitTeam.Ally;
            var miniontype = EntityManager.MinionsAndMonsters.GetLaneMinions(teamtype, Champion.ServerPosition, range).ToArray();
            if (isMonster)
                miniontype = EntityManager.MinionsAndMonsters.GetJungleMonsters(Champion.ServerPosition, range).ToArray();

            // Check list objects
            if (miniontype.Length == 0) return null;

            var target = miniontype
                .OrderByDescending(a => a.HealthPercent)
                .FirstOrDefault(a => a.IsValidTarget(range) && ((isAlly && a.IsAlly) || (!isAlly && a.IsEnemy))
                                     && ((isMonster && a.IsMonster) || (!isMonster && !a.IsMonster))
                                     && !a.IsDead && !a.IsZombie
                                     && TargetStatus(a)
                                     && ((ksdamage > -1 && a.Health <= Champion.CalculateDamageOnUnit(a, damagetype, ksdamage)) || ksdamage == -1)
                                     && !Champion.IsRecalling()
                                     && a.Distance(Champion) <= range);
            return target;
        }

        public bool WillQHitEnemy(Obj_AI_Base enemy)
        {
            PredictionResult result = Prediction.Position.PredictCircularMissile(enemy, SpellManager.Q.Range, 50, 250, 1700, Champion.Position);

            if (result.CollisionObjects[0] == enemy)
                return true;
            return false;
        }

        public static Obj_AI_Turret GetTurretTarget(float range, bool isAlly = false)
        {
            var turrettype = EntityManager.Turrets.AllTurrets;
            var target = turrettype
                .OrderByDescending(a => a.HealthPercent)
                .FirstOrDefault(a => a.IsValidTarget(range) && ((isAlly && a.IsAlly) || (!isAlly && a.IsEnemy))
                                     && !a.IsDead
                                     && !Champion.IsRecalling()
                                     && a.Distance(Champion) <= range);
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