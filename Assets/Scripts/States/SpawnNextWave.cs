using HoH_StateManagerTest.Units;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HoH_StateManagerTest.States
{
    internal class SpawnNextWave : State
    {
        protected Action OnCompleteCallback;
        public SpawnNextWave(Action onCompleteCallback)
        {
            OnCompleteCallback = onCompleteCallback;
        }
        
        private async void StartNextWave(CancellationToken token)
        {
            UnitSpawner.instance.SpawnMinions(UnityEngine.Random.Range(1,3));
            await Task.Delay(TimeSpan.FromSeconds(1));
            
            // Is it still good to use this thread
            if (token.IsCancellationRequested)
                return;

            if (OnCompleteCallback != null)
                OnCompleteCallback();
        }

        internal override void RunState()
        {
            StartNextWave(ThreadingUtility.QuitToken);
        }
    }
}