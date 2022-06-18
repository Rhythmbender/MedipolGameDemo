using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public float speed;

    // RigidBody 2D'ye eriþmem lazým. (Hareket kontrolü için)
    private Rigidbody2D rigid;

    // Sprite Renderer'a eriþmem lazým (Karakterin Saða Sola bakýnca oraya bakmasý için)
    private SpriteRenderer sprite;

    private void Start()
    {
        // Inspector kýsmýndaki Rigidbody2D'ye referans oluþturdum.
        rigid = GetComponent<Rigidbody2D>();

        // Inspector kýsmýndaki SpriteRenderer'a referans oluþturdum.
        sprite = GetComponent<SpriteRenderer>();

        FindDirection();
    }


    private void Update()
    {
        // Burada düþmanýmýzý hareket ettiren fonksiyonu çaðýrdýk.
        Move(); 
    }

    private void Move()
    {
        // düþmanýmýzý hareket ettiriyoruz.
        // rigid.velocity = new Vector2(speed, 0f);

        Vector2 temp = rigid.velocity;
        temp.x = speed;
        rigid.velocity = temp;
    }

    private void FindDirection()
    {
        // Hýzým sýfýrdan küçükse sola git.
        if (speed < 0)
        {
            // bunun için Flip X özelliðinin aktif olmasý lazým.
            sprite.flipX = true;
        }
        // Hýzým sýfýrdan büyükse saða git.
        else if (speed > 0)
        {
            sprite.flipX = false;
        }
    }

    // Düþmaný 2 tile arasýnda gidip gelmesini saðlayacaðým.
    // Tile'a çarpma iþlemi için OnCollisionEnter2D metodu çaðrýlýyordu.
    private void OnCollisionEnter2D(Collision2D other)
    {
        // bana çarpan objectin etiketi player deðilse þunu yap. (Yani tile'lar)
        if (!other.gameObject.CompareTag("Player"))
        {
            // hýzýmý, - yaparak sað sola gitmesini saðlayacaðým.
            speed = -speed;
            FindDirection();
        }
    }

}
