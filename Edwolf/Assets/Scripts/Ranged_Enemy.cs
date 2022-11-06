using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged_Enemy : MonoBehaviour
{
    private GameObject wayPoint;
    public GameObject bullet;
    GameObject prefab;

    //This will be the enemy's speed. Adjust as necessary. It's firing speed in this case.
    public float enemySpeed;
    public float enemySpeedReset = 6.0f;
    private int health = 2;
    float IFrames = 0.5f;
    bool injured = false;

    public Player_Movement playerScript;

    [SerializeField] Transform playerCheck;
    [SerializeField] LayerMask playerMask;

    void Start()
    {
        //When spawned, enemies will find the gameobject called wayPoint.
        wayPoint = GameObject.Find("Waypoint");

        enemySpeed = enemySpeedReset;
    }

    void Update()
    {
        //Only moves when it sees the player
        if (Physics.CheckSphere(playerCheck.position, 9f, playerMask) == true)
        {
            //To make them stunned during IFrames
            if (injured == false)
            {
                if (wayPoint.transform.position.x < transform.position.x) //Sprite rotator
                {
                    transform.localRotation = Quaternion.Euler(0, 180, 0);
                }
                else
                {
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                }

                if (enemySpeed == enemySpeedReset)
                {
                    prefab = Instantiate(bullet, transform.position, transform.rotation);
                    prefab.transform.position = new Vector3(prefab.transform.position.x, prefab.transform.position.y + 0.4f, prefab.transform.position.z);

                    enemySpeed -= 0.0001f;
                }
                else if (enemySpeed < enemySpeedReset && enemySpeed > 0)
                {
                    enemySpeed -= Time.deltaTime;
                }
                else
                {
                    enemySpeed = enemySpeedReset;
                }

            }
        }

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
