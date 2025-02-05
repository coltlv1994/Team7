// Created by Linus Jernström
using System;
using UnityEngine;

namespace Puzzles
{
    [Serializable]
    public abstract class CS_Sender : MonoBehaviour
    {
        public event Action OnSendInput;
        public event Action OnRevokeInput;
        
        protected virtual void SendInput()
        {
            OnSendInput?.Invoke();
        }

        protected virtual void RevokeInput()
        {
            OnRevokeInput?.Invoke();
        }
        
    }
}
