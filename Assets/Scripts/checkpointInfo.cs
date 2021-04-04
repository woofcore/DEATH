using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpointInfo : MonoBehaviour
{
    public bool doesUpdateObjective = false;
    public string objectiveUpdateText = "default objective";
    public bool doesSpawnEnemies = false;
    public GameObject[] enemiesToSpawn;
    GameManager gm;
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < enemiesToSpawn.Length - 1; i++)
        {
            enemiesToSpawn[i].SetActive(true);
            Debug.Log("Spawned Enemy " + enemiesToSpawn[i].name + " at " + enemiesToSpawn[i].transform.position);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gm.SetCheckpoint(transform);
            if (doesSpawnEnemies)
            {
                SpawnEnemies();
            }
        }
    }
}
