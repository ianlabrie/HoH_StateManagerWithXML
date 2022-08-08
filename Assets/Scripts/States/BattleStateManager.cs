using Assets.Scripts.Units;
using UnityEngine;

namespace Assets.Scripts.States
{
    public class BattleStateManager : MonoBehaviour
    {
        [SerializeField] State[] _loopedStates;

        int _currentLoopedStateIndex = 0;
        static bool _bStatesInterrupted;

        void Start()
        {
            SetupStates();
            StartFirstState();
        }

        private void StartFirstState() => _loopedStates[0].RunState();
        private void SetupStates()
        {
            var spawnPlayerUnits = new SpawnNextWave(LoopedStateComplete, UnitSpawner.PlayerSpawner);
            var spawnEnemyUnits = new SpawnNextWave(LoopedStateComplete, UnitSpawner.EnemySpawner);
            var playerMinionsActivate = new MinionsActivate(LoopedStateComplete, UnitSpawner.PlayerSpawner);
            var enemyMinionsActivate = new MinionsActivate(LoopedStateComplete, UnitSpawner.EnemySpawner);

            _loopedStates = new State[8]; // quick and dirty hard-coding the list
            _loopedStates[0] = spawnPlayerUnits;
            _loopedStates[1] = spawnEnemyUnits;
            _loopedStates[2] = playerMinionsActivate;
            _loopedStates[3] = enemyMinionsActivate;
            _loopedStates[4] = playerMinionsActivate;
            _loopedStates[5] = enemyMinionsActivate;
            _loopedStates[6] = playerMinionsActivate;
            _loopedStates[7] = enemyMinionsActivate;
        }

        public void LoopedStateComplete()
        {
            if (_bStatesInterrupted)
                return;

            // log statements should be pushed to a Util class
            Debug.Log($"<Color=green>{_loopedStates[_currentLoopedStateIndex].GetTitle()} Completed</Color>, calling Run State on the next Looped State");
            _currentLoopedStateIndex = (_currentLoopedStateIndex + 1) % _loopedStates.Length;
            _loopedStates[_currentLoopedStateIndex].RunState();
        }

        internal static void BattleWon()
        {
            _bStatesInterrupted = true;
            Debug.Log("<Color=red>Battle Won!</Color>");
        }

        internal static void BattleLost()
        {
            _bStatesInterrupted = true;
            Debug.Log("<Color=red>Battle Lost!</Color>");
        }
    }
}
