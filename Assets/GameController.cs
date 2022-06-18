using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameController : MonoBehaviour
{
    // EffectController'de olduðu gibi GameController oluþturduk.
    // Ölünce yeniden doðma, skorun tutulmasý, can barý gibi þeyler bu controllerde olacak.

    [Tooltip("Restart delay")]
    public float delay;

    public static GameController instance;

    // Aþþaðýda yazdýðýmýz GameData'ya bir object yapmamýz lazým.
    public GameData data;

    public UI ui;

    // NPC'nin altýndaki kutularý temsil ediyor.
    public GameObject wall;

    // Otomatik düþman üretimi için;
    public GameObject enemySpawner;

    public GameObject rewardCoin;


    // kullanacaðýmýz BinaryFormatter
    private BinaryFormatter binaryFormatter;
    // verileri kaydedeceðimiz dosya ismi için;
    private string filePath;

    private bool paused;

    

    private void Awake()
    {
        // instance'm herhangi bir þey referans etmiyorsa;
        if (instance == null)
        {
            // instance'm mevcut class'ý referans etsin.
            instance = this;
        }

        // BinaryFormatter'i Awake fonksiyonumuza tanýttýk.
        binaryFormatter = new BinaryFormatter();
        // Þimdi ise dosyamýzýn yolunu kaydedeceðiz.
        filePath = Application.persistentDataPath + "/game.dat";
    }

    private void Start()
    {
        DataCtrl.instance.LoadData();
        data = DataCtrl.instance.data;
        RefreshUI();
        LoadGame();
        UpdateHearts();

        paused = false;

        // LevelComplete();
    }

    private void Update()
    {
        /*
        // bu kontrol ile delete tuþuna basýnca skorumuz silinecek.
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            DeleteData();
        }
        */

        if (paused)
        {
            Time.timeScale = 0;
        }
        if (!paused)
        {
            Time.timeScale = 1;
        }
    }

    // Oyunu burada reStart edecek.
    public void PlayerDied(GameObject player)
    {
        player.SetActive(false);
        CheckLives();
        // Invoke("RestartLevel", delay);
    }

    public void PlayerHit(GameObject player)
    {
        // Düþmana çarptýðým zaman bana bir kuvvet uygulasýn.
        Rigidbody2D rigid = player.GetComponent<Rigidbody2D>();
        rigid.AddForce(new Vector2(-100f, 300f));
        player.transform.Rotate(new Vector3(0, 0, 30f));
        // öldükten sonra sað-sol tuþlarýný kullanmamam lazým.
        // Bu sað-sol tuþlarý PlayerController'a tanýmlýydý.
        // O aktivasyonlarý false etmem lazým.
        player.GetComponent<PlayerController>().enabled = false;
        // karakterin aþþaðý doðru düþmesini saðlamam lazým, bunun içinde bir çarpýþma olmasý lazým.
        // Karakterimizin üstündeki collider'larý false yapmam lazým. 
        player.GetComponent<Collider2D>().enabled = false;
        // Player'a kayýtlý Feet, RightBulletPos, LeftBulletPos var.
        // Bunlarý deaktif etmemiz lazým.
        foreach (Transform child in player.transform)
        {
            child.gameObject.SetActive(false);
        }
        // bütün eksenlerdeki hýzýmý sabitledim.
        rigid.velocity = Vector2.zero;

        // Öldükten sonra PlayerDied() fonskiyonuma eriþmem lazým.
        StartCoroutine("GamePause", player);
    }

    IEnumerator GamePause(GameObject player)
    {
        yield return new WaitForSeconds(2f);
        PlayerDied(player);
    }

    public void BulletHit(GameObject enemy)
    {
        EffectController.instance.EnemyDie(enemy);
        // Düþman ölme sesi devreye girecek.
        AudioController.instance.EnemyDieSound(enemy.transform.position);
        // Instantiate ile reward coin'imizi oluþturacaðýz. (Düþman ölünce coin çýkacak)
        Instantiate(rewardCoin, enemy.transform.position, Quaternion.identity);
        // Düþman öldükten sonra ödül býrakcak onu alýnca skorumuz daha fazla artacak.
        Destroy(enemy);
        // Update Score

    }

    private void RestartLevel()
    {
        // mevcut aktif sahneyi tekrar baþlatýr.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void CoinCount()
    {
        data.coin += 1;
        ui.myText.text = " " + data.coin;
        // ScoreCount(coinValue);
    }

    public void ScoreCount(int val)
    {
        data.score += val;
        ui.scoreTxt.text = " " + data.score;
    }

    public void KeyCount(int key)
    {
        data.keyValue[key] = true;

        if (key == 0)
            ui.blue.sprite = ui.blueFull;
        else if (key == 1)
            ui.green.sprite = ui.greenFull;
        else if (key == 2)
            ui.yellow.sprite = ui.yellowFull;
    }

    /*
    public void SaveData()
    {
        // veri dosyasý oluþturuyoruz.
        FileStream fileStream = new FileStream(filePath, FileMode.Create);
        // ardýndan oluþturduðumuz dosyaya veri kaydedeceðiz. bunun için;
        // GameData'daki data, coin bilgisini taþýyor. Bunu da üstte yazdýðýmýz fileStream'e ekleyeceðiz.
        binaryFormatter.Serialize(fileStream, data);
        fileStream.Close();
    }
    */

    /*
    public void LoadData()
    {
        // dosyayý yüklemeden önce dosyam var mý yok mu onu kontrol etmem gerekiyor.
        if (File.Exists(filePath))
        {
            FileStream fileStream = new FileStream(filePath, FileMode.Open);
            // dosyamda bilgi varsa bunu çekmem lazým.
            // aþþaðýda yazdýðýmýz kod bize bir deðer döndürüyor, bu deðeri data'ya yolluyoruz.

            data = (GameData) binaryFormatter.Deserialize(fileStream);

            // oyunu açtýðýmýzda, en son toplananki coinleri tekrar ekrana basmasý için aþþaðýdaki kod parçasýný yazýyoruz.
            ui.myText.text = " " + data.coin;

            // data skor bilgisini ekrana basacak.
            ui.scoreTxt.text = " " + data.score;

            // dosyayý kapatacaðýz.
            fileStream.Close();
        }
    }
    */

    public void RefreshUI()
    {
        // oyunu açtýðýmýzda, en son toplananki coinleri tekrar ekrana basmasý için aþþaðýdaki kod parçasýný yazýyoruz.
        ui.myText.text = " " + data.coin;

        // data skor bilgisini ekrana basacak.
        ui.scoreTxt.text = " " + data.score;
    }

    /*
    public void DeleteData()
    {
        // filePath'ý oluþturacak. 
        FileStream fileStream = new FileStream(filePath, FileMode.Create);
        // coin'imi sýfýrla.
        data.coin = 0;
        // skorumu sýfýrla.
        data.score = 0;
        // ayný zamanda oyundaki güncel deðerimi de görmek (görsel olarak, oyundayken) istiyorum.
        ui.myText.text = "0";
        // scoreTxt.text = " " + data.score;
        ui.scoreTxt.text = "0";
        // full canlý olmak istiyorum.
        data.hearts = 3;
        for (int i = 0; i < 3; i++)
        {
            data.keyValue[i] = false;
        }

        foreach (LevelData level in data.levelData)
        {
            if (level.levelNum != 1)
            {
                level.unLocked = false;
            }
        }

        // yeni datalarýmý fileStream'e kaydet.
        binaryFormatter.Serialize(fileStream, data);
        // dosyayý kapat.
        fileStream.Close();
    }
    */

    // oyun yüklendiðinde/çalýþtýðýnda mevcut bilgileri yüklemem lazým.
    private void OnEnable()
    {
        RefreshUI();
    }

    // Oyundan çýkarken verileri kaydetmem gereken metod.
    private void OnDisable()
    {
        // SaveData();
        DataCtrl.instance.SaveData(data);

        Time.timeScale = 1;
    }

    // bu metot çaðrýldýðý zaman, tüm ayarlarýmýz baþa dönecek.
    private void LoadGame()
    {
        if (data.firstLoading)
        {
            data.hearts = 3;
            data.coin = 0;
            data.score = 0;
            data.firstLoading = false;

            // toplanan anahtarlarý oyun baþlatýnca boþ yapacak.
            for (int i = 0; i < 3; i++)
            {
                data.keyValue[i] = false;
            }
        }
    }

    private void  UpdateHearts()
    {
        if (data.hearts == 3)
        {
            ui.heart1.sprite = ui.fullHeart;
            ui.heart2.sprite = ui.fullHeart;
            ui.heart3.sprite = ui.fullHeart;
        }

        // 2 kalbim kaldýysa, 1 kalbim boþ demek
        if (data.hearts == 2)
        {
            ui.heart1.sprite = ui.emptyHeart;
        }
        // 1 kalbim kaldýysa, 2 kalbim boþ demek
        if (data.hearts == 1)
        {
            ui.heart1.sprite = ui.emptyHeart;
            ui.heart2.sprite = ui.emptyHeart;
        }
    }

    private void CheckLives()
    {
        // mevcut canýmý bunun içine atacaðým (geçici olarak)
        int currentHearts = data.hearts;
        // currentHearts'ý 1 azaltýcam. Öldüðüm zaman caným 1 azalacak.
        currentHearts -= 1;
        // güncel canýmý basacðým ekrana.
        data.hearts = currentHearts;

        if (data.hearts == 0)
        {
            data.hearts = 3;
            // mevcut data'mý saklýyorum.
            // SaveData();
            DataCtrl.instance.SaveData(data);

            // Game Over metodunu çaðýrmam lazým belirli bir delay sonra.
            Invoke("GameOver", delay);
        }
        // Canlarým 0'a eþit deðil ise;
        else
        {
            DataCtrl.instance.SaveData(data);
            Invoke("RestartLevel", delay);
        }
    }

    private void GameOver()
    {
        ui.gameOverPanel.SetActive(true);
    }

    public void StopCamera()
    {
        // Burada Camera'ya ulaþýyoruz.
        Camera.main.GetComponent<CameraController>().enabled = false;
    }

    // duvarlar yýkýlacak, efekt ve ses çýkacak ve düþman üretimi duruacak.
    public void DisableWall()
    {
        wall.SetActive(false);
        EffectController.instance.ShowPowerUpEffect(wall.transform.position);
        AudioController.instance.EnemyDieSound(wall.transform.position);
        DisableEnemySpawner();

        // kutular yok olunca oyunu bitireceðim. 3 saniye sonra...
        Invoke("LevelComplete", 3f);
    }

    public void LevelComplete()
    {
        ui.mobileUI.SetActive(false);
        ui.levelCompleteUI.SetActive(true);
    }

    // Düþman üretimi baþlayacak.
    public void EnableSpawner()
    {
        enemySpawner.SetActive(true);
    }

    // Düþman üretiminin durmasý için;
    public void DisableEnemySpawner()
    {
        enemySpawner.SetActive(false);
    }

    public int GetScore()
    {
        return data.score;
    }

    public void UnlockLevel(int levelNum)
    {
        data.levelData[levelNum].unLocked = true;
    }

    public void ShowPauseMenu()
    {
        if (ui.mobileUI.activeInHierarchy)
        {
            ui.mobileUI.SetActive(false);
        }

        ui.pauseUI.SetActive(true);

        paused = true;
    }

    public void HidePauseMenu()
    {
        if (!ui.mobileUI.activeInHierarchy)
        {
            ui.mobileUI.SetActive(true);
        }

        ui.pauseUI.SetActive(false);

        paused = false;
    }
}

[System.Serializable]
public class GameData
{
    // Coinlerimizi tutacak.
    public int coin;
    // Skorumuzu tutacak.
    public int score;
    // can durumumuzu kontrol edecek.
    public int hearts;
    // oyun ilk yüklenirken kontrol edeceðimiz bir bool deðiþkenine ihtiyacýmýz var (can için)
    public bool firstLoading;

    // keyValue'yi bool yapýyorum çünkü anahtarlarý toplarken true ya da false olma durumlarýna ihtiyacým var.
    // False ise içi boþ kalmaya devam edecek, true olursa içini dolduracak ve anahtar toplanmýþ olup
    // oyunumuzun sað üst köþesindeki UI kýsmýnda gösterim yapýlacak.
    public bool[] keyValue;

    public LevelData[] levelData;

    public bool playMusic;


}

[System.Serializable]
public class UI
{
    [Header("Text Özellikler")]
    public Text myText; // Coin Text
    public Text scoreTxt; // Score Text
    
    [Header("Image Özellikler")]
    public Image blue;
    public Image green;
    public Image yellow;
    public Sprite blueFull;
    public Sprite greenFull;
    public Sprite yellowFull;

    public Image heart1;
    public Image heart2;
    public Image heart3;
    public Sprite emptyHeart;
    public Sprite fullHeart;

    [Header("Oyun Sonu Ekranlarý")]
    public GameObject gameOverPanel;
    public GameObject levelCompleteUI;
    public GameObject mobileUI;
    public GameObject pauseUI;
}

[System.Serializable]
public class LevelData
{
    // Kilidimiz kalktý mý kalkmadý mý onun kontrolünü saðlayacaðýz.
    public bool unLocked;

    public int levelNum;

}
