using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class Player : MonoBehaviour
{
    public GameObject BombBullet;
    public GameObject BombBulletPoint;
    public ParticleSystem FireEffectt;
    float BoombFirePower;

    [Header("PowerSettings")]
     Image PowerBar;
    bool PowerBarCheck=true;
    float PowerAmount =0.01f;
    Coroutine PowerCorutine;
    bool AllCheckPower=true;
    GameControl GameControl;

    PhotonView pw;
    public bool IsFireActive=false;
    void Start()
    {
        GameControl = GameControl = GameObject.FindWithTag("GameControl").GetComponent<GameControl>();
        PowerBar = GameObject.FindWithTag("PowerBar").GetComponent<Image>();
        pw = GetComponent<PhotonView>();

        if (pw.IsMine)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                transform.position = GameObject.FindWithTag("Player1CreatePoint").transform.position;
                transform.rotation = GameObject.FindWithTag("Player1CreatePoint").transform.rotation;
              //  gameObject.tag = "Player1";
                BoombFirePower = 2;
            }
            else
            {
                transform.position = GameObject.FindWithTag("Player2CreatePoint").transform.position;
                transform.rotation = GameObject.FindWithTag("Player2CreatePoint").transform.rotation;
              //  gameObject.tag = "Player2";
                BoombFirePower = -2;

            }
           
        }
        InvokeRepeating("IsGameStart", 0, .5f);

    }

    void Update()
    {
        if (pw.IsMine)
        {
            if (Input.touchCount>0 && IsFireActive)
            {
                StopPowerBar();
                IsFireActive = false;
                FireEffectt.Play();
               // GameObject Bombs = Instantiate(BombBullet, BombBulletPoint.transform.position, BombBulletPoint.transform.rotation);
                GameObject Bomb = PhotonNetwork.Instantiate("BombBullet", BombBulletPoint.transform.position, BombBulletPoint.transform.rotation, 0, null);

                Bomb.GetComponent<BombBullet>().SetBombPlayer = gameObject.tag;
                Rigidbody2D rg = Bomb.GetComponent<Rigidbody2D>();
                rg.AddForce(new Vector2(BoombFirePower, 0f) * PowerBar.fillAmount * 14, ForceMode2D.Impulse);

            }
        }

    }


    public void IsGameStart()
    {
        if (PhotonNetwork.PlayerList.Length==2)
        {
            
            if (pw.IsMine)
            {
                PowerCorutine = StartCoroutine(StartPowerBar());
                CancelInvoke("IsGameStart");
                //GameControl  GameControl.StartCreateGift();

            }
        }
    }
    IEnumerator StartPowerBar()
    {
        IsFireActive = true;
        PowerBar.fillAmount = 0;
        PowerBarCheck = true;

        while (true)
        {
            if (PowerBar.fillAmount<1 && PowerBarCheck)
            {
                PowerBar.fillAmount += PowerAmount;
                yield return new WaitForSeconds(0.001f);

            }
            else
            {
                PowerBarCheck = false;
                PowerBar.fillAmount -= PowerAmount;
                yield return new WaitForSeconds(0.001f);

                if (PowerBar.fillAmount ==0 )
                {
                    PowerBarCheck = true;
                }
            }
            

        }
    }

   


    public void StopPowerBar()
    {
        StopCoroutine(PowerCorutine);
        AllCheckPower = false;


    }
    public void StartPower()
    {
        if (pw.IsMine)
        {
            if (!AllCheckPower)
            {
                PowerCorutine = StartCoroutine(StartPowerBar());
                AllCheckPower = true;
            }
        }
       
       
    }

    public void Result(int Value)
    {
     
        if (PhotonNetwork.IsMasterClient)
        {
            if (pw.IsMine)
            {
                if (Value==1)
                {
                    PlayerPrefs.SetInt("TotalMatch", PlayerPrefs.GetInt("TotalMatch") + 1);
                    PlayerPrefs.SetInt("TotalWin", PlayerPrefs.GetInt("TotalWin") + 1);
                    PlayerPrefs.SetInt("TotalScore", PlayerPrefs.GetInt("TotalScore") + 50);
                }
                else
                {
                    PlayerPrefs.SetInt("TotalMatch", PlayerPrefs.GetInt("TotalMatch") + 1);
                    PlayerPrefs.SetInt("TotalLose", PlayerPrefs.GetInt("TotalLose") + 1);
                    PlayerPrefs.SetInt("TotalScore", PlayerPrefs.GetInt("TotalScore") - 25);
                }
            }
        }
        else
        {
            if (pw.IsMine)
            {

           
            if (Value==2)
           {

                PlayerPrefs.SetInt("TotalMatch", PlayerPrefs.GetInt("TotalMatch") + 1);
                PlayerPrefs.SetInt("TotalWin", PlayerPrefs.GetInt("TotalWin") + 1);
                PlayerPrefs.SetInt("TotalScore", PlayerPrefs.GetInt("TotalScore") + 50);
            }
            else
            {
                PlayerPrefs.SetInt("TotalMatch", PlayerPrefs.GetInt("TotalMatch") + 1);
                PlayerPrefs.SetInt("TotalLose", PlayerPrefs.GetInt("TotalLose") + 1);
                PlayerPrefs.SetInt("TotalScore", PlayerPrefs.GetInt("TotalScore") - 25);
            }

            }
        }
        Time.timeScale = 0;
    }
}
