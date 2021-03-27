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
                        else if (hitTag == "Physics")
                        {
                            hitInfo.rigidbody.AddForceAtPosition(-hitInfo.normal * force, hitInfo.point);
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
        GameObject ob = new GameObject();
        PropMaterialHandler surface = hitObject.GetComponent<PropMaterialHandler>();

        VisualEffect vfx;
        AudioSource src;

        ob = Instantiate(ob, pos, rot);
        vfx = ob.AddComponent<VisualEffect>();
        src = main.GetComponent<AudioSource>();

        // Shift Pitch
        if (src.pitch != 1)
        {
            src.pitch = 1;
        }

        src.pitch = Random.Range(1f, 1.2f);

        switch (surface.surfaceMaterial)
        {
            case common.surfaceMat.ROCK:
                vfx.visualEffectAsset = settings.bullet_rock;
                AudioSource.PlayClipAtPoint(settings.sound_rock, pos);
                break;
            case common.surfaceMat.METAL:
                vfx.visualEffectAsset = settings.bullet_metal;
                AudioSource.PlayClipAtPoint(settings.sound_metal, pos);
                break;
            case common.surfaceMat.FLESH:
                vfx.visualEffectAsset = settings.bullet_flesh;
                AudioSource.PlayClipAtPoint(settings.sound_flesh, pos);
                break;
            default:
                vfx.visualEffectAsset = settings.bullet_default;
                AudioSource.PlayClipAtPoint(settings.sound_default, pos);
                break;
        }

        ob.transform.SetParent(hitObject);
        vfx.Play();

        Destroy(ob, 10);
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
