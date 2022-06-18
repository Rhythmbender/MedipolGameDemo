using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    public static EffectController instance;
    public Effect effect;


    private void Awake()
    {
        // instance null'sa yâni herhangi bir þey atanmamýþsa, referans etmiyorsa;
        if (instance == null)
        {
            // script'i buna refere et.
            instance = this;
        }
    }

    // Coin'lere temas ettiðimizde çalýþacak olan metodu tanýmladýk.
    public void ShowCoinEffect(Vector3 pos)
    {
        // Instantiate ile efekti, oyun ekranýnda gösteriyoruz.
        Instantiate(effect.coinEffect, pos, Quaternion.identity);
    }

    // Ateþ ettiðimizde çalýþacak olan metodu tanýmladýk.
    public void ShowPowerUpEffect(Vector3 pos)
    {
        // Instantiate ile efekti, oyun ekranýnda gösteriyoruz.
        Instantiate(effect.powerUpEffect, pos, Quaternion.identity);
    }

    // Yere temas ettiðimizde çalýþacak olan metodu tanýmladýk.
    public void ShowDustEffect(Vector3 pos)
    {
        // Instantiate ile efekti, oyun ekranýnda gösteriyoruz.
        Instantiate(effect.dustEffect, pos, Quaternion.identity);
    }

    // Kýrmýzý su'ya temas ettiðimizde çalýþacak olan metodu tanýmladýk.
    public void ShowWaterEffect(Vector3 pos)
    {
        // Instantiate ile efekti, oyun ekranýnda gösteriyoruz.
        Instantiate(effect.waterEffect, pos, Quaternion.identity);
    }

    public void EnemyDie(GameObject enemy)
    {
        // düþmanýn olduðu yerin bilgisini giriyorum.
        Instantiate(effect.enemyExplosion, enemy.transform.position, Quaternion.identity);
    }

}


// Unity'de efektlerimin gruplandýrýlmasýna yarýyor. Effekt'in yanýnda ">" bastýðýmda büyüyecek.
[System.Serializable]

public class Effect
{
    public GameObject coinEffect;
    public GameObject powerUpEffect;
    public GameObject dustEffect;
    public GameObject waterEffect;
    public GameObject enemyExplosion;
}
