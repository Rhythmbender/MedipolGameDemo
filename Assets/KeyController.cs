using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    // Sarý anahtar için keyNumber'a Ýnsepctor kýsmýnda 2 verdim. 2 numara'yý buraya atýcak sonra GameController'da
    // KeyCount fonksiyonunda 2'yi kontrol edecek.
    // Böylelikle 2 numara Sarý anahtarla iliþkili olduðu için boþ olan kýsmý dolu olan kýsýmla deðiþtirecek.
    public int keyNumber;
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Eðer bana temas eden gameObject'in etiketi Player ise;
        if (other.gameObject.CompareTag("Player"))
        {
            // GameController'da instance'a, KeyCount'a parametre olarak keyNumber ekleyeceðim.
            GameController.instance.KeyCount(keyNumber);
            // Ardýndan gameObject'i yok edeceðim.
            Destroy(gameObject);
        }
    }
}
