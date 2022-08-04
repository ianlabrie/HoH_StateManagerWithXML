using HoH_StateManagerTest.Data;
using HoH_StateManagerTest.States;
using HoH_StateManagerTest.Units;
using System.Collections.Generic;
using UnityEngine;

// quick and dirty spawner to test the XML
namespace HoH_StateManagerTest.Units
{
    public class UnitSpawner : MonoBehaviour
    {
        public static UnitSpawner PlayerSpawner;
        public static UnitSpawner EnemySpawner;

        [SerializeField] bool UseAsPlayerSpawner;
        [SerializeField] Minion MinionPrefab;

        List<MinionXML> MinionTypes;
        List<Minion> MinionHolder;
        void Awake()
        {
            if(UseAsPlayerSpawner) // could be refactored to not require this dependency
                PlayerSpawner = this;
            else
                EnemySpawner = this;

            MinionTypes = MinionLoaderXML.LoadData();
            MinionHolder = new List<Minion>();
        }

        internal void MinionsActivate()
        {
            foreach (Minion m in MinionHolder)
                m.Activate();
        }

        public void SpawnMinions(int minionCount)
        {
            Debug.Log($"Spawning {minionCount} random minions"); // logging and random numbers should come from a Util class
            for (int i = 0; i < minionCount; i++)
            {
                Minion minion = Instantiate(MinionPrefab, transform);
                MinionXML type = MinionTypes[Random.Range(1, MinionTypes.Count)]; // start at 1 due to headers in the file
                minion.SetStatsFromXML(type, this);
                Debug.Log($"___New Minion___ Type: {minion.MinionType} Health: {minion.Health} Attack Range:{minion.AttackRange}");
                MinionHolder.Add(minion);
            }
        }

        public Minion GetRandomMinion()
        {
            if (MinionHolder.Count == 0)
                return null;

            return MinionHolder[Random.Range(0, MinionHolder.Count)];
        }

        internal UnitSpawner GetOpposingSpawner()
        {
            if (UseAsPlayerSpawner)
                return EnemySpawner;
            return PlayerSpawner;
        }

        internal static void UnitDied(Minion minion)
        {
            EnemySpawner.MinionHolder.Remove(minion);
            PlayerSpawner.MinionHolder.Remove(minion);

            if (EnemySpawner.MinionHolder.Count == 0)
                BattleStateManager.BattleWon();
            else if (PlayerSpawner.MinionHolder.Count == 0)
                BattleStateManager.BattleLost();

            Destroy(minion.gameObject);
        }

        internal static void DamageRandomUnit(Minion attackingUnit, UnitSpawner targetSpawner)
        {
            Minion targetUnit = targetSpawner.GetRandomMinion();
            if (targetUnit)
                targetUnit.TakeDamage(attackingUnit.Damage);
            else
                Debug.Log("Info: No valid targets to attack");
        }
    }
}
