using System.Collections.Generic;
using Assets.Scripts.Data;
using Assets.Scripts.States;
using UnityEngine;

// quick and dirty spawner to test the XML
namespace Assets.Scripts.Units
{
    public class UnitSpawner : MonoBehaviour
    {
        public static UnitSpawner PlayerSpawner;
        public static UnitSpawner EnemySpawner;

        [SerializeField] bool _useAsPlayerSpawner;
        [SerializeField] Minion _minionPrefab;

        List<MinionXml> _minionTypes;
        List<Minion> _minionHolder;

        void Awake()
        {
            if(_useAsPlayerSpawner) // could be refactored to not require this dependency
                PlayerSpawner = this;
            else
                EnemySpawner = this;

            _minionTypes = MinionLoaderXml.LoadData();
            _minionHolder = new List<Minion>();
        }

        internal void MinionsActivate()
        {
            foreach (Minion m in _minionHolder)
                m?.Activate();
        }

        public void SpawnMinions(int minionCount)
        {
            Debug.Log($"Spawning {minionCount} random minions"); // logging and random numbers should come from a Util class
            for (int i = 0; i < minionCount; i++)
            {
                Minion minion = Instantiate(_minionPrefab, transform);
                MinionXml type = _minionTypes[Random.Range(1, _minionTypes.Count)]; // start at 1 due to headers in the file
                minion.SetStatsFromXml(type, this);
                Debug.Log($"___New Minion___ Type: {minion.MinionType} Health: {minion.Health} Attack Range:{minion.AttackRange}");
                _minionHolder.Add(minion);
            }
        }

        public Minion GetRandomMinion()
        {
            if (_minionHolder.Count == 0)
                return null;

            return _minionHolder[Random.Range(0, _minionHolder.Count)];
        }

        internal UnitSpawner GetOpposingSpawner()
        {
            if (_useAsPlayerSpawner)
                return EnemySpawner;
            return PlayerSpawner;
        }

        internal static void UnitDied(Minion minion)
        {
            if (!minion)
                return;

            EnemySpawner._minionHolder.Remove(minion);
            PlayerSpawner._minionHolder.Remove(minion);

            if (EnemySpawner._minionHolder.Count == 0)
                BattleStateManager.BattleWon();
            else if (PlayerSpawner._minionHolder.Count == 0)
                BattleStateManager.BattleLost();

            Destroy(minion.gameObject);
        }

        internal static void DamageRandomUnit(Minion attackingUnit, UnitSpawner targetSpawner)
        {
            targetSpawner?.GetRandomMinion()?.TakeDamage(attackingUnit.Damage);
        }
    }
}
