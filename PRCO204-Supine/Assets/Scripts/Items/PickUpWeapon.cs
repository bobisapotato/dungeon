using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpWeapon : MonoBehaviour
{
    [SerializeField]
    private GameObject swordPrefab;
    [SerializeField]
    private GameObject crossbowPrefab;

    private GameObject heldWeapon = null;
    private GameObject weaponPlayerIsStandingOn;

    [SerializeField]
    private float timer = 2f;
    private float dropCoolDown = 0.1f;
    private float pickUpCoolDown = 0.2f;

    private PlayerControls controls;

    [SerializeField]
    private bool isStandingOnWeapon = false;

    // model stuff.
    public GameObject playerNoWeapon;
    public GameObject playerSword;
    public GameObject playerCrossbow;

    void Awake()
    {
        controls = new PlayerControls();

        // Controller input.
        controls.Gameplay.PlayerPickUpWeapon.performed += ctx => PickUp();

        switchPlayerModel();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
    }

    // Display UI here: "Pick Up/Press X".
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Weapon" && !isStandingOnWeapon && timer >= pickUpCoolDown)
        {
            isStandingOnWeapon = true;
            weaponPlayerIsStandingOn = other.gameObject;
            timer = 0f;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon" && isStandingOnWeapon)
        {
            isStandingOnWeapon = false;
            weaponPlayerIsStandingOn = null;
        }
    }

    // Spawns a prefab of the weapon they're currently holding.
    void DropWeapon()
    {
        PlayerAttack.isHoldingWeapon = false;
        PlayerAttack.isHoldingRangedWeapon = false;

        if (heldWeapon.name.Contains("Sword"))
        {
            Instantiate(swordPrefab, transform.position, transform.rotation);
        }
        else if (heldWeapon.name.Contains("Crossbow"))
        {
            Instantiate(crossbowPrefab, transform.position, transform.rotation);
        }

        timer = 0f;
        heldWeapon = null;

        // Make the player model one holding nothing:
        switchPlayerModel();
    }

    void PickUp()
    {
        
        // If the cooldown is over.
        if (timer >= dropCoolDown)
        {
            // If the player is holding a weapon, and is not stood on a new one.
            if (PlayerAttack.isHoldingWeapon && !isStandingOnWeapon && weaponPlayerIsStandingOn == null)
            {
                DropWeapon();
            }
            // If the player is holding a weapon, and is stood on a new one.
            else if (PlayerAttack.isHoldingWeapon && isStandingOnWeapon && weaponPlayerIsStandingOn != null)
            {
                DropWeapon();

                if (weaponPlayerIsStandingOn.name.Contains("Sword"))
                {
                    PlayerAttack.isHoldingWeapon = true;
                    PlayerAttack.isHoldingRangedWeapon = false;

                    heldWeapon = weaponPlayerIsStandingOn;
                    
                    // Make the player model one holding a sword:
                    // ...
                }
                else if (weaponPlayerIsStandingOn.name.Contains("Crossbow"))
                {
                    PlayerAttack.isHoldingWeapon = true;
                    PlayerAttack.isHoldingRangedWeapon = true;

                    heldWeapon = weaponPlayerIsStandingOn;

                    // Make the player model one holding a crossbow:
                    // ...
                }

                weaponPlayerIsStandingOn.SetActive(false);
                weaponPlayerIsStandingOn = null;
                isStandingOnWeapon = false;
            }
            // If the player is not holding a weapon and is standing on a new one.
            else if (!PlayerAttack.isHoldingWeapon && isStandingOnWeapon && weaponPlayerIsStandingOn != null)
            {
                if (weaponPlayerIsStandingOn.name.Contains("Sword"))
                {
                    PlayerAttack.isHoldingWeapon = true;
                    PlayerAttack.isHoldingRangedWeapon = false;

                    heldWeapon = weaponPlayerIsStandingOn;
                }
                else if (weaponPlayerIsStandingOn.name.Contains("Crossbow"))
                {
                    PlayerAttack.isHoldingWeapon = true;
                    PlayerAttack.isHoldingRangedWeapon = true;

                    heldWeapon = weaponPlayerIsStandingOn;
                }

                weaponPlayerIsStandingOn.SetActive(false);
                weaponPlayerIsStandingOn = null;
                isStandingOnWeapon = false;
            }
        }
        switchPlayerModel();
    }

    public void switchPlayerModel()
    {
        // based on the heldweapon, change the model used for the player

        if (heldWeapon)
        {
            if (heldWeapon.name.Contains("Sword"))
            {
                showWeapon(playerSword);
            }
            else if (heldWeapon.name.Contains("Crossbow"))
            {
                showWeapon(playerCrossbow);
            }
        }
        else
        {
            showWeapon(playerNoWeapon);
        }
    }

    public void showWeapon(GameObject currentWeapon)
    {
        playerNoWeapon.SetActive(false);
        playerSword.SetActive(false);
        playerCrossbow.SetActive(false);

        currentWeapon.SetActive(true);
    }


    // Required for the input system.
    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }
}
