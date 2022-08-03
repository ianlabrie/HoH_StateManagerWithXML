using System;

namespace HoH_StateManagerTest.States
{
    public abstract class State
    {
        internal abstract string GetTitle();
        internal abstract void RunState();
    }
}