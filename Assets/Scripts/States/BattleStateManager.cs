using System.Collections.Generic;
using Assets.Scripts.Units;
using UnityEngine;

namespace Assets.Scripts.States
{
    public class BattleStateManager : MonoBehaviour
    {
        static State[] _loopedStates;
        static int _currentLoopedStateIndex;
        static readonly Queue<State> PriorityStateQueue = new Queue<State>();

        private void StartFirstState()
        {
            if (PriorityStateQueue.Count > 0)
            {
                PriorityStateQueue.Dequeue().RunState();
                return;
            }

            _loopedStates[0].RunState();
        }

        void Start()
        {
            SetupStates();
            StartFirstState();
        }

        private void SetupStates()
        {
            SpawnNextWave spawnPlayerUnits = new SpawnNextWave(StateComplete, UnitSpawner.PlayerSpawner);
            SpawnNextWave spawnEnemyUnits = new SpawnNextWave(StateComplete, UnitSpawner.EnemySpawner);
            MinionsActivate playerMinionsActivate = new MinionsActivate(StateComplete, UnitSpawner.PlayerSpawner);
            MinionsActivate enemyMinionsActivate = new MinionsActivate(StateComplete, UnitSpawner.EnemySpawner);
            
            _loopedStates = new State[2];
            _loopedStates[0] = playerMinionsActivate;
            _loopedStates[1] = enemyMinionsActivate;

            PriorityStateQueue.Enqueue(spawnPlayerUnits);
            PriorityStateQueue.Enqueue(spawnEnemyUnits);
        }

        public static void StateComplete()
        {
            Debug.Log($"<Color=green>{_loopedStates[_currentLoopedStateIndex].GetTitle()} Completed</Color>");
            if (PriorityStateQueue.Count > 0)
            {
                PriorityStateQueue.Dequeue().RunState();
                return;
            }

            Debug.Log("Calling Run State on the next Looped State");
            _currentLoopedStateIndex = (_currentLoopedStateIndex + 1) % _loopedStates.Length;
            _loopedStates[_currentLoopedStateIndex].RunState();
        }

        internal static void BattleWon()
        {
            PriorityStateQueue.Enqueue(new GameOver($"<Color=red> Battle Won! </Color>"));
        }

        internal static void BattleLost()
        {
            PriorityStateQueue.Enqueue(new GameOver($"<Color=red>Battle Lost!</Color>"));
        }
    }
}
