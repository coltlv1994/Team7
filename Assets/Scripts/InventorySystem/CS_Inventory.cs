// Created by Linus Jernström
using System.Collections.Generic;
using UnityEngine;
// ReSharper disable ConditionIsAlwaysTrueOrFalse

namespace InventorySystem
{
    public class CS_Inventory : MonoBehaviour
    {
        private List<CS_ItemStack> _inventoryItems;
        
        public List<CS_ItemStack> GetInventoryItems => _inventoryItems;
        public void ClearInventory() => _inventoryItems = new List<CS_ItemStack>();
        public bool IsItemInInventory(CS_ItemStack csItemStack) => FindMatchingStack(csItemStack) != null;
        public CS_ItemStack GetMatchingItemStack(CS_ItemStack csItemStack) => FindMatchingStack(csItemStack);
        private CS_ItemStack FindMatchingStack(CS_ItemStack csItemStack) => _inventoryItems.Find(i => i.GetSoItemData == csItemStack.GetSoItemData);
        
        
        public void AddItemStack(CS_ItemStack csItemStack)
        {
            if (_inventoryItems.Contains(csItemStack) && csItemStack.GetSoItemData.isStackable)
                FindMatchingStack(csItemStack).currentStackSize += csItemStack.currentStackSize;
            
            else
                _inventoryItems.Add(csItemStack);
        }
        
        public void RemoveSingleItemFromStack(CS_ItemStack csItemStack)
        {
            CS_ItemStack stack = FindMatchingStack(csItemStack);
            
            stack.currentStackSize --;
            
            if (stack.currentStackSize < 1)
                _inventoryItems.Remove(stack);
        }
        
        public void RemoveItemsFromStack(CS_ItemStack csItemStack, int nrItemsToRemove = 1)
        {
            CS_ItemStack stack = FindMatchingStack(csItemStack);
            
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
        [SerializeField] private List<CS_ItemStack> _spawnItems = new List<CS_ItemStack>();

        private void Start()
        {
            //Return if there is a save file
            ClearInventory();
            _inventoryItems.AddRange(_spawnItems);
        }
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            foreach (CS_ItemStack stack in _spawnItems)
            {
                ValidateStackSize(stack);
            }
        }
#endif

        private void ValidateStackSize(CS_ItemStack stack)
        {
            if (stack.GetSoItemData == null)
                return;
            if (stack.GetSoItemData.isStackable || stack.currentStackSize <= 1)
                return;
            
            Debug.LogWarning($"Non-stackable item \"{stack.GetSoItemData.itemName}\" can only have stack size of 1.");
            stack.currentStackSize = 1;
        }
        #endregion
    }
}
