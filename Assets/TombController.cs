using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TombController : MonoBehaviour
{
    // Mezar ta��n�n "Is Trigger" �zelli�i aktif oldu�u i�in OnTriggerEnter2D metodunu �a��raca��z.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Kameray� durdurucaz
            // GameController.instance.StopCamera();
            // Karakterimiz, mezar ta��na de�ince, spawner'dan d��man �retimi ba�layacak.
            GameController.instance.EnableSpawner();
        }
    }
}
