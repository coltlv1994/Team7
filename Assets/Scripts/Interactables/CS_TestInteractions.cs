// Created by Linus Jernström

using System;
using UnityEngine;

namespace Interactables
{
    public class CS_TestInteractions : MonoBehaviour, IInteractable
    {
        public bool Focused { get; set; }
        public bool IsActive { get; set; }

        [SerializeField] private Material _outline;
        private Renderer _renderer;

        public Material Outline => _outline;
        public Renderer Renderer => _renderer ??= GetComponent<Renderer>();

        public void OnInteract()
        {
            Debug.Log("INTERACT!");
        }
        public void OnActivate()
        {
            Debug.Log("ACTIVATE");
        }
        public void OnDeactivate()
        {
            Debug.Log("DEACTIVATE");
        }
    }
}
