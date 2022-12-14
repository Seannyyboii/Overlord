using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController charController;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private bool lerpCrouch;
    public bool lerpSwitch;
    public bool crouching;
    public bool sprinting;
    public bool walking;
    public bool inspecting;
    public bool flipped;
    public bool flashlightActive;

    private float walkBobSpeed = 12f;
    private float walkBobAmount = 0.05f;
    private float runBobSpeed = 16f;
    private float runBobAmount = 0.1f;
    private float crouchBobSpeed = 8f;
    private float crouchBobAmount = 0.025f;
    private float originalYPosition = 0;
    private float timer;

    private float baseStepSpeed = 0.5f;
    private float crouchStepMultiplier = 1.5f;
    private float sprintStepMultplier = 0.6f;
    [SerializeField] private AudioSource footsteps = default;
    [SerializeField] private AudioClip[] grassClips = default;
    [SerializeField] private AudioClip[] defaultClips = default;
    private float footstepTimer = 0;
    private float GetCurrentOffset => crouching ? baseStepSpeed * crouchStepMultiplier : sprinting ? baseStepSpeed * sprintStepMultplier : baseStepSpeed;

    public float speed = 5f;
    public float gravity = -9.8f;
    public float jumpHeight = 1.5f;
    public float crouchTimer = 0f;
    public float switchTimer = 0f;
    public bool aiming;
    public bool cantedAiming;
    public int endTime;

    public Transform leanPivot;
    private float currentLean;
    private float targetLean;
    private float leanVelocity;
    public float leanAngle;
    public float leanSmoothness;

    public bool isLeaningLeft;
    public bool isLeaningRight;

    public GameObject primaryHolder;
    public GameObject secondaryHolder;
    Camera cam;

    private Animator animator;
    private Transform weaponPosition;

    public bool isSwitching;

    public Vector3 primaryAimPosition;
    public Quaternion primaryAimRotation;

    public Vector3 secondaryAimPosition;
    public Quaternion secondaryAimRotation;

    // Start is called before the first frame update
    void Start()
    {
        charController = GetComponent<CharacterController>();
    }

    void Awake()
    {
        cam = Camera.main;
        originalYPosition = cam.transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateLeaning();
        CheckActiveWeapon();

        isGrounded = charController.isGrounded;

        if (isGrounded)
        {
            animator.SetBool("isGrounded", true);
        }
        else
        {
            animator.SetBool("isGrounded", false);
        }

        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1;
            p *= p;
            if (crouching)
            {
                charController.height = Mathf.Lerp(charController.height, 1, p);
            }

            else
            {
                charController.height = Mathf.Lerp(charController.height, 2, p);
            }
                
            if (p > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }

        if (aiming && speed < 5f && !inspecting)
        {
            animator.SetBool("isAiming", true);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 40f, 5f * Time.deltaTime);
            
            if (primaryHolder.activeSelf && !cantedAiming)
            {
                weaponPosition.localPosition = Vector3.Lerp(weaponPosition.localPosition, primaryAimPosition, 10 * Time.deltaTime);
                weaponPosition.localRotation = Quaternion.Lerp(weaponPosition.localRotation, primaryAimRotation, 10 * Time.deltaTime);

                //weaponPosition.localPosition = Vector3.Lerp(weaponPosition.localPosition, new Vector3(0f, 0.093f, 0.4f), 10 * Time.deltaTime);
                //weaponPosition.localRotation = Quaternion.Lerp(weaponPosition.localRotation, Quaternion.Euler(0, 0, -9.825f), 10 * Time.deltaTime);
            }

            else if (primaryHolder.activeSelf && cantedAiming)
            {
                weaponPosition.localPosition = Vector3.Lerp(weaponPosition.localPosition, new Vector3(0.0465f, 0.119f, 0.4f), 10 * Time.deltaTime);
                weaponPosition.localRotation = Quaternion.Lerp(weaponPosition.localRotation, Quaternion.Euler(0, 0, 34.865f), 10 * Time.deltaTime);
            }

            else
            {
                weaponPosition.localPosition = Vector3.Lerp(weaponPosition.localPosition, secondaryAimPosition, 10 * Time.deltaTime);
                weaponPosition.localRotation = Quaternion.Lerp(weaponPosition.localRotation, secondaryAimRotation, 10 * Time.deltaTime);

                //weaponPosition.localPosition = Vector3.Lerp(weaponPosition.localPosition, new Vector3(0.041f, 0.172f, 0.45f), 10 * Time.deltaTime);
                //weaponPosition.localRotation = Quaternion.Lerp(weaponPosition.localRotation, Quaternion.Euler(0, 0, -10.95f), 10 * Time.deltaTime);
            }
            
        }
        else
        {
            animator.SetBool("isAiming", false);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 60f, 5f * Time.deltaTime);
            
            if (primaryHolder.activeSelf)
            {
                weaponPosition.localPosition = Vector3.Lerp(weaponPosition.localPosition, new Vector3(0.085f, 0.085f, 0.4f), 10 * Time.deltaTime);
                weaponPosition.localRotation = Quaternion.Lerp(weaponPosition.localRotation, Quaternion.Euler(0, 0, 0), 10 * Time.deltaTime);
            }
            else
            {
                weaponPosition.localPosition = Vector3.Lerp(weaponPosition.localPosition, new Vector3(0.085f, 0.15f, 0.275f), 10 * Time.deltaTime);
                weaponPosition.localRotation = Quaternion.Lerp(weaponPosition.localRotation, Quaternion.Euler(0, 0, 0), 10 * Time.deltaTime);
            }
            
        }
        
        /*
        if (primaryHolder.activeSelf && isSwitching)
        {
            weaponPosition.localPosition = Vector3.Lerp(weaponPosition.localPosition, new Vector3(-0.15f, -1f, 0.4f), 10 * Time.deltaTime);
            weaponPosition.localRotation = Quaternion.Lerp(weaponPosition.localRotation, Quaternion.Euler(24.15f, 0, 0), 10 * Time.deltaTime);
            animator.SetBool("isSwitching", true);
        }
        */

        if (lerpSwitch)
        {
            switchTimer += Time.deltaTime;
            float p = switchTimer / 1;
            p *= p;

            if (isSwitching)
            {
                weaponPosition.localPosition = Vector3.Lerp(weaponPosition.localPosition, new Vector3(0.085f, -0.25f, 0.4f), p);
                weaponPosition.localRotation = Quaternion.Lerp(weaponPosition.localRotation, Quaternion.Euler(37.5f, 0, 0), p);
                animator.SetBool("isSwitching", true);
            }

            if (p > 1)
            {
                lerpSwitch = false;
                switchTimer = 0f;
            }
        }
    }

    // receives the inputs for the InputManager.cs and apply them to the character controller
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        charController.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;

        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        charController.Move(playerVelocity * Time.deltaTime);
        HeadBobbing(input);
        FootstepSounds(input);
    }

    // checks if the character collider is on the ground
    /*public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }*/

    public IEnumerator Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
            animator.SetBool("isJumping", true);
            yield return new WaitForSeconds(0.1f);
            animator.SetBool("isJumping", false);
        }
    }

    // checks if the player is crouching, if true, speed is decreased
    public void Crouch()
    {
        crouching = !crouching;

        if (crouching)
        {
            speed = 1.25f;
            animator.SetBool("isCrouching", true);
        } 
        else
        {
            speed = 2.5f;
            animator.SetBool("isCrouching", false);
        }

        crouchTimer = 0;
        lerpCrouch = true;
    }

    public void Sprint()
    {
        sprinting = !sprinting;

        if (sprinting && walking)
        {
            speed = 5f;
            animator.SetBool("isRunning", true);
            animator.SetBool("isSprinting", false);
        }
        else
        {
            speed = 2.5f;
            animator.SetBool("isRunning", false);
            animator.SetBool("isSprinting", false);
        } 
    }

    public void Walk()
    {
        walking = !walking;

        if (walking || speed < 10f && walking)
        {
            animator.SetFloat("Speed", 1);
            animator.SetBool("isSprinting", false);
        }   
        else
        {
            animator.SetFloat("Speed", 0);
            animator.SetBool("isSprinting", false);
        }
    }

    public void Inspect()
    {
        inspecting = !inspecting;
        if (inspecting)
        {
            animator.SetBool("isInspecting", true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

        }
        else
        {
            animator.SetBool("isInspecting", false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void FlipWeapon()
    {
        flipped = !flipped;

        if (inspecting && !flipped)
        {
            animator.SetBool("isFlipped", false);
        }
        else
        {
            animator.SetBool("isFlipped", true);
        }
    }

    public void TurnOnFlashlight()
    {
        if (FindObjectOfType<Gun>().canUseFlashlight)
        {
            flashlightActive = !flashlightActive;

            if (flashlightActive)
            {
                FindObjectOfType<CursorController>().lightSource.GetComponent<Light>().enabled = true;
            }
            else
            {
                FindObjectOfType<CursorController>().lightSource.GetComponent<Light>().enabled = false;
            }
        }
    }

    public void HeadBobbing(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        if (!isGrounded) return;
        if (Mathf.Abs(moveDirection.x) > 0.1f || Mathf.Abs(moveDirection.z) > 0.1f)
        {
            timer += Time.deltaTime * (crouching ? crouchBobSpeed : sprinting ? runBobSpeed : walkBobSpeed);
            cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, originalYPosition + Mathf.Sin(timer) * (crouching ? crouchBobAmount : sprinting ? runBobAmount : walkBobAmount), cam.transform.localPosition.z);
        }
    }

    private void CalculateLeaning()
    {
        if (aiming && isLeaningLeft)
        {
            targetLean = leanAngle;
        }
        else if (aiming && isLeaningRight)
        {
            targetLean = -leanAngle;
        }
        else
        {
            targetLean = 0;
        }
        currentLean = Mathf.SmoothDamp(currentLean, targetLean, ref leanVelocity, leanSmoothness);

        leanPivot.localRotation = Quaternion.Euler(new Vector3(0, 0, currentLean));
    }

    private void FootstepSounds(Vector2 input)
    {
        if (!charController.isGrounded) return;
        if (input == Vector2.zero) return;

        footstepTimer -= Time.deltaTime;

        if(footstepTimer <= 0)
        {
            if(Physics.Raycast(cam.transform.position, Vector3.down, out RaycastHit hit, 3))
            {
                switch (hit.collider.tag)
                {
                    case "Footsteps/Grass":
                        footsteps.PlayOneShot(grassClips[Random.Range(0, grassClips.Length - 1)]);
                        //footsteps.PlayOneShot(footstepClips);
                        break;
                    default:
                        footsteps.PlayOneShot(defaultClips[Random.Range(0, defaultClips.Length - 1)]);
                        //footsteps.PlayOneShot(footstepClips); 
                        break;
                }
            }
            footstepTimer = GetCurrentOffset;
        }
    }

    private void CheckActiveWeapon()
    {
        if (primaryHolder.activeSelf)
        {
            animator = primaryHolder.GetComponentInChildren<Animator>();
            weaponPosition = primaryHolder.transform;
        }
        else if (secondaryHolder.activeSelf)
        {
            animator = secondaryHolder.GetComponentInChildren<Animator>();
            weaponPosition = secondaryHolder.transform;
        }
    }
}
