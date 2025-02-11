// Created by Linus JernstrÃ¶m
using UnityEngine;

namespace Interactables
{
    public class CS_PlaceholderFood : CS_InteractableObject
    {
        [SerializeField] bool isSupaFood;
        [SerializeField] AudioClip foodSound;
        AudioManager audioManager;
        private InteractBunny interactBunny;
        private void Start()
        {
            audioManager = FindFirstObjectByType<AudioManager>();
            interactBunny = GameObject.Find("PlayerNew").GetComponent<InteractBunny>();
        }
        public override void OnInteract()
        {
            if (interactBunny.pTimer.gameData.foods < InteractBunny.maxFoods && !isSupaFood) //limits food to max 3
            {
                interactBunny.UpdateFood();
                Destroy(this.gameObject);
                audioManager.PlaySFX(foodSound);

                
            }
            else
            {
                interactBunny.increaseTimer = true;
                interactBunny.supaCarrotCount++;
                audioManager.PlaySFX(foodSound);
                Destroy(this.gameObject);
            }

        }
        public override void OnActivate() { }
        public override void OnDeactivate() { }
    }
}