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
        public static UnitSpawner player;
        public static UnitSpawner enemy;
        [SerializeField] Minion MinionPrefab;
        List<MinionXML> MinionTypes;
        List<Minion> MinionHolder;
        void Awake()
        {
            if(enemy == null) // hack to set the two up
                enemy = this;
            else if(player == null)
                player = this;
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

        internal static void UnitDied(Minion minion)
        {
            enemy.MinionHolder.Remove(minion);
            player.MinionHolder.Remove(minion);

            if (enemy.MinionHolder.Count == 0)
                BattleStateManager.BattleWon();
            else if (player.MinionHolder.Count == 0)
                BattleStateManager.BattleLost();

            Destroy(minion.gameObject);
        }

        internal static void DamageRandomUnit(Minion attackingUnit, UnitSpawner targetPlayer)
        {
            Minion targetUnit = targetPlayer.GetRandomMinion();
            if (targetUnit)
                targetUnit.TakeDamage(attackingUnit.Damage);
            else
                Debug.Log("Info: No valid targets to attack");
        }

        public Minion GetRandomMinion()
        {
            if (MinionHolder.Count == 0)
                return null;

            return MinionHolder[Random.Range(0, MinionHolder.Count)];
        }
    }
}
