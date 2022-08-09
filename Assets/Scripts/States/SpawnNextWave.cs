using System;
using System.Threading;
using System.Threading.Tasks;
using Assets.Scripts.Units;
using Assets.Scripts.Util;

namespace Assets.Scripts.States
{
    internal class SpawnNextWave : State
    {
        readonly Action _onCompleteCallback;
        readonly UnitSpawner _targetSpawner;

        public SpawnNextWave(Action onCompleteCallback, UnitSpawner spawner)
        {
            _onCompleteCallback = onCompleteCallback;
            _targetSpawner = spawner;
        }
        
        private async void StartNextWave(CancellationToken token, UnitSpawner spawner)
        {
            // Is it still good to use this thread
            if (token.IsCancellationRequested)
                return;

            spawner.SpawnMinions(UnityEngine.Random.Range(2,4));
            await Task.Delay(TimeSpan.FromSeconds(1), token);

            _onCompleteCallback?.Invoke();
        }

        internal override void RunState()
        {
            StartNextWave(ThreadingUtility.QuitToken, _targetSpawner);
        }

        internal override string GetTitle()
        {
            return $"Activating {_targetSpawner?.GetType().Name }";
        }
    }
}
