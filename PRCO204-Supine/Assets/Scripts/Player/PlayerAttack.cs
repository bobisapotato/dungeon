using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // Variables
    Collider hitbox;
    [SerializeField] 
    GameObject meleeObject;

    [SerializeField]
    GameObject projectile;

    private PlayerControls controls;
    private float rightTriggerDown;

    private float timer = 0f;
    private float reloadTime = 0.5f;

    public static bool isHoldingWeapon;
    public static bool isHoldingRangedWeapon;

    public Animator animator;

    void Awake()
    {
        controls = new PlayerControls();

        // Controller input.
        controls.Gameplay.PlayerAttack.performed += ctx => rightTriggerDown
        = ctx.ReadValue<float>();
        controls.Gameplay.PlayerAttack.canceled += ctx => rightTriggerDown
        = ctx.ReadValue<float>();


        animator = this.gameObject.GetComponent<Animator>();
    }

    // Start is called before the first frame update
    // Gets the collider of the gameobject and stores it 
    // as hitbox for later reference.
    void Start()
    {
        hitbox = meleeObject.GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame and checks for key presses.
    // Executes the corrisponding attack.
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= reloadTime && isHoldingWeapon)
        {
            if (!isHoldingRangedWeapon && (Input.GetKeyDown(KeyCode.Mouse0) == true || rightTriggerDown != 0))
            {
                Attack1();
                timer = 0f;

                // Play "swinging a sword" animation:
                animator.Play("SwingSword2");

            }
            else if (isHoldingRangedWeapon && (Input.GetKeyDown(KeyCode.Mouse1) == true || rightTriggerDown != 0))
            {
                Attack2();
                timer = 0f;

                // Play "pulling back the crossbow" animation:
                // ...
            }
        }
    }

    // Enables the hitbox to attack.
    private void Attack1()
    {
        hitbox.enabled = true;
        StartCoroutine(DisableCollider());
    }

    // Instantiates the projectile to be shot.
    private void Attack2()
    {
        Instantiate(projectile, meleeObject.transform.position, meleeObject.transform.rotation);
    }

    // After 1 second, if nothing with the enemy tag
    // is in the hitbox, it disables it.
    private IEnumerator DisableCollider()
    {
        yield return new WaitForSeconds(1.0f);

        hitbox.enabled = false;
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
