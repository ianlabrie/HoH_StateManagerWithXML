using Assets.Scripts.Data;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public class Minion : MonoBehaviour
    {
        internal string MinionType { get; private set; }
        internal int Health { get; private set; }
        internal int AttackRange { get; private set; }
        internal int Damage { get; private set; }

        private UnitSpawner _spawnerRef;
        internal void SetStatsFromXml(MinionXml loadedData, UnitSpawner spawner)
        {
            MinionType = loadedData.MinionType;
            Health = loadedData.Health;
            AttackRange = loadedData.AttackRange;
            Damage = loadedData.Damage;
            _spawnerRef = spawner;
        }

        internal void TakeDamage(int damageAmount)
        {
            Health = Mathf.Max(0, Health - damageAmount);
            if(Health == 0)
            {
                Debug.Log($"{MinionType} has taken {damageAmount} and is now <b>dead</b>.");
                UnitSpawner.UnitDied(this);
            }
            else
            {
                Debug.Log($"{MinionType} has taken {damageAmount} and is now at {Health} health.");
            }
        }
        internal virtual void Activate() // should be refactored, likely componentized
        {
            UnitSpawner targetSpawner = _spawnerRef?.GetOpposingSpawner();
            bool unitInRange = targetSpawner && SimulateAttackRangeChance();

            if (unitInRange)
            {
                Debug.Log($"{MinionType} is attacking towards the {targetSpawner.name}");
                UnitSpawner.DamageRandomUnit(this, targetSpawner);
            }
            else
            {
                Debug.Log($"{MinionType} could not find an enemy in range!");
            }
        }

        private bool SimulateAttackRangeChance() => (Random.Range(0, AttackRange)) > 0;
    }
}
