using REInventory.Core.Filters;
using REInventory.Core.Items;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace REInventory.Behaviours.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Image))]
    internal sealed class ActionMap : MonoBehaviour
    {
        #region Inspector
        [Header("Dependencies")]
        [SerializeField] private InventoryInputHandler inventoryInputHandler = null;

        [Header("Properties")]
        [SerializeField] private Vector2 displayOffset = Vector2.zero;
        #endregion

        #region Non-Inspector
        private Image image = null;
        #endregion

        #region Properties
        public List<ActionUI> ActionUIs { get; } = new List<ActionUI>();
        public List<ActionUI> ActiveActionUIs
        {
            get
            {
                List<ActionUI> activeActionUIs = new List<ActionUI>();

                foreach (ActionUI actionUI in ActionUIs)
                {
                    if (actionUI.gameObject.activeSelf)
                        activeActionUIs.Add(actionUI);
                }

                return activeActionUIs;
            }
        }
        #endregion

        #region Unity Callbacks
        private void Start()
        {
            image = GetComponent<Image>();

            // Initialize ActionUI list.
            for (int i = 0; i < transform.childCount; i++)
                ActionUIs.Add(transform.GetChild(i).GetComponent<ActionUI>());
        }
        #endregion

        #region Public Methods
        public void Activate(Item item, Vector2 position)
        {
            Color currentColor = image.color;
            currentColor.a = 255;
            image.color = currentColor;
            image.raycastTarget = true;

            transform.position = position + displayOffset;

            // Filter the action map based on selected item's property.
            Filter.Apply(item, ActionUIs);

            inventoryInputHandler.Assign(ActiveActionUIs[0]);
            inventoryInputHandler.CurrentNavigationMode = InventoryInputHandler.NavigationMode.ActionMap;
        }

        public void Disable()
        {
            Color currentColor = image.color;
            currentColor.a = 0;
            image.color = currentColor;
            image.raycastTarget = false;

            foreach (ActionUI actionUI in ActionUIs)
                actionUI.gameObject.SetActive(false);
        }
        #endregion
    }
}