using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // bu yarat�lacak olan d��man� temsil edecek.
    public GameObject enemy;
    // D��man�n ��kaca�� yeri de berlirlememiz laz�m.
    public GameObject enemyPos;
    // S�rekli d��man yaratmayaca��z. Bir Bool de�i�kenine ihtiyac�m var. (Boss �l�nce, d��manlar spawn olmayacak.)
    private bool canSpawn;

    void Start()
    {
        // evet d��man �retilebilir.
        canSpawn = true;
    }

    void Update()
    {
        // E�er d��man �retilebilme true ise; d��man� yarataca��m.
        if (canSpawn)
        {
            StartCoroutine("SpawnEnemy");
        }
    }

    // 2 saniyede bir d��man �retece�imiz metodu tan�mlad�k.
    IEnumerator SpawnEnemy()
    {
        // Ekrana gameobjectimiz olan enemy'i basacak. (gameobject'in pozisyonunda)
        Instantiate(enemy, enemyPos.transform.position, Quaternion.identity);
        // d��man �retme, dur.
        canSpawn = false;
        // bir s�re bekle. (2 saniye)
        yield return new WaitForSeconds(2f);
        // d��man �ret
        canSpawn = true;
    }
}
