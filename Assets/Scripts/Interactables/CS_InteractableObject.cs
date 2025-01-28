// Created by Linus Jernström
using UnityEngine;

namespace Interactables
{
    public abstract class CS_InteractableObject : MonoBehaviour, IInteractable
    {
        public bool Focused { get; set; }
        public bool IsActive { get; set; }

        [SerializeField] private Material _outline;
        private Renderer _renderer;

        public Material Outline => _outline;
        public Renderer Renderer => _renderer ??= GetComponent<Renderer>();

        public abstract void OnInteract();
        public abstract void OnActivate();
        public abstract void OnDeactivate();
    }
}
