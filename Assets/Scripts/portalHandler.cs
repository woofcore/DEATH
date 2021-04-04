using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portalHandler : MonoBehaviour
{
    public Transform newPosition;
    Transform oldPosition;

    public bool isLevelEnd = false;
    GameManager gm;

    private void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("enterer portal");

        if (other.CompareTag("Player") && !isLevelEnd)
        {
            other.GetComponent<CharacterController>().enabled = false;
            other.GetComponent<quakeMovement>().enabled = false;

            oldPosition = other.transform;

            other.transform.position = newPosition.position;

            other.GetComponent<CharacterController>().enabled = true;
            other.GetComponent<quakeMovement>().enabled = true;
        }
        if (other.CompareTag("Player") && isLevelEnd)
        {
            Debug.Log("level finished!");
        }
    }

}
