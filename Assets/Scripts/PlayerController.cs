using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player movement, script, jump, turnleft, turnright etc.
/// </summary>

public class PlayerController : MonoBehaviour
{
    // Speed'de ne anlama geldi�ini ve nas�l kullan�lmas� gerekti�ini a��klamak i�in ToolT�p kulland�m.

    [Tooltip("Only positive values for speed")]
    public float speed;

    [Tooltip("Only positive values for jumpForce")]
    public float jumpForce;

    // Karakter z�pl�yor mu? Onu kontrol etmemiz gerekecek ona g�re animasyon ge�i�leri yapaca��z.
    public bool isJumping;

    public Transform feet;
    public float feetRadius;
    public LayerMask myLayer;

    public float width, heigth;

    private bool doubleJump;
    public float jumpDelay;

    public bool isGrounded;


    private float timer;
    public float nextFire = 0.5f;
    private bool leftClicked, rightClicked;
    public bool effect;
    private bool canFire;

    // Su'da �l�rken iki ayr� tetikleyicim var, biri di�eini ezdi�i i�in DeadTrigger'imi disable etmem laz�m.
    // Yoksa GameOver ekran� gelmeyecek.
    public GameObject deathTrigger;

    public GameObject rightBullet, leftBullet;
    public Transform leftBulletPos;
    public Transform rightBulletPos;

    private Rigidbody2D rigid;

    // Awake metodu i�erisinde RigidBody2D'mizi bulduk.
    // ��nk� karakterimizi hareket ettirmemiz gerekecek.
    // Start metodu'da kullanbiliriz ama Awake, Start'tan daha �nce �al���r.

    // Sa�a bakmas� ve sola bakmas� Sprite Renderer'in Flip �zelli�i i�inde.
    // SpriteRenderer'i tan�mlayaca��m.
    private SpriteRenderer sprite;

    private Animator anim;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();

        sprite = GetComponent<SpriteRenderer>();

