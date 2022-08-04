using HoH_StateManagerTest.Units;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace HoH_StateManagerTest.States
{
    public class BattleStateManager : MonoBehaviour
    {
        [SerializeField] State[] LoopedStates;
        [SerializeField] readonly List<State> InterruptableStates;

        int currentLoopedStateIndex = 0;
        static bool bStatesInterrupted;

        void Start()
        {
            var spawnPlayerUnits = new SpawnNextWave(LoopedStateComplete, UnitSpawner.PlayerSpawner, "Spawn Player Units");
            var spawnEnemyUnits = new SpawnNextWave(LoopedStateComplete, UnitSpawner.EnemySpawner, "Spawn Enemy Units");
            var playerMinionsActivate = new MinionsActivate(LoopedStateComplete, UnitSpawner.PlayerSpawner, "Player Units Activate 1/3");
            var enemyMinionsActivate = new MinionsActivate(LoopedStateComplete, UnitSpawner.EnemySpawner, "Enemy Units Activate 1/3");

            LoopedStates = new State[8]; // quick and dirty hardcoding the list
            LoopedStates[0] = spawnPlayerUnits;
            LoopedStates[1] = spawnEnemyUnits;
            LoopedStates[2] = playerMinionsActivate;
            LoopedStates[3] = enemyMinionsActivate;
            LoopedStates[4] = new MinionsActivate(LoopedStateComplete, UnitSpawner.PlayerSpawner, "Player Units Activate 2/3"); ;
            LoopedStates[5] = new MinionsActivate(LoopedStateComplete, UnitSpawner.EnemySpawner, "Enemy Units Activate 2/3");
            LoopedStates[6] = new MinionsActivate(LoopedStateComplete, UnitSpawner.PlayerSpawner, "Player Units Activate 3/3"); ;
            LoopedStates[7] = new MinionsActivate(LoopedStateComplete, UnitSpawner.EnemySpawner, "Enemy Units Activate 3/3");

            LoopedStates[0].RunState();
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
