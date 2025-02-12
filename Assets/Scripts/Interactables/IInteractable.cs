// Created by Linus Jernström
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Interactables
{
    interface IInteractable
    {
        public bool Focused { get; set; }
        public bool IsActive { get; set; }
        public Material Outline { get; }
        
        Renderer Renderer { get; }

        void OnFocus()
        {
            if (Renderer != null && Outline != null)
            {
                Material[] newMaterials = new Material[Renderer.materials.Length + 1];
                Renderer.materials.CopyTo(newMaterials, 0);
                newMaterials[^1] = Outline;
                Renderer.materials = newMaterials;
            }
        }
        void OnUnfocus()
        {
            if (Renderer != null && Outline != null)
            {
                Material[] currentMaterials = Renderer.sharedMaterials;
                List<Material> newMaterials = new List<Material>();
    
                foreach (Material mat in currentMaterials)
                    if (mat != Outline)
                        newMaterials.Add(mat);
                    
                Renderer.materials = newMaterials.ToArray();
            }
        }
        void OnInteract();
        void OnActivate();
        void OnDeactivate();
    }
}
