//Created by Linus Jernström

using UnityEngine;

namespace Interactables
{
    public class CS_BunnyInteract : MonoBehaviour, IInteractable
    {
        private InteractBunny _interactBunny;

        public bool Focused { get; set; }
        public bool IsActive { get; set; }

        [SerializeField] private Material _outline;
        private Renderer _renderer;
        
        public Material Outline => _outline;
        public Renderer Renderer => _renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        private void OnEnable()
        {
            _interactBunny = GameObject.FindAnyObjectByType<InteractBunny>();
        }

        public void OnInteract()
        {
            _interactBunny.BunnyInteraction(gameObject);
        }
        public void OnActivate() { }
        public void OnDeactivate() { }
    }
}
