using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BigBoss : MonoBehaviour
{
    // Boss'un saðlýðý belli bir miktar azalýnca zýplama yapacak, bu speed zýplama hýzýný belirliyecek.
    public float speed;
    // Boss'un saðlýðý belli bir miktar azalýnca zýplama yapacak, zýplama yapacaðý noktayý belirlemek için jumpAt'ý kullanacaðým.
    public float jumpAt;
    // Düþman ateþ edecek.
    public GameObject bullet;
    // merminin çýkacaðý yeri de tanýmlamamýz lazým.
    public GameObject bulletPos;
    // Boss'un ateþ etme sýklýðýný belirleyeceðiz.
    public float nextFire;
    // Boss'un caný olacak.
    public float enemyHealth;
    // Slider'daki deðeri güncellemek içinse; UnityEngine.UI kütüphanesini 4.satýrda çaðýrdým ve slider referansýný kullandým.
    public Slider healthSlider;

    // Boss, bize ateþ edecek fakat oyun baþlar baþlamaz ateþ etmeyecek. Bunun kontrolünü yapmamýz lazým.
    // Ayrýca zýplamamýzý da kontrol etmemiz lazým.
    private bool canFire, isJumping;

    // Kuvvet uygulamak için bir rigidbody tanýmlayacaðým.
    private Rigidbody2D rigid;

    void Start()
    {
        // RigidBody'mizi bulacaðýz.
        rigid = GetComponent<Rigidbody2D>();

        // nextFire örneðin 4 olsun, 4 saniye sonra CanFire çalýþacak.
        canFire = false;
        // 1 ile nextfire 4 demiþtik arasýndaki saniye aralýðýnda rasgele ateþ edecek.
        Invoke("CanFire", Random.Range(1, nextFire));
    }

    // Update is called once per frame
    void Update()
    {
        if (canFire)
        {
            // CanFire true oldu, düþmaným ateþ edecek.
            EnemyFire();
            //  Sonra canFire false durumuna geçecek.
            canFire = false;

            // Eðer düþmanýn saðlýðý zýplama noktamdan küçükse ve zýplamýyorsam (isJumping, false-çalýþmýyor)
            if (enemyHealth < jumpAt && !isJumping)
            {
                // 0 saniye sonra baþla, 2 saniyede bir ise tekrar et.
                InvokeRepeating("JumpEnemy", 0, 2);
                // zýplamayý aktifleþtir.
                isJumping = true;
            }
        }
    }

    private void JumpEnemy()
    {
        // x ekseninde bir hareket olmayacak 0f yazdýk, yukarý doðru bir hareket olacak o da speed olsun.
        rigid.AddForce(new Vector2(0f, speed));
    }

    // Düþmanýn ateþ etmesi true olacak ve bossumuz ateþ edecek.
    private void CanFire()
    {
        canFire = true;
    }

    private void EnemyFire()
    {
        Instantiate(bullet, bulletPos.transform.position, Quaternion.identity);
        Invoke("CanFire", Random.Range(1, nextFire));
    }

    // Böylelikle yukarda yazdýðýmýz bu olay döngüye girecek ve düþmaným her 1-4 saniye aralýðýnda rasgele saniyede ateþ edecek.

    // Kediden çýkan mermide collider var, BigBoss'da da collider var. Ýki collider çarpýþtýðý zaman OnCollisionEnter2D metodu çalýþýyor.

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("PlayerBullet"))
        {
            // Düþmanýn caný sýfýr ise;
            if (enemyHealth == 0)
            {
                GameController.instance.BulletHit(gameObject);
            }

            // Düþmanýn caný 0'dan büyükse;
            if (enemyHealth > 0)
            {
                // Düþmanýn canýný bir bir azaltýcam.
                enemyHealth--;
                healthSlider.value = enemyHealth;
                // Boss hasar görünce rengi kýrmýzý olmasý için kýrmýzý renkli animasyonu çýkaracaðým.
                // BigBoss.cs scripti'nin baðlý olduðu gameObject'i getcomponent ile çaðrýcam (Animation'u)
                gameObject.GetComponent<Animation>().Play("DamagedBoss");
            }
        }
    }



}
