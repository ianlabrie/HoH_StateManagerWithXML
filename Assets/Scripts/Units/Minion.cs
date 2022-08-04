using System;
using System.Collections.Generic;
using HoH_StateManagerTest.Data;
using UnityEngine;
using UnityEngine.Assertions;

namespace HoH_StateManagerTest.Units
{
    public class Minion : MonoBehaviour
    {
        private string _minionType;
        internal string MinionType { get => _minionType; set => _minionType = value; }
        private int _health;
        internal int Health { get => _health; set => _health = value; }
        private int _attackRange;
        internal int AttackRange { get => _attackRange; set => _attackRange = value; }
        private int _damage;
        internal int Damage { get => _damage; set => _damage = value; }

        private UnitSpawner SpawnerRef;
        public void SetStatsFromXML(MinionXML loadedData, UnitSpawner spawner)
        {
            MinionType = loadedData.MinionType;
            Health = loadedData.Health;
            AttackRange = loadedData.AttackRange;
            Damage = loadedData.Damage;
            SpawnerRef = spawner;
        }

        internal void TakeDamage(int damageAmount)
        {
            Health = Mathf.Max(0, Health - damageAmount);
            if(Health == 0)
            {
                Debug.Log($"{MinionType} has taken {damageAmount} and is now dead.");
                UnitSpawner.UnitDied(this);
            }
            else
            {
                Debug.Log($"{MinionType} has taken {damageAmount} and is now at {Health} health.");
            }
        }
        internal virtual void Activate() // should be refactored, likely componentized
        {
            // using Attack range as a % chance to attack
            bool UnitInRange = (UnityEngine.Random.Range(0, AttackRange)) > 0;

            UnitSpawner targetSpawner = SpawnerRef?.GetOpposingSpawner();
            if (UnitInRange && targetSpawner)
            {
                Debug.Log($"{MinionType} is attacking towards the {targetSpawner.name}");
                UnitSpawner.DamageRandomUnit(this, targetSpawner);
            }
            else
                Debug.Log($"{MinionType} could not find an enemy in range!");
        }
    }
}
