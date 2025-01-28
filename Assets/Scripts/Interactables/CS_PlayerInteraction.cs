// Created by Linus Jernström
using UnityEngine;
using UnityEngine.InputSystem;

namespace Interactables
{
    public class CS_PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private float _interactionDistance = 6f; //from how far can the player start interacting with an object
        //[SerializeField] private float _timeoutDistance = 10f; //at what distance does an interaction automatically stop (unused for now)
        [SerializeField] private bool _drawDebugRays;
        
        private Camera _camera;
        private IInteractable _focusedInteractable;
        private LayerMask _layerMask;

        private void OnEnable()
        {
            _camera = this.gameObject.GetComponentInChildren<Camera>();
            _layerMask = ~LayerMask.GetMask("Player");
        }

        private void Update()
        {
            TraceInterface();
        }

        private void TraceInterface()
        {
            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, _interactionDistance, _layerMask))
            {
                bool hasInteractable = hit.collider.TryGetComponent(out IInteractable hitInteractable);
                
                if(_drawDebugRays)
                    Debug.DrawRay(_camera.transform.position, _camera.transform.forward * hit.distance, hasInteractable ? Color.green : Color.yellow, 1f);
                
                if (!hasInteractable)
                {
                    UnfocusInteractable();
                    return;
                }
                
                if (hitInteractable != _focusedInteractable)
                {
                    UnfocusInteractable();
                    FocusInteractable(hitInteractable);
                }
            }
            else
            {
                UnfocusInteractable();
                if(_drawDebugRays)
                    Debug.DrawRay(_camera.transform.position, _camera.transform.forward * _interactionDistance, Color.cyan, 1f);
            }
                
        }

        private void FocusInteractable(IInteractable interactable)
        {
            _focusedInteractable = interactable;
            
            interactable.Focused = true;
            interactable.OnFocus();
        }
        
        private void UnfocusInteractable()
        {
            if (_focusedInteractable == null) 
                return;
            
            _focusedInteractable.Focused = false;
            _focusedInteractable.OnUnfocus();
            _focusedInteractable = null;
        }

        public void OnInteract()
        {
            if (_focusedInteractable == null)
                return;

            _focusedInteractable.OnInteract();
            
            if (_focusedInteractable.IsActive)
            {
                _focusedInteractable.IsActive = false;
                _focusedInteractable.OnDeactivate();
            }
            else
            {
                _focusedInteractable.IsActive = true;
                _focusedInteractable.OnActivate();
            }
        }
        
    }
}
