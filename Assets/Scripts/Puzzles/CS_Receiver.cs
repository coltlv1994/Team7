// Created by Linus Jernström
using System.Collections;
using UnityEngine;

namespace Puzzles
{
    public abstract class CS_Receiver : MonoBehaviour
    {
        [SerializeField] private CS_Sender[] _senders;
        [SerializeField][Range(1, 100)] private float _percentageToActivate = 100f;
        [SerializeField] private float _timeToActivate = 0f;
        
        protected ReceiverState State = ReceiverState.Inactive;
        private static int _maxInputs;
        private int _currentInputs = 0;
        
        private void OnEnable()
        {
            foreach (var sender in _senders)
                if (sender != null)
                {
                    sender.OnSendInput += ReceiveInput;
                    sender.OnRevokeInput += RemoveInput;
                }
            
            _maxInputs = _senders.Length;
        }
        private void OnDisable()
        {
            foreach (var sender in _senders)
                if (sender != null)
                {
                    sender.OnSendInput -= ReceiveInput;
                    sender.OnRevokeInput -= RemoveInput;
                }
        }

        private void ReceiveInput()
        {
            _currentInputs++;
            
            if(_currentInputs >= _maxInputs * _percentageToActivate / 100)
                Activate();
        }

        private void RemoveInput()
        {
            _currentInputs--;
            
            if(_currentInputs < _maxInputs * _percentageToActivate / 100)
                Deactivate();
        }

        protected abstract void Activate();
        protected abstract void Deactivate();
    }

    public enum ReceiverState
    {
        Activating,
        Active,
        Deactivating,
        Inactive
    }
}
