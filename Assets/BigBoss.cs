using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BigBoss : MonoBehaviour
{
    // Boss'un sa�l��� belli bir miktar azal�nca z�plama yapacak, bu speed z�plama h�z�n� belirliyecek.
    public float speed;
    // Boss'un sa�l��� belli bir miktar azal�nca z�plama yapacak, z�plama yapaca�� noktay� belirlemek i�in jumpAt'� kullanaca��m.
    public float jumpAt;
    // D��man ate� edecek.
    public GameObject bullet;
    // merminin ��kaca�� yeri de tan�mlamam�z laz�m.
    public GameObject bulletPos;
    // Boss'un ate� etme s�kl���n� belirleyece�iz.
    public float nextFire;
    // Boss'un can� olacak.
    public float enemyHealth;
    // Slider'daki de�eri g�ncellemek i�inse; UnityEngine.UI k�t�phanesini 4.sat�rda �a��rd�m ve slider referans�n� kulland�m.
    public Slider healthSlider;

    // Boss, bize ate� edecek fakat oyun ba�lar ba�lamaz ate� etmeyecek. Bunun kontrol�n� yapmam�z laz�m.
    // Ayr�ca z�plamam�z� da kontrol etmemiz laz�m.
    private bool canFire, isJumping;

    // Kuvvet uygulamak i�in bir rigidbody tan�mlayaca��m.
    private Rigidbody2D rigid;

    void Start()
    {
        // RigidBody'mizi bulaca��z.
        rigid = GetComponent<Rigidbody2D>();

        // nextFire �rne�in 4 olsun, 4 saniye sonra CanFire �al��acak.
        canFire = false;
        // 1 ile nextfire 4 demi�tik aras�ndaki saniye aral���nda rasgele ate� edecek.
        Invoke("CanFire", Random.Range(1, nextFire));
    }

    // Update is called once per frame
    void Update()
    {
        if (canFire)
        {
            // CanFire true oldu, d��man�m ate� edecek.
            EnemyFire();
            //  Sonra canFire false durumuna ge�ecek.
            canFire = false;

            // E�er d��man�n sa�l��� z�plama noktamdan k���kse ve z�plam�yorsam (isJumping, false-�al��m�yor)
            if (enemyHealth < jumpAt && !isJumping)
            {
                // 0 saniye sonra ba�la, 2 saniyede bir ise tekrar et.
                InvokeRepeating("JumpEnemy", 0, 2);
                // z�plamay� aktifle�tir.
                isJumping = true;
            }
        }
    }

    private void JumpEnemy()
    {
        // x ekseninde bir hareket olmayacak 0f yazd�k, yukar� do�ru bir hareket olacak o da speed olsun.
        rigid.AddForce(new Vector2(0f, speed));
    }

    // D��man�n ate� etmesi true olacak ve bossumuz ate� edecek.
    private void CanFire()
    {
        canFire = true;
    }

    private void EnemyFire()
    {
        Instantiate(bullet, bulletPos.transform.position, Quaternion.identity);
        Invoke("CanFire", Random.Range(1, nextFire));
    }

    // B�ylelikle yukarda yazd���m�z bu olay d�ng�ye girecek ve d��man�m her 1-4 saniye aral���nda rasgele saniyede ate� edecek.

    // Kediden ��kan mermide collider var, BigBoss'da da collider var. �ki collider �arp��t��� zaman OnCollisionEnter2D metodu �al���yor.

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("PlayerBullet"))
        {
            // D��man�n can� s�f�r ise;
            if (enemyHealth == 0)
            {
                GameController.instance.BulletHit(gameObject);
            }

            // D��man�n can� 0'dan b�y�kse;
            if (enemyHealth > 0)
            {
                // D��man�n can�n� bir bir azalt�cam.
                enemyHealth--;
                healthSlider.value = enemyHealth;
                // Boss hasar g�r�nce rengi k�rm�z� olmas� i�in k�rm�z� renkli animasyonu ��karaca��m.
                // BigBoss.cs scripti'nin ba�l� oldu�u gameObject'i getcomponent ile �a�r�cam (Animation'u)
                gameObject.GetComponent<Animation>().Play("DamagedBoss");
            }
        }
    }



}
