using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class LevelController : MonoBehaviour
{
    public Button nextButton;
    // Beyaz y�ld�zlar�n yerini sar� y�ld�zlar alarak, b�l�m sonunda alaca��m�z y�ld�z say�s�n� belirleyecek.
    public Sprite goldStar;
    // Ekrandaki beyaz y�ld�zlar
    public Image star1, star2, star3;
    // Ekrandaki skorumuz;
    public Text txtScore;

    // Bunlara ilaveten hangi levelde oldu�umuzu bilmemiz laz�m.
    public int levelnum;
    // Mevcut skorumuzu tutmam�z laz�m.
    public int score;
    // Skora g�re y�ld�z sistemi olacak.
    public int score3Star;
    public int score2Star;
    public int score1Star;
    // di�er levele ge�mek i�in gerekli olan skor miktar�;
    public int nextLevelScore;
    // Y�ld�zlara animasyon verelim. Belirli s�relerle gelsin. Bunlar�n de�i�kenlerini tan�mlayaca��m.
    public float startDelayAnim;
    // Bir y�ld�z geldikten sonra di�er y�ld�z�n gelmesi i�in biraz beklicez;
    public float delayAnim;
    // iki y�ld�z g�ster;
    private bool show2Star, show3Star;



    void Start()    
    {
        // Ekranda kay�tl� olan skorumu �ekece�im. Bu bilgiler gamecontroller'daki GameData b�l�m�nde mevcut.
        // score = GameController.instance.GetScore();
        // a��lan ekrana skorumu basmam laz�m.
        txtScore.text = "" + score;

        // e�er benim skorum, 3 y�ld�z kazanmam gereken skordan b�y�kse; 3 y�ld�z g�ster
        if (score >= score3Star)
        {
            show3Star = true;
            Invoke("GoldStarAnim", startDelayAnim);
        }

        // e�er benim skorum, 2 y�ld�z kazanmam gereken skordan b�y�kse ve 3 y�ld�zdan k���kse; 2 y�ld�z g�ster
        if (score >= score2Star && score < score3Star)
        {
            show2Star = true;
            Invoke("GoldStarAnim", startDelayAnim);
        }

        // skorum 0'a e�it de�ilse skorum, 1 y�ld�z kazanmam gereken skordan k���kse �unu yap;
        if (score !=0 && score <= score1Star)
        {
            Invoke("GoldStarAnim", startDelayAnim);
        }
    }

    private void GoldStarAnim()
    {
        StartCoroutine("FirstStarAnim", star1);
    }
    
    IEnumerator FirstStarAnim(Image starImg)
    {
        ShowAnim(starImg);

        yield return new WaitForSeconds(delayAnim);

        // ekrana bir y�ld�z daha basmas� i�in
        if (show2Star || show3Star)
        {
            StartCoroutine("SecondStarAnim", star2);
        }

        else
        {
            // belli bir saniye sonra o next buttonumuzun aktif olmas� laz�m Invoke metodunu kullanaca��m bu y�zden.
            Invoke("CheckStatus", 2f); 
        }
    }

    IEnumerator SecondStarAnim(Image starImg)
    {
        ShowAnim(starImg);

        yield return new WaitForSeconds(delayAnim);

        show2Star = false;

        if (show3Star)
        {
            StartCoroutine("ThirdStarAnim", star3);
        }
        else
        {
            // belli bir saniye sonra o next buttonumuzun aktif olmas� laz�m Invoke metodunu kullanaca��m bu y�zden.
            Invoke("CheckStatus", 2f);
        }
    }

    IEnumerator ThirdStarAnim(Image starImg)
    {
        ShowAnim(starImg);
        yield return new WaitForSeconds(delayAnim);

        show3Star = false;

        // belli bir saniye sonra o next buttonumuzun aktif olmas� laz�m Invoke metodunu kullanaca��m bu y�zden.
        Invoke("CheckStatus", 2f);

    }

    private void ShowAnim(Image starImg)
    {
        // Y�ld�zlar�n ebat� ba�lang��ta 175 e 175, �imdi 225 e 225 yapaca��m animasyonlu olsun diye.
        starImg.rectTransform.sizeDelta = new Vector2(225f, 225f);
        starImg.sprite = goldStar;

        // starImg'nin mevcuttaki ebat bilgisini alaca��m.
        RectTransform temp = starImg.rectTransform;
        // eski ebat�m�za geri d�nd�k. 175 e 175
        temp.DOSizeDelta(new Vector2(175f, 175f), 0.5f, false);

        // �imdi ekrana efekt ve ses verece�iz.
        EffectController.instance.ShowPowerUpEffect(starImg.transform.position);
        AudioController.instance.KeySound(starImg.transform.position);
    }

    // skorumuz, next level skordan b�y�k m� k���k m� onu kontrol edecek
    private void CheckStatus()
    {
        if (true)
        {
            
            if (score >= nextLevelScore)
            {
                // next butonumuz, aktif olacak.
                nextButton.interactable = true;
                // �imdi ekrana efekt ve ses verece�iz.
                EffectController.instance.ShowPowerUpEffect(nextButton.transform.position);
                AudioController.instance.KeySound(nextButton.transform.position);

                // levelin kilidini ortadan kald�rmam laz�m.
                GameController.instance.UnlockLevel(levelnum);
            }
            else
            {
                nextButton.interactable = false;
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
