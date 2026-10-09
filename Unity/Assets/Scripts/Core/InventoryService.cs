using System;
using ValleyTapping;

namespace ValleyTapping
{
    public sealed class InventoryService
    {
        private readonly SaveData save;
        public InventoryService(SaveData data)
        {
            if (data == null) throw new ArgumentNullException("data");
            save = data;
            if (save.inventory == null) save.inventory = new System.Collections.Generic.List<InventoryEntry>();
        }

        public int GetQuantity(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return 0;
            foreach (InventoryEntry entry in save.inventory)
                if (entry != null && entry.itemId == itemId) return Math.Max(0, entry.quantity);
            return 0;
        }

        public bool TryAdd(string itemId, int quantity)
        {
            if (string.IsNullOrEmpty(itemId) || quantity <= 0) return false;
            foreach (InventoryEntry entry in save.inventory)
            {
                if (entry == null || entry.itemId != itemId) continue;
                if (entry.quantity > int.MaxValue - quantity) return false;
                entry.quantity += quantity;
                return true;
            }
            save.inventory.Add(new InventoryEntry { itemId = itemId, quantity = quantity });
            return true;
        }

        public bool TryRemove(string itemId, int quantity)
        {
            if (string.IsNullOrEmpty(itemId) || quantity <= 0) return false;
            for (int i = 0; i < save.inventory.Count; i++)
            {
                InventoryEntry entry = save.inventory[i];
                if (entry == null || entry.itemId != itemId) continue;
                if (entry.quantity < quantity) return false;
                entry.quantity -= quantity;
                if (entry.quantity == 0) save.inventory.RemoveAt(i);
                return true;
            }
            return false;
        }
    }
}
