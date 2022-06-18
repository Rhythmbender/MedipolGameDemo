using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed;
    public Transform player;
    public float minX, maxX;
    public float minY, maxY;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 nextPos = new Vector3(Mathf.Clamp(player.position.x, minX, maxX), Mathf.Clamp(player.position.y + 0.5f, minY, maxY), transform.position.z);
        transform.position = Vector3.Lerp(transform.position, nextPos, speed * Time.deltaTime);
    }
}
