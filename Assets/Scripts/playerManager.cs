using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerManager : MonoBehaviour
{
    private int currentWeapon = 0;
    public GameObject pistol;

    private List<weaponHandler> Inventory = new List<weaponHandler>();
    private bool canShoot = true;

    void Start()
    {
        Pickup(pistol);

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

        if (scrollInput >= .5f && scrollInput != 0)
        {
            SwitchWeapon(1);
        }
        if (scrollInput <= -.5 && scrollInput != 0)
        {
            SwitchWeapon(-1);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (canShoot)
            {
                canShoot = false;
                Inventory[currentWeapon].Shoot();
                StartCoroutine(startCooldown(Inventory[currentWeapon].cooldown));
            }
            
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
        if (currentWeapon < 0)
            currentWeapon = Inventory.Count - 1;

        Inventory[oldWeapon].Hide();
        Inventory[currentWeapon].Show();
    }

    void Pickup(GameObject weapon)
    {
        Inventory.Add(weapon.GetComponent<weaponHandler>());
    }

    IEnumerator startCooldown(float t)
    {
        yield return new WaitForSeconds(t);
        canShoot = true;
    }

}
