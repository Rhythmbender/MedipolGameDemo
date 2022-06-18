using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    public static AudioController instance;
    public Audio playerAudio;
    public bool bgMusic;
    // GO = GameObject
    public GameObject bgMusicGO;

    public GameObject musicBtn;
    public Sprite musicOn, MusicOff;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        /*
        if (bgMusic)
            bgMusicGO.SetActive(true);
        */

        if (DataCtrl.instance.data.playMusic)
        {
            bgMusicGO.SetActive(true);

            musicBtn.GetComponent<Image>().sprite = musicOn;
        }

        else
        {
            bgMusicGO.SetActive(false);

            musicBtn.GetComponent<Image>().sprite = MusicOff;
        }
    }

    public void JumpSound(Vector3 player)
    {
        AudioSource.PlayClipAtPoint(playerAudio.jumpSound, player);
    }

    public void CoinSound(Vector3 player)
    {
        AudioSource.PlayClipAtPoint(playerAudio.coinSound, player);
    }

    public void FireSound(Vector3 player)
    {
        AudioSource.PlayClipAtPoint(playerAudio.fireSound, player);
    }

    public void EnemyDieSound(Vector3 player)
    {
        AudioSource.PlayClipAtPoint(playerAudio.enemyDieSound, player);
    }

    public void WaterSound(Vector3 player)
    {
        AudioSource.PlayClipAtPoint(playerAudio.waterSound, player);
    }

    public void KeySound(Vector3 player)
    {
        AudioSource.PlayClipAtPoint(playerAudio.keySound, player);
    }

    public void PlayerDieSound(Vector3 player)
    {
        AudioSource.PlayClipAtPoint(playerAudio.playerDieSound, player);
    }
    
    public void MusicOnOff()
    {
        if (DataCtrl.instance.data.playMusic)
        {
            bgMusicGO.SetActive(false);

            musicBtn.GetComponent<Image>().sprite = MusicOff;

            DataCtrl.instance.data.playMusic = false;
        }

        else
        {
            bgMusicGO.SetActive(true);

            musicBtn.GetComponent<Image>().sprite = musicOn;

            DataCtrl.instance.data.playMusic = true;
        }

    }

}

[System.Serializable]

public class Audio
{
    // Z�plad���m�zda ��kacak olan sesi, Unity'nin Inspector k�sm�na tan�mlad�k.
    public AudioClip jumpSound;
    // Coin toplad���m�zda ��kacak olan sesi, Unity'nin Inspector k�sm�na tan�mlad�k.
    public AudioClip coinSound;
    // Ate� etti�imizde ��kacak olan sesi, Unity'nin Inspector k�sm�na tan�mlad�k.
    public AudioClip fireSound;
    // D��man� �ld�rd���m�zde ��kacak olan sesi Unity'nin Inspector k�sm�na tan�mlad�k.
    public AudioClip enemyDieSound;
    // Suya d��t���m�zde ��kacak olan sesi Unity'nin Inspector k�sm�na tan�mlad�k.
    public AudioClip waterSound;
    // Anahtar� toplad���m�zda ��kacak olan sesi Unity'nin Inspector k�sm�na tan�mlad�k.
    public AudioClip keySound;
    // �ld���m�zde ��kacak olan sesi Unity'nin Inspector k�sm�na tan�mlad�k.
    public AudioClip playerDieSound;

}
