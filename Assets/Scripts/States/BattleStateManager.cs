using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace HoH_StateManagerTest.States
{
    public class BattleStateManager : MonoBehaviour
    {
        [SerializeField] State[] LoopedStates;
        [SerializeField] List<State> InterruptableStates;

        int _currentLoopedStateIndex = 0;

        void Start()
        {
            var mockState = new SpawnNextWave(LoopedStateComplete);

            LoopedStates = new State[1];
            LoopedStates[0] = mockState;
            mockState.RunState();
        }        
        
        public void LoopedStateComplete()
        {
            Debug.Log($"{LoopedStates[_currentLoopedStateIndex].GetType().Name} Completed, starting next one");
            _currentLoopedStateIndex = (_currentLoopedStateIndex + 1) % LoopedStates.Length;
            LoopedStates[_currentLoopedStateIndex].RunState();
        }
    }
}