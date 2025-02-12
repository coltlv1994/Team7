//Created by Linus Jernström
using System.Collections;
using UnityEngine;

namespace Puzzles
{
    public class CS_Door : CS_Receiver
    {
        [SerializeField] private bool _staysActive;
        protected override void Activate()
        {
            
        }
        protected override void Deactivate()
        {
            if (_staysActive)
                return;
            
        }
    }
}
