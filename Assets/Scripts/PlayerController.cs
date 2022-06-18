using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player movement, script, jump, turnleft, turnright etc.
/// </summary>

public class PlayerController : MonoBehaviour
{
    // Speed'de ne anlama geldiðini ve nasýl kullanýlmasý gerektiðini açýklamak için ToolTÝp kullandým.

    [Tooltip("Only positive values for speed")]
    public float speed;

    [Tooltip("Only positive values for jumpForce")]
    public float jumpForce;

    // Karakter zýplýyor mu? Onu kontrol etmemiz gerekecek ona göre animasyon geçiþleri yapacaðýz.
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

    // Su'da ölürken iki ayrý tetikleyicim var, biri diðeini ezdiði için DeadTrigger'imi disable etmem lazým.
    // Yoksa GameOver ekraný gelmeyecek.
    public GameObject deathTrigger;

    public GameObject rightBullet, leftBullet;
    public Transform leftBulletPos;
    public Transform rightBulletPos;

    private Rigidbody2D rigid;

    // Awake metodu içerisinde RigidBody2D'mizi bulduk.
    // Çünkü karakterimizi hareket ettirmemiz gerekecek.
    // Start metodu'da kullanbiliriz ama Awake, Start'tan daha önce çalýþýr.

    // Saða bakmasý ve sola bakmasý Sprite Renderer'in Flip özelliði içinde.
    // SpriteRenderer'i tanýmlayacaðým.
    private SpriteRenderer sprite;

    private Animator anim;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();

        sprite = GetComponent<SpriteRenderer>();

