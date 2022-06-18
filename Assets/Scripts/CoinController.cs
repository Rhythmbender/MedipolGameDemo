using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public float speed;
    public float coinSpeed;

    // Çeþitli animasyonlar göstermek için enum yapýsý tanýmladýk.
    public enum Coin
    {
        FlyCoin,
        DestroyCoin
    }

    public Coin coin;

    private bool isFlying;
    // hudCoin sol üst köþedeki CoinImg'mizi ifade ediyor. Start metodunun içinde bu ifadeye referansýný oluþturacaðýz.
    private GameObject hudCoin;

    private void Start()
    {
        isFlying = false;
        // Unity'de bulunan CoinImg'mize referans oluþturduk.
        hudCoin = GameObject.Find("CoinImg");
    }

    private void Update()
    {
        Rotate();

        if (isFlying)
        {
            // transform.position CoinController'ýn baðlý olduðu object'i (oyundaki altýnlarýmýz) ifade eder.
            // ='den sonraki kýsým ise; Mevcut konumdan Coin'imin bulunduðu yere (ekranýmýn sol üst köþesi) geçiþ yapacaðýz.
            transform.position = Vector2.Lerp(transform.position, hudCoin.transform.position, coinSpeed * Time.deltaTime);
        }
    }

    // Other (Player karakterin) bize temas eden objenin bilgilerini taþýyor.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (coin == Coin.DestroyCoin)
            {
                Destroy(gameObject);
            }
            // Destroy(gameObject);

            // Coin'im, Coin.FlyCoin'e eþit ise isFlying true olacak.
            // True olduktan sonra Update metodu sürekli çalýþtýðý için, benim coinim ekranýn sol üst köþesine gidecek.
            else if (coin == Coin.FlyCoin)
            {
                isFlying = true;
            }

        }
    }

    private void Rotate()
    {
        transform.Rotate(new Vector3(0, speed, 0));
        
    }
}
