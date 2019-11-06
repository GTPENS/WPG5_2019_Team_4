using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{

    public GameObject playerPref;

    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayer();
        
    }

    public void SpawnPlayer()
    {
        PhotonNetwork.Instantiate(playerPref.name, playerPref.transform.position, playerPref.transform.rotation);
    }
}
