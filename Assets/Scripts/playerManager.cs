using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerManager : MonoBehaviour
{
    private int currentWeapon = 0;
    public GameObject pistol;

    Camera mainCamera;
    float initialFOV;
    public float zoomFOV;
    public float zoomTime = 1f;

    public Transform swayRotationPoint;
    public float swayAmount;
    public float swaySmoothAmount;
    public float swayRotationMultiplier;
    Vector3 swayInitialPosition;
    Quaternion swayInitialRotation;

    private List<weaponHandler> Inventory = new List<weaponHandler>();
    private bool canShoot = true;

    GameManager gm;
    CharacterController cc;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        cc = GetComponent<CharacterController>();
        mainCamera = Camera.main;
        
        Pickup(pistol);
        initialFOV = mainCamera.fieldOfView;
        swayInitialPosition = swayRotationPoint.localPosition;

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
        if (!gm.isPaused)
        {
            // Get Input
            float scrollInput = Input.mouseScrollDelta.y;

            if (scrollInput >= .5f && scrollInput != 0)
            {
                SwitchWeapon(1);
                swayInitialPosition = swayRotationPoint.localPosition;
            }
            if (scrollInput <= -.5 && scrollInput != 0)
            {
                SwitchWeapon(-1);
                swayInitialPosition = swayRotationPoint.localPosition;
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

            if (Input.GetMouseButtonDown(1))
            {
                Zoom("in");
            }
            if (Input.GetMouseButtonUp(1))
            {
                Zoom("out");
            }
        }

        DoWeaponSway();
    }

    private void OnTriggerEnter(Collider other) // Do level trigger stuff.
    {
        string colTag = other.tag;
        if (colTag == "hazard")
        {
            Debug.Log("Hazard! " + other.transform.position);
            gm.GoToCheckpoint();
        }
        if (colTag == "checkpoint")
        {
            gm.SetCheckpoint(other.transform);
            Debug.Log("Checkpoint! " + gm.lastCheckpoint);
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

    void Zoom(string which)
    {
        switch (which) {
            case "in":
                mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, zoomFOV, zoomTime);
                break;
            case "out":
                mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, initialFOV, zoomTime);
                break;
            default:
                mainCamera.fieldOfView = initialFOV;
                break;
        }
    }

    void DoWeaponSway()
    {
        // Weapon Sway
        float swayX = -Input.GetAxis("Mouse X") * swayAmount;
        float swayY = -Input.GetAxis("Mouse Y") * swayAmount;
        Vector3 finalSwayPosition = new Vector3(swayX, swayY, 0);
        swayRotationPoint.localPosition = Vector3.Lerp(swayRotationPoint.localPosition, finalSwayPosition + swayInitialPosition, Time.deltaTime * swaySmoothAmount);

        //swayRotationPoint.localRotation = Quaternion.Euler(0, 0, Input.GetAxis("Horizontal") * cc.velocity.magnitude);
        swayRotationPoint.localRotation = Quaternion.Euler(0, 0, Input.GetAxis("Horizontal") * cc.velocity.magnitude * swayRotationMultiplier);
    }

    IEnumerator startCooldown(float t)
    {
        yield return new WaitForSeconds(t);
        canShoot = true;
    }

}
