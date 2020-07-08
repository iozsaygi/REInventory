using System.Collections.Generic;
using UnityEngine;

namespace REInventory.Core.Items
{
    [CreateAssetMenu(fileName = "Combinable Item", menuName = "REInventory/Combinable Item")]
    internal sealed class Combinable : Item
    {
        #region Inspector
        [Header("Combinable Item Properties")]
        [SerializeField] private List<CombinePair> combinePairs = new List<CombinePair>();
        #endregion

        #region Properties
        public List<CombinePair> CombinePairs => combinePairs;
        #endregion

        #region Custom Types
        [System.Serializable]
        public struct CombinePair
        {
            public Item RequiredItem;
            public Item Result;
        }
        #endregion
    }
}