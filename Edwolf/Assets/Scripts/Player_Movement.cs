using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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

    float horizontalInput = 0;
    float verticalInput = 0;

    float IFrames = 0.3f;
    bool injured = false;

    public AudioClip damagedSound;
    public AudioClip deathSound;
    public AudioClip healthSound;

    PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();

        controls.Gameplay.Jump.performed += ctx => Jump();

        controls.Gameplay.Attack1.performed += ctx => Attack1();

        controls.Gameplay.MoveLeft.performed += ctx => MoveLeft();
        controls.Gameplay.MoveRight.performed += ctx => MoveRight();
        controls.Gameplay.MoveLeft.canceled += ctx => StopMoveHorizontal();
        controls.Gameplay.MoveRight.canceled += ctx => StopMoveHorizontal();

        controls.Gameplay.MoveUp.performed += ctx => MoveUp();
        controls.Gameplay.MoveDown.performed += ctx => MoveDown();
        controls.Gameplay.MoveUp.canceled += ctx => StopMoveVertical();
        controls.Gameplay.MoveDown.canceled += ctx => StopMoveVertical();
    }

    private void Start()
    {
        rigibody = GetComponent<Rigidbody>();
    }

    void Update()
    {

        // Movin
        rigibody.velocity = new Vector3(horizontalInput * MoveSpeedHorizontal, rigibody.velocity.y, verticalInput * MoveSpeedVertical);

        //Punch time

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

            if (health < 1)
            {
                Destroy(this.gameObject);
                SceneManager.LoadScene("EndMenu");
            }
        }
        if (collision.gameObject.tag == "Health Pickup")
        {
            if (health < 5)
            {
                health += 1;
                GetComponent<AudioSource>().PlayOneShot(healthSound);
            }
            Destroy(collision.gameObject);
        }
      
    }

    bool GroundTouch()
    {
       return Physics.CheckSphere(groundCheck.position, 0.1f, ground);
    }

    public void HitEnemy()
    {
        GetComponent<AudioSource>().PlayOneShot(damagedSound);
    }
    
    public void KillEnemy()
    {
        GetComponent<AudioSource>().PlayOneShot(deathSound);
    }

    public void Jump()
    {
        if (GroundTouch() == true)
        {
            rigibody.velocity = new Vector3(rigibody.velocity.x, JumpVelocity, rigibody.velocity.z);
        }
    }

    public void Attack1()
    {
        if (punchCooldown == false)
        {
            punchActive = true;
            punchCooldown = true;
        }
    }

    public void MoveLeft()
    {
        horizontalInput = -1;

        if (direction == "right")
        {
            direction = "left";
            //PunchBox.transform.position = new Vector3(PunchBox.transform.position.x - 1f, PunchBox.transform.position.y, PunchBox.transform.position.z);
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }
    public void MoveRight()
    {
        horizontalInput = 1;

        if (direction == "left")
        {
            direction = "right";
            //PunchBox.transform.position = new Vector3(PunchBox.transform.position.x + 1f, PunchBox.transform.position.y, PunchBox.transform.position.z);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
    public void StopMoveHorizontal()
    {
        horizontalInput = 0;
    }

    public void MoveDown()
    {
        verticalInput = -1;
    }
    public void MoveUp()
    {
        verticalInput = 1;
    }
    public void StopMoveVertical()
    {
        verticalInput = 0;
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
    }
    void OnDisable()
    {
        controls.Gameplay.Disable();
    }

}