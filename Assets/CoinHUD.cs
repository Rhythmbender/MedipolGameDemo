using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinHUD : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Bize temas eden þey Coin olacak.
        if (other.gameObject.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
        }
    }
}
