using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Enemy : MonoBehaviour
{
    private GameObject wayPoint;
    private GameObject bullet;
    public GameObject healthPickup;
    GameObject prefab;
    private Vector3 wayPointPos;
    //This will be the enemy's speed. Adjust as necessary.
    public float enemySpeed = 1.1f;
    private int health = 8;
    float oldPosition;
    float IFrames = 0.5f;
    bool injured = false;
    public float patrolTimer = 3;
    string patrolDirection = "up";

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
        //Only moves to player when it sees the player
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
        //Patrols if not seeing the player
        else
        {
            if (patrolDirection == "up")
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.01f);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.01f);
            }

            patrolTimer -= Time.deltaTime;
            if(patrolTimer < 0 && patrolDirection == "up")
            {
                patrolTimer = 3;
                patrolDirection = "down";
            }
            if (patrolTimer < 0 && patrolDirection == "down")
            {
                patrolTimer = 3;
                patrolDirection = "up";
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

                float pickupChance = Random.Range(0.0f, 1.0f);

                if (pickupChance > 0.75f)
                {
                    prefab = Instantiate(healthPickup, transform.position, transform.rotation);
                }
            }

        }
    }
}
