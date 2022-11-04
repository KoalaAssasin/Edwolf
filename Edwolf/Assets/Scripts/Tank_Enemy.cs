using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Enemy : MonoBehaviour
{
    private GameObject wayPoint;
    private GameObject bullet;
    private Vector3 wayPointPos;
    //This will be the enemy's speed. Adjust as necessary.
    public float enemySpeed = 1.1f;
    private int health = 8;
    float oldPosition;
    float IFrames = 0.5f;
    bool injured = false;

    public Player_Movement playerScript;

    [SerializeField] Transform playerCheck;
    [SerializeField] LayerMask playerMask;

    void Start()
    {
        //When spawned, enemies will find the gameobject called wayPoint.
        wayPoint = GameObject.Find("Waypoint");
    }

    void Update()
    {
        //Only moves when it sees the player
        if (Physics.CheckSphere(playerCheck.position, 7f, playerMask) == true)
        {
            //To make them stunned during IFrames
            if (injured == false)
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

            }
        }

        if (injured == true)
        {
            IFrames -= Time.deltaTime;
        }
        if (IFrames <= 0)
        {
            injured = false;
            IFrames = 0.2f;
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
