using REInventory.Core.Items;
using UnityEngine;

namespace REInventory.Core
{
    [System.Serializable]
    internal sealed class Slot
    {
        #region Inspector
        [SerializeField] private int id = 0;
        [SerializeField] private Item item = null;
        #endregion

        #region Properties
        public int ID => id;
        public Item Item => item;
        public bool IsEmpty => item == null;
        #endregion

        #region Callbacks
        public Slot(int id)
        {
            this.id = id;
        }
        #endregion

        #region Public Methods
        public void AssignItem(Item newItem)
        {
            if (item == null)
                item = newItem;
        }

        public void Clear()
        {
            item = null;
        }
        #endregion
    }
}