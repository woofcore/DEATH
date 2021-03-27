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

    private List<weaponHandler> Inventory = new List<weaponHandler>();
    private bool canShoot = true;

    UIInputHandler uiHandler;

    void Start()
    {
        uiHandler = GameObject.Find("UI").GetComponent<UIInputHandler>();
        mainCamera = Camera.main;
        Pickup(pistol);
        initialFOV = mainCamera.fieldOfView;

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
        if (!uiHandler.isPaused)
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

            if (Input.GetMouseButtonDown(1))
            {
                Zoom("in");
            }
            if (Input.GetMouseButtonUp(1))
            {
                Zoom("out");
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

    IEnumerator startCooldown(float t)
    {
        yield return new WaitForSeconds(t);
        canShoot = true;
    }

}
