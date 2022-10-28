using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_Enemy : MonoBehaviour
{
    private GameObject wayPoint;
    private GameObject bullet;
    private Vector3 wayPointPos;
    //This will be the enemy's speed. Adjust as necessary.
    public float enemySpeed = 6.0f;
    private int health = 3;
    float oldPosition;
    float IFrames = 0.5f;
    bool injured = false;

    public Player_Movement playerScript;

    void Start()
    {
        //When spawned, enemies will find the gameobject called wayPoint.
        wayPoint = GameObject.Find("Waypoint");
    }

    void Update()
    {
        if (oldPosition < transform.position.x) //Sprite rotator
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        oldPosition = transform.position.x;

        wayPointPos = new Vector3(wayPoint.transform.position.x, wayPoint.transform.position.y, wayPoint.transform.position.z);
        //Here the enemies will follow the waypoint.
       transform.position = Vector3.MoveTowards(transform.position, wayPointPos, enemySpeed * Time.deltaTime);

        if (injured == true)
        {
            IFrames -= Time.deltaTime;
        }
        if (IFrames <= 0)
        {
            injured = false;
            IFrames = 0.5f;
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "PlayerPunchBox")
        {

            if (playerScript.punchActive && injured == false)
            {
                health -= 1;
                injured = true;
            }
            if (health == 0)
            {
                Destroy(this.gameObject);
            }

        }
    }
}
