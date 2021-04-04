using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Transform lastCheckpoint;
    public playerManager player;
    public GameObject pauseMenu;
    public GameObject hitmarker;
    public Text objectiveTextObject;

    public bool isPaused = false;

    public static float gameStartTime;
    public static AudioSource gameMusicPlayer;
    public AudioClip levelMusic;

    private float initialAudioVolume;
    public float pausedAudioVolume = 0.4f;
    private float initialAudioLowPassCutoff;
    public float pausedAudioLowPassCutoff = 400f;

    private void Start()
    {
        gameMusicPlayer = GetComponent<AudioSource>();
        initialAudioVolume = gameMusicPlayer.volume;
        initialAudioLowPassCutoff = GetComponent<AudioLowPassFilter>().cutoffFrequency;

        gameStartTime = Time.time;

        player = GameObject.Find("Player").GetComponent<playerManager>();

        gameMusicPlayer.clip = levelMusic;
        gameMusicPlayer.loop = true;
        gameMusicPlayer.Play();
    }
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

        if (player.health <= 0)
        {
            DoPlayerDeath();
        }
    }

    public void SetCheckpoint(Transform location)
    {
        lastCheckpoint = location;
        var info = location.GetComponent<checkpointInfo>();

        if (info.doesUpdateObjective)
        {
            string objectiveText = info.objectiveUpdateText;
            objectiveTextObject.text = objectiveText;
        }
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

    public void DoPlayerDeath()
    {
        // Show death UI

        // Disable input
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);

        gameMusicPlayer.volume = pausedAudioVolume;
        GetComponent<AudioLowPassFilter>().cutoffFrequency = pausedAudioLowPassCutoff;

        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isPaused = true;
    }

    public void UnPause()
    {
        pauseMenu.SetActive(false);

        gameMusicPlayer.volume = initialAudioVolume;
        GetComponent<AudioLowPassFilter>().cutoffFrequency = initialAudioLowPassCutoff;

        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPaused = false;
    }

    public void Quit()
    {
        // probably show a "Are you sure?" Screen here.
        Application.Quit();
    }

    public IEnumerator ShowHitmarker()
    {
        
        hitmarker.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        hitmarker.SetActive(false);
        
    }

}
