using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

  public class ServerManager : MonoBehaviourPunCallbacks
{
    TextMeshProUGUI ServerInfoText;
    GameObject SaveNickNameButton;
    GameObject CreateRoomButton;
    public bool WithIsEndGame;
    void Start()
    {
        ServerInfoText = GameObject.FindWithTag("ServerInfoText").GetComponent<TextMeshProUGUI>();
        PhotonNetwork.ConnectUsingSettings();
        DontDestroyOnLoad(gameObject);
    }

    public override void OnConnectedToMaster()
    {
        ServerInfoText.text = "Waiting To Connect To Server";
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        ServerInfoText.text = "Connected To Server";

        if (!PlayerPrefs.HasKey("NickName"))
        {
           GameObject.FindWithTag("SaveNickButton").GetComponent<Button>().interactable = true;
           
        }
        else
        {
           GameObject.FindWithTag("CreateRoomButton").GetComponent<Button>().interactable = true;
            GameObject.FindWithTag("JoinRoomButton").GetComponent<Button>().interactable = true;

        }
    }

      
   public void JoinRandomRoom()
    {
        PhotonNetwork.LoadLevel(1);
        PhotonNetwork.JoinRandomRoom();
    }
    public void CreataRoomAndJoin()
    {
        PhotonNetwork.LoadLevel(1);
        string RoomName = Random.Range(0, 999999).ToString();
        PhotonNetwork.CreateRoom(RoomName, new RoomOptions { IsVisible = true, MaxPlayers = 2, IsOpen = true }, TypedLobby.Default);
    }





    public override void OnJoinedRoom()
    {
        InvokeRepeating("CheckInformations", 0, 1f);  
       GameObject Player= PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity,0,null);
        Player.GetComponent<PhotonView>().Owner.NickName = PlayerPrefs.GetString("NickName");
        if (PhotonNetwork.PlayerList.Length==2)
        {
            Player.gameObject.tag = "Player2";
            GameObject.FindWithTag("GameControl").gameObject.GetComponent<PhotonView>().RPC("StartCreateGift", RpcTarget.All);
        }      
        
    }

    public override void OnLeftRoom()
    {
        Time.timeScale = 1;
        if (WithIsEndGame)
        {
            PhotonNetwork.ConnectUsingSettings();

        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
            PlayerPrefs.SetInt("TotalMatch", PlayerPrefs.GetInt("TotalMatch") + 1);
            PlayerPrefs.SetInt("TotalLose", PlayerPrefs.GetInt("TotalLose") + 1);
            PlayerPrefs.SetInt("TotalScore", PlayerPrefs.GetInt("TotalScore") - 25);
        }
    

       
    }

    public override void OnLeftLobby()
    {
    
    }


    public override void OnCreatedRoom()
    {
        // oda oluþturulduðunda
    }
   

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
            // odaya oyuncu girince
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Time.timeScale = 1;
        if (!WithIsEndGame)
        {
            PlayerPrefs.SetInt("TotalMatch", PlayerPrefs.GetInt("TotalMatch") + 1);
            PlayerPrefs.SetInt("TotalWin", PlayerPrefs.GetInt("TotalWin") + 1);
            PlayerPrefs.SetInt("TotalScore", PlayerPrefs.GetInt("TotalScore") + 50);
        }
        
        InvokeRepeating("CheckInformations", 0, 1f);
      
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        ServerInfoText.text = "An Error Occurred While Entering The Room.";
       
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        ServerInfoText.text = "Random Error Entering A Room.";
    }


    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        ServerInfoText.text = "Error While Creating Room.";
        
    }

   
    void CheckInformations()
    {
        if (PhotonNetwork.PlayerList.Length==2)
        {
            GameObject.FindWithTag("WaitOtherPlayer").SetActive(false);
            GameObject.FindWithTag("Player1Name").GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[0].NickName;
            GameObject.FindWithTag("Player2Name").GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[1].NickName;
            CancelInvoke("CheckInformations");
        }
        else
        {
           GameObject.FindWithTag("WaitOtherPlayer").SetActive(true);
            GameObject.FindWithTag("Player1Name").GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[0].NickName;
            GameObject.FindWithTag("Player2Name").GetComponent<TextMeshProUGUI>().text ="......";
            
        }
    }
    

}
