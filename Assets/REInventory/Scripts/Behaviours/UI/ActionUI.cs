using REInventory.Behaviours.UI.InventoryActions;
using REInventory.Core;
using REInventory.Core.Filters;
using UnityEngine;
using UnityEngine.Events;

namespace REInventory.Behaviours.UI
{
    [DisallowMultipleComponent]
    internal sealed class ActionUI : MonoBehaviour
    {
        #region Inspector
        [Header("Dependencies")]
        [SerializeField] private Inventory inventory = null;

        [Header("Properties")]
        [SerializeField] private FilterType filterType = FilterType.None;

        [Header("Events")]
        [SerializeField] private UnityEvent onActionExecuted = null;
        #endregion

        #region Properties
        public FilterType FilterType => filterType;
        #endregion

        #region Public Methods
        public void Trigger(Slot slot)
        {
            GetComponent<InventoryAction>().Execute(inventory, slot);
            onActionExecuted.Invoke();
        }
        #endregion
    }
}