using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


public struct Weapon
{
    public string name { get; set; } // Name of the weapon.

    public int bulletCount { get; set; } // How many bullets are fired.
    public float bulletSpread { get; set; } // How far a bullet may stray from the center.
    public float bulletDamage { get; set; } // How much damage the bullet does to entities.
    public float bulletSpeed { get; set; } // How fast the bullet travels (doesn't matter if hitscan).
    public float coolDownTime { get; set; } // Time between allowed shots.

    public AudioClip shootNoise { get; set; } // What noise the bullet makes when shot.
    public AudioClip hitNoise { get; set; } // What noise the bullet makes upon impact.

    public GameObject weaponObject { get; set; } // Object the weapon will appear as.
    public Animator weaponAnimator { get; set; } // Animator the weapon uses.
    public VisualEffect muzzleFlash { get; set; } // VFX for muzzle flash (plays on shoot).
    public GameObject bulletDecal { get; set; }

    public GameObject projectileObject { get; set; } // Object the weapon shoots, if projectile.
    public WeaponType currentWeaponType { get; set; } // What type of weapon is this?

    public static bool canShoot = true;
    public GameManager gm;

    public Weapon(WeaponType type, GameObject obj, string name, AudioClip shoot, AudioClip hit, VisualEffect muzzle, GameObject decal, float cooldown)
    {
        this.name = name;

        this.currentWeaponType = type;
        this.bulletCount = 1;
        this.bulletSpread = 0;
        this.bulletDamage = 1;
        this.bulletSpeed = 1;
        this.coolDownTime = cooldown;

        this.shootNoise = shoot;
        this.hitNoise = hit;
        this.weaponObject = obj;
        this.projectileObject = null;
        this.weaponAnimator = obj.GetComponent<Animator>();
        this.muzzleFlash = muzzle;
        this.bulletDecal = decal;

        this.gm = GameObject.Find("GameManager").GetComponent<GameManager>();
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
                if (hitTag != "Player")
                {
                    if (hitTag == "enemy") // if hitting an enemy
                    {
                        // damage entity:
                        // hitinfo.transform.getcomponent(script).damage(value) or something

                        Debug.DrawRay(Camera.main.transform.position, hitInfo.point, Color.green);

                    }
                    else // if hitting something other than an enemy, i.e. a wall.
                    {
                        Debug.DrawRay(Camera.main.transform.position, hitInfo.point, Color.red);
                    }
                    Debug.Log("HIT: " + hitInfo.transform.name + " WITH: " + name);
                    gm.SpawnDecal(hitInfo.point, Quaternion.LookRotation(hitInfo.normal), bulletDecal, hitNoise);
                }
                
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
        weaponAnimator.SetTrigger("Shoot");
        muzzleFlash.Play();
    }

    void PlayShootSound()
    {
        var src = Camera.main.GetComponent<AudioSource>();
        // Shift Pitch
        if (src.pitch != 1)
        {
            src.pitch = 1;
        }
        
        src.pitch = Random.Range(1f, 1.2f);
        
        src.PlayOneShot(shootNoise);

        
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