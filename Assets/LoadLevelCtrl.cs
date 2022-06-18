using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadLevelCtrl : MonoBehaviour
{
    public static LoadLevelCtrl instance = null; 

    // biz bu script'i kullanarak loading bar'ý ve içindeki slider deðerini ekrana vereceðiz.
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
        // loadingUI'yý aktif ediyorum.
        loadingUI.SetActive(true);

        // allowSceneActivation ise diðer level'e geçip geçemeyeceðimizi kontrol ediyor.
        // progress ise yükleme iþleminin sürecini söylüyor
        test = SceneManager.LoadSceneAsync(levelName);

        // diðer level'e geçmemesini saðlýyorum.
        test.allowSceneActivation = false;

        // kontrolü while ile saðlayacaðým.
        // yükleme iþlemi bitene kadar; demek
        // Async metodunda isDone var, bu isDone yükleme iþleminin yapýlýp yapýlmadýðýný gösteriyor (true ya da false)
        while (test.isDone == false)
        {
            // mevcut deðerimizi slider deðerine atýyor.
            slider.value = test.progress;

            // arkaplan'daki yükleme iþleminde alýnabilecek maksimum deðer 0.9 olduðu zaman
            if (test.progress == 0.9f)
            {
                // slider deðerimi 1 olarak kaydediyorum.
                slider.value = 1f;

                // diðer level'e geçmesini saðlýyorum.
                test.allowSceneActivation = true;
            }

            yield return null;

        }

        

    }


}
