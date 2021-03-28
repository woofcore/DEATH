using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PropMaterialHandler : MonoBehaviour
{
    public common.surfaceMat surfaceMaterial;
    public bool isBreakable = false;
    public int health;

    public void Damage(int amount)
    {
        if (isBreakable)
        {
            health -= amount;
        }
    }

    private void Update()
    {
        if (health <= 0 && isBreakable)
        {
            Break();
        }
    }

    void Break()
    {
        if (isBreakable)
        {

            Destroy(gameObject, .1f);
        }
    }

}
