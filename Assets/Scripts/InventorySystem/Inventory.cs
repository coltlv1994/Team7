// Created by Linus Jernström
using System.Collections.Generic;
using UnityEngine;
// ReSharper disable ConditionIsAlwaysTrueOrFalse

namespace InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        private List<ItemStack> _inventoryItems;
        
        public List<ItemStack> GetInventoryItems => _inventoryItems;
        public void ClearInventory() => _inventoryItems = new List<ItemStack>();
        public bool IsItemInInventory(ItemStack itemStack) => FindMatchingStack(itemStack) != null;
        public ItemStack GetMatchingItemStack(ItemStack itemStack) => FindMatchingStack(itemStack);
        private ItemStack FindMatchingStack(ItemStack itemStack) => _inventoryItems.Find(i => i.GetItemData == itemStack.GetItemData);
        
        
        public void AddItemStack(ItemStack itemStack)
        {
            if (_inventoryItems.Contains(itemStack) && itemStack.GetItemData.isStackable)
                FindMatchingStack(itemStack).currentStackSize += itemStack.currentStackSize;
            
            else
                _inventoryItems.Add(itemStack);
        }
        
        public void RemoveSingleItemFromStack(ItemStack itemStack)
        {
            ItemStack stack = FindMatchingStack(itemStack);
            
            stack.currentStackSize --;
            
            if (stack.currentStackSize < 1)
                _inventoryItems.Remove(stack);
        }
        
        public void RemoveItemsFromStack(ItemStack itemStack, int nrItemsToRemove = 1)
        {
            ItemStack stack = FindMatchingStack(itemStack);
            
            stack.currentStackSize -= nrItemsToRemove;
            
            if (stack.currentStackSize < 1)
                _inventoryItems.Remove(stack);
        }

        public int GetTotalItemsInInventory()
        {
            int inventorySize = 0;
            foreach (var itemStack in _inventoryItems)
                inventorySize += itemStack.currentStackSize;
            return inventorySize;
        }
        
        #region Spawning
        [SerializeField] private List<ItemStack> _spawnItems = new List<ItemStack>();

        private void Start()
        {
            //Return if there is a save file
            ClearInventory();
            _inventoryItems.AddRange(_spawnItems);
        }
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            foreach (ItemStack stack in _spawnItems)
            {
                ValidateStackSize(stack);
            }
        }
#endif

        private void ValidateStackSize(ItemStack stack)
        {
            if (stack.GetItemData == null)
                return;
            if (stack.GetItemData.isStackable || stack.currentStackSize <= 1)
                return;
            
            Debug.LogWarning($"Non-stackable item \"{stack.GetItemData.itemName}\" can only have stack size of 1.");
            stack.currentStackSize = 1;
        }
        #endregion
    }
}
