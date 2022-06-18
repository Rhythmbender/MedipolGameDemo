using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    // mevcut levellerimizi takip edebilece�imiz levelNumber de�i�kenini tan�ml�yorum.
    int levelNumber;
    // script'in ba�l� oldu�u butona onu component'ten almam�z gerekiyor.
    Button btn;
    // buton �st�ndeki image de�i�imi i�in;
    Image buttonImg;
    // butonun �st�ndeki yaz� i�in;
    Text buttonTxt;

    // kilitli buton
    public Sprite lockedButton;
    // kilitli olmayan buton
    public Sprite unlockedButton;

    // b�l�m ad�n� tan�mlayaca��m.
    public string levelName;

    // Start is called before the first frame update
    void Start()
    {
        // levelnumber'i �ekecez.
        // text olan gameobjectimizi al�p integar'a d�n��t�recek.
        levelNumber = int.Parse(transform.gameObject.name);

        // buton componentine eri�tim.
        btn = transform.gameObject.GetComponent<Button>();
        // buton �st�ndeki image'i almam laz�m.
        buttonImg = btn.GetComponent<Image>();
        // buton �stndeki text'i almam laz�m (UI'da 1 yaz�yor.). |||  0--> 0.index olan Text eleman�.
        buttonTxt = btn.gameObject.transform.GetChild(0).GetComponent<Text>();

        ButtonStatus();
    }

    // hangi butonlarda kilit var hangilerinde olmayacak. bunun kontrol�n� sa�lamak laz�m.
    void ButtonStatus()
    {
        bool unlocked = DataCtrl.instance.isUnlocked(levelNumber);

        // button kitli mi de�il mi onun kontrol�n� yapal�m.
        // unlcoked kilidi a��lm�� demek, �nlemle beraber kilidi a��lmam�� oluyor. (kitli)
        if (!unlocked)
        {
            // kitli foto g�z�kecek.
            buttonImg.overrideSprite = lockedButton;
            // hi�bir �ey yazmayacak.
            buttonTxt.text = "";
        }
        // buton kitli de�ilse a��a��s� �al���yor, loadscene ile se�ti�imiz b�l�m ba�lat�l�yor.
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
