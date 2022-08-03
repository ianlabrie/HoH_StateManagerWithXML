using HoH_StateManagerTest.Data;
using HoH_StateManagerTest.Units;
using System.Collections.Generic;
using UnityEngine;

// quick and dirty spawner to test the XML
namespace HoH_StateManagerTest.Units
{
    public class UnitSpawner : MonoBehaviour
    {
        public static UnitSpawner instance;
        [SerializeField] Minion MinionPrefab;

        List<MinionXML> MinionTypes;
        List<Minion> MinionHolder;
        void Awake()
        {
            instance = this; // should be refactored, just an easy hack for now
            MinionTypes = MinionLoaderXML.LoadData();
            MinionHolder = new List<Minion>();
        }

        public void SpawnMinions(int minionCount)
        {
            Debug.Log($"Spawning {minionCount} random minions"); // logging and random numbers should come from a Util class
            for (int i = 0; i < minionCount; i++)
            {
                Minion minion = Instantiate(MinionPrefab);
                MinionXML type = MinionTypes[Random.Range(1, MinionTypes.Count)]; // start at 1 due to headers in the file
                minion.SetStatsFromXML(type);
                Debug.Log($"___New Minion___ Type: {minion.MinionType} Health: {minion.Health} Attack Range:{minion.AttackRange}");
                MinionHolder.Add(minion);
            }
        }
    }
}
