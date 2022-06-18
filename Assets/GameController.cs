using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameController : MonoBehaviour
{
    // EffectController'de oldu�u gibi GameController olu�turduk.
    // �l�nce yeniden do�ma, skorun tutulmas�, can bar� gibi �eyler bu controllerde olacak.

    [Tooltip("Restart delay")]
    public float delay;

    public static GameController instance;

    // A��a��da yazd���m�z GameData'ya bir object yapmam�z laz�m.
    public GameData data;

    public UI ui;

    // NPC'nin alt�ndaki kutular� temsil ediyor.
    public GameObject wall;

    // Otomatik d��man �retimi i�in;
    public GameObject enemySpawner;

    public GameObject rewardCoin;


    // kullanaca��m�z BinaryFormatter
    private BinaryFormatter binaryFormatter;
    // verileri kaydedece�imiz dosya ismi i�in;
    private string filePath;

    private bool paused;

    

    private void Awake()
    {
        // instance'm herhangi bir �ey referans etmiyorsa;
        if (instance == null)
        {
            // instance'm mevcut class'� referans etsin.
            instance = this;
        }

        // BinaryFormatter'i Awake fonksiyonumuza tan�tt�k.
        binaryFormatter = new BinaryFormatter();
        // �imdi ise dosyam�z�n yolunu kaydedece�iz.
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
        // bu kontrol ile delete tu�una bas�nca skorumuz silinecek.
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
        // D��mana �arpt���m zaman bana bir kuvvet uygulas�n.
        Rigidbody2D rigid = player.GetComponent<Rigidbody2D>();
        rigid.AddForce(new Vector2(-100f, 300f));
        player.transform.Rotate(new Vector3(0, 0, 30f));
        // �ld�kten sonra sa�-sol tu�lar�n� kullanmamam laz�m.
        // Bu sa�-sol tu�lar� PlayerController'a tan�ml�yd�.
        // O aktivasyonlar� false etmem laz�m.
        player.GetComponent<PlayerController>().enabled = false;
        // karakterin a��a�� do�ru d��mesini sa�lamam laz�m, bunun i�inde bir �arp��ma olmas� laz�m.
        // Karakterimizin �st�ndeki collider'lar� false yapmam laz�m. 
        player.GetComponent<Collider2D>().enabled = false;
        // Player'a kay�tl� Feet, RightBulletPos, LeftBulletPos var.
        // Bunlar� deaktif etmemiz laz�m.
        foreach (Transform child in player.transform)
        {
            child.gameObject.SetActive(false);
        }
        // b�t�n eksenlerdeki h�z�m� sabitledim.
        rigid.velocity = Vector2.zero;

        // �ld�kten sonra PlayerDied() fonskiyonuma eri�mem laz�m.
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
        // D��man �lme sesi devreye girecek.
        AudioController.instance.EnemyDieSound(enemy.transform.position);
        // Instantiate ile reward coin'imizi olu�turaca��z. (D��man �l�nce coin ��kacak)
        Instantiate(rewardCoin, enemy.transform.position, Quaternion.identity);
        // D��man �ld�kten sonra �d�l b�rakcak onu al�nca skorumuz daha fazla artacak.
        Destroy(enemy);
        // Update Score

    }

    private void RestartLevel()
    {
        // mevcut aktif sahneyi tekrar ba�lat�r.
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
        // veri dosyas� olu�turuyoruz.
        FileStream fileStream = new FileStream(filePath, FileMode.Create);
        // ard�ndan olu�turdu�umuz dosyaya veri kaydedece�iz. bunun i�in;
        // GameData'daki data, coin bilgisini ta��yor. Bunu da �stte yazd���m�z fileStream'e ekleyece�iz.
        binaryFormatter.Serialize(fileStream, data);
        fileStream.Close();
    }
    */

    /*
    public void LoadData()
    {
        // dosyay� y�klemeden �nce dosyam var m� yok mu onu kontrol etmem gerekiyor.
        if (File.Exists(filePath))
        {
            FileStream fileStream = new FileStream(filePath, FileMode.Open);
            // dosyamda bilgi varsa bunu �ekmem laz�m.
            // a��a��da yazd���m�z kod bize bir de�er d�nd�r�yor, bu de�eri data'ya yolluyoruz.

            data = (GameData) binaryFormatter.Deserialize(fileStream);

            // oyunu a�t���m�zda, en son toplananki coinleri tekrar ekrana basmas� i�in a��a��daki kod par�as�n� yaz�yoruz.
            ui.myText.text = " " + data.coin;

            // data skor bilgisini ekrana basacak.
            ui.scoreTxt.text = " " + data.score;

            // dosyay� kapataca��z.
            fileStream.Close();
        }
    }
    */

    public void RefreshUI()
    {
        // oyunu a�t���m�zda, en son toplananki coinleri tekrar ekrana basmas� i�in a��a��daki kod par�as�n� yaz�yoruz.
        ui.myText.text = " " + data.coin;

        // data skor bilgisini ekrana basacak.
        ui.scoreTxt.text = " " + data.score;
    }

    /*
    public void DeleteData()
    {
        // filePath'� olu�turacak. 
        FileStream fileStream = new FileStream(filePath, FileMode.Create);
        // coin'imi s�f�rla.
        data.coin = 0;
        // skorumu s�f�rla.
        data.score = 0;
        // ayn� zamanda oyundaki g�ncel de�erimi de g�rmek (g�rsel olarak, oyundayken) istiyorum.
        ui.myText.text = "0";
        // scoreTxt.text = " " + data.score;
        ui.scoreTxt.text = "0";
        // full canl� olmak istiyorum.
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

        // yeni datalar�m� fileStream'e kaydet.
        binaryFormatter.Serialize(fileStream, data);
        // dosyay� kapat.
        fileStream.Close();
    }
    */

    // oyun y�klendi�inde/�al��t���nda mevcut bilgileri y�klemem laz�m.
    private void OnEnable()
    {
        RefreshUI();
    }

    // Oyundan ��karken verileri kaydetmem gereken metod.
    private void OnDisable()
    {
        // SaveData();
        DataCtrl.instance.SaveData(data);

        Time.timeScale = 1;
    }

    // bu metot �a�r�ld��� zaman, t�m ayarlar�m�z ba�a d�necek.
    private void LoadGame()
    {
        if (data.firstLoading)
        {
            data.hearts = 3;
            data.coin = 0;
            data.score = 0;
            data.firstLoading = false;

            // toplanan anahtarlar� oyun ba�lat�nca bo� yapacak.
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

        // 2 kalbim kald�ysa, 1 kalbim bo� demek
        if (data.hearts == 2)
        {
            ui.heart1.sprite = ui.emptyHeart;
        }
        // 1 kalbim kald�ysa, 2 kalbim bo� demek
        if (data.hearts == 1)
        {
            ui.heart1.sprite = ui.emptyHeart;
            ui.heart2.sprite = ui.emptyHeart;
        }
    }

    private void CheckLives()
    {
        // mevcut can�m� bunun i�ine ataca��m (ge�ici olarak)
        int currentHearts = data.hearts;
        // currentHearts'� 1 azalt�cam. �ld���m zaman can�m 1 azalacak.
        currentHearts -= 1;
        // g�ncel can�m� basac��m ekrana.
        data.hearts = currentHearts;

        if (data.hearts == 0)
        {
            data.hearts = 3;
            // mevcut data'm� sakl�yorum.
            // SaveData();
            DataCtrl.instance.SaveData(data);

            // Game Over metodunu �a��rmam laz�m belirli bir delay sonra.
            Invoke("GameOver", delay);
        }
        // Canlar�m 0'a e�it de�il ise;
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
        // Burada Camera'ya ula��yoruz.
        Camera.main.GetComponent<CameraController>().enabled = false;
    }

    // duvarlar y�k�lacak, efekt ve ses ��kacak ve d��man �retimi duruacak.
    public void DisableWall()
    {
        wall.SetActive(false);
        EffectController.instance.ShowPowerUpEffect(wall.transform.position);
        AudioController.instance.EnemyDieSound(wall.transform.position);
        DisableEnemySpawner();

        // kutular yok olunca oyunu bitirece�im. 3 saniye sonra...
        Invoke("LevelComplete", 3f);
    }

    public void LevelComplete()
    {
        ui.mobileUI.SetActive(false);
        ui.levelCompleteUI.SetActive(true);
    }

    // D��man �retimi ba�layacak.
    public void EnableSpawner()
    {
        enemySpawner.SetActive(true);
    }

    // D��man �retiminin durmas� i�in;
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
    // oyun ilk y�klenirken kontrol edece�imiz bir bool de�i�kenine ihtiyac�m�z var (can i�in)
    public bool firstLoading;

    // keyValue'yi bool yap�yorum ��nk� anahtarlar� toplarken true ya da false olma durumlar�na ihtiyac�m var.
    // False ise i�i bo� kalmaya devam edecek, true olursa i�ini dolduracak ve anahtar toplanm�� olup
    // oyunumuzun sa� �st k��esindeki UI k�sm�nda g�sterim yap�lacak.
    public bool[] keyValue;

    public LevelData[] levelData;

    public bool playMusic;


}

[System.Serializable]
public class UI
{
    [Header("Text �zellikler")]
    public Text myText; // Coin Text
    public Text scoreTxt; // Score Text
    
    [Header("Image �zellikler")]
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

    [Header("Oyun Sonu Ekranlar�")]
    public GameObject gameOverPanel;
    public GameObject levelCompleteUI;
    public GameObject mobileUI;
    public GameObject pauseUI;
}

[System.Serializable]
public class LevelData
{
    // Kilidimiz kalkt� m� kalkmad� m� onun kontrol�n� sa�layaca��z.
    public bool unLocked;

    public int levelNum;

}
