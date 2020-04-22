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

    public static bool isHoldingKey = false;

    [SerializeField]
    private Mesh openTrapDoorMesh;

    public GameManager gameManager;

    public Models models;

    void Awake()
    {
        controls = new PlayerControls();

        // Controller input.
        controls.Gameplay.PlayerPickUpWeapon.performed += ctx => PickUp();
        models = GetComponentInChildren<Models>();
        gameManager = FindObjectOfType<GameManager>();
        newPlayerWeapon();
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

        if (other.tag == "Key")
        {
            isHoldingKey = true;
            Destroy(other.gameObject);
        }

        if (other.tag == "Trap Door" && isHoldingKey)
        {
            // Open Trap Door
            other.gameObject.GetComponentInChildren<MeshFilter>().mesh = openTrapDoorMesh;

            // Invoke level change OR game end in this method.
            Invoke("NextLevel", 0.5f);
        }
        else if (other.tag == "Trap Door" && !isHoldingKey)
        {
            // Display "You need to find the key" message here:
            // ...

            Debug.Log("You need to find the key");
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
        // ...
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
        newPlayerWeapon();
    }

    void NextLevel()
    {
        // Reset static values for next level.
        LevelGeneration.hasTrapDoorSpawned = false;
        isHoldingKey = false;

        gameManager.openDemoWin2();
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

    public void newPlayerWeapon()
    {
        // finds what current weapon is, and sends message to change model accordingly.
        if (heldWeapon)
        {
            if (heldWeapon.name.Contains("Sword"))
            {
                setPlayerModel(models.sword);
            }
            else if (heldWeapon.name.Contains("Crossbow"))
            {
                setPlayerModel(models.crossbow);
            }
        }
        else
        {
            setPlayerModel(models.noweapon);
        }
    }
    public void setPlayerModel(GameObject currentWeapon)
    {
        // sets all the weapons to disables apart from current heldweapon.
        models.setAll(false);

        currentWeapon.SetActive(true);

    }
}
