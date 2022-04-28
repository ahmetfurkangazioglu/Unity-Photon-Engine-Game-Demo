using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class CoverWithBox : MonoBehaviour
{
    public Image  HealthBar;
    public float Heal = 100;
    public GameObject HealthBarCanvas;
    GameControl GameControl;

    PhotonView pw;

    private void Start()
    {
        GameControl = GameObject.FindWithTag("GameControl").GetComponent<GameControl>();
        pw = GetComponent<PhotonView>();

    }
    IEnumerator HealBarCanvasActive()
    {
        if (PhotonNetwork.IsMasterClient)
        {

        
        if (!HealthBarCanvas.activeInHierarchy)
        {
            HealthBarCanvas.SetActive(true);

            yield return new WaitForSeconds(2f);

            HealthBarCanvas.SetActive(false);
        }
       }

    }


    [PunRPC]
    public void GetHit(float Hit)
    {
        if (pw.IsMine)
        {

        
        Heal -= Hit;
        HealthBar.fillAmount = Heal / 100;
        if (Heal<=0)
        {
            PhotonNetwork.Instantiate("BoombEffectWithBoomTx", transform.position, transform.rotation, 0, null);
            PhotonNetwork.Destroy(gameObject);
        }
        else
        {
         
            StartCoroutine(HealBarCanvasActive());
        }
        }

    }

}
