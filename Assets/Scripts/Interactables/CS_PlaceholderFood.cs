// Created by Linus Jernström
using UnityEngine;

namespace Interactables
{
    public class CS_PlaceholderFood : CS_InteractableObject
    {
        public override void OnInteract()
        {
            Destroy(this.gameObject);
        }
        public override void OnActivate() { }
        public override void OnDeactivate() { }
    }
}