        anim = GetComponent<Animator>();
    }

    // FixedUpdate belirli zaman aralýklarýnda sürekli çaðrýlan bir metottur. 
    // Fiziksel iþlemler (hareket etme gibi) FixedUpdate metodu tercih ediliyor.

    private void FixedUpdate()
    {
        timer += Time.deltaTime;

        // OverlapCircle(Vector2 point ---> Feet'imizin bulunduðu yer, float radius ---> ayaðýmýzýn kapladýðý alan, int layerMask ---> Temas ettiðimiz zeminin ismi)
        // isGrounded = Physics2D.OverlapCircle(feet.position, feetRadius, myLayer);

        isGrounded = Physics2D.OverlapBox(new Vector2(feet.position.x, feet.position.y), new Vector2(width, heigth), 360f, myLayer);

        // Burada klavye'den veri aldýk.
        // Horizontal ile 'y' ekseninde karakterimizi hareket edebiliyoruz.
        // Karakterimiz böylelikle saða sola hareket ettirebiliriz. (< > yön tuþlarý veya A D ile)
        // GetAxisRaw --> -1, 0, 1 deðerlerini alýr.

        float h = Input.GetAxisRaw("Horizontal");

        // Karakterimizi MovePlayer ile hareket ettiriyoruz.

        if (h != 0)
        {
            // Karakterin yatay'da (horizontal) hareketi fonksiyonu.
            // Sýfýrdan farklý bir deðerse [(-1(sol), 1(sað)] karakterimi hareket ettir.
            MovePlayer(h);
        }
        else
        {
            // Karakterin olduðu yerde sabit kalarak durmasý fonksiyonu.
            // Sýfýr'a (0) eþitse karakterimi durdur.
            StopPlayer();
        }

        // Burada karakterimizi 'SPACE (BOÞLUK)' tuþuna basýnca zýplatma iþlemini yapacaðýz.
        // GetKeyDown tuþa basýlý olduðu zaman çalýþacak metottur.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        // Control tuþuna bastýðýmda ateþ eder.
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

            // Sola bakmýyorsa (sað);
            if (!sprite.flipX)
            {
                Instantiate(rightBullet, rightBulletPos.position, Quaternion.identity);
            }

            // Sola bakýyorsa;
            if (sprite.flipX)
            {
                Instantiate(leftBullet, leftBulletPos.position, Quaternion.identity);
            }
            // Ateþ ederken ses efekti çalýþacak.
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
        // klavyeden aldýðýmýz deðer (h ---> -1, 0 1), Unity'de açtýðýmýz Speed input'una yazdýðýmýz (5) ile çarpýlýyor.)
        // Bu iþlem sonucunda karakterimiz daha fazla hýzlanýyor.
        rigid.velocity = new Vector2(h * speed, rigid.velocity.y);

        // Burada karakterimizin saða gittiðinde saða, sola gittiðinde sola bakmasýný saðlayacaðýz.
        // (-) deðer aldýðýnda karakter sol, (+) deðer aldýðýnda saða gidecek.

        if (h < 0)
        {
            // 0'dan küçükse karakter sol'a baksýn.
            sprite.flipX = true;
        }
        else if (h > 0)
        {
            // 0'dan büyükse karakter sað'a baksýn.
            sprite.flipX = false;
        }

        // Eðer zýplamýyorsam;
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
            // Karakteri zýplatmak için yukarý doðru bir kuvvet uygulayacaðýz.
            // new Vector2(float x, float y) ---> Zýplatmak için bizim x deðiþkeniyle bir iþimiz yok, 0 vereceðim.
            rigid.AddForce(new Vector2(0f, jumpForce));

            // Zýplýyor muyum? Evet.
            isJumping = true;

            // baþýna ünlem koyduðum için otomatikman kapalý olan zýplamamýz otomatikman true oluyor..
            if (isJumping)
            {
                // Zýplama animasyonunu çýkarýyoruz.
                anim.SetInteger("Status", 2);
                // Zýplama ses efektini çýkarýyoruz.
                AudioController.instance.JumpSound(gameObject.transform.position);
                Invoke("DoubleJump", jumpDelay);
            }
        }
            // doubleJump true ve yerde deðilsem (havadaysam)
            if (doubleJump && !isGrounded)
            {
                rigid.velocity = Vector2.zero;
                rigid.AddForce(new Vector2(0f, jumpForce));
                anim.SetInteger("Status", 2);
                // Zýplama ses efektini çýkarýyoruz.
                AudioController.instance.JumpSound(gameObject.transform.position);
                // Havada üçüncü zýplamayý engellemek için tekrar false yapýyorum. 
                doubleJump = false;
            }


    }

    private void DoubleJump()
    {
        doubleJump = true;
    }

    private void StopPlayer()
    {
        // Hiçbir tuþa basmadýðýmýz zaman, klavyeden aldýðýmýz deðer sýfýr (0) oluyor ve karakterimiz bulunduðu yerde sabit kalarak duruyor.
        rigid.velocity = new Vector2(0, rigid.velocity.y);

        // Eðer zýplamýyorsam;
        if (!isJumping)
        {
            anim.SetInteger("Status", 0);
        }

        
    }

    // Zeminler ve üstüne çýktýðýmýz kutularý BoxCollider yaparak temas etmesini saðlamýþtýk.
    // Karakter ve BoxCollider'lý yapý birbirine temas ettiði zaman onCollisionEnter metodu çaðrýlýr.
    // Bunu 2 boyutlu yaptýðýmýz için onCollisionEnter2D metodunu kullanacaðýz.
    private void OnCollisionEnter2D(Collision2D other)
    {
        // karakter yere temas ettiðinde, zýplýyor mu?
        // Hayýr zýplamýyor. Þu an karakter yerde.
        // yere temas ettiðiniz sürece zýplama durumumuz otomatikman kapalý durumda.
        isJumping = false;

        // düþman bana temas ettiðinde ölmem lazým.
        // bana (Player) çarpan nesnenin (düþmanýmýz) etiketi Enemy olduðunda çalýþtýracaðýz. 
        if (other.gameObject.CompareTag("Enemy"))
        {
            GameController.instance.PlayerHit(gameObject);
            // Düþmana karþý öldüðümüzde ses efekti çýkacak.
            AudioController.instance.PlayerDieSound(gameObject.transform.position);
        }

        // Eðer temas ettiðim objenin etiketi RewardCoin ise;
        if (other.gameObject.CompareTag("RewardCoin"))
        {
            GameController.instance.CoinCount();
            // Ardýndan ekrana bir efekt vermek istiyorum.
            EffectController.instance.ShowCoinEffect(other.transform.position);
            // Temas ettikten sonra coin'imizin yok olmasý lazým.
            Destroy(other.gameObject);
            // Bu özel bir Coin olduðu için skorumuzu da istediðim sayý kadar arttýrmam lazým.
            GameController.instance.ScoreCount(50);
            // Coin toplama sesi ekliyorum.
            AudioController.instance.CoinSound(other.transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Eðer temas ettiðim GameObject'in adý Coin ise;
        // EffectController'de ki ShowCoinEffect adlý metodu çalýþtýr.
        switch (other.gameObject.tag)
        {
            // GameObject'imin etiketi Coin ise aþþaðýdaki iþlmeleri yap.
            // Coin ---> Oyunda topladýðýmýz altýn.
            case "Coin":
                if (effect)
                {
                    // Burada bana temas eden GameObject'in pozisyonunu belirtiyorum.
                    EffectController.instance.ShowCoinEffect(other.gameObject.transform.position);
                    // Burada coin sayýmýzý 1 1 arttýracaðýz.
                    GameController.instance.CoinCount();
                    // Burada score sayacýmýzý 5 5 arttýracaðýz.
                    GameController.instance.ScoreCount(5);
                    // Coinleri toplarken ses çýkacak.
                    AudioController.instance.CoinSound(gameObject.transform.position);
                }
                break;
            // GameObject'imin etiketi PowerUp ise aþþaðýdaki iþlemleri yap.
            // Power Up ---> Anahtarý aldýðýmýzda ateþ etme özelliðimizin aktifleþtiði oyun objesi.
            case "PowerUp":
                if (effect)
                {
                    canFire = true;
                    // Burada bana temas eden GameObject'in pozisyonunu belirtiyorum.
                    EffectController.instance.ShowPowerUpEffect(other.gameObject.transform.position);
                    Destroy(other.gameObject);
                    // Anahtarý alýrken ses efekti çýkacak.
                    AudioController.instance.KeySound(other.transform.position);
                }
                break;

            case "Water":
                deathTrigger.SetActive(false);
                // Burada bana temas eden GameObject'in pozisyonunu belirtiyorum.
                EffectController.instance.ShowWaterEffect(gameObject.transform.position);
                // Yazdýðýmýz GameController'dan karakterin ölme fonksiyonunu ve bölümü restart etme fonksiyonunu çekiyorum.
                // Suya temas edince, karakterin ölmesini saðlýyorum ve oyunu yeniden baþlatýyorum.
                GameController.instance.PlayerDied(gameObject);
                // Suya düþerken ses efekti çýkacak.
                AudioController.instance.WaterSound(gameObject.transform.position);
                // Suya düþerken ayný zamanda karakterin ölme sesi de çýkmalý.
                AudioController.instance.PlayerDieSound(gameObject.transform.position);
                break;

            case "BossKey":
                // Burada bana temas eden GameObject'in pozisyonunu belirtiyorum.
                EffectController.instance.ShowPowerUpEffect(other.gameObject.transform.position);
                Destroy(other.gameObject);
                // Anahtarý alýrken ses efekti çýkacak.
                AudioController.instance.KeySound(other.transform.position);
                // NPC'nin aþaðýsýndaki duvar kutular yok olacak.
                GameController.instance.DisableWall();
                break;
        }


    }

    // mobil butonda sola hareket etmek için
    public void MoveLeftMobile()
    {
        leftClicked = true;
    }

    // mobil butonda saða hareket etmek için
    public void MoveRightMobile()
    {
        rightClicked = true;
    }

    // hiç bir tuþa basmýyorsak, karakterimiz duruyor.
    public void StopPlayerMobile()
    {
        leftClicked = false;
        rightClicked = false;
    }

    // zýplama tuþu
    public void JumpMobile()
    {
        Jump();
    }

    // mobilde butonda ateþ etme tuþu
    public void FireMobile()
    {
        FireBullet();
    }

}
