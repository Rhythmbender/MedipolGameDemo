using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataCtrl : MonoBehaviour
{
    // buraya bir datacontroller t�r�nde bir gameobject yapaca��z.
    public static DataCtrl instance = null;

    // Oyun verilerimizin saklanaca�� GameController'da da yapt���m GameData'y� olu�turaca��m.
    public GameData data;

    // dosyam�z�n saklanaca�� yerin ismini de�i�ken olarak tan�ml�yorum.
    string filePathName;

    // a��a��daki metot, verilerimizi binary formatta saklamam�za yarayacak bir y�ntem.
    BinaryFormatter bf;

    // yapt���m�z de�i�iklilklerin hepsini mobil cihaza kurgulanmas�n� istemiyorsak; developer modu olu�turduk.
    // tikli oldu�u zaman, her de�i�iklik telefonun databaseine i�lenecek.
    public bool devMODE;

    // Start'tan �nce �al��an awake metodunu kullanaca��m.
    private void Awake()
    {
        // instance de�i�kenimiz null'sa yani herhangi bir gameobject'i g�stermiyor ise; instance'mizi this (DataCtrl'ye) atayaca��z.
        if (instance == null)
        {
            instance = this;
            // Bu gameobject'inBa�lang��ta oyun a��ld��� zaman di�er herhangi bir sahne'ye de ge�ti�i zaman silinmesini yok edilmesini istemiyorsak DontDestroyOnLoad metodunu kullanaca��z.
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }

        bf = new BinaryFormatter();

        filePathName = Application.persistentDataPath + "/game.dat";

        // e�er dosyam�z�n yerini g�rmek istiyorsak;
        Debug.Log(filePathName);
    }

    // oyundaki g�ncel bilgileri �ekebilece�imiz bir metot yaratmam�z laz�m.
    
    public void LoadData()
    {
        if (File.Exists(filePathName))
        {
            FileStream fs = new FileStream(filePathName, FileMode.Open);
            // bilgiyi �ekip, data objectimize atmam�z laz�m.
            data = (GameData)bf.Deserialize(fs);
            // filestream'imimizi mutlaka kapatmam�z laz�m.
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

    // oyunu ba�lat����m�zda, nerede kalm��sak oradaki bilgileri �ekip, oyuna basacak.
    private void OnEnable()
    {
        // LoadData();
        DatabaseControl();
    }
    
    void DatabaseControl()
    {
        // filepathname ad�nda dosya var m� yok mu bunun kontrol�n� yap�yoruz.
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
