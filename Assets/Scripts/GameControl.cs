using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class GameControl : MonoBehaviour
{
    GameObject Player1;
    GameObject Player2;
    [Header("Bomb Settings")]
    public ParticleSystem BombExplosionEffect; 
    public AudioSource BombExplosionVoice;

    [Header("Cover With Box")]
    public AudioSource BombFinishExplosionVoice;
    public ParticleSystem BombFinishExplosionEffect;

    [Header("Ground Settings")]

    public ParticleSystem GroundExplosionEffect;
    public AudioSource GroundExplosionVoice;

    [Header("Player Health Settings")]
    public Image Player1_HealthBar;
    float Player1_Heal=100;
    public Image Player2_HealthBar;
    float Player2_Heal=100;


    [Header("Gift Settings")]
    public GameObject[] GiftCreatePoint;
    public int AmountGift;

    public bool IsEndGmae=true;
    PhotonView pw;
    void Start()
    {
        pw = GetComponent<PhotonView>();
        
    }


    void Update()
    {
        
    }

    public void VoiceAndEffectS(string Type,GameObject obje)
    {
        switch (Type)
        {
            case "Bomb":
                BombExplosionVoice.Play();
                Instantiate(BombExplosionEffect,obje.gameObject.transform.position,obje.gameObject.transform.rotation);
                break;
            case "BombFinish":
                BombFinishExplosionVoice.Play();
                Instantiate(BombFinishExplosionEffect, obje.gameObject.transform.position, obje.gameObject.transform.rotation);
                break;
            case "Ground":
                Debug.Log("Ground Collider");
             // GroundExplosionVoice.Play();
             //   Instantiate(GroundExplosionVoice, obje.gameObject.transform.position, obje.gameObject.transform.rotation);
                break;

        }
    }

   IEnumerator Gift()
    {        
        AmountGift = 0;
        Debug.Log("calýstý");
        while (true && AmountGift <=7)
        {
            Debug.Log("calýstý2");
            yield return new WaitForSeconds(15f);
            Debug.Log("calýstý3");
            int RandomValue = Random.Range(0, GiftCreatePoint.Length);
            PhotonNetwork.Instantiate("Gift", GiftCreatePoint[RandomValue].transform.position, GiftCreatePoint[RandomValue].transform.rotation, 0, null);
            AmountGift += 1;
        }
        
    }
    [PunRPC]
    public void StartCreateGift()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(Gift());
        }
    }

    [PunRPC]
    public void HitPlayer(string Type,float Hit)
    {             
        switch (Type) 
        {
            case "Player1":
                Player1_Heal -= Hit;
                Player1_HealthBar.fillAmount = Player1_Heal / 100;
                if (Player1_Heal<=0)   
                {

                    foreach (GameObject objem in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[]) 
                    {
                        if (objem.gameObject.CompareTag("GameOverPanel"))
                        {
                            objem.gameObject.SetActive(true);
                            GameObject.FindWithTag("GameOverWin").GetComponent<TextMeshProUGUI>().text = "Player2";
                        }
                    }
                    Win(2);          
                }
                break;

            case "Player2":

                Player2_Heal -= Hit;
                Player2_HealthBar.fillAmount = Player2_Heal / 100;
                if (Player2_Heal <= 0)
                {


                    foreach (GameObject objem in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
                    {
                        if (objem.gameObject.CompareTag("GameOverPanel"))
                        {
                            objem.gameObject.SetActive(true);
                            GameObject.FindWithTag("GameOverWin").GetComponent<TextMeshProUGUI>().text = "Player1";
                        }
                    }
                    Win(1);           
                }

                break;
        }
        

    }
    public void Win(int Value)
    {
        if (IsEndGmae)
        {
            GameObject.FindWithTag("Player1").GetComponent<Player>().Result(Value);
            GameObject.FindWithTag("Player2").GetComponent<Player>().Result(Value);
            IsEndGmae = false;
        }
    }
    public void HealBox(string WhichPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {

     
        switch (WhichPlayer)
        {
            case "Player1":
              
                Player1_Heal +=30;
                if (Player1_Heal>100)
                {
                    Player1_Heal = 100;
                }
                Player1_HealthBar.fillAmount = Player1_Heal / 100;
                break;
            case "Player2":
                Player2_Heal += 30;
                if (Player2_Heal > 100)
                {
                    Player2_Heal = 100;
                }
                Player2_HealthBar.fillAmount = Player2_Heal / 100;
                break;
        }
        }

    }
 
    public void BackToMenu()
    {
        GameObject.FindWithTag("ServerManager").GetComponent<ServerManager>().WithIsEndGame = true;
        PhotonNetwork.LoadLevel(0);
    }
    public void BackToMenuChoose()
    {
        GameObject.FindWithTag("ServerManager").GetComponent<ServerManager>().WithIsEndGame =false;
        PhotonNetwork.LoadLevel(0);
    }
}
