using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]

public class UI : MonoBehaviour
{

    public Sprite FIVEHP, FOURHP, THREEHP, TWOHP, ONEHP, ZEROHP;
    public Player_Movement playerScript;
    Image image;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (!(image = gameObject.GetComponent<Image>()))
            Debug.Log("I have no Image component! Fix meeeeeeeeeeeee");

        if (playerScript.health == 5)
        {
            image.sprite = FIVEHP;
        }
        if (playerScript.health == 4)
        {
            image.sprite = FOURHP;
        }
        if (playerScript.health == 3)
        {
            image.sprite = THREEHP;
        }
        if (playerScript.health == 2)
        {
            image.sprite = TWOHP;
        }
        if (playerScript.health == 1)
        {
            image.sprite = ONEHP;
        }
        if (playerScript.health == 0)
        {
            image.sprite = ZEROHP;
        }
    }
}
