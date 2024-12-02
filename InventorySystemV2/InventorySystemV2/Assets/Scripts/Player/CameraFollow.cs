using UnityEngine;

namespace Player {
    /// <summary>
    ///  This script is used to make the camera follow the player
    /// </summary>
    public class CameraFollow : MonoBehaviour {
        [SerializeField] private Transform player;


        // Update is called once per frame
        private void Update() {
            transform.position = new Vector3(player.position.x+5f, 0, -10);
        }
    }
}
