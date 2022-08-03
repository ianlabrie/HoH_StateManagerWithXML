using System;

namespace HoH_StateManagerTest.States
{
    [Serializable]
    public abstract class State
    {
        internal abstract void RunState();
    }
}