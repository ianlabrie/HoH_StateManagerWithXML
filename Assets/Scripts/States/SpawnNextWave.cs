using HoH_StateManagerTest.Units;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HoH_StateManagerTest.States
{
    internal class SpawnNextWave : State
    {
        protected Action OnCompleteCallback;
        readonly UnitSpawner _targetSpawner;
        private readonly string _title;

        public SpawnNextWave(Action onCompleteCallback, UnitSpawner spawner, string title)
        {
            _title = title;
            OnCompleteCallback = onCompleteCallback;
            _targetSpawner = spawner;
        }
        
        private async void StartNextWave(CancellationToken token, UnitSpawner spawner)
        {
            // Is it still good to use this thread
            if (token.IsCancellationRequested)
                return;

            spawner.SpawnMinions(UnityEngine.Random.Range(1,3));
            await Task.Delay(TimeSpan.FromSeconds(1));

            OnCompleteCallback?.Invoke();
        }

        internal override void RunState()
        {
            StartNextWave(ThreadingUtility.QuitToken, _targetSpawner);
        }

        internal override string GetTitle()
        {
            return _title;
        }
    }
}