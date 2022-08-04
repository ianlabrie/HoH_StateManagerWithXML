using System;

namespace HoH_StateManagerTest.States
{
    internal abstract class State
    {
        internal abstract string GetTitle();
        internal abstract void RunState();
    }
}
