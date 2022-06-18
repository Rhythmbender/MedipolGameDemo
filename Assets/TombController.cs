using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TombController : MonoBehaviour
{
    // Mezar taþýnýn "Is Trigger" özelliði aktif olduðu için OnTriggerEnter2D metodunu çaðýracaðýz.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Kamerayý durdurucaz
            // GameController.instance.StopCamera();
            // Karakterimiz, mezar taþýna deðince, spawner'dan düþman üretimi baþlayacak.
            GameController.instance.EnableSpawner();
        }
    }
}
