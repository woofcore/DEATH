using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GameManager : MonoBehaviour
{
    public void SpawnDecal(Vector3 pos, Quaternion rot, GameObject bulletDecal, AudioClip sound)
    {
        var ob = Instantiate(bulletDecal, pos, rot);
        ob.GetComponent<VisualEffect>().Play();

        var src = ob.GetComponent<AudioSource>();
        src.PlayOneShot(sound);

        Destroy(ob, 10);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

}
