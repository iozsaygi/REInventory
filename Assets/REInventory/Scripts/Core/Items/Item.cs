using REInventory.Core.Filters;
using System.Collections.Generic;
using UnityEngine;

namespace REInventory.Core.Items
{
    [CreateAssetMenu(fileName = "Item", menuName = "REInventory/Item")]
    internal class Item : ScriptableObject
    {
        #region Inspector
        [Header("Basic Item Properties")]
        [SerializeField] private new string name = string.Empty;
        [SerializeField, Multiline] private string description = string.Empty;
        [SerializeField] private Sprite icon = null;
        [SerializeField] private List<FilterType> actionMapFilterTypes = new List<FilterType>();
        #endregion

        #region Properties
        public string Name => name;
        public string Description => description;
        public Sprite Icon => icon;
        public List<FilterType> FilterTypes => actionMapFilterTypes;
        #endregion
    }
}