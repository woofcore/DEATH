using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerHandler : MonoBehaviour
{
    

    List<Weapon> Inventory; // Example array.
    int currentWeapon = 0; // Points to location in weapons array.
    bool canShoot = true;

    Weapon WEP_Pistol;
    public GameObject pistolObj;
    public AudioClip pistolShoot;
    public AudioClip pistolSwitchTo;
    public GameObject pistolShootPoint;
    VisualEffect pistolVFX;

    public VisualEffect pistolDecalVFX;

    Weapon WEP_Shotgun;
    public GameObject shotgunObj;

    void Start()
    {
        pistolVFX = pistolShootPoint.GetComponent<VisualEffect>();

        // Add weapons!
        WEP_Pistol = new Weapon(Weapon.WeaponType.HITSCAN, pistolObj, "Pistol", pistolShoot, pistolVFX, pistolDecalVFX, .1f);
        WEP_Pistol.shootNoise = pistolShoot;
        //WEP_Pistol.coolDownTime = .8f;
        //WEP_Shotgun = new Weapon(Weapon.WeaponType.HITSCAN, shotgunObj, "Shotgun");

        // Add weapons to Inventory list!
        Inventory = new List<Weapon>();
        Inventory.Add(WEP_Pistol);
        //Inventory.Add(WEP_Shotgun);

        for (int i = 0; i < Inventory.Count; i++)
        {
            if (i > 0)
            {
                // Loop through all weapons and hide those that aren't selected.
                Inventory[i].Hide();
            }
        }

    }
    void Update()
    {
        // Get Input
        float scrollInput = Input.mouseScrollDelta.y;

        
        if (scrollInput >= .5f && scrollInput != 0) {
            SwitchWeapon(1);
        } 
        if(scrollInput <= -.5 && scrollInput != 0) {
            SwitchWeapon(-1);
        }

        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            // Shoot currently selected weapon.
            Inventory[currentWeapon].Shoot();

            // Cooldown until next shot:
            canShoot = false;
            StartCoroutine(ShootCooldown(Inventory[currentWeapon].coolDownTime));
        }

    }

    void SwitchWeapon(int direction)
    {
        // Temporarily store the old weapon,
        int oldWeapon = currentWeapon;
        
        // Move pointer:
        currentWeapon += direction;

        // Constrain scrollwheel input to upper & lower bounds of Inventory list:
        if (currentWeapon > Inventory.Count - 1)
            currentWeapon = 0;
        if(currentWeapon < 0)
            currentWeapon = Inventory.Count - 1;

        // Hide old weapon, show newly selected one.
        Inventory[oldWeapon].Hide();
        Inventory[currentWeapon].Show();
        /* Note:
            To play SwitchTo animation, make SwitchTo the default and transition it to Idle,
            this way, SwitchTo will play whenever the object is enabled.
        */

        // Debug
        // Debug.Log("Switched weapon to: " + Inventory[currentWeapon].name + " in slot " + currentWeapon);
    }

    IEnumerator ShootCooldown(float t)
    {
        yield return new WaitForSeconds(t);
        canShoot = true;
    }
}
