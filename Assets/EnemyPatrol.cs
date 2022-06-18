using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float speed;
    public Transform left, right;

    private Rigidbody2D rigid;
    private SpriteRenderer sprite;

    // Düþman karakterimin, saða sola giderken; gidebildiði kadar sað yolununun sonunda biraz durup sonra sola
    // gidebildiði kadar sola gittikten sonra birazz durup tekrar saða gitmesini saðlamak için 2 adet deðiþken tanýmlýyorum. (turn ve currentSpeed)

    private bool turn;

    // Mevcut hýzýmýzý aþþaðýdaki deðiþkene atayacaðýz.
    private float currentSpeed;

    // Düþmanýn en uç sað ve en uç solda 1 saniye beklerkenki animasyonu için Animator'u tanýmlýyorum.
    private Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        // RigidBody2D'ye ulaþtým.
        rigid = GetComponent<Rigidbody2D>();
        // SpriteRenderer'e ulaþtým.
        sprite = GetComponent<SpriteRenderer>();
        // Animator'e ulaþtým.
        anim = GetComponent<Animator>();
        FindDirection();
        turn = true;
    }

    // Update is called once per frame
    void Update()
    {
        MoveEnemy();
        TurnEnemy();
    }

    private void MoveEnemy()
    {
        rigid.velocity = new Vector2(speed, 0f);
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

    // düþman saða gittiði zaman sola dönecek, sola gittiði zaman saða dönecek.
    private void TurnEnemy()
    {
        if (!sprite.flipX && transform.position.x >= right.position.x)
        {

            // diyelim oyun baþladý, hýzým sýfýrdan büyük saða doðru hareket ediyorum. Tam saða geldim (saðdaki nokta)
            if (turn)
            {
                turn = false;
                // mevcut hýzýmý saklýyorum.
                currentSpeed = speed;
                // hýzýmý sýfýr yapýyorum.
                speed = 0;
                // Bulunduðum yerde küçük bir süre beklemek istiyorum.
                // Bekleme durumu varsa StartCoroutine metodu lazým olacak.
                StartCoroutine("TurnLeft", currentSpeed);

            }

        }
        else if (sprite.flipX && transform.position.x <= left.position.x)
        {


            if (turn)
            {
                turn = false;
                currentSpeed = speed;
                speed = 0;
                StartCoroutine("TurnRight", currentSpeed);
            }
        }
    }

    IEnumerator TurnLeft(float currentSpeed)
    {
        // En uç saðda oludðumu varsayýyorum, düþman karakter duruyor ve animasyonunu çýkartýyor.
        anim.SetBool("Idle", true);
        // Tam saða geldim biraz beklicem. (1 saniye = 1f)
        yield return new WaitForSeconds(1f);
        // 1 Saniye bekledikten sonra durma animasyonum bitiyor ve düþman karakter, sola doðru dönüyor.
        anim.SetBool("Idle", false);

        sprite.flipX = true;
        speed = -currentSpeed;
        // bu sefer sola gidecek.
        turn = true;
    }

    IEnumerator TurnRight(float currentSpeed)
    {
        // En uç solda olduðumu varsayýyorum, düþman karakter duruyor ve animasyonunu çýkartýyor.
        anim.SetBool("Idle", true);
        // Tam sola geldim biraz beklicem (1 saniye = 1f)
        yield return new WaitForSeconds(1f);
        // 1 Saniye bekledikten sonra durma animasyonum bitiyor ve düþman karakter, saða doðru dönüyor.
        anim.SetBool("Idle", false);

        sprite.flipX = false;
        speed = -currentSpeed;
        // bu sefer saða gidecek.
        turn = true;
    }

}