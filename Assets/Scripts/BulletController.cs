using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    // Mermi h�zlar�n� de�i�tirmek i�in Unity Oyun Motoru'na speed de�i�keni ekledik.

    public Vector2 speed;
    private Rigidbody2D rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rigid.velocity = speed;
    }

    //  E�er temas eden nesnenin (merminin) etiketi Enemy ise d��man�n �lmesi laz�m.
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            GameController.instance.BulletHit(other.gameObject);
            Destroy(gameObject);
        }

        else if (!other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
