using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainMenuControl : MonoBehaviour
{
    public GameObject NickNamePanel;
    public GameObject ServerPanel;
    public TMP_InputField NickName;
    public TextMeshProUGUI NickNameText;
    public TextMeshProUGUI[] DataTexts;
    
    void Start()
    {
        if (!PlayerPrefs.HasKey("NickName"))
        {
            NickNamePanel.SetActive(true);
        }
        else
        {
            ServerPanel.SetActive(true);
            NickNameText.text = PlayerPrefs.GetString("NickName");
            GetData();        
        }
    }

  
    public void SaveNickName()
    {
        PlayerPrefs.SetString("NickName", NickName.text);
        NickNamePanel.SetActive(false);
        ServerPanel.SetActive(true);
        NickNameText.text = PlayerPrefs.GetString("NickName");
        PlayerPrefs.SetInt("TotalMatch",0);
        PlayerPrefs.SetInt("TotalLose", 0);
        PlayerPrefs.SetInt("TotalWin", 0);
        PlayerPrefs.SetInt("TotalScore", 0);
        GameObject.FindWithTag("CreateRoomButton").GetComponent<Button>().interactable = true;
        GameObject.FindWithTag("JoinRoomButton").GetComponent<Button>().interactable = true;

    }
    public void GetData()
    {
        DataTexts[0].text = PlayerPrefs.GetInt("TotalMatch").ToString();
        DataTexts[1].text = PlayerPrefs.GetInt("TotalLose").ToString();
        DataTexts[2].text = PlayerPrefs.GetInt("TotalWin").ToString();
        DataTexts[3].text = PlayerPrefs.GetInt("TotalScore").ToString();
    }

}
