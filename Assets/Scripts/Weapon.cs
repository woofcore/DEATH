using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct Weapon
{
    public string name { get; set; } // Name of the weapon.

    public int bulletCount { get; set; } // How many bullets are fired.
    public float bulletSpread { get; set; } // How far a bullet may stray from the center.
    public float bulletDamage { get; set; } // How much damage the bullet does to entities.
    public float bulletSpeed { get; set; } // How fast the bullet travels (doesn't matter if hitscan).

    public AudioClip shootNoise { get; set; } // What noise the bullet makes when shot.
    public AudioClip hitNoise { get; set; } // What noise the bullet makes upon impact.

    public GameObject weaponObject { get; set; } // Object the weapon will appear as.
    public GameObject currentWeaponObject { get; set; }
    public Vector3 weaponObjectOffset { get; set; }
    public Animator weaponAnimator { get; set; } // Animator the weapon uses.

    public GameObject projectileObject { get; set; } // Object the weapon shoots, if projectile.
    public WeaponType currentWeaponType { get; set; } // What type of weapon is this?

    public Weapon(WeaponType type, GameObject obj, string name)
    {
        this.name = name;

        this.currentWeaponType = type;
        this.bulletCount = 1;
        this.bulletSpread = 0;
        this.bulletDamage = 1;
        this.bulletSpeed = 1;

        this.shootNoise = null;
        this.hitNoise = null;
        this.weaponObject = obj;
        this.currentWeaponObject = null;
        this.projectileObject = null;
        this.weaponAnimator = obj.GetComponent<Animator>();
        this.weaponObjectOffset = Vector3.zero;
    }

    public void Shoot()
    {
        PlayShootAnimation();
        PlayShootSound();

        if (currentWeaponType == WeaponType.HITSCAN)
        {
            RaycastHit hitInfo;
            float limit = 100f;

            // raycast from centre of screen:
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, limit))
            {
                // check for entity at raycast:

                string hitTag = hitInfo.transform.tag; // Get tag of HitInfo

                if (hitTag == "enemy") // if hitting an enemy
                {
                    // damage entity:
                    // hitinfo.transform.getcomponent(script).damage(value) or something
                    PlayImpactSound();

                    Debug.DrawRay(Camera.main.transform.position, hitInfo.point, Color.green);

                }
                else // if hitting something other than an enemy, i.e. a wall.
                {
                    Debug.DrawRay(Camera.main.transform.position, hitInfo.point, Color.red);
                }

                Debug.Log("HIT: " + hitInfo.transform.name + " WITH: " + name);
            }

        }

        if (currentWeaponType == WeaponType.PROJECTILE || currentWeaponType == WeaponType.HOMING)
        {
            // shoot projectile object:

            // collision check:

            if (currentWeaponType == WeaponType.HOMING)
            {
                // Home
            }
        }
    }
    void PlayShootAnimation()
    {
        //weaponAnimator.SetTrigger("Shoot");
    }
    void PlayShootSound()
    {

    }
    void PlayImpactSound()
    {

    }

    public void Spawn()
    {
        currentWeaponObject = GameObject.Instantiate(weaponObject, Camera.main.transform);
        //Hide();

        currentWeaponObject.name = "weapon_" + name;
        currentWeaponObject.transform.position += weaponObjectOffset;
    }
    public void Hide()
    {
        weaponObject.SetActive(false);
    }
    public void Show()
    {
        weaponObject.SetActive(true);
    }

    public enum WeaponType
    {
        HITSCAN,
        PROJECTILE,
        HOMING
    }
    public enum ProjectileType
    {
        ORB,

    }
}