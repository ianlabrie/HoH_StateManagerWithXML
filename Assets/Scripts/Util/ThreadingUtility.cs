/*******
This makes it easy to see if the app is exiting so you can force your thread logic to stop - Ian L 8/3/22
Found online, Post #9: https://forum.unity.com/threads/non-stopping-async-method-after-in-editor-game-is-stopped.558283/
*****/

using System.Threading;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace Assets.Scripts.Util
{
    [InitializeOnLoad]
#endif
    public static class ThreadingUtility
    {
        static readonly CancellationTokenSource QuitSource;

        public static CancellationToken QuitToken { get; }

        public static SynchronizationContext UnityContext { get; private set; }

        static ThreadingUtility()
        {
            QuitSource = new CancellationTokenSource();
            QuitToken = QuitSource.Token;
        }

#if UNITY_EDITOR
        [InitializeOnLoadMethod]
#endif
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void MainThreadInitialize()
        {
            UnityContext = SynchronizationContext.Current;
            Application.quitting += QuitSource.Cancel;
#if UNITY_EDITOR
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
#endif
        }

#if UNITY_EDITOR
        static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
                QuitSource.Cancel();
        }
#endif
    }
}
