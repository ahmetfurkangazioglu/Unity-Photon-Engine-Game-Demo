using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GiftSetings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyTime",11f);
    }
    public void DestroyTime()
    {
        PhotonNetwork.Destroy(gameObject);
    }
}

   
