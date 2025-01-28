// Created by Linus Jernström
using UnityEngine;
using UnityEngine.Serialization;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Item")]
    public class SO_Item : ScriptableObject
    {
        public Texture2D itemIcon; // sprite representing the item
        public string itemName; // the item's name
        public bool isStackable; // does the item stack?
        [FormerlySerializedAs("itemType")] public SC_ItemType _scItemType; // in case we want to categorize the items
        public bool canBeForSale; // can the item be found for sale
        public Vector2Int priceRange; // the lower and upper bounds for prices, if the item is for sale
    }
}
