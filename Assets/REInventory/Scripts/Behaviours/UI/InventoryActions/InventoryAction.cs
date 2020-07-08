using REInventory.Core;
using UnityEngine;

namespace REInventory.Behaviours.UI.InventoryActions
{
    [DisallowMultipleComponent]
    internal abstract class InventoryAction : MonoBehaviour
    {
        #region Dependencies
        [SerializeField] private InventoryUI inventoryUI = null;
        #endregion

        #region Properties
        public InventoryUI InventoryUI => inventoryUI;
        #endregion

        #region Public Methods
        public abstract void Execute(Inventory inventory, Slot slot);
        #endregion
    }
}