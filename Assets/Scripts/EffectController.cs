using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    public static EffectController instance;
    public Effect effect;


    private void Awake()
    {
        // instance null'sa y�ni herhangi bir �ey atanmam��sa, referans etmiyorsa;
        if (instance == null)
        {
            // script'i buna refere et.
            instance = this;
        }
    }

    // Coin'lere temas etti�imizde �al��acak olan metodu tan�mlad�k.
    public void ShowCoinEffect(Vector3 pos)
    {
        // Instantiate ile efekti, oyun ekran�nda g�steriyoruz.
        Instantiate(effect.coinEffect, pos, Quaternion.identity);
    }

    // Ate� etti�imizde �al��acak olan metodu tan�mlad�k.
    public void ShowPowerUpEffect(Vector3 pos)
    {
        // Instantiate ile efekti, oyun ekran�nda g�steriyoruz.
        Instantiate(effect.powerUpEffect, pos, Quaternion.identity);
    }

    // Yere temas etti�imizde �al��acak olan metodu tan�mlad�k.
    public void ShowDustEffect(Vector3 pos)
    {
        // Instantiate ile efekti, oyun ekran�nda g�steriyoruz.
        Instantiate(effect.dustEffect, pos, Quaternion.identity);
    }

    // K�rm�z� su'ya temas etti�imizde �al��acak olan metodu tan�mlad�k.
    public void ShowWaterEffect(Vector3 pos)
    {
        // Instantiate ile efekti, oyun ekran�nda g�steriyoruz.
        Instantiate(effect.waterEffect, pos, Quaternion.identity);
    }

    public void EnemyDie(GameObject enemy)
    {
        // d��man�n oldu�u yerin bilgisini giriyorum.
        Instantiate(effect.enemyExplosion, enemy.transform.position, Quaternion.identity);
    }

}


// Unity'de efektlerimin grupland�r�lmas�na yar�yor. Effekt'in yan�nda ">" bast���mda b�y�yecek.
[System.Serializable]

public class Effect
{
    public GameObject coinEffect;
    public GameObject powerUpEffect;
    public GameObject dustEffect;
    public GameObject waterEffect;
    public GameObject enemyExplosion;
}
