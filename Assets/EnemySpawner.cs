using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // bu yaratýlacak olan düþmaný temsil edecek.
    public GameObject enemy;
    // Düþmanýn çýkacaðý yeri de berlirlememiz lazým.
    public GameObject enemyPos;
    // Sürekli düþman yaratmayacaðýz. Bir Bool deðiþkenine ihtiyacým var. (Boss ölünce, düþmanlar spawn olmayacak.)
    private bool canSpawn;

    void Start()
    {
        // evet düþman üretilebilir.
        canSpawn = true;
    }

    void Update()
    {
        // Eðer düþman üretilebilme true ise; düþmaný yaratacaðým.
        if (canSpawn)
        {
            StartCoroutine("SpawnEnemy");
        }
    }

    // 2 saniyede bir düþman üreteceðimiz metodu tanýmladýk.
    IEnumerator SpawnEnemy()
    {
        // Ekrana gameobjectimiz olan enemy'i basacak. (gameobject'in pozisyonunda)
        Instantiate(enemy, enemyPos.transform.position, Quaternion.identity);
        // düþman üretme, dur.
        canSpawn = false;
        // bir süre bekle. (2 saniye)
        yield return new WaitForSeconds(2f);
        // düþman üret
        canSpawn = true;
    }
}
