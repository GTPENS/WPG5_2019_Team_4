using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Playe : MonoBehaviourPun, IPunObservable
{
    public PhotonView photonView;
     
     public float speedMove = 10;
     public float jump = 850;

     private Vector3 smoothMove;

     private GameObject sceneCamera;
     public GameObject playerCamera;

    public SpriteRenderer sr;

    
     public void Start()
     {
         if(photonView.IsMine)
         {
            sceneCamera = GameObject.Find("Main Camera");

            sceneCamera.SetActive(false);
            playerCamera.SetActive(true);
         }
     }

     public void Update()
     {
         if(photonView.IsMine)
         {
             InputProcess();
         }else{
             smoothMovement();
         }
     }

    private void InputProcess()
    {
        var move = new Vector3(Input.GetAxisRaw("Horizontal"), 0);
        transform.position += move * speedMove * Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            sr.flipX = false;
            photonView.RPC("OnDirectionChange_RIGHT", RpcTarget.Others);
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            sr.flipX = true;
            photonView.RPC("OnDirectionChange_LEFT", RpcTarget.Others);
        }
    }

    private void smoothMovement()
    {
        transform.position = Vector3.Lerp(transform.position, smoothMove, Time.deltaTime * 10);
    }

    [PunRPC]
    void OnDirectionChange_LEFT()
    {
        sr.flipX = true;
    }

    [PunRPC]
    void OnDirectionChange_RIGHT()
    {
        sr.flipX = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }else if (stream.IsReading){
            smoothMove = (Vector3) stream.ReceiveNext();
        }
    }
}
