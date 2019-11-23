using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullete : MonoBehaviourPun
{
    public float speed = 10f;
    public float destroyTime = 2f;
    public bool shootLeft = false;

    IEnumerator destroyBullete()
    {
        yield return new WaitForSeconds(destroyTime);
        // Destroy(this.gameObject);
        this.GetComponent<PhotonView>().RPC("destroy", RpcTarget.AllBuffered);
    }

    // Start is called before the first frame update
    // void Start()
    // {
    //     StartCoroutine(destroyBullete());
    // }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(Vector3.forward * Time.deltaTime * speed);
        if(!shootLeft)
        transform.Translate(Vector2.right * Time.deltaTime * speed);
        else
            transform.Translate(Vector2.left * Time.deltaTime * speed);
    }

    [PunRPC]
    public void destroy()
    {
        Destroy(this.gameObject);
    }

    [PunRPC]
    public void changeDirection()
    {
        shootLeft = true;
    }
}
