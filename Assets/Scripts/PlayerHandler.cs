using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{

    List<Weapon> Inventory; // Example array.
    int currentWeapon = 0; // Points to location in weapons array.

    Weapon WEP_Pistol;
    public GameObject pistolObj;

    Weapon WEP_Shotgun;
    public GameObject shotgunObj;

    void Start()
    {
        WEP_Pistol = new Weapon(Weapon.WeaponType.HITSCAN, pistolObj, "Pistol");
        WEP_Shotgun = new Weapon(Weapon.WeaponType.HITSCAN, shotgunObj, "Shotgun");

        Inventory = new List<Weapon>();
        Inventory.Add(WEP_Pistol);
        Inventory.Add(WEP_Shotgun);

        for (int i = 0; i < Inventory.Count; i++)
        {
            if (i > 0)
            {
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

        if (Input.GetMouseButtonDown(0))
        {
            Inventory[currentWeapon].Shoot();
        }

    }

    void SwitchWeapon(int direction)
    {

        int oldWeapon = currentWeapon;
        // Move pointer:
        currentWeapon += direction; 

        // Constrain to array length:
        if (currentWeapon > Inventory.Count - 1)
            currentWeapon = 0;
        if(currentWeapon < 0)
            currentWeapon = Inventory.Count - 1;

        // Stuff to actually do:
        // - hide old weapon + play respective hide animation
        Inventory[oldWeapon].Hide();

        // - show new weapon + play respective show animation
        Inventory[currentWeapon].Show();

        // Debug
        Debug.Log("Switched weapon to: " + Inventory[currentWeapon].name + " in slot " + currentWeapon);
    }
}
