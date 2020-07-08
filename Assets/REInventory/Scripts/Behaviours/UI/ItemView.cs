using REInventory.Core.Items;
using TMPro;
using UnityEngine;

namespace REInventory.Behaviours.UI
{
    [DisallowMultipleComponent]
    internal sealed class ItemView : MonoBehaviour
    {
        #region Inspector
        [Header("Dependencies")]
        [SerializeField] private InventoryUI inventoryUI = null;
        [SerializeField] private InventoryInputHandler inventoryInputHandler = null;
        [SerializeField] private TextMeshProUGUI itemNameView = null;
        [SerializeField] private GameObject line = null;
        [SerializeField] private TextMeshProUGUI itemDescriptionView = null;
        #endregion

        #region Public Methods
        public void UpdateView(Item item)
        {
            Refresh(item);
        }

        public void UpdateView(Item item, int slotID)
        {
            if (inventoryUI.SlotViews.IndexOf(inventoryInputHandler.CurrentPointingSlotView) == slotID)
                Refresh(item);
        }
        #endregion

        #region Private Methods
        private void Refresh(Item item)
        {
            if (item == null)
            {
                itemNameView.text = string.Empty;
                line.SetActive(false);
                itemDescriptionView.text = string.Empty;
            }
            else
            {
                itemNameView.text = item.Name;
                line.SetActive(true);
                itemDescriptionView.text = item.Description;
            }
        }
        #endregion
    }
}