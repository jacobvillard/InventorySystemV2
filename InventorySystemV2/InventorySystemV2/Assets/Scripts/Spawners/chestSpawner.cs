using Inventory;
using UnityEngine;
using UnityEngine.Serialization;

namespace Spawners {
    /// <summary>
    ///     This script is used to spawn a chest
    /// </summary>
    public class chestSpawner : MonoBehaviour {
        [SerializeField] private GameObject parent;
        [SerializeField] private GameObject chestPrefab;

        [FormerlySerializedAs("INVGRIDS")] [SerializeField]
        private GameObject invgrids;

        [SerializeField] private GameObject ChestUI;
        [SerializeField] private GameObject InvUI;
        [SerializeField] private GameObject InvController;

        private GameObject chest;

        /// <summary>
        /// Spawns a chest
        /// </summary>
        public void SpawnChest() {
            var spawnPos = new Vector3(Camera.main.transform.position.x - 2.43f, -3.25f, 0.17f);
            if (chest != null) //Deletes previous chest
            {
                chest.GetComponent<Chest>().DestroyChest();
                Destroy(chest);
            }

            chest = Instantiate(chestPrefab, spawnPos,
                transform.rotation); //Instanties a chest and sets all the serialzed references stored here
            chest.transform.SetParent(parent.transform);
            chest.GetComponent<Chest>().chestUI = ChestUI;
            chest.GetComponent<Chest>().invgrids = invgrids;
            chest.GetComponent<Chest>().invController = InvController;
            chest.GetComponent<Chest>().invUI = InvUI;
            chest.GetComponent<Chest>().SpawnChest();
        }
    }
}