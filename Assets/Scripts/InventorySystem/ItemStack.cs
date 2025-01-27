// Created by Linus Jernström
using UnityEngine;

namespace InventorySystem
{
    [System.Serializable]
    public class ItemStack
    {
        // All items are kept in stacks. This includes single items (which are just in a stack of size 1), and unstackables (which are always stack size 1).
        
        [SerializeField] private ItemScriptableObject itemData;
        [Range(1, 64)] public int currentStackSize;
        
        public ItemScriptableObject GetItemData => itemData;

        public ItemStack(ItemScriptableObject itemData, int currentStackSize)
        {
            this.itemData = itemData;
            this.currentStackSize = currentStackSize < 1 ? 1 : currentStackSize;
        }
    }
}