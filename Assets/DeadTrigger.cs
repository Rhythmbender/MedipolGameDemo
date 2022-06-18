using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // GameController'dan instance ile PlayerDied fonksiyonumuzu �a��rd�k.
            GameController.instance.PlayerDied(other.gameObject);
        }
    }
}
