// Created by Linus Jernström
using UnityEngine;

namespace InventorySystem
{

    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Item")]
    public class ItemScriptableObject : ScriptableObject
    {
        public Texture2D itemIcon; // sprite representing the item
        public string itemName; // the item's name
        public bool isStackable; // does the item stack?
        public ItemType itemType; // in case we want to categorize the items
        public bool canBeForSale; // can the item be found for sale
        public int[] priceRange; // the lower and upper bounds for prices, if the item is for sale
    }
}
