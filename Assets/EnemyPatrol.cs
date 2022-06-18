using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float speed;
    public Transform left, right;

    private Rigidbody2D rigid;
    private SpriteRenderer sprite;

    // D��man karakterimin, sa�a sola giderken; gidebildi�i kadar sa� yolununun sonunda biraz durup sonra sola
    // gidebildi�i kadar sola gittikten sonra birazz durup tekrar sa�a gitmesini sa�lamak i�in 2 adet de�i�ken tan�ml�yorum. (turn ve currentSpeed)

    private bool turn;

    // Mevcut h�z�m�z� a��a��daki de�i�kene atayaca��z.
    private float currentSpeed;

    // D��man�n en u� sa� ve en u� solda 1 saniye beklerkenki animasyonu i�in Animator'u tan�ml�yorum.
    private Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        // RigidBody2D'ye ula�t�m.
        rigid = GetComponent<Rigidbody2D>();
        // SpriteRenderer'e ula�t�m.
        sprite = GetComponent<SpriteRenderer>();
        // Animator'e ula�t�m.
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

    // d��man sa�a gitti�i zaman sola d�necek, sola gitti�i zaman sa�a d�necek.
    private void TurnEnemy()
    {
        if (!sprite.flipX && transform.position.x >= right.position.x)
        {

            // diyelim oyun ba�lad�, h�z�m s�f�rdan b�y�k sa�a do�ru hareket ediyorum. Tam sa�a geldim (sa�daki nokta)
            if (turn)
            {
                turn = false;
                // mevcut h�z�m� sakl�yorum.
                currentSpeed = speed;
                // h�z�m� s�f�r yap�yorum.
                speed = 0;
                // Bulundu�um yerde k���k bir s�re beklemek istiyorum.
                // Bekleme durumu varsa StartCoroutine metodu laz�m olacak.
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
        // En u� sa�da olud�umu varsay�yorum, d��man karakter duruyor ve animasyonunu ��kart�yor.
        anim.SetBool("Idle", true);
        // Tam sa�a geldim biraz beklicem. (1 saniye = 1f)
        yield return new WaitForSeconds(1f);
        // 1 Saniye bekledikten sonra durma animasyonum bitiyor ve d��man karakter, sola do�ru d�n�yor.
        anim.SetBool("Idle", false);

        sprite.flipX = true;
        speed = -currentSpeed;
        // bu sefer sola gidecek.
        turn = true;
    }

    IEnumerator TurnRight(float currentSpeed)
    {
        // En u� solda oldu�umu varsay�yorum, d��man karakter duruyor ve animasyonunu ��kart�yor.
        anim.SetBool("Idle", true);
        // Tam sola geldim biraz beklicem (1 saniye = 1f)
        yield return new WaitForSeconds(1f);
        // 1 Saniye bekledikten sonra durma animasyonum bitiyor ve d��man karakter, sa�a do�ru d�n�yor.
        anim.SetBool("Idle", false);

        sprite.flipX = false;
        speed = -currentSpeed;
        // bu sefer sa�a gidecek.
        turn = true;
    }

}