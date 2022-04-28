using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;


public class BombBullet : MonoBehaviour
{
   public float HitPower;
   GameControl GameControl;
   Player Player;
    public string SetBombPlayer;
    string WhichPlayer;

    public AudioSource BombExplosionVoice;
    public AudioSource BombFinishExplosionVoice;
    public AudioSource GroundExplosionVoice;


    private void Start()
    {
        BombInfo();
        GameControl = GameObject.FindWithTag("GameControl").GetComponent<GameControl>();
        Player = GameObject.FindWithTag(SetBombPlayer).GetComponent<Player>();
    }

    public void BombInfo()
    {
        if (SetBombPlayer=="Player1")
        {
            WhichPlayer = "Player1";
        }
        else
        {
            WhichPlayer = "Player2";
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    { 
        if (collision.gameObject.CompareTag("CoverWithBox"))
        {
            BombExplosionVoice.Play();
            PhotonNetwork.Instantiate("Hit3", transform.position, transform.rotation, 0, null);
            collision.gameObject.GetComponent<PhotonView>().RPC("GetHit", RpcTarget.All,HitPower);
            PhotonNetwork.Destroy(gameObject);
            //GameControl.VoiceAndEffectS("Bomb", collision.gameObject);
        }
        
        if (collision.gameObject.CompareTag("Ground"))
        {            
            GameControl.VoiceAndEffectS("Ground", collision.gameObject);
            PhotonNetwork.Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Gift"))
        {
            GameControl.HealBox(WhichPlayer);
            PhotonNetwork.Destroy(collision.gameObject);
            PhotonNetwork.Destroy(gameObject);
        }



        if (collision.gameObject.CompareTag("Player1Tower") || collision.gameObject.CompareTag("Player1"))
        {
            BombFinishExplosionVoice.Play();
            PhotonNetwork.Instantiate("BoombEffectWithBoomTx", transform.position, transform.rotation, 0, null);
            GameControl.GetComponent<PhotonView>().RPC("HitPlayer",RpcTarget.All, "Player1", HitPower);
            PhotonNetwork.Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Player2Tower") || collision.gameObject.CompareTag("Player2"))
        {
            BombFinishExplosionVoice.Play();
            PhotonNetwork.Instantiate("BoombEffectWithBoomTx", transform.position, transform.rotation, 0, null);
            // GameControl.VoiceAndEffectS("BombFinish", collision.gameObject);
            GameControl.GetComponent<PhotonView>().RPC("HitPlayer", RpcTarget.All, "Player2", HitPower);
            PhotonNetwork.Destroy(gameObject);

        }
        Player.StartPower();
        PhotonNetwork.Destroy(gameObject);
    }
}
