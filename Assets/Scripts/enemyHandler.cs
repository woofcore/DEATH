using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHandler : MonoBehaviour
{
    public int health;
    public int contactDamage;

    Transform player;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
    }

    void LateUpdate()
    {
        Quaternion lookRotation = Quaternion.LookRotation (player.position - transform.position);
        lookRotation *= Quaternion.Euler(0, 90, 0);

        
        transform.localRotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
    }
}
