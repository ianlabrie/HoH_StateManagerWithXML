namespace Assets.Scripts.States
{
    internal abstract class State
    {
        internal abstract string GetTitle();
        internal abstract void RunState();
    }
}
