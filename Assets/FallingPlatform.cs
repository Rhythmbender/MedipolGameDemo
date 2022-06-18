using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    // 'Düþen Platform'un temas ettikten kaç saniye sonra düþeceðini manuel girmek için
    // Unity'nin Inspector kýsmýna input ekliyorum.
    [Tooltip("Positive fall delay time")]
    public float delay;


    // Önce RigidBody 2D'ye eriþmem lazým.
    private Rigidbody2D rigid;

    private void Awake()
    {
        // Inspector'da ki GameObject'imin (Paint Mode'da map'e eklediðim platform) üstünde yer alan RigidBody'e referans oluþturdum.
        rigid = GetComponent<Rigidbody2D>();
    }

    // Box Collider 2D ile ayarladýðým dikdörtgene temas etmesi durumunda çaðýracaðým metodu yazacaðým.
    private void OnCollisionEnter2D(Collision2D other)
    {
        // bana temas eden (other), gameObject'in Etiketi (CompareTag) eðer (if), Player ise
        if (other.gameObject.tag == "Player")
        {
            // Aþþaðýya tanýmladýðýmýz Fall() metodunu direk çaðýramýyoruz.
            // Bunun yerine StartCoroutine() ile içine yollayacaðýz.
            StartCoroutine(Fall());
        }
    }

    // Platform hemen düþmeyecek, bir süre bekleyip düþeceði için IEnumerator metodu kullanýlýyor.
    // Örneðin '2' saniye sonra düþeceðini girdik.
    // Bu metot sayesinde platform'un 2 saniye boyunca sabit durmasýný saðlýyor.
    IEnumerator Fall()
    {
        yield return new WaitForSeconds(delay);
        // Kinematiði false yaparsak platform Dinamik olur. (Yer çekimi devreye girecek.)
        rigid.isKinematic = false;
        // isTrigger özelliðini true yapmamýzýn sebebi, düþen platformun herhangi bir yere çarpýp takla atmasýný önlemek
        // Bu sayede düþen platformumuz hayali bir nesne gibi gözükecek ve direk yerçekimine baðlý olarak hayalet olarak
        // Aþþaðý doðru geçip gidecek.
        gameObject.GetComponent<Collider2D>().isTrigger = true;
        // yield return 0 ile bu durumu sonlandýrmamýz lazým.
        yield return 0;
    }

}
