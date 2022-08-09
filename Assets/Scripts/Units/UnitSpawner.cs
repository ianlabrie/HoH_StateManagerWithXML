using System.Collections.Generic;
using Assets.Scripts.Data;
using Assets.Scripts.States;
using UnityEngine;
using UnityEngine.Pool;

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

        IObjectPool<Minion> _minionPool;

        void Awake()
        {
            if(_useAsPlayerSpawner) // could be refactored to not require this dependency
                PlayerSpawner = this;
            else
                EnemySpawner = this;

            _minionTypes = MinionLoaderXml.LoadData();
            _minionHolder = new List<Minion>();

            _minionPool = new ObjectPool<Minion>(CreateMinionFromPool, OnTakeMinionFromPool, OnReturnMinionToPool,
                default, default, 6, 500);
        }

        private void OnReturnMinionToPool(Minion minion)
        {
            minion.ShowMinionWaitingInPool();
            minion.gameObject.SetActive(false);
        }

        private void OnTakeMinionFromPool(Minion minion)
        {
            MinionXml type = _minionTypes[Random.Range(1, _minionTypes.Count)]; // start at 1 due to headers in the file
            minion.SetStatsFromXml(type, this);
            Debug.Log($"___New Minion___ Type: {minion.MinionType} Health: {minion.Health} Attack Range:{minion.AttackRange}");
            minion.gameObject.SetActive(true);
        }

        private Minion CreateMinionFromPool()
        {
            var minion = Instantiate(_minionPrefab, transform);
            minion.SetPool(_minionPool);
            return minion;
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
                _minionPool.Get(out var minion);
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
            
            minion.Release();

            if (EnemySpawner._minionHolder.Count == 0)
                BattleStateManager.BattleWon();
            else if (PlayerSpawner._minionHolder.Count == 0)
                BattleStateManager.BattleLost();

        }

        internal static void DamageRandomUnit(Minion attackingUnit, UnitSpawner targetSpawner)
        {
            targetSpawner?.GetRandomMinion()?.TakeDamage(attackingUnit.Damage);
        }
    }
}
