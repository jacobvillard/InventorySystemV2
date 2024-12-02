using Generics;
using UnityEngine;
using UnityEngine.Serialization;

namespace Inventory {
    /// <summary>
    /// Class that handles the chest inventory.
    /// </summary>
    public class Chest : Singleton<Chest> {
        [FormerlySerializedAs("INVGRIDS")] public GameObject invgrids;
        [FormerlySerializedAs("ChestUI")] public GameObject chestUI;
        [FormerlySerializedAs("InvUI")] public GameObject invUI;
        [FormerlySerializedAs("GridInvPrefab")] [SerializeField] private GameObject gridInvPrefab;
        [FormerlySerializedAs("InvController")] public GameObject invController;
        public int numberOfItemsToSpawn = 10;
        private bool chestGenerated;
        private GameObject chestInv;
        private Item item;

        /// <summary>
        /// Spawns the chest inventory.
        /// </summary>
        public void SpawnChest() {
            chestUI.SetActive(true); //enables gameobject for parenting
            var spawnPos = new Vector3(Camera.main.transform.position.x - 6f, 1, 0);

            if (!chestGenerated) //Only instiate if an inventory hasnt been generated
            {
                //isntanstaies a chest inventory
                chestInv = Instantiate(gridInvPrefab, spawnPos, transform.rotation);
                chestInv.transform.SetParent(chestUI.transform);
                chestInv.transform.localScale = new Vector3(1, 1, 1);
                chestInv.transform.position = new Vector3(1405.4473876953125f, 971.8651123046875f, 19.33611297607422f);
            }

            chestUI.SetActive(false);
        }

        /// <summary>
        /// Method that handles the chest click event.
        /// </summary>
        public void ClickChest() //Sets ui elements active
        {
            invUI.SetActive(true);
            chestUI.SetActive(true);
            chestInv.SetActive(true);
            if (!chestGenerated)
                Invoke("AddItems",
                    0.5f); //invokes function as items require parenting to an object that needs to be activated
        }

        /// <summary>
        /// Adds items to the chest inventory.
        /// </summary>
        private void AddItems() {
            var fails = 0;
            for (var n = 0; n < numberOfItemsToSpawn; n++) //Will loop for number of items to spawm
            {
                item = invController.GetComponent<InventoryController>().CreateRandomItem(); //generates a random item 
                item.transform.localScale = new Vector3(1.9f, 1.9f, 1.9f);
                for (var x = 0;
                     x < gridInvPrefab.GetComponent<GridInv>().width;
                     x++) //loops for the size of the inventory
                for (var y = 0; y < gridInvPrefab.GetComponent<GridInv>().height; y++)
                    if (chestInv.GetComponent<GridInv>()
                        .AcceptablePlacementCeck(item, y, x)) //if the item can be placed place it
                    {
                        chestInv.GetComponent<GridInv>().AddItem(item, y, x);
                        x = gridInvPrefab.GetComponent<GridInv>()
                            .width; //exits the for loop so the item isnt placed on every tile
                        y = gridInvPrefab.GetComponent<GridInv>().height;
                    }
                    else {
                        if (x == gridInvPrefab.GetComponent<GridInv>().width &&
                            y == gridInvPrefab.GetComponent<GridInv>()
                                .height) //if iem cant be placed on the the last tile log a fail
                            fails++;
                        if (fails == 30) //if 30 fails exit the placement loop
                            n = numberOfItemsToSpawn;
                    }
            }

            chestGenerated = true;
        }

        /// <summary>
        /// Destroys the chest inventory.
        /// </summary>
        public void DestroyChest() {
            Destroy(chestInv); //destroy the invetory space
        }
    }
}