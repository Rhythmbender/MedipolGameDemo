using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileUIController : MonoBehaviour
{
    // PlayerController C# Script'ime yazd���m fonksiyonlar� burada �a��raca��m.
    PlayerController player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // sola hareket etmek i�in 
    public void MoveLeftMobile()
    {
        player.MoveLeftMobile();
    }

    // sa�a hareket etmek i�in
    public void MoveRightMobile()
    {
        player.MoveRightMobile();
    }

    public void StopPlayerMobile()
    {
        player.StopPlayerMobile();
    }

    public void JumpMobile()
    {
        player.JumpMobile();
    }

    public void FireMobile()
    {
        player.FireMobile();
    }


}
