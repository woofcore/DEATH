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
    private GameManager gm;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
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
                        else if (hitTag == "Breakable")
                        {
                            if (hitInfo.transform.GetComponent<PropMaterialHandler>().isBreakable)
                            {
                                hitInfo.transform.GetComponent<PropMaterialHandler>().Damage(1);
                                StartCoroutine(gm.ShowHitmarker());
                            }
                        }
                        else // if hitting something other than an enemy, i.e. a wall.
                        {
                            Debug.DrawRay(Camera.main.transform.position, hitInfo.point, Color.red);
                        }
                        Debug.Log("HIT: " + hitInfo.transform.name + " WITH: " + name);
                        if (hitInfo.transform.GetComponent<PropMaterialHandler>())
                        {
                            SpawnDecal(hitInfo.point, Quaternion.LookRotation(hitInfo.normal), hitInfo.transform);
                        }
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
        AudioSource src;


        switch (surface.surfaceMaterial)
        {
            case common.surfaceMat.ROCK:
                vfx.visualEffectAsset = settings.bullet_rock;
                src = common.PlayClipAt(settings.sound_rock, pos);
                break;
            case common.surfaceMat.METAL:
                vfx.visualEffectAsset = settings.bullet_metal;
                src = common.PlayClipAt(settings.sound_metal, pos);
                break;
            case common.surfaceMat.FLESH:
                vfx.visualEffectAsset = settings.bullet_flesh;
                src = common.PlayClipAt(settings.sound_flesh, pos);
                break;
            case common.surfaceMat.WOOD:
                vfx.visualEffectAsset = settings.bullet_wood;
                src = common.PlayClipAt(settings.sound_wood, pos);
                break;
            default:
                vfx.visualEffectAsset = settings.bullet_default;
                src = common.PlayClipAt(settings.sound_default, pos);
                break;
        }

        src.spatialBlend = 1;
        src.volume = .7f;

        // Shift Pitch
        if (src.pitch != 1)
        {
            src.pitch = 1;
        }

        src.pitch = Random.Range(.8f, 1.4f);

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
