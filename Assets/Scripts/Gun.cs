using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Animator animator;

    Transform cam;

    public bool reloading;
    public float bulletSpread;

    [SerializeField] float range = 50f;
    [SerializeField] float damage = 10f;

    [SerializeField] float fireRate = 5f;
    [SerializeField] float reloadTime;
    WaitForSeconds reloadWait;

    [SerializeField] int maxAmmo;
    int currentAmmo;

    [SerializeField] public bool rapidFire = false;
    WaitForSeconds rapidFireWait;

    private Recoil recoil;
    private GameObject recoilCam;

    private FirstOptic firstOptic;
    private SecondOptic secondOptic;
    private CantedOptic cantedOptic;
    private Flashlight flashlight;
    private Laser laser;
    private Muzzle muzzle;
    private Grip grip;

    public bool canUseFlashlight;

    public void Awake()
    {
        cam = Camera.main.transform;
        rapidFireWait = new WaitForSeconds(1 / fireRate);
        reloadWait = new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;

        recoilCam = GameObject.FindWithTag("RecoilController");
        recoil = recoilCam.transform.GetComponent<Recoil>();

        StartCoroutine(CheckAttachments());
    }

    public void Shoot()
    {
        StartCoroutine(wait());
        currentAmmo--;
        RaycastHit hit;
        Vector3 shootDirection = cam.forward;
        shootDirection = shootDirection + cam.TransformDirection(new Vector3(Random.Range(-bulletSpread, bulletSpread), Random.Range(-bulletSpread, bulletSpread)));
        if (Physics.Raycast(cam.position, shootDirection, out hit, range))
        {
            if(hit.collider.GetComponent<Damageable>() != null)
            {
                hit.collider.GetComponent<Damageable>().TakeDamage(damage, hit.point, hit.normal);
            }
            if (hit.collider.GetComponent<SphereCollider>() != null)
            {
                FindObjectOfType<EnemyPatrol>().TakeDamage(100);
            }
            if (hit.collider.GetComponent<BoxCollider>() != null && hit.collider.name == "Enemy")
            {
                FindObjectOfType<EnemyPatrol>().TakeDamage(50);
            }
            if (hit.collider.GetComponent<CapsuleCollider>() != null)
            {
                FindObjectOfType<EnemyPatrol>().TakeDamage(25);
            }

            if(hit.collider.gameObject.layer == 7)
            {
                if (hit.collider.gameObject.TryGetComponent<Target>(out Target target))
                {
                    target.TakeDamage(100);

                    if(target.health <= 0)
                    {
                        FindObjectOfType<TargetManager>().targetCount += 1;
                    }
                }
            }
            
        }
        //print(currentAmmo);
        recoil.RecoilFire();
    }

    public IEnumerator RapidFire()
    {
        if (CanShoot() && !reloading && !FindObjectOfType<PlayerController>().inspecting)
        {
            animator.Play("Shoot");
            //Shoot();

            if (rapidFire && !reloading)
            {
                while (CanShoot() && !reloading)
                {
                    yield return rapidFireWait;
                    animator.Play("Shoot");
                    Shoot();
                }
                //print("Reload!");
            }
        }
        else
        {
            //print("Reload!");
        }
    } 
     
    public IEnumerator Reload()
    {
        if(currentAmmo == maxAmmo || reloading == true)
        {
            yield return null;
        } 
        else if (reloading == false && currentAmmo != maxAmmo)
        {
            animator.SetBool("isShooting", false);
            animator.SetBool("isReloading", true);
            reloading = true;

            print("Reloading...");
            yield return reloadWait;

            currentAmmo = maxAmmo;

            animator.SetBool("isReloading", false);
            reloading = false;
        }
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.1f);
    }

    public bool CanShoot()
    {
        bool enoughAmmo = currentAmmo > 0;
        return enoughAmmo;
    }

    IEnumerator CheckAttachments()
    {
        yield return new WaitForSeconds(0.1f);

        firstOptic = GetComponent<FirstOptic>();
        secondOptic = GetComponent<SecondOptic>();
        cantedOptic = GetComponent<CantedOptic>();
        flashlight = GetComponent<Flashlight>();
        laser = GetComponent<Laser>();
        muzzle = GetComponent<Muzzle>();
        grip = GetComponent<Grip>();

        bool firstOpticAttached = PlayerPrefs.GetInt(FindObjectOfType<FirstOptic>().targetMesh.name, 0) == 1;
        bool laserAttached = PlayerPrefs.GetInt(FindObjectOfType<Laser>().targetMesh.name, 0) == 1;
        bool muzzleAttached = PlayerPrefs.GetInt(FindObjectOfType<Muzzle>().targetMesh.name, 0) == 1;

        if (FindObjectOfType<PlayerController>().primaryHolder.activeSelf)
        {
            bool secondOpticAttached = PlayerPrefs.GetInt(FindObjectOfType<SecondOptic>().targetMesh.name, 0) == 1;
            bool flashlightAttached = PlayerPrefs.GetInt(FindObjectOfType<Flashlight>().targetMesh.name, 0) == 1;
            bool gripAttached = PlayerPrefs.GetInt(FindObjectOfType<Grip>().targetMesh.name, 0) == 1;

            if (secondOpticAttached)
            {
                secondOptic.Enable();

                yield return new WaitForSeconds(0.1f);

                if (FindObjectOfType<CantedOptic>().targetMesh != null)
                {
                    bool cantedOpticAttached = PlayerPrefs.GetInt(FindObjectOfType<CantedOptic>().targetMesh.name, 0) == 1;

                    if (cantedOpticAttached)
                    {
                        cantedOptic.Enable();
                    }
                }
            }
            else
            {
                secondOptic.Disable();
            }

            if (flashlightAttached)
            {
                flashlight.Enable();

                if (flashlight.targetMesh.name == "Flashlight")
                {
                    canUseFlashlight = true;
                }
                else
                {
                    canUseFlashlight = false;
                }
            }
            else
            {
                flashlight.Disable();
            }

            if (gripAttached)
            {
                grip.Enable();
            }
            else
            {
                grip.Disable();
            }
        }

        if (firstOpticAttached)
        {
            firstOptic.Enable();
        }
        else
        {
            firstOptic.Disable();
        }

        if (laserAttached)
        {
            laser.Enable();
        }
        else
        {
            laser.Disable();
        }

        if (muzzleAttached)
        {
            muzzle.Enable();
        }
        else
        {
            muzzle.Disable();
        }
    }
}
