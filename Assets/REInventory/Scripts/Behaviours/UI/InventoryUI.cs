using REInventory.Core;
using REInventory.Core.Items;
using System.Collections.Generic;
using UnityEngine;

namespace REInventory.Behaviours.UI
{
    [DisallowMultipleComponent]
    internal sealed class InventoryUI : MonoBehaviour
    {
        #region Inspector
        [Header("Dependencies")]
        [SerializeField] private Inventory inventory = null;
        [SerializeField] private Transform slotViewParent = null;
        [SerializeField] private GameObject slotViewPrefab = null;
        #endregion

        #region Properties
        public List<SlotView> SlotViews { get; } = new List<SlotView>();
        #endregion

        #region Public Methods
        /// <summary>
        /// This function is subscriber to "OnSlotAdded" event of "Inventory" class.
        /// Generates the missing slot view(s) when new slot(s) gets added to the inventory.
        /// </summary>
        /// <param name="addedSlotCount"></param>
        public void GenerateMissingSlotViews(int addedSlotCount)
        {
            for (int i = 0; i < addedSlotCount; i++)
            {
                GameObject slotView = Instantiate(slotViewPrefab);
                slotView.transform.SetParent(slotViewParent, false);
                SlotViews.Add(slotView.GetComponent<SlotView>());
            }
        }

        /// <summary>
        /// Updates the view of the slot UI with given item and slot ID.
        /// </summary>
        /// <param name="item">Item to view.</param>
        /// <param name="slotID">Slot to view given item.</param>
        public void UpdateSlotView(Item item, int slotID)
        {
            SlotViews[slotID].UpdateView(item);
        }

        public void UpdateSlotView(Slot slot)
        {
            SlotViews[slot.ID].UpdateView(slot.Item);
        }

        public void HighlightSlotViews()
        {
            foreach (SlotView slotView in SlotViews)
                slotView.Highlight();
        }
        #endregion
    }
}