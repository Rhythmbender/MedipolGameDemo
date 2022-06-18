using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetController : MonoBehaviour
{
    // Toz efektimin nas�l ��kaca�� bilgisini yazaca��m.
    // Y�ksekten d��erken yere temas edince gibi vs.
    public Transform pos;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            EffectController.instance.ShowDustEffect(pos.position);
        }
    }

}
