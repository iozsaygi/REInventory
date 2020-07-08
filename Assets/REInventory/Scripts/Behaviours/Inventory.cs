using REInventory.Core;
using REInventory.Core.Items;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace REInventory.Behaviours
{
    [DisallowMultipleComponent]
    internal sealed class Inventory : MonoBehaviour
    {
        #region Inspector
        [Header("Properties")]
        [SerializeField, Min(1)] private int initialSize = 1;
        [SerializeField, Min(1)] private int capacity = 1;

        [Header("Events")]
        [SerializeField] private OnSlotAddedEvent onSlotAdded = null;
        [SerializeField] private OnItemAddedEvent onItemAdded = null;
        [SerializeField] private OnItemRemovedEvent onItemRemoved = null;
        #endregion

        #region Properties
        public List<Slot> Slots { get; } = new List<Slot>();
        public int EmptySlotCount
        {
            get
            {
                int emptySlotCount = 0;

                foreach (Slot slot in Slots)
                {
                    if (slot.IsEmpty)
                        emptySlotCount++;
                }

                return emptySlotCount;
            }
        }
        #endregion

        #region Unity Callbacks
        private void Awake()
        {
            Debug.Assert(initialSize <= capacity);
            AddSlot(initialSize);
        }
        #endregion

        #region Public Methods
        public void AddSlot(int count)
        {
            Debug.Assert(count > 0);

            // Do not add new slot if there is not enough room for that.
            if (Slots.Count + count > capacity)
                return;

            for (int i = 0; i < count; i++)
            {
                if (Slots.Count > 0)
                {
                    Slots.Add(new Slot(Slots[Slots.Count - 1].ID + 1));
                }
                else
                {
                    Slots.Add(new Slot(i));
                }
            }

            onSlotAdded.Invoke(count);
        }

        public void AddItem(Item item)
        {
            // Check if inventory has enough space to carry given item.
            if (EmptySlotCount > 0)
            {
                Slot firstEmptySlot = GetFirstEmptySlot();
                firstEmptySlot.AssignItem(item);
                onItemAdded.Invoke(item, firstEmptySlot.ID);
            }
        }

        public void AddItem(Slot slot, Item item)
        {
            if (slot.IsEmpty)
            {
                slot.AssignItem(item);
                onItemAdded.Invoke(item, slot.ID);
            }
        }

        public void RemoveItem(Slot slot)
        {
            Debug.Assert(slot != null);
            slot.Clear();
            onItemRemoved.Invoke(slot);
        }
        #endregion

        #region Private Methods
        private Slot GetFirstEmptySlot()
        {
            foreach (Slot slot in Slots)
            {
                if (slot.IsEmpty)
                    return slot;
            }

            return null;
        }
        #endregion

        #region Custom Types
        /// <summary>
        /// Specific event type will be triggered when new slot gets added to the inventory.
        /// The "int" parameter represents the count of added slots.
        /// </summary>
        [System.Serializable]
        private sealed class OnSlotAddedEvent : UnityEvent<int>
        {

        }

        /// <summary>
        /// Specific event type that will be triggered when new item gets added to the inventory.
        /// The "Item" parameter represents the item that gets added to the inventory.
        /// The "int" parameter represents the ID of slot that item added in.
        /// </summary>
        [System.Serializable]
        private sealed class OnItemAddedEvent : UnityEvent<Item, int>
        {

        }
        
        /// <summary>
        /// Specific event type that will be triggered when item gets removed from inventory.
        /// </summary>
        [System.Serializable]
        private sealed class OnItemRemovedEvent : UnityEvent<Slot>
        {

        }
        #endregion
    }
}