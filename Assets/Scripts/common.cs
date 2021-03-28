using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class common : MonoBehaviour
{
    public VisualEffectAsset bullet_rock;
    public VisualEffectAsset bullet_metal;
    public VisualEffectAsset bullet_flesh;
    public VisualEffectAsset bullet_default;
    public VisualEffectAsset bullet_wood;

    public AudioClip sound_rock;
    public AudioClip sound_metal;
    public AudioClip sound_flesh;
    public AudioClip sound_default;
    public AudioClip sound_wood;

    public GameObject PauseMenu;

    public enum surfaceMat
    {
        ROCK,
        METAL,
        FLESH,
        WOOD
    }

    public enum WeaponType
    {
        HITSCAN,
        PROJECTILE
    }

    public static AudioSource PlayClipAt(AudioClip clip, Vector3 pos){
       var tempGO = new GameObject("TempAudio"); // create the temp object
       tempGO.transform.position = pos; // set its position
       var aSource = tempGO.AddComponent<AudioSource>(); // add an audio source
       aSource.clip = clip; // define the clip

       // set other aSource properties here, if desired
       aSource.Play(); // start the sound
       Destroy(tempGO, clip.length); // destroy object after clip duration
       return aSource; // return the AudioSource reference
    }
}
