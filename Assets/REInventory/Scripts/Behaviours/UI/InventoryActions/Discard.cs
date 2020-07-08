using REInventory.Core;

namespace REInventory.Behaviours.UI.InventoryActions
{
    internal sealed class Discard : InventoryAction
    {
        #region Implemented Methods
        public override void Execute(Inventory inventory, Slot slot)
        {
            inventory.RemoveItem(slot);
        }
        #endregion
    }
}