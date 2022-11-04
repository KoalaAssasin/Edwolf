using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    Rigidbody rigibody;
    [SerializeField] float MoveSpeedHorizontal = 5f;
    [SerializeField] float MoveSpeedVertical = 5f;
    [SerializeField] float JumpVelocity = 5f;

    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;

    public GameObject PunchBox;

    public bool punchActive = false;
    bool punchCooldown = false;
    float punchActiveTime = 0.2f;
    public int health = 3;
    string direction = "right";

    float IFrames = 0.3f;
    bool injured = false;

    private void Start()
    {
        rigibody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        rigibody.velocity = new Vector3(horizontalInput * MoveSpeedHorizontal, rigibody.velocity.y, verticalInput * MoveSpeedVertical);

        if (Input.GetKeyDown("x") && GroundTouch() == true)
        {
            rigibody.velocity = new Vector3(rigibody.velocity.x, JumpVelocity, rigibody.velocity.z);
        }

        if (Input.GetKeyDown("left") && direction == "right")
        {
            direction = "left";
            PunchBox.transform.position = new Vector3(PunchBox.transform.position.x - 1f, PunchBox.transform.position.y, PunchBox.transform.position.z);
        }

        if (Input.GetKeyDown("right") && direction == "left")
        {
            direction = "right";
            PunchBox.transform.position = new Vector3(PunchBox.transform.position.x + 1f, PunchBox.transform.position.y, PunchBox.transform.position.z);
        }

        //Punch time
        if (Input.GetKeyDown("z") && punchCooldown == false)
        {
            punchActive = true;
            punchCooldown = true;
        }
        //Countdown the punch active time and cooldown
        if (punchActiveTime > -2f && punchCooldown == true)
        {
            punchActiveTime -= Time.deltaTime;
        }
        //Deactivate after punch active time hits 0
        if (punchActiveTime < 0f && punchActive == true)
        {
            //Debug.Log("punch inactive");
            punchActive = false;
        }
        //Punch cooldown is done 0.2 seconds after punch is active (times to be edited)
        if (punchActiveTime < -0.2f)
        {
            punchCooldown = false;
            punchActiveTime = 0.2f;
            //Debug.Log("punch cooldowned");
        }

        if (injured == true)
        {
            IFrames -= Time.deltaTime;
        }
        if (IFrames <= 0)
        {
            injured = false;
            IFrames = 0.3f;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy" && injured == false)
        {
            health -= 1;
            injured = true;

            if(health < 1)
            {
                Destroy(this.gameObject);
            }
        }
      
    }

    bool GroundTouch()
    {
       return Physics.CheckSphere(groundCheck.position, 0.1f, ground);
    }

}