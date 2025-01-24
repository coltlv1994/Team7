// Created by Linus Jernström

namespace InventorySystem
{
    public class ItemStack
    {
        public string itemStackName { get; private set; }
        public ItemScriptableObject itemData { get; private set; }
        public int currentStackSize;

        public ItemStack(ItemScriptableObject itemData, int currentStackSize)
        {
            this.itemData = itemData;
            this.currentStackSize = currentStackSize < 1 ? 1 : currentStackSize;
            
            itemStackName = itemData.isStackable ? "Stack of " + itemData.itemName + "(s)" : "Single " + itemData.itemName;
        }
    }
}