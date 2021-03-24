using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GameManager : MonoBehaviour
{
    public void SpawnDecal(Vector3 pos, Quaternion rot, VisualEffect bulletDecal)
    {
        Instantiate(bulletDecal, pos, rot);
    }
}
