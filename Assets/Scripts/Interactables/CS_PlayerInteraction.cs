// Created by Linus Jernström
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Interactables
{
    [RequireComponent(typeof(PlayerInput))]
    public class CS_PlayerInteraction : MonoBehaviour
    {
        #region Properties
        [Header("Interaction Logic Variables")]
        [SerializeField] private float _interactionDistance = 6f; //from how far can the player start interacting with an object
        //[SerializeField] private float _timeoutDistance = 10f; //at what distance does an interaction automatically stop (unused for now)
        [SerializeField] private float _timeToInteract = .5f;
        private float _interactTimer;
        private bool _isInteracting;
        private Camera _camera;
        private IInteractable _focusedInteractable;
        private LayerMask _layerMask;
        
        [Header("Interaction Visualisation Variables")]
        [SerializeField] private bool _drawDebugRays;
        [SerializeField] private GameObject _uiPrefab;
        private Canvas _UI;
        private Image _progressImage;
        #endregion
        
        #region Setup
        private void Awake()
        {
            _UI = Instantiate(_uiPrefab, gameObject.transform).GetComponent<Canvas>();
        }

        private void OnEnable()
        {
            _camera = this.gameObject.GetComponentInChildren<Camera>();
            _layerMask = ~LayerMask.GetMask("Player", "Ignore Raycast");
            
            _progressImage = _UI.GetComponentsInChildren<Image>()[^1];
            _UI.gameObject.SetActive(false);
        }
        #endregion

        private void Update()
        {
            TraceInteractable();
            
            if(_isInteracting)
                ProgressInteraction();
            
            else if (_focusedInteractable != null)
                RemoveProgress();
        }

        #region Focusing Interactables
        private void TraceInteractable()
        {
            if (Physics.Raycast(_camera.transform.position + _camera.transform.forward, _camera.transform.forward, out RaycastHit hit, _interactionDistance, _layerMask))
            {
                bool hasInteractable = hit.collider.TryGetComponent(out IInteractable hitInteractable);
                
                if(_drawDebugRays)
                    Debug.DrawRay(_camera.transform.position + _camera.transform.forward, _camera.transform.forward * hit.distance, hasInteractable ? Color.green : Color.yellow, 1f);
                
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
                _isInteracting = false;
                if(_drawDebugRays)
                    Debug.DrawRay(_camera.transform.position + _camera.transform.forward, _camera.transform.forward * _interactionDistance, Color.cyan, 1f);
            }
        }

        private void FocusInteractable(IInteractable interactable)
        {
            _focusedInteractable = interactable;
            
            interactable.Focused = true;
            interactable.OnFocus();
            
            _UI.gameObject.SetActive(true);
        }
        
        private void UnfocusInteractable()
        {
            if (_focusedInteractable == null) 
                return;
            
            _focusedInteractable.Focused = false;
            _focusedInteractable.OnUnfocus();
            _focusedInteractable = null;
            
            _isInteracting = false;
            _interactTimer = 0;
            ResetProgress();
            
            _UI.gameObject.SetActive(false);
        }
        #endregion
        
        #region Detecting and Sending Interactions
        public void OnInteract(InputAction.CallbackContext context)
        {
            if (_focusedInteractable == null)
                return;

            if (context.started)
                _isInteracting = true;
            else if (context.canceled)
                _isInteracting = false;
        }

        private void ProgressInteraction()
        {
            if (!_isInteracting || _focusedInteractable == null)
            {
                UnfocusInteractable();
                return;
            }

            _interactTimer += Time.deltaTime;
            SetProgressByTime();
            
            if (_interactTimer >= _timeToInteract)
            {
                IInteractable temp = _focusedInteractable;
                UnfocusInteractable();
                SendInteraction(temp);
            }
        }

        private void RemoveProgress()
        {
            if (_interactTimer > 0f)
            {
                _interactTimer -= Time.deltaTime;
                SetProgressByTime();
            }
        }

        private void SendInteraction(IInteractable interactable)
        {
            interactable.OnInteract();
            
            if (interactable.IsActive)
            {
                interactable.IsActive = false;
                interactable.OnDeactivate();
            }
            else
            {
                interactable.IsActive = true;
                interactable.OnActivate();
            }
        }
        #endregion
        
        #region ProgressBar
        private void SetProgressByTime() => _progressImage.fillAmount = _interactTimer / _timeToInteract;
        private void ResetProgress() => _progressImage.fillAmount = 0;
        #endregion
    }
}
