using REInventory.Core;
using REInventory.Core.Filters;
using REInventory.Core.Items;
using UnityEngine;
using UnityEngine.Events;
using static REInventory.Core.Items.Combinable;

namespace REInventory.Behaviours.UI.InventoryActions
{
    internal sealed class Combine : InventoryAction
    {
        #region Inspector
        [Header("Dependencies")]
        [SerializeField] private Inventory inventory = null;
        [SerializeField] private InventoryInputHandler inventoryInputHandler = null;

        [Header("Events")]
        [SerializeField] private UnityEvent onCombineFinished = null;
        #endregion

        #region Non-Inspector
        private Slot baseSlot = null;
        #endregion

        #region Implemented Methods
        public override void Execute(Inventory inventory, Slot slot)
        {
            // Hold the reference to slot.
            baseSlot = slot;

            // Filter the inventory.
            Filter.Apply(inventory, InventoryUI, (Combinable)slot.Item);

            // Enter the "Combine" navigation mode.
            inventoryInputHandler.CurrentNavigationMode = InventoryInputHandler.NavigationMode.Combine;
        }
        #endregion

        #region Public Methods
        public void Generate(Slot slot)
        {
            Combinable baseCombinable = (Combinable)baseSlot.Item;

            if (baseCombinable != slot.Item)
            {
                foreach (CombinePair combinePair in baseCombinable.CombinePairs)
                {
                    if (combinePair.RequiredItem == slot.Item)
                    {
                        inventory.RemoveItem(slot);
                        inventory.RemoveItem(baseSlot);
                        inventory.AddItem(slot, combinePair.Result);
                        break;
                    }
                }
            }

            onCombineFinished.Invoke();
        }
        #endregion
    }
}