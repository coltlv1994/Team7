// Created by Linus Jernström
using UnityEngine;
using UnityEngine.Serialization;

namespace InventorySystem
{
    [System.Serializable]
    public class CS_ItemStack
    {
        // All items are kept in stacks. This includes single items (which are just in a stack of size 1), and unstackables (which are always stack size 1).
        
        [FormerlySerializedAs("itemData")] [SerializeField] private SO_Item _soItemData;
        [Range(1, 64)] public int currentStackSize;
        
        public SO_Item GetSoItemData => _soItemData;

        public CS_ItemStack(SO_Item soItemData, int currentStackSize)
        {
            this._soItemData = soItemData;
            this.currentStackSize = currentStackSize < 1 ? 1 : currentStackSize;
        }
    }
}