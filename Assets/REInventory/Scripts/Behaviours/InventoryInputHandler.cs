using REInventory.Behaviours.UI;
using REInventory.Core;
using REInventory.Core.Items;
using UnityEngine;
using UnityEngine.Events;

namespace REInventory.Behaviours
{
    [DisallowMultipleComponent]
    internal sealed class InventoryInputHandler : MonoBehaviour
    {
        #region Inspector
        [Header("Dependencies")]
        [SerializeField] private Inventory inventory = null;
        [SerializeField] private InventoryUI inventoryUI = null;
        [SerializeField] private ActionMap actionMap = null;
        [SerializeField] private Transform slotViewParent = null;
        [SerializeField] private GameObject inputViewPrefab = null;

        [Header("Events")]
        [SerializeField] private OnAssignedEvent onAssigned = null;
        [SerializeField] private OnItemSelectedEvent onItemSelected = null;
        [SerializeField] private UnityEvent onCancelButtonDown = null;
        [SerializeField] private UnityEvent onCombineCanceled = null;
        [SerializeField] private OnCombineAttemptEvent onCombineAttempt = null;
        #endregion

        #region Non-Inspector
        private GameObject inputView = null;
        private bool isProcessingInput = false;
        private ActionUI currentPointingActionUI = null;
        #endregion

        #region Properties
        public SlotView CurrentPointingSlotView { get; private set; } = null;
        public NavigationMode CurrentNavigationMode { get; set; } = NavigationMode.Inventory;
        #endregion

        #region Unity Callbacks
        private void Start() => InitializeInputView();
        private void Update() => ProcessInput();
        #endregion

        #region Public Methods
        public void Assign(ActionUI actionUI)
        {
            Debug.Assert(actionUI != null);
            currentPointingActionUI = actionUI;
            inputView.transform.SetParent(actionUI.transform, false);
        }

        /// <summary>
        /// This function will be called from "OnActionExecuted" event of every ActionUI instance.
        /// </summary>
        public void AssignToLastSlotView()
        {
            Assign(CurrentPointingSlotView.transform);
        }

        public void EnableInventoryNavigation()
        {
            CurrentNavigationMode = NavigationMode.Inventory;
        }
        #endregion

        #region Private Methods
        private void InitializeInputView()
        {
            if (inputView == null)
                inputView = Instantiate(inputViewPrefab);

            Assign(slotViewParent.GetChild(0));
        }

        private void Assign(Transform parent)
        {
            Debug.Assert(parent != null);
            inputView.transform.SetParent(parent, false);
            CurrentPointingSlotView = parent.GetComponent<SlotView>();
            onAssigned.Invoke(CurrentPointingSlotView.CurrentDisplayedItem);
        }

        private bool IsOnFirstSlotView()
        {
            return inventoryUI.SlotViews.IndexOf(CurrentPointingSlotView) ==
                inventoryUI.SlotViews.IndexOf(inventoryUI.SlotViews[0]);
        }

        private bool IsOnLastSlotView()
        {
            return inventoryUI.SlotViews.IndexOf(CurrentPointingSlotView) ==
                            inventoryUI.SlotViews.IndexOf(inventoryUI.SlotViews[inventoryUI.SlotViews.Count - 1]);
        }

        private SlotView GetNextSlotView()
        {
            return inventoryUI.SlotViews[inventoryUI.SlotViews.IndexOf(CurrentPointingSlotView) + 1];
        }

        private SlotView GetPreviousSlotView()
        {
            return inventoryUI.SlotViews[inventoryUI.SlotViews.IndexOf(CurrentPointingSlotView) - 1];
        }

        private void ProcessInput()
        {
            if (!isProcessingInput)
            {
                float horizontalAxis = Input.GetAxisRaw("Horizontal");
                float verticalAxis = Input.GetAxisRaw("Vertical");

                switch (horizontalAxis)
                {
                    case 1.0f:
                        if (CurrentNavigationMode == NavigationMode.Inventory || CurrentNavigationMode == NavigationMode.Combine)
                        {
                            if (IsOnLastSlotView())
                            {
                                Assign(slotViewParent.GetChild(0));
                            }
                            else
                            {
                                Assign(GetNextSlotView().transform);
                            }
                        }
                        break;

                    case -1.0f:
                        if (CurrentNavigationMode == NavigationMode.Inventory || CurrentNavigationMode == NavigationMode.Combine)
                        {
                            if (IsOnFirstSlotView())
                            {
                                Assign(slotViewParent.GetChild(slotViewParent.childCount - 1));
                            }
                            else
                            {
                                Assign(GetPreviousSlotView().transform);
                            }
                        }
                        break;
                }

                switch (verticalAxis)
                {
                    case 1.0f:
                        if (CurrentNavigationMode == NavigationMode.Inventory || CurrentNavigationMode == NavigationMode.Combine)
                        {
                            int upTargetID = inventoryUI.SlotViews.IndexOf(CurrentPointingSlotView) - 4;

                            if (upTargetID >= 0)
                            {
                                Assign(slotViewParent.GetChild(upTargetID));
                            }
                        }
                        else
                        {
                            if (currentPointingActionUI == actionMap.ActiveActionUIs[0])
                            {
                                Assign(actionMap.ActiveActionUIs[actionMap.ActiveActionUIs.Count - 1]);
                            }
                            else
                            {
                                Assign(actionMap.ActiveActionUIs[actionMap.ActiveActionUIs.IndexOf(currentPointingActionUI) - 1]);
                            }
                        }
                        break;

                    case -1.0f:
                        if (CurrentNavigationMode == NavigationMode.Inventory || CurrentNavigationMode == NavigationMode.Combine)
                        {
                            int downTargetID = inventoryUI.SlotViews.IndexOf(CurrentPointingSlotView) + 4;

                            if (downTargetID < inventoryUI.SlotViews.Count)
                            {
                                Assign(slotViewParent.GetChild(downTargetID));
                            }
                        }
                        else
                        {
                            if (currentPointingActionUI == actionMap.ActiveActionUIs[actionMap.ActiveActionUIs.Count - 1])
                            {
                                Assign(actionMap.ActiveActionUIs[0]);
                            }
                            else
                            {
                                Assign(actionMap.ActiveActionUIs[actionMap.ActiveActionUIs.IndexOf(currentPointingActionUI) + 1]);
                            }
                        }
                        break;
                }

                isProcessingInput = true;
            }

            if (Input.GetAxisRaw("Horizontal") == 0.0f && Input.GetAxisRaw("Vertical") == 0.0f)
                isProcessingInput = false;

            if (Input.GetButtonDown("Fire1") && CurrentPointingSlotView.CurrentDisplayedItem != null)
            {
                switch (CurrentNavigationMode)
                {
                    case NavigationMode.Inventory:
                        onItemSelected.Invoke(CurrentPointingSlotView.CurrentDisplayedItem, inputView.transform.position);
                        break;

                    case NavigationMode.ActionMap:
                        currentPointingActionUI.Trigger(inventory.Slots[inventoryUI.SlotViews.IndexOf(CurrentPointingSlotView)]);
                        break;

                    case NavigationMode.Combine:
                        onCombineAttempt.Invoke(inventory.Slots[inventoryUI.SlotViews.IndexOf(CurrentPointingSlotView)]);
                        break;
                }
            }

            if (Input.GetButtonDown("Fire2"))
            {
                switch (CurrentNavigationMode)
                {
                    case NavigationMode.Combine:
                        CurrentNavigationMode = NavigationMode.Inventory;
                        onCombineCanceled.Invoke();
                        break;

                    case NavigationMode.ActionMap:
                        Assign(CurrentPointingSlotView.transform);
                        CurrentNavigationMode = NavigationMode.Inventory;
                        onCancelButtonDown.Invoke();
                        break;
                }
            }
        }
        #endregion

        #region Custom Types
        public enum NavigationMode
        {
            Inventory,
            ActionMap,
            Combine
        }

        /// <summary>
        /// Specific event type that will triggered when input view gets assigned to the new slot view.
        /// The "Item" parameter specifies the item that slot view is displaying.
        /// </summary>
        [System.Serializable]
        private sealed class OnAssignedEvent : UnityEvent<Item>
        {

        }

        [System.Serializable]
        private sealed class OnItemSelectedEvent : UnityEvent<Item, Vector2>
        {

        }

        [System.Serializable]
        private sealed class OnCombineAttemptEvent : UnityEvent<Slot>
        {

        }
        #endregion
    }
}