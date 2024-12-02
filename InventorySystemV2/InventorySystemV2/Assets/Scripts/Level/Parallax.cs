using UnityEngine;

namespace Level {
    /// <summary>
    /// Parallax scrolling effect for the background
    /// </summary>
    public class Parallax : MonoBehaviour {
        [SerializeField] private float speed;
        [SerializeField] private Transform playerCam;
        private float tempLength, length, startpos, distance;

        private void Start() {
            startpos = transform.position.x;
            length = GetComponent<SpriteRenderer>().bounds.size.x;
        }

        private void FixedUpdate() {
            tempLength = playerCam.position.x * (1 - speed);
            distance = playerCam.position.x * speed;

            transform.position = new Vector3(startpos + distance, transform.position.y, transform.position.z);
            if (tempLength > startpos + length) startpos += length;
            else if (tempLength < startpos - length) startpos -= length;
        }
    }
}