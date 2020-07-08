using REInventory.Behaviours;
using REInventory.Behaviours.UI;
using REInventory.Core.Items;
using System.Collections.Generic;
using static REInventory.Core.Items.Combinable;

namespace REInventory.Core.Filters
{
    internal enum FilterType
    {
        None,
        Use,
        Combine,
        Discard,
    }

    internal static class Filter
    {
        public static void Apply(Item item, List<ActionUI> actionUIs)
        {
            foreach (FilterType itemFilterType in item.FilterTypes)
            {
                foreach (ActionUI actionUI in actionUIs)
                {
                    if (actionUI.FilterType == itemFilterType)
                        actionUI.gameObject.SetActive(true);
                }
            }
        }

        public static void Apply(Inventory inventory, InventoryUI inventoryUI, Combinable combinable)
        {
            List<string> requiredItemNames = new List<string>();

            for (int i = 0; i < combinable.CombinePairs.Count; i++)
                requiredItemNames.Add(combinable.CombinePairs[i].RequiredItem.Name);

            foreach (Slot slot in inventory.Slots)
            {
                if (!slot.IsEmpty)
                {
                    if (!requiredItemNames.Contains(slot.Item.Name) || slot.Item == combinable)
                        inventoryUI.SlotViews[slot.ID].Hide();
                }
            }
        }
    }
}