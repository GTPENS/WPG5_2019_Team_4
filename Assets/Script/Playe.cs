using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Playe : MonoBehaviourPun, IPunObservable
{
    public PhotonView photonView;
     
     public float speedMove = 10;
     public float jumpforce = 850;

     private Vector3 smoothMove;

     private GameObject sceneCamera;
     public GameObject playerCamera;

    public SpriteRenderer sr;
    public Text nameText;
    private Rigidbody2D rigidbody;
    private bool IsGrounded;

    public GameObject bulletePrefab;
    public Transform bulleteSpawn;
    public Transform bulleteSpawnLeft;
     public void Start()
     {
         PhotonNetwork.SendRate = 20;
         PhotonNetwork.SerializationRate = 15;
         if(photonView.IsMine)
         {
            nameText.text = PhotonNetwork.NickName;
            rigidbody = GetComponent<Rigidbody2D>();
            sceneCamera = GameObject.Find("Main Camera");

            sceneCamera.SetActive(false);
            playerCamera.SetActive(true);
         }else{
             nameText.text = photonView.Owner.NickName;
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
            photonView.RPC("OnDirectionChange_RIGHT", RpcTarget.Others, null);
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            sr.flipX = true;
            photonView.RPC("OnDirectionChange_LEFT", RpcTarget.Others, null);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            //jump
            Jump();
        }
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            Shoot();
        }
    }

    private void smoothMovement()
    {
        transform.position = Vector3.Lerp(transform.position, smoothMove, Time.deltaTime * 10);
    }

    public void Shoot()
    {
        GameObject bullete;
        
        if(sr.flipX == true)
        {
            bullete = PhotonNetwork.Instantiate(bulletePrefab.name, bulleteSpawnLeft.position, Quaternion.identity);
        }else{
            bullete = PhotonNetwork.Instantiate(bulletePrefab.name, bulleteSpawn.position, Quaternion.identity);
        }

        if(sr.flipX == true)
        {
            bullete.GetComponent<PhotonView>().RPC("changeDirection", RpcTarget.AllBuffered);
        }
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

    void OnCollisionEnter2D(Collision2D other)
    {
        if(photonView.IsMine){
            if(other.gameObject.tag == "Ground"){
                IsGrounded = true;
            }
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if(photonView.IsMine){
            if(other.gameObject.tag == "Ground"){
                IsGrounded = false;
            }
        }
    }

    //[PunRPC]
    public void Jump()
    {
        rigidbody.AddForce(Vector2.up * jumpforce);
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
