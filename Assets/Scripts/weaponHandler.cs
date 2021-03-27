using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class weaponHandler : MonoBehaviour
{
    public string weaponName = "Gun"; // The name of this weapon.
    public common.WeaponType weaponType;
    public float damage = 10f; // How much damage this weapon inflicts.
    public float force = 100f; // How much force this weapon exerts on rigidbodies.
    public float cooldown = .4f; // How long to wait between shots.
    public int shotCount = 1; // How many bullets the weapon shoots every shot.

    public VisualEffect muzzleFlashEffect;

    public AudioClip shootNoise;

    private Animator weaponAnimator;
    private Camera main;
    private common settings;

    void Start()
    {
        weaponAnimator = GetComponent<Animator>();
        main = Camera.main;
        settings = GameObject.Find("common").GetComponent<common>();
    }

    public void Shoot()
    {
        PlayShootAnimation();
        PlayShootNoise();

        switch (weaponType)
        {
            case common.WeaponType.HITSCAN:
                RaycastHit hitInfo;
                float limit = 100f;
                // raycast from centre of screen:
                if (Physics.Raycast(main.transform.position, main.transform.forward, out hitInfo, limit))
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
                        else if (hitTag == "Physics")
                        {
                            //hitInfo.rigidbody.AddForceAtPosition(-hitInfo.normal * force, hitInfo.point);

                            Vector3 direction = (main.transform.position - hitInfo.transform.position).normalized;

                            hitInfo.rigidbody.AddForce(-direction * force);
                        }
                        else // if hitting something other than an enemy, i.e. a wall.
                        {
                            Debug.DrawRay(Camera.main.transform.position, hitInfo.point, Color.red);
                        }
                        Debug.Log("HIT: " + hitInfo.transform.name + " WITH: " + name);
                        SpawnDecal(hitInfo.point, Quaternion.LookRotation(hitInfo.normal), hitInfo.transform);
                    }

                }
                break;
            case common.WeaponType.PROJECTILE:
                break;
            default:
                break;
        }
    }

    void PlayShootAnimation()
    {
        muzzleFlashEffect.Play();
        weaponAnimator.SetTrigger("shoot");
    }

    void PlayShootNoise()
    {
        var src = main.GetComponent<AudioSource>();
        // Shift Pitch
        if (src.pitch != 1)
        {
            src.pitch = 1;
        }

        src.pitch = Random.Range(1f, 1.2f);

        src.PlayOneShot(shootNoise);
    }

    void SpawnDecal(Vector3 pos, Quaternion rot, Transform hitObject)
    {
        GameObject tempobj = new GameObject();

        tempobj.transform.position = pos;
        tempobj.transform.rotation = rot;
        tempobj.transform.parent = hitObject;

        PropMaterialHandler surface = hitObject.GetComponent<PropMaterialHandler>();
        VisualEffect vfx = tempobj.AddComponent<VisualEffect>();
        AudioSource src = tempobj.AddComponent<AudioSource>();

        src.spatialBlend = 1;
        src.volume = .7f;

        // Shift Pitch
        if (src.pitch != 1)
        {
            src.pitch = 1;
        }

        src.pitch = Random.Range(.8f, 1.4f);

        switch (surface.surfaceMaterial)
        {
            case common.surfaceMat.ROCK:
                vfx.visualEffectAsset = settings.bullet_rock;
                src.PlayOneShot(settings.sound_rock);
                break;
            case common.surfaceMat.METAL:
                vfx.visualEffectAsset = settings.bullet_metal;
                src.PlayOneShot(settings.sound_metal);
                break;
            case common.surfaceMat.FLESH:
                vfx.visualEffectAsset = settings.bullet_flesh;
                src.PlayOneShot(settings.sound_flesh);
                break;
            default:
                vfx.visualEffectAsset = settings.bullet_default;
                src.PlayOneShot(settings.sound_default);
                break;
        }

        vfx.Play();

        Destroy(tempobj, 10);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }


}
