using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    // mevcut levellerimizi takip edebileceðimiz levelNumber deðiþkenini tanýmlýyorum.
    int levelNumber;
    // script'in baðlý olduðu butona onu component'ten almamýz gerekiyor.
    Button btn;
    // buton üstündeki image deðiþimi için;
    Image buttonImg;
    // butonun üstündeki yazý için;
    Text buttonTxt;

    // kilitli buton
    public Sprite lockedButton;
    // kilitli olmayan buton
    public Sprite unlockedButton;

    // bölüm adýný tanýmlayacaðým.
    public string levelName;

    // Start is called before the first frame update
    void Start()
    {
        // levelnumber'i çekecez.
        // text olan gameobjectimizi alýp integar'a dönüþtürecek.
        levelNumber = int.Parse(transform.gameObject.name);

        // buton componentine eriþtim.
        btn = transform.gameObject.GetComponent<Button>();
        // buton üstündeki image'i almam lazým.
        buttonImg = btn.GetComponent<Image>();
        // buton üstndeki text'i almam lazým (UI'da 1 yazýyor.). |||  0--> 0.index olan Text elemaný.
        buttonTxt = btn.gameObject.transform.GetChild(0).GetComponent<Text>();

        ButtonStatus();
    }

    // hangi butonlarda kilit var hangilerinde olmayacak. bunun kontrolünü saðlamak lazým.
    void ButtonStatus()
    {
        bool unlocked = DataCtrl.instance.isUnlocked(levelNumber);

        // button kitli mi deðil mi onun kontrolünü yapalým.
        // unlcoked kilidi açýlmýþ demek, ünlemle beraber kilidi açýlmamýþ oluyor. (kitli)
        if (!unlocked)
        {
            // kitli foto gözükecek.
            buttonImg.overrideSprite = lockedButton;
            // hiçbir þey yazmayacak.
            buttonTxt.text = "";
        }
        // buton kitli deðilse aþþaðýsý çalýþýyor, loadscene ile seçtiðimiz bölüm baþlatýlýyor.
        else
        {
            btn.onClick.AddListener(LoadScene);
        }
    }

    void LoadScene()
    {
        // SceneManager.LoadScene(levelName);

        LoadLevelCtrl.instance.LoadLevel(levelName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
