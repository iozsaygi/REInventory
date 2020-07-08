using REInventory.Core.Items;
using UnityEngine;
using UnityEngine.UI;

namespace REInventory.Behaviours.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Image))]
    internal sealed class SlotView : MonoBehaviour
    {
        #region Non-Inspector
        private Image image = null;
        private Color hiddenColor = new Color32(114, 114, 114, 255);
        private Color defaultColor = Color.white;
        private Sprite defaultSprite = null;
        #endregion

        #region Properties
        public Item CurrentDisplayedItem { get; private set; } = null;
        #endregion

        #region Unity Callbacks
        private void Start() => Initialize();
        #endregion

        #region Public Methods
        public void UpdateView(Item item)
        {
            if (item != null)
            {
                image.sprite = item.Icon;
                CurrentDisplayedItem = item;
            }
            else
            {
                image.sprite = defaultSprite;
                CurrentDisplayedItem = null;
            }
        }

        public void Highlight()
        {
            image.color = defaultColor;
        }

        public void Hide()
        {
            image.color = hiddenColor;
        }
        #endregion

        #region Private Methods
        private void Initialize()
        {
            image = GetComponent<Image>();
            defaultSprite = image.sprite;
        }
        #endregion
    }
}