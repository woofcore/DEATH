using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumper : MonoBehaviour
{
    public float jumpheight = 30f;
    float initialHeight;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            initialHeight = other.GetComponent<quakeMovement>().jumpSpeed;
            other.GetComponent<quakeMovement>().jumpSpeed = 35f;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<quakeMovement>().jumpSpeed = initialHeight;
        }
    }

}