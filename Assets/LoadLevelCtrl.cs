using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadLevelCtrl : MonoBehaviour
{
    public static LoadLevelCtrl instance = null; 

    // biz bu script'i kullanarak loading bar'� ve i�indeki slider de�erini ekrana verece�iz.
    public GameObject loadingUI;
    public Slider slider;

    AsyncOperation test;

    public void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void LoadLevel(string levelName)
    {
        StartCoroutine(LoadNextLevel(levelName));
    }

    IEnumerator LoadNextLevel(string levelName)
    {
        // loadingUI'y� aktif ediyorum.
        loadingUI.SetActive(true);

        // allowSceneActivation ise di�er level'e ge�ip ge�emeyece�imizi kontrol ediyor.
        // progress ise y�kleme i�leminin s�recini s�yl�yor
        test = SceneManager.LoadSceneAsync(levelName);

        // di�er level'e ge�memesini sa�l�yorum.
        test.allowSceneActivation = false;

        // kontrol� while ile sa�layaca��m.
        // y�kleme i�lemi bitene kadar; demek
        // Async metodunda isDone var, bu isDone y�kleme i�leminin yap�l�p yap�lmad���n� g�steriyor (true ya da false)
        while (test.isDone == false)
        {
            // mevcut de�erimizi slider de�erine at�yor.
            slider.value = test.progress;

            // arkaplan'daki y�kleme i�leminde al�nabilecek maksimum de�er 0.9 oldu�u zaman
            if (test.progress == 0.9f)
            {
                // slider de�erimi 1 olarak kaydediyorum.
                slider.value = 1f;

                // di�er level'e ge�mesini sa�l�yorum.
                test.allowSceneActivation = true;
            }

            yield return null;

        }

        

    }


}
