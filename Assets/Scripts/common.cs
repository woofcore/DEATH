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

    public AudioClip sound_rock;
    public AudioClip sound_metal;
    public AudioClip sound_flesh;
    public AudioClip sound_default;

    public enum surfaceMat
    {
        ROCK,
        METAL,
        FLESH
    }

    public enum WeaponType
    {
        HITSCAN,
        PROJECTILE
    }
}
