using HoH_StateManagerTest.Units;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HoH_StateManagerTest.States
{
    internal class MinionsActivate : State
    {
        readonly Action _onCompleteCallback;
        readonly UnitSpawner _targetSpawner;

        public MinionsActivate(Action onCompleteCallback, UnitSpawner spawner)
        {
            _onCompleteCallback = onCompleteCallback;
            _targetSpawner = spawner;
        }
        
        private async void StartActivateMinions(CancellationToken token, UnitSpawner targetSpawner, Action onCompleteCallback)
        {
            if (token.IsCancellationRequested)
                return;

            targetSpawner?.MinionsActivate();
            await Task.Delay(TimeSpan.FromSeconds(2));

            onCompleteCallback?.Invoke();
        }

        internal override void RunState()
        {
            StartActivateMinions(ThreadingUtility.QuitToken, _targetSpawner, _onCompleteCallback);
        }

        internal override string GetTitle()
        {
            return $"Activating minions that have been spawned from {_targetSpawner?.GetType().Name }";
        }
    }
}
