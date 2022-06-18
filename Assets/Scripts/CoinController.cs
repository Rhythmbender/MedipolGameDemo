using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public float speed;
    public float coinSpeed;

    // �e�itli animasyonlar g�stermek i�in enum yap�s� tan�mlad�k.
    public enum Coin
    {
        FlyCoin,
        DestroyCoin
    }

    public Coin coin;

    private bool isFlying;
    // hudCoin sol �st k��edeki CoinImg'mizi ifade ediyor. Start metodunun i�inde bu ifadeye referans�n� olu�turaca��z.
    private GameObject hudCoin;

    private void Start()
    {
        isFlying = false;
        // Unity'de bulunan CoinImg'mize referans olu�turduk.
        hudCoin = GameObject.Find("CoinImg");
    }

    private void Update()
    {
        Rotate();

        if (isFlying)
        {
            // transform.position CoinController'�n ba�l� oldu�u object'i (oyundaki alt�nlar�m�z) ifade eder.
            // ='den sonraki k�s�m ise; Mevcut konumdan Coin'imin bulundu�u yere (ekran�m�n sol �st k��esi) ge�i� yapaca��z.
            transform.position = Vector2.Lerp(transform.position, hudCoin.transform.position, coinSpeed * Time.deltaTime);
        }
    }

    // Other (Player karakterin) bize temas eden objenin bilgilerini ta��yor.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (coin == Coin.DestroyCoin)
            {
                Destroy(gameObject);
            }
            // Destroy(gameObject);

            // Coin'im, Coin.FlyCoin'e e�it ise isFlying true olacak.
            // True olduktan sonra Update metodu s�rekli �al��t��� i�in, benim coinim ekran�n sol �st k��esine gidecek.
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
