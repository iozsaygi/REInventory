using REInventory.Core.Items;
using System.Collections.Generic;
using UnityEngine;

namespace REInventory.Behaviours
{
    [DisallowMultipleComponent]
    internal sealed class ItemDatabase : MonoBehaviour
    {
        #region Inspector
        [Header("Dependencies")]
        [SerializeField] private Inventory inventory = null;

        [Header("Data")]
        [SerializeField] private List<Item> availableItems = new List<Item>();
        #endregion

        #region Public Methods
        public void AddRandomItem()
        {
            inventory.AddItem(availableItems[Random.Range(0, availableItems.Count)]);
        }
        #endregion
    }
}