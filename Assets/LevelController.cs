using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class LevelController : MonoBehaviour
{
    public Button nextButton;
    // Beyaz yýldýzlarýn yerini sarý yýldýzlar alarak, bölüm sonunda alacaðýmýz yýldýz sayýsýný belirleyecek.
    public Sprite goldStar;
    // Ekrandaki beyaz yýldýzlar
    public Image star1, star2, star3;
    // Ekrandaki skorumuz;
    public Text txtScore;

    // Bunlara ilaveten hangi levelde olduðumuzu bilmemiz lazým.
    public int levelnum;
    // Mevcut skorumuzu tutmamýz lazým.
    public int score;
    // Skora göre yýldýz sistemi olacak.
    public int score3Star;
    public int score2Star;
    public int score1Star;
    // diðer levele geçmek için gerekli olan skor miktarý;
    public int nextLevelScore;
    // Yýldýzlara animasyon verelim. Belirli sürelerle gelsin. Bunlarýn deðiþkenlerini tanýmlayacaðým.
    public float startDelayAnim;
    // Bir yýldýz geldikten sonra diðer yýldýzýn gelmesi için biraz beklicez;
    public float delayAnim;
    // iki yýldýz göster;
    private bool show2Star, show3Star;



    void Start()    
    {
        // Ekranda kayýtlý olan skorumu çekeceðim. Bu bilgiler gamecontroller'daki GameData bölümünde mevcut.
        // score = GameController.instance.GetScore();
        // açýlan ekrana skorumu basmam lazým.
        txtScore.text = "" + score;

        // eðer benim skorum, 3 yýldýz kazanmam gereken skordan büyükse; 3 yýldýz göster
        if (score >= score3Star)
        {
            show3Star = true;
            Invoke("GoldStarAnim", startDelayAnim);
        }

        // eðer benim skorum, 2 yýldýz kazanmam gereken skordan büyükse ve 3 yýldýzdan küçükse; 2 yýldýz göster
        if (score >= score2Star && score < score3Star)
        {
            show2Star = true;
            Invoke("GoldStarAnim", startDelayAnim);
        }

        // skorum 0'a eþit deðilse skorum, 1 yýldýz kazanmam gereken skordan küçükse þunu yap;
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

        // ekrana bir yýldýz daha basmasý için
        if (show2Star || show3Star)
        {
            StartCoroutine("SecondStarAnim", star2);
        }

        else
        {
            // belli bir saniye sonra o next buttonumuzun aktif olmasý lazým Invoke metodunu kullanacaðým bu yüzden.
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
            // belli bir saniye sonra o next buttonumuzun aktif olmasý lazým Invoke metodunu kullanacaðým bu yüzden.
            Invoke("CheckStatus", 2f);
        }
    }

    IEnumerator ThirdStarAnim(Image starImg)
    {
        ShowAnim(starImg);
        yield return new WaitForSeconds(delayAnim);

        show3Star = false;

        // belli bir saniye sonra o next buttonumuzun aktif olmasý lazým Invoke metodunu kullanacaðým bu yüzden.
        Invoke("CheckStatus", 2f);

    }

    private void ShowAnim(Image starImg)
    {
        // Yýldýzlarýn ebatý baþlangýçta 175 e 175, þimdi 225 e 225 yapacaðým animasyonlu olsun diye.
        starImg.rectTransform.sizeDelta = new Vector2(225f, 225f);
        starImg.sprite = goldStar;

        // starImg'nin mevcuttaki ebat bilgisini alacaðým.
        RectTransform temp = starImg.rectTransform;
        // eski ebatýmýza geri döndük. 175 e 175
        temp.DOSizeDelta(new Vector2(175f, 175f), 0.5f, false);

        // þimdi ekrana efekt ve ses vereceðiz.
        EffectController.instance.ShowPowerUpEffect(starImg.transform.position);
        AudioController.instance.KeySound(starImg.transform.position);
    }

    // skorumuz, next level skordan büyük mü küçük mü onu kontrol edecek
    private void CheckStatus()
    {
        if (true)
        {
            
            if (score >= nextLevelScore)
            {
                // next butonumuz, aktif olacak.
                nextButton.interactable = true;
                // þimdi ekrana efekt ve ses vereceðiz.
                EffectController.instance.ShowPowerUpEffect(nextButton.transform.position);
                AudioController.instance.KeySound(nextButton.transform.position);

                // levelin kilidini ortadan kaldýrmam lazým.
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
