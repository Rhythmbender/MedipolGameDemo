using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public float speed;

    // RigidBody 2D'ye eri�mem laz�m. (Hareket kontrol� i�in)
    private Rigidbody2D rigid;

    // Sprite Renderer'a eri�mem laz�m (Karakterin Sa�a Sola bak�nca oraya bakmas� i�in)
    private SpriteRenderer sprite;

    private void Start()
    {
        // Inspector k�sm�ndaki Rigidbody2D'ye referans olu�turdum.
        rigid = GetComponent<Rigidbody2D>();

        // Inspector k�sm�ndaki SpriteRenderer'a referans olu�turdum.
        sprite = GetComponent<SpriteRenderer>();

        FindDirection();
    }


    private void Update()
    {
        // Burada d��man�m�z� hareket ettiren fonksiyonu �a��rd�k.
        Move(); 
    }

    private void Move()
    {
        // d��man�m�z� hareket ettiriyoruz.
        // rigid.velocity = new Vector2(speed, 0f);

        Vector2 temp = rigid.velocity;
        temp.x = speed;
        rigid.velocity = temp;
    }

    private void FindDirection()
    {
        // H�z�m s�f�rdan k���kse sola git.
        if (speed < 0)
        {
            // bunun i�in Flip X �zelli�inin aktif olmas� laz�m.
            sprite.flipX = true;
        }
        // H�z�m s�f�rdan b�y�kse sa�a git.
        else if (speed > 0)
        {
            sprite.flipX = false;
        }
    }

    // D��man� 2 tile aras�nda gidip gelmesini sa�layaca��m.
    // Tile'a �arpma i�lemi i�in OnCollisionEnter2D metodu �a�r�l�yordu.
    private void OnCollisionEnter2D(Collision2D other)
    {
        // bana �arpan objectin etiketi player de�ilse �unu yap. (Yani tile'lar)
        if (!other.gameObject.CompareTag("Player"))
        {
            // h�z�m�, - yaparak sa� sola gitmesini sa�layaca��m.
            speed = -speed;
            FindDirection();
        }
    }

}
