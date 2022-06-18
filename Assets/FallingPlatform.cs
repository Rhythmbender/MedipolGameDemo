using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    // 'D��en Platform'un temas ettikten ka� saniye sonra d��ece�ini manuel girmek i�in
    // Unity'nin Inspector k�sm�na input ekliyorum.
    [Tooltip("Positive fall delay time")]
    public float delay;


    // �nce RigidBody 2D'ye eri�mem laz�m.
    private Rigidbody2D rigid;

    private void Awake()
    {
        // Inspector'da ki GameObject'imin (Paint Mode'da map'e ekledi�im platform) �st�nde yer alan RigidBody'e referans olu�turdum.
        rigid = GetComponent<Rigidbody2D>();
    }

    // Box Collider 2D ile ayarlad���m dikd�rtgene temas etmesi durumunda �a��raca��m metodu yazaca��m.
    private void OnCollisionEnter2D(Collision2D other)
    {
        // bana temas eden (other), gameObject'in Etiketi (CompareTag) e�er (if), Player ise
        if (other.gameObject.tag == "Player")
        {
            // A��a��ya tan�mlad���m�z Fall() metodunu direk �a��ram�yoruz.
            // Bunun yerine StartCoroutine() ile i�ine yollayaca��z.
            StartCoroutine(Fall());
        }
    }

    // Platform hemen d��meyecek, bir s�re bekleyip d��ece�i i�in IEnumerator metodu kullan�l�yor.
    // �rne�in '2' saniye sonra d��ece�ini girdik.
    // Bu metot sayesinde platform'un 2 saniye boyunca sabit durmas�n� sa�l�yor.
    IEnumerator Fall()
    {
        yield return new WaitForSeconds(delay);
        // Kinemati�i false yaparsak platform Dinamik olur. (Yer �ekimi devreye girecek.)
        rigid.isKinematic = false;
        // isTrigger �zelli�ini true yapmam�z�n sebebi, d��en platformun herhangi bir yere �arp�p takla atmas�n� �nlemek
        // Bu sayede d��en platformumuz hayali bir nesne gibi g�z�kecek ve direk yer�ekimine ba�l� olarak hayalet olarak
        // A��a�� do�ru ge�ip gidecek.
        gameObject.GetComponent<Collider2D>().isTrigger = true;
        // yield return 0 ile bu durumu sonland�rmam�z laz�m.
        yield return 0;
    }

}
