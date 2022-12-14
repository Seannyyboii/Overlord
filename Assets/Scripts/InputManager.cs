using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.OnFootActions onFoot;

    GameObject crosshair;

    private PlayerController playerController;
    private PlayerLook look;
    private WeaponSway sway;
    private WeaponSwitching switching;

    private Gun gun;

    private const float doubleShiftTime = .2f;
    private float lastShiftTime;

    private float tacSprintDuration;

    Coroutine fireCoroutine;

    // Start is called before the first frame update
    void Awake()
    {
        crosshair = GameObject.Find("Crosshair");
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;

        playerController = GetComponent<PlayerController>();
        look = GetComponent<PlayerLook>();
        sway = GetComponent<WeaponSway>();
        switching = GetComponent<WeaponSwitching>();
        switching = GameObject.Find("WeaponHolder").GetComponent<WeaponSwitching>();

        onFoot.Movement.started += ctx => playerController.Walk();
        onFoot.Movement.canceled += ctx => playerController.Walk();
        onFoot.Jump.performed += ctx => StartCoroutine(playerController.Jump()); // any time the OnFoot.Jump is performed, a callback context to call the playerController.Jump() function
        onFoot.Customise.performed += ctx => playerController.Inspect();
        onFoot.Flip.performed += ctx => playerController.FlipWeapon();
        onFoot.Flashlight.performed += ctx => playerController.TurnOnFlashlight();

        onFoot.Crouch.started += ctx => playerController.Crouch(); // any time the OnFoot.Crouch is performed, a callback context to call the playerController.Crouch() function
        onFoot.Crouch.canceled += ctx => playerController.Crouch(); // any time the OnFoot.Crouch is performed, a callback context to call the playerController.Crouch() function
        onFoot.Sprint.started += ctx => playerController.Sprint(); // any time the OnFoot.Sprint is performed, a callback context to call the playerController.Sprint() function
        onFoot.Sprint.canceled += ctx => playerController.Sprint();
        onFoot.Sprint.performed += ctx => TacticalSprint();

        onFoot.AimDownSights.started += ctx => AimDownSights();
        onFoot.AimDownSights.canceled += ctx => AimDownSights();

        onFoot.LeanLeft.started += ctx => playerController.isLeaningLeft = true;
        onFoot.LeanLeft.canceled += ctx => playerController.isLeaningLeft = false;

        onFoot.LeanRight.started += ctx => playerController.isLeaningRight = true;
        onFoot.LeanRight.canceled += ctx => playerController.isLeaningRight = false;

        onFoot.Shoot.started += _ => StartFiring();
        onFoot.Shoot.canceled += _ => StopFiring();

        onFoot.Reload.performed += _ => StartCoroutine(gun.Reload());
        onFoot.ChangeFireRate.performed += _ => ChangeFireRate();

        onFoot.WeaponSwitch.performed += ctx => switching.SelectWeapon();
        onFoot.WeaponSwitch.performed += ctx => switching.mouseScrollY = ctx.ReadValue<float>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // tell the playerController to move using the value from the movement action
        playerController.ProcessMove(onFoot.Movement.ReadValue<Vector2>());

        CheckActiveWeapon();

        if (tacSprintDuration > 0)
        {
            tacSprintDuration -= Time.deltaTime;
        }

        if (tacSprintDuration <= 0 && playerController.sprinting)
        {
            playerController.speed = 5f;
            gun.animator.SetBool("isSprinting", false);
        } 
        else if(tacSprintDuration <= 0 && playerController.walking)
        {
            playerController.speed = 2.5f;
            gun.animator.SetBool("isSprinting", false);
        }

        if (playerController.crouching)
        {
            playerController.speed = 1.25f;
        }
    }

    private void LateUpdate()
    {
        // tell the look script to move using the value from the movement action
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
        sway = GameObject.Find("WeaponHolder").GetComponent<WeaponSway>();
        sway.WeaponSwaying(onFoot.Look.ReadValue<Vector2>());

        if (!gun.CanShoot())
        {
            StopFiring();
        }
    }

    private void OnEnable()
    {
        onFoot.Enable();
    }

    private void OnDisable()
    {
        onFoot.Disable();
    }

    void StartFiring()
    {
        fireCoroutine = StartCoroutine(gun.RapidFire());
    }

    void StopFiring()
    {
        if (fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
        }
    }

    void ChangeFireRate()
    {
        gun.rapidFire = !gun.rapidFire;
    }

    void TacticalSprint()
    {
        if (onFoot.Sprint.WasPressedThisFrame())
        {
            float timeSinceLastShift = Time.time - lastShiftTime;

            if(timeSinceLastShift <= doubleShiftTime)
            {
                tacSprintDuration = 5f;
                playerController.speed = 7.5f;
                gun.animator.SetBool("isSprinting", true);
            }
            else
            {
                playerController.speed = 5f;
                gun.animator.SetBool("isSprinting", false);
            }
            lastShiftTime = Time.time;
        }
    }

    void AimDownSights()
    {
        FindObjectOfType<GunSFX>().Aim();
        playerController.aiming = !playerController.aiming;

        if (playerController.aiming && !playerController.sprinting)
        {
            gun.bulletSpread = 0f;
            //crosshair.SetActive(false);
            onFoot.CantedAimDownSights.performed += ctx => CantedAimDownSights();
        }
        else
        {
            gun.bulletSpread = 0.1f;
            //crosshair.SetActive(true);
        }
    }

    private void CheckActiveWeapon()
    {
        if (playerController.primaryHolder.activeSelf)
        {
            gun = playerController.primaryHolder.GetComponentInChildren<Gun>();
        }
        else if (playerController.secondaryHolder.activeSelf)
        {
            gun = playerController.secondaryHolder.GetComponentInChildren<Gun>();
        }
    }

    void CantedAimDownSights()
    {
        if (FindObjectOfType<PlayerController>().primaryHolder.activeSelf && FindObjectOfType<SecondOptic>().targetMesh.name == "Canted Rail" && FindObjectOfType<CantedOptic>().targetMesh.name != "No Canted Optic")
        {
            playerController.cantedAiming = !playerController.cantedAiming;
        }
        else
        {
            playerController.cantedAiming = false;
        }

    }
}
