using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerName : MonoBehaviour
{

    public InputField nameTF;
    public Button btn;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTFChange()
    {
        if(nameTF.text.Length > 2)
        {
            btn.interactable = true;
        }else{
            btn.interactable = false;
        }
    }

    public void OnClick_SetName()
    {
        PhotonNetwork.NickName = nameTF.text;
    }
}
