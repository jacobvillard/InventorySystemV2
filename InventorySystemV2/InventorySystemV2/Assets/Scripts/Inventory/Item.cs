using UnityEngine;
using UnityEngine.UI;

namespace Inventory {
    /// <summary>
    /// A class that represents an item in the inventory.
    /// </summary>
    public class Item : MonoBehaviour {
        public ItemData itemdata;

        //Item Variables
        public bool rotated = false;
        public int orginalPlacementPosX;
        public int orginalPlacementPosY;

        /// <summary>
        /// Initializes the item with the given item data.
        /// </summary>
        /// <param name="itemdata"></param>
        internal void Set(ItemData itemdata) {
            this.itemdata = itemdata;
            GetComponent<Image>().sprite = itemdata.img;

            var size = new Vector2 {
                x = itemdata.height * GridInv.TileSizeHeight,
                y = itemdata.width * GridInv.TileSizeWidth
            };
            GetComponent<RectTransform>().sizeDelta = size;
            transform.localScale = new Vector3(1f, 1f, 1f);

        }

    }
}
