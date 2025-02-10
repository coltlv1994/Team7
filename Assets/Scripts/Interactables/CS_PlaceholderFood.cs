// Created by Linus JernstrÃ¶m
using UnityEngine;

namespace Interactables
{
    public class CS_PlaceholderFood : CS_InteractableObject
    {
        [SerializeField] bool isSupaFood;

        private InteractBunny interactBunny;
        private void Start()
        {
            interactBunny = GameObject.Find("PlayerNew").GetComponent<InteractBunny>();
        }
        public override void OnInteract()
        {
            if (interactBunny.timer.gameData.foods < InteractBunny.maxFoods) //limits food to max 3
            {
                interactBunny.UpdateFood();
                Destroy(this.gameObject);

                if (isSupaFood) interactBunny.increaseTimer = true;
            }
            else
            {
            
            }

        }
        public override void OnActivate() { }
        public override void OnDeactivate() { }
    }
}