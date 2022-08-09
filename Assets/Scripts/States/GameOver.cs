using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.States;
using UnityEngine;

namespace Assets.Scripts.States
{
    internal class GameOver : State
    {
        readonly string _displayString;
        internal GameOver(string displayString)
        {
            _displayString = displayString;
        }
        internal override string GetTitle()
        {
            return _displayString;
        }

        internal override void RunState()
        {
            Debug.Log(_displayString);
        }
    }
}