        anim = GetComponent<Animator>();
    }

    // FixedUpdate belirli zaman aral�klar�nda s�rekli �a�r�lan bir metottur. 
    // Fiziksel i�lemler (hareket etme gibi) FixedUpdate metodu tercih ediliyor.

    private void FixedUpdate()
    {
        timer += Time.deltaTime;

        // OverlapCircle(Vector2 point ---> Feet'imizin bulundu�u yer, float radius ---> aya��m�z�n kaplad��� alan, int layerMask ---> Temas etti�imiz zeminin ismi)
        // isGrounded = Physics2D.OverlapCircle(feet.position, feetRadius, myLayer);

        isGrounded = Physics2D.OverlapBox(new Vector2(feet.position.x, feet.position.y), new Vector2(width, heigth), 360f, myLayer);

        // Burada klavye'den veri ald�k.
        // Horizontal ile 'y' ekseninde karakterimizi hareket edebiliyoruz.
        // Karakterimiz b�ylelikle sa�a sola hareket ettirebiliriz. (< > y�n tu�lar� veya A D ile)
        // GetAxisRaw --> -1, 0, 1 de�erlerini al�r.

        float h = Input.GetAxisRaw("Horizontal");

        // Karakterimizi MovePlayer ile hareket ettiriyoruz.

        if (h != 0)
        {
            // Karakterin yatay'da (horizontal) hareketi fonksiyonu.
            // S�f�rdan farkl� bir de�erse [(-1(sol), 1(sa�)] karakterimi hareket ettir.
            MovePlayer(h);
        }
        else
        {
            // Karakterin oldu�u yerde sabit kalarak durmas� fonksiyonu.
            // S�f�r'a (0) e�itse karakterimi durdur.
            StopPlayer();
        }

        // Burada karakterimizi 'SPACE (BO�LUK)' tu�una bas�nca z�platma i�lemini yapaca��z.
        // GetKeyDown tu�a bas�l� oldu�u zaman �al��acak metottur.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        // Control tu�una bast���mda ate� eder.
        if(Input.GetButtonDown("Fire1") && timer > nextFire)
        {
            FireBullet();
        }

        if (leftClicked)
        {
            MovePlayer(-1f);
        }

        if (rightClicked)
        {
            MovePlayer(1f);
        }

        PlayerFall();
    }

    private void FireBullet()
    {
        if (canFire)
        {
            timer = 0f;

            // Sola bakm�yorsa (sa�);
            if (!sprite.flipX)
            {
                Instantiate(rightBullet, rightBulletPos.position, Quaternion.identity);
            }

            // Sola bak�yorsa;
            if (sprite.flipX)
            {
                Instantiate(leftBullet, leftBulletPos.position, Quaternion.identity);
            }
            // Ate� ederken ses efekti �al��acak.
            AudioController.instance.FireSound(gameObject.transform.position);
        }


        
    }

    private void OnDrawGizmos()
    {
        // Gizmos.DrawWireSphere(feet.position, feetRadius);

        Gizmos.DrawWireCube(feet.position, new Vector3(width, heigth, 0f));
    }
    private void MovePlayer(float h)
    {
        // klavyeden ald���m�z de�er (h ---> -1, 0 1), Unity'de a�t���m�z Speed input'una yazd���m�z (5) ile �arp�l�yor.)
        // Bu i�lem sonucunda karakterimiz daha fazla h�zlan�yor.
        rigid.velocity = new Vector2(h * speed, rigid.velocity.y);

        // Burada karakterimizin sa�a gitti�inde sa�a, sola gitti�inde sola bakmas�n� sa�layaca��z.
        // (-) de�er ald���nda karakter sol, (+) de�er ald���nda sa�a gidecek.

        if (h < 0)
        {
            // 0'dan k���kse karakter sol'a baks�n.
            sprite.flipX = true;
        }
        else if (h > 0)
        {
            // 0'dan b�y�kse karakter sa�'a baks�n.
            sprite.flipX = false;
        }

        // E�er z�plam�yorsam;
        if (!isJumping)
        {
            anim.SetInteger("Status", 1);
        }
        
    }

    private void PlayerFall()
    {
        if (rigid.velocity.y < 0)
        {
            anim.SetInteger("Status", 3);
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {
            // Karakteri z�platmak i�in yukar� do�ru bir kuvvet uygulayaca��z.
            // new Vector2(float x, float y) ---> Z�platmak i�in bizim x de�i�keniyle bir i�imiz yok, 0 verece�im.
            rigid.AddForce(new Vector2(0f, jumpForce));

            // Z�pl�yor muyum? Evet.
            isJumping = true;

            // ba��na �nlem koydu�um i�in otomatikman kapal� olan z�plamam�z otomatikman true oluyor..
            if (isJumping)
            {
                // Z�plama animasyonunu ��kar�yoruz.
                anim.SetInteger("Status", 2);
                // Z�plama ses efektini ��kar�yoruz.
                AudioController.instance.JumpSound(gameObject.transform.position);
                Invoke("DoubleJump", jumpDelay);
            }
        }
            // doubleJump true ve yerde de�ilsem (havadaysam)
            if (doubleJump && !isGrounded)
            {
                rigid.velocity = Vector2.zero;
                rigid.AddForce(new Vector2(0f, jumpForce));
                anim.SetInteger("Status", 2);
                // Z�plama ses efektini ��kar�yoruz.
                AudioController.instance.JumpSound(gameObject.transform.position);
                // Havada ���nc� z�plamay� engellemek i�in tekrar false yap�yorum. 
                doubleJump = false;
            }


    }

    private void DoubleJump()
    {
        doubleJump = true;
    }

    private void StopPlayer()
    {
        // Hi�bir tu�a basmad���m�z zaman, klavyeden ald���m�z de�er s�f�r (0) oluyor ve karakterimiz bulundu�u yerde sabit kalarak duruyor.
        rigid.velocity = new Vector2(0, rigid.velocity.y);

        // E�er z�plam�yorsam;
        if (!isJumping)
        {
            anim.SetInteger("Status", 0);
        }

        
    }

    // Zeminler ve �st�ne ��kt���m�z kutular� BoxCollider yaparak temas etmesini sa�lam��t�k.
    // Karakter ve BoxCollider'l� yap� birbirine temas etti�i zaman onCollisionEnter metodu �a�r�l�r.
    // Bunu 2 boyutlu yapt���m�z i�in onCollisionEnter2D metodunu kullanaca��z.
    private void OnCollisionEnter2D(Collision2D other)
    {
        // karakter yere temas etti�inde, z�pl�yor mu?
        // Hay�r z�plam�yor. �u an karakter yerde.
        // yere temas etti�iniz s�rece z�plama durumumuz otomatikman kapal� durumda.
        isJumping = false;

        // d��man bana temas etti�inde �lmem laz�m.
        // bana (Player) �arpan nesnenin (d��man�m�z) etiketi Enemy oldu�unda �al��t�raca��z. 
        if (other.gameObject.CompareTag("Enemy"))
        {
            GameController.instance.PlayerHit(gameObject);
            // D��mana kar�� �ld���m�zde ses efekti ��kacak.
            AudioController.instance.PlayerDieSound(gameObject.transform.position);
        }

        // E�er temas etti�im objenin etiketi RewardCoin ise;
        if (other.gameObject.CompareTag("RewardCoin"))
        {
            GameController.instance.CoinCount();
            // Ard�ndan ekrana bir efekt vermek istiyorum.
            EffectController.instance.ShowCoinEffect(other.transform.position);
            // Temas ettikten sonra coin'imizin yok olmas� laz�m.
            Destroy(other.gameObject);
            // Bu �zel bir Coin oldu�u i�in skorumuzu da istedi�im say� kadar artt�rmam laz�m.
            GameController.instance.ScoreCount(50);
            // Coin toplama sesi ekliyorum.
            AudioController.instance.CoinSound(other.transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // E�er temas etti�im GameObject'in ad� Coin ise;
        // EffectController'de ki ShowCoinEffect adl� metodu �al��t�r.
        switch (other.gameObject.tag)
        {
            // GameObject'imin etiketi Coin ise a��a��daki i�lmeleri yap.
            // Coin ---> Oyunda toplad���m�z alt�n.
            case "Coin":
                if (effect)
                {
                    // Burada bana temas eden GameObject'in pozisyonunu belirtiyorum.
                    EffectController.instance.ShowCoinEffect(other.gameObject.transform.position);
                    // Burada coin say�m�z� 1 1 artt�raca��z.
                    GameController.instance.CoinCount();
                    // Burada score sayac�m�z� 5 5 artt�raca��z.
                    GameController.instance.ScoreCount(5);
                    // Coinleri toplarken ses ��kacak.
                    AudioController.instance.CoinSound(gameObject.transform.position);
                }
                break;
            // GameObject'imin etiketi PowerUp ise a��a��daki i�lemleri yap.
            // Power Up ---> Anahtar� ald���m�zda ate� etme �zelli�imizin aktifle�ti�i oyun objesi.
            case "PowerUp":
                if (effect)
                {
                    canFire = true;
                    // Burada bana temas eden GameObject'in pozisyonunu belirtiyorum.
                    EffectController.instance.ShowPowerUpEffect(other.gameObject.transform.position);
                    Destroy(other.gameObject);
                    // Anahtar� al�rken ses efekti ��kacak.
                    AudioController.instance.KeySound(other.transform.position);
                }
                break;

            case "Water":
                deathTrigger.SetActive(false);
                // Burada bana temas eden GameObject'in pozisyonunu belirtiyorum.
                EffectController.instance.ShowWaterEffect(gameObject.transform.position);
                // Yazd���m�z GameController'dan karakterin �lme fonksiyonunu ve b�l�m� restart etme fonksiyonunu �ekiyorum.
                // Suya temas edince, karakterin �lmesini sa�l�yorum ve oyunu yeniden ba�lat�yorum.
                GameController.instance.PlayerDied(gameObject);
                // Suya d��erken ses efekti ��kacak.
                AudioController.instance.WaterSound(gameObject.transform.position);
                // Suya d��erken ayn� zamanda karakterin �lme sesi de ��kmal�.
                AudioController.instance.PlayerDieSound(gameObject.transform.position);
                break;

            case "BossKey":
                // Burada bana temas eden GameObject'in pozisyonunu belirtiyorum.
                EffectController.instance.ShowPowerUpEffect(other.gameObject.transform.position);
                Destroy(other.gameObject);
                // Anahtar� al�rken ses efekti ��kacak.
                AudioController.instance.KeySound(other.transform.position);
                // NPC'nin a�a��s�ndaki duvar kutular yok olacak.
                GameController.instance.DisableWall();
                break;
        }


    }

    // mobil butonda sola hareket etmek i�in
    public void MoveLeftMobile()
    {
        leftClicked = true;
    }

    // mobil butonda sa�a hareket etmek i�in
    public void MoveRightMobile()
    {
        rightClicked = true;
    }

    // hi� bir tu�a basm�yorsak, karakterimiz duruyor.
    public void StopPlayerMobile()
    {
        leftClicked = false;
        rightClicked = false;
    }

    // z�plama tu�u
    public void JumpMobile()
    {
        Jump();
    }

    // mobilde butonda ate� etme tu�u
    public void FireMobile()
    {
        FireBullet();
    }

}
