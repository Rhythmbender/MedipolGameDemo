using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Destroys bullet after a specific delay
/// Belirli bir zaman sonra mermiyi yok eden bir script yazýyoruz. (Merminin sonsuza kadar gitmesini istemiyorum.)
/// </summary>
public class DestroyBullet : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 2f);
    }
}
