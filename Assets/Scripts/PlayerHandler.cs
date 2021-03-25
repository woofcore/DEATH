using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerHandler : MonoBehaviour
{
    public float swayAmount;
    public float swaySmoothAmount;
    public float swayRotationMultiplier;

    bool crouching = false;

    public Transform swayRotationPoint;
    CharacterController cc;

    Vector3 swayInitialPosition;
    Quaternion swayInitialRotation;

    List<Weapon> Inventory; // Example array.
    int currentWeapon = 0; // Points to location in weapons array.
    bool canShoot = true;

    public AudioClip impactSound;
    public GameObject decalVFX;

    Weapon WEP_Pistol;
    public GameObject pistolObj;
    public AudioClip pistolShoot;
    public AudioClip pistolSwitchTo;
    public GameObject pistolShootPoint;
    VisualEffect pistolVFX;



    Weapon WEP_Shotgun;
    public GameObject shotgunObj;

    void Start()
    {
        cc = GetComponent<CharacterController>();


        pistolVFX = pistolShootPoint.GetComponent<VisualEffect>();

        // Add weapons!
        WEP_Pistol = new Weapon(Weapon.WeaponType.HITSCAN, pistolObj, "Pistol", pistolShoot, impactSound, pistolVFX, decalVFX, .1f);
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

        swayInitialPosition = Inventory[currentWeapon].weaponObject.transform.localPosition;
    }
    void Update()
    {
        // Get Input
        float scrollInput = Input.mouseScrollDelta.y;

        
        if (scrollInput >= .5f && scrollInput != 0) {
            SwitchWeapon(1);

            swayInitialPosition = Inventory[currentWeapon].weaponObject.transform.localPosition;
        } 
        if(scrollInput <= -.5 && scrollInput != 0) {
            SwitchWeapon(-1);

            swayInitialPosition = Inventory[currentWeapon].weaponObject.transform.localPosition;
        }

        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            // Shoot currently selected weapon.
            Inventory[currentWeapon].Shoot();

            // Cooldown until next shot:
            canShoot = false;
            StartCoroutine(ShootCooldown(Inventory[currentWeapon].coolDownTime));
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Dash();
        }

        /*
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (!crouching)
            {
                Crouch();
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {

        }
        */

        // Weapon Sway
        float swayX = -Input.GetAxis("Mouse X") * swayAmount;
        float swayY = -Input.GetAxis("Mouse Y") * swayAmount;
        Vector3 finalSwayPosition = new Vector3(-Input.GetAxis("Horizontal") * swayX, swayY, 0);
        Inventory[currentWeapon].weaponObject.transform.localPosition = Vector3.Lerp(Inventory[currentWeapon].weaponObject.transform.localPosition, finalSwayPosition + swayInitialPosition, Time.deltaTime * swaySmoothAmount);

        //swayRotationPoint.localRotation = Quaternion.Euler(0, 0, Input.GetAxis("Horizontal") * cc.velocity.magnitude);
        swayRotationPoint.localRotation = Quaternion.Euler(0, 0, Input.GetAxis("Horizontal") * cc.velocity.magnitude * swayRotationMultiplier);
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

    void Dash()
    {

        Vector3 move = cc.transform.TransformDirection(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        cc.Move(move * 5f);
    }

    IEnumerator ShootCooldown(float t)
    {
        yield return new WaitForSeconds(t);
        canShoot = true;
    }
}
