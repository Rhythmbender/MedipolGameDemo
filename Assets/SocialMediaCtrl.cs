using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialMediaCtrl : MonoBehaviour
{
    public string facebook, twitter, rating;

    public GameObject socialPanel;

    // panel açýk mý kapalý mý kontrolünü yapacaðým.
    private bool isOpen;

    private void Start()
    {
        // panelim ilk baþta kapalý olacak.
        isOpen = false;

    }

    // facebook sayfasýný açar.
    public void facebookPage()
    {
        Application.OpenURL(facebook);
    }

    // twitter sayfasýný açar.
    public void twitterPage()
    {
        Application.OpenURL(twitter);
    }

    // googleplay sayfasýný açar.
    public void ratingPage()
    {
        Application.OpenURL(rating);
    }

    // Ayarlar butonu için panel açýp kapatma ayarlarý.
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
