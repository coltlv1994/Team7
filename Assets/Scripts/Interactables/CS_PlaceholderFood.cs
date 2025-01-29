// Created by Linus JernstrÃ¶m
using UnityEngine;

namespace Interactables
{
    public class CS_PlaceholderFood : CS_InteractableObject
    {
        private InteractBunny interactBunny;
        private void Start()
        {
            interactBunny = GameObject.Find("PlayerNew").GetComponent<InteractBunny>();
        }
        public override void OnInteract()
        {
            interactBunny.UpdateFood();
            Destroy(this.gameObject);
        }
        public override void OnActivate() { }
        public override void OnDeactivate() { }
    }
}