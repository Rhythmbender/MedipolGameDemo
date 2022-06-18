using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    // Sar� anahtar i�in keyNumber'a �nsepctor k�sm�nda 2 verdim. 2 numara'y� buraya at�cak sonra GameController'da
    // KeyCount fonksiyonunda 2'yi kontrol edecek.
    // B�ylelikle 2 numara Sar� anahtarla ili�kili oldu�u i�in bo� olan k�sm� dolu olan k�s�mla de�i�tirecek.
    public int keyNumber;
    private void OnTriggerEnter2D(Collider2D other)
    {
        // E�er bana temas eden gameObject'in etiketi Player ise;
        if (other.gameObject.CompareTag("Player"))
        {
            // GameController'da instance'a, KeyCount'a parametre olarak keyNumber ekleyece�im.
            GameController.instance.KeyCount(keyNumber);
            // Ard�ndan gameObject'i yok edece�im.
            Destroy(gameObject);
        }
    }
}
