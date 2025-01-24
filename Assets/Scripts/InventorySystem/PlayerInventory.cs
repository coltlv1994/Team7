// Created by Linus Jernström
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    public class PlayerInventory : MonoBehaviour
    {
        private List<ItemStack> _heldItems;
        
        public List<ItemStack> GetInventory => _heldItems;
        private ItemStack FindMatchingStack(ItemStack itemStack) => _heldItems.Find(i => i.itemData == itemStack.itemData);
        
        public void AddItem(ItemStack itemStack)
        {
            if (_heldItems.Contains(itemStack) && itemStack.itemData.isStackable)
                FindMatchingStack(itemStack).currentStackSize += itemStack.currentStackSize;
            
            else
                _heldItems.Add(itemStack);
        }
        
        public void RemoveSingleItem(ItemStack itemStack)
        {
            ItemStack stack = FindMatchingStack(itemStack);
            
            stack.currentStackSize --;
            
            if (stack.currentStackSize < 1)
                _heldItems.Remove(stack);
        }
        
        public void RemoveItems(ItemStack itemStack, int nrItemsToRemove = 1)
        {
            ItemStack stack = FindMatchingStack(itemStack);
            
            stack.currentStackSize -= nrItemsToRemove;
            
            if (stack.currentStackSize < 1)
                _heldItems.Remove(stack);
        }

        public int GetTotalItemsInInventory()
        {
            int inventorySize = 0;
            foreach (var itemStack in _heldItems)
                inventorySize += itemStack.currentStackSize;
            return inventorySize;
        }
    }
}
