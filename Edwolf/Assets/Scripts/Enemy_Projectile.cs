using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Projectile : MonoBehaviour
{

    Vector3 travelDirection;

    float bulletSpeed = 0.025f; // was previously 0.03f

    float bulletLifetime = 3.0f;


    // Start is called before the first frame update
    void Awake()
    {
        travelDirection = GameObject.Find("Player").transform.position - transform.position;
        travelDirection.Normalize();
        travelDirection *= bulletSpeed;
        transform.position += travelDirection;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += travelDirection;
        bulletLifetime -= Time.deltaTime;
        if (bulletLifetime < 0) Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }
    }
}
