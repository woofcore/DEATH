using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform lastCheckpoint;
    public GameObject player;
    public GameObject pauseMenu;

    public bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            Pause();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            UnPause();
        }
    }

    public void SetCheckpoint(Transform location)
    {
        lastCheckpoint = location;
    }

    public void GoToCheckpoint()
    {
        Debug.Log("Returning to last checkpoint...");
        player.GetComponent<CharacterController>().enabled = false;
        player.GetComponent<quakeMovement>().enabled = false;
        player.transform.position = lastCheckpoint.position;
        player.transform.rotation = Quaternion.Euler(player.transform.rotation.x, lastCheckpoint.rotation.y, player.transform.rotation.z);
        player.GetComponent<CharacterController>().enabled = true;
        player.GetComponent<quakeMovement>().enabled = true;

    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        isPaused = true;
    }

    public void UnPause()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;
    }

    public void Quit()
    {
        // probably show a "Are you sure?" Screen here.
        Application.Quit();
    }

}
