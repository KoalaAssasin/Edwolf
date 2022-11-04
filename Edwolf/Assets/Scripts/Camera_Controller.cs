using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controller : MonoBehaviour
{

    bool cameraFollow = true;
    public GameObject player;

    Vector3 offset = new Vector3(1, 1, 1);

    public Player_Movement playerScript;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (player.transform.position.x < 0)
        {
            cameraFollow = false;
        }
        else
        {
            cameraFollow = true;
        }

        if (cameraFollow)
        {
            transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
        }

    }

}
