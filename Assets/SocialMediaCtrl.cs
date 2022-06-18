using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialMediaCtrl : MonoBehaviour
{
    public string facebook, twitter, rating;

    public GameObject socialPanel;

    // panel a��k m� kapal� m� kontrol�n� yapaca��m.
    private bool isOpen;

    private void Start()
    {
        // panelim ilk ba�ta kapal� olacak.
        isOpen = false;

    }

    // facebook sayfas�n� a�ar.
    public void facebookPage()
    {
        Application.OpenURL(facebook);
    }

    // twitter sayfas�n� a�ar.
    public void twitterPage()
    {
        Application.OpenURL(twitter);
    }

    // googleplay sayfas�n� a�ar.
    public void ratingPage()
    {
        Application.OpenURL(rating);
    }

    // Ayarlar butonu i�in panel a��p kapatma ayarlar�.
    public void OpenPanel()
    {
        if (isOpen)
        {
            socialPanel.SetActive(false);
            isOpen = false;
        }
        else
        {
            socialPanel.SetActive(true);
            isOpen = true;
        }
    }
}
