using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataCtrl : MonoBehaviour
{
    // buraya bir datacontroller türünde bir gameobject yapacaðýz.
    public static DataCtrl instance = null;

    // Oyun verilerimizin saklanacaðý GameController'da da yaptýðým GameData'yý oluþturacaðým.
    public GameData data;

    // dosyamýzýn saklanacaðý yerin ismini deðiþken olarak tanýmlýyorum.
    string filePathName;

    // aþþaðýdaki metot, verilerimizi binary formatta saklamamýza yarayacak bir yöntem.
    BinaryFormatter bf;

    // yaptýðýmýz deðiþiklilklerin hepsini mobil cihaza kurgulanmasýný istemiyorsak; developer modu oluþturduk.
    // tikli olduðu zaman, her deðiþiklik telefonun databaseine iþlenecek.
    public bool devMODE;

    // Start'tan önce çalýþan awake metodunu kullanacaðým.
    private void Awake()
    {
        // instance deðiþkenimiz null'sa yani herhangi bir gameobject'i göstermiyor ise; instance'mizi this (DataCtrl'ye) atayacaðýz.
        if (instance == null)
        {
            instance = this;
            // Bu gameobject'inBaþlangýçta oyun açýldýðý zaman diðer herhangi bir sahne'ye de geçtiði zaman silinmesini yok edilmesini istemiyorsak DontDestroyOnLoad metodunu kullanacaðýz.
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }

        bf = new BinaryFormatter();

        filePathName = Application.persistentDataPath + "/game.dat";

        // eðer dosyamýzýn yerini görmek istiyorsak;
        Debug.Log(filePathName);
    }

    // oyundaki güncel bilgileri çekebileceðimiz bir metot yaratmamýz lazým.
    
    public void LoadData()
    {
        if (File.Exists(filePathName))
        {
            FileStream fs = new FileStream(filePathName, FileMode.Open);
            // bilgiyi çekip, data objectimize atmamýz lazým.
            data = (GameData)bf.Deserialize(fs);
            // filestream'imimizi mutlaka kapatmamýz lazým.
            fs.Close();
            Debug.Log("Data Loaded");
        }
    }

    public void SaveData()
    {
        FileStream fs = new FileStream(filePathName, FileMode.Create);
        bf.Serialize(fs, data);
        fs.Close();
        Debug.Log("Data Saved");
    }

    public void SaveData(GameData data)
    {
        FileStream fs = new FileStream(filePathName, FileMode.Create);
        bf.Serialize(fs, data);
        fs.Close();
        Debug.Log("Data Saved");
    }

    public bool isUnlocked(int levelNumber)
    {
        return data.levelData[levelNumber].unLocked;
    }

    // oyunu baþlatýýðýmýzda, nerede kalmýþsak oradaki bilgileri çekip, oyuna basacak.
    private void OnEnable()
    {
        // LoadData();
        DatabaseControl();
    }
    
    void DatabaseControl()
    {
        // filepathname adýnda dosya var mý yok mu bunun kontrolünü yapýyoruz.
        if (!File.Exists(filePathName))
        {
            #if UNITY_ANDROID

            DatabaseCopy();

            #endif
        }
        else
        {
            if (SystemInfo.deviceType == DeviceType.Desktop)
            {
                string destinationFile = System.IO.Path.Combine(Application.streamingAssetsPath, "game.dat");
                File.Delete(destinationFile);
                File.Copy(filePathName, destinationFile);
            }
            if (devMODE)
            {
                if (SystemInfo.deviceType == DeviceType.Handheld)
                {
                    File.Delete(filePathName);
                    DatabaseCopy();
                }
            }

            LoadData();
        }
        
    }
    
    void DatabaseCopy()
    {
        string file = Path.Combine(Application.streamingAssetsPath, "game.dat");
        WWW data = new WWW(file);

        while (!data.isDone)
        {

        }

        File.WriteAllBytes(filePathName, data.bytes);
        LoadData();
    }
}
