using HoH_StateManagerTest.Data;
using UnityEngine;

namespace HoH_StateManagerTest.Units
{
    public class Minion : MonoBehaviour
    {
        internal string MinionType { get => _minionType; }
        private string _minionType;
        internal int Health { get => _health; }
        private int _health;
        internal int AttackRange { get => _attackRange; }
        private int _attackRange;
        internal int Damage { get => _damage; }
        private int _damage;

        private UnitSpawner SpawnerRef;
        internal void SetStatsFromXML(MinionXML loadedData, UnitSpawner spawner)
        {
            _minionType = loadedData.MinionType;
            _health = loadedData.Health;
            _attackRange = loadedData.AttackRange;
            _damage = loadedData.Damage;
            SpawnerRef = spawner;
        }

        internal void TakeDamage(int damageAmount)
        {
            _health = Mathf.Max(0, Health - damageAmount);
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
            UnitSpawner targetSpawner = SpawnerRef?.GetOpposingSpawner();
            bool UnitInRange = targetSpawner && SimulateAttackRangeChance();

            if (UnitInRange)
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
