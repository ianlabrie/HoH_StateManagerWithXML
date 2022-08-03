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

        public void SetStatsFromXML(MinionXML loadedData)
        {
            MinionType = loadedData.MinionType;
            Health = loadedData.Health;
            AttackRange = loadedData.AttackRange;
        }
    }
}