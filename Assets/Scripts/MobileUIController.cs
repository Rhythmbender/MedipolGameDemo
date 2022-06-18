using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileUIController : MonoBehaviour
{
    // PlayerController C# Script'ime yazdýðým fonksiyonlarý burada çaðýracaðým.
    PlayerController player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // sola hareket etmek için 
    public void MoveLeftMobile()
    {
        player.MoveLeftMobile();
    }

    // saða hareket etmek için
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
