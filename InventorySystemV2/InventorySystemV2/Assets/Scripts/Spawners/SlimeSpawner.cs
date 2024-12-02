using Enemy;
using TMPro;
using UnityEngine;

namespace Spawners {
    /// <summary>
    /// This script is used to spawn slimes
    /// </summary>
    public class SlimeSpawner : MonoBehaviour {
        [SerializeField] private GameObject slimePrefab;
        [SerializeField] private GameObject healthInputField;
        [SerializeField] private GameObject damageInputField;
        [SerializeField] private Transform parentObject;
        private int damageInt;
        private string damageString;

        private int healthInt;
        private string healthString;

        private Vector3 spawnPos;


        /// <summary>
        /// This method is used to spawn a slime
        /// </summary>
        public void SpawnSlime() {
            spawnPos = new Vector3(Camera.main.transform.position.x + 7.5f, 1, 0);
            var enemy = Instantiate(slimePrefab, spawnPos, transform.rotation);
            enemy.transform.SetParent(parentObject);
            healthString = healthInputField.GetComponent<TMP_InputField>().text;
            damageString = damageInputField.GetComponent<TMP_InputField>().text;

            int.TryParse(healthString, out healthInt);
            int.TryParse(damageString, out damageInt);
            enemy.GetComponent<Slime>().health = healthInt;
            enemy.GetComponent<Slime>().damage = damageInt;
        }
    }
}