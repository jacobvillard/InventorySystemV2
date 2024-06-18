using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Transform playerCam;
    private float tempLength, length, startpos, distance;

    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }
    void FixedUpdate()
    {
        tempLength = (playerCam.position.x * (1 - speed));
        distance = (playerCam.position.x * speed);

        transform.position = new Vector3(startpos + distance, transform.position.y, transform.position.z);
        if (tempLength > startpos + length) startpos += length;
        else if (tempLength < startpos - length) startpos -= length;
    }
}
