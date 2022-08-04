using HoH_StateManagerTest.Units;
using System.Collections.Generic;
using UnityEngine;

namespace HoH_StateManagerTest.States
{
    public class BattleStateManager : MonoBehaviour
    {
        [SerializeField] State[] LoopedStates;

        int currentLoopedStateIndex = 0;
        static bool bStatesInterrupted;

        void Start()
        {
            SetupStates();
            StartFirstState();
        }

        private void StartFirstState() => LoopedStates[0].RunState();
        private void SetupStates()
        {
            var spawnPlayerUnits = new SpawnNextWave(LoopedStateComplete, UnitSpawner.PlayerSpawner);
            var spawnEnemyUnits = new SpawnNextWave(LoopedStateComplete, UnitSpawner.EnemySpawner);
            var playerMinionsActivate = new MinionsActivate(LoopedStateComplete, UnitSpawner.PlayerSpawner);
            var enemyMinionsActivate = new MinionsActivate(LoopedStateComplete, UnitSpawner.EnemySpawner);

            LoopedStates = new State[8]; // quick and dirty hardcoding the list
            LoopedStates[0] = spawnPlayerUnits;
            LoopedStates[1] = spawnEnemyUnits;
            LoopedStates[2] = playerMinionsActivate;
            LoopedStates[3] = enemyMinionsActivate;
            LoopedStates[4] = playerMinionsActivate;
            LoopedStates[5] = enemyMinionsActivate;
            LoopedStates[6] = playerMinionsActivate;
            LoopedStates[7] = enemyMinionsActivate;
        }

        public void LoopedStateComplete()
        {
            if (bStatesInterrupted)
                return;

            // log statements should be pushed to a Util class
            Debug.Log($"<Color=green>{LoopedStates[currentLoopedStateIndex].GetTitle()} Completed</Color>, calling Run State on the next Looped State");
            currentLoopedStateIndex = (currentLoopedStateIndex + 1) % LoopedStates.Length;
            LoopedStates[currentLoopedStateIndex].RunState();
        }

        internal static void BattleWon()
        {
            bStatesInterrupted = true;
            Debug.Log("<Color=red>Battle Won!</Color>");
        }

        internal static void BattleLost()
        {
            bStatesInterrupted = true;
            Debug.Log("<Color=red>Battle Lost!</Color>");
        }
    }
}
