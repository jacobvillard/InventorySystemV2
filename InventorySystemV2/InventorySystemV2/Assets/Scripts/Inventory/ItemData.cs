using UnityEngine;

namespace Inventory {
    /// <summary>
    /// This script holds the data for an item
    /// </summary>
    [CreateAssetMenu(menuName = "ItemData")]
    public class ItemData : ScriptableObject {
        public string id = "";  // Unique identifier for the item
        public string desc = "";// Description of the item
        public Sprite img;      // Image of the item
        public int width = 1;   // Width of the item
        public int height = 1;  // Height of the item
        public string type = "";// Type of the item
        
    }
}
