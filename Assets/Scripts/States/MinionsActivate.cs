using HoH_StateManagerTest.Units;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HoH_StateManagerTest.States
{
    internal class MinionsActivate : State
    {
        protected Action OnCompleteCallback;
        readonly UnitSpawner _targetSpawner;

        public MinionsActivate(Action onCompleteCallback, UnitSpawner spawner)
        {
            OnCompleteCallback = onCompleteCallback;
            _targetSpawner = spawner;
        }
        
        private async void StartActivateMinions(CancellationToken token, UnitSpawner targetSpawner)
        {
            // Is it still good to use this thread
            if (token.IsCancellationRequested)
                return;

            targetSpawner.MinionsActivate();
            await Task.Delay(TimeSpan.FromSeconds(2));

            OnCompleteCallback?.Invoke();
        }

        internal override void RunState()
        {
            StartActivateMinions(ThreadingUtility.QuitToken, _targetSpawner);
        }

        internal override string GetTitle()
        {
            return $"Activating minions that have been spawned from {_targetSpawner?.GetType().Name }";
        }
    }
}
