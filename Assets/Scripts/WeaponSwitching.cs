using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    public int selectedWeapon = 0;
    [HideInInspector] public float mouseScrollY;

    private PlayerController playerController;
    private Transform weaponPosition;

    public void Start()
    {

    }

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
    }
    // Update is called once per frame
    public void Update()
    {
        CheckTransform();

        int previousSelectedWeapon = selectedWeapon;

        if (mouseScrollY > 0f)
        {
            if(selectedWeapon >= transform.childCount - 1)
            {
                selectedWeapon = 0;
            }
            else
            {
                selectedWeapon++;
            }
        }

        if (mouseScrollY < 0f)
        {
            if (selectedWeapon <= 0)
            {
                selectedWeapon = transform.childCount - 1;
            }
            else
            {
                selectedWeapon--;
            }
        }

        if(previousSelectedWeapon != selectedWeapon)
        {
            StartCoroutine(SelectWeapon());
            FindObjectOfType<GunSFX>().Withdraw();
        }
    }

    public IEnumerator SelectWeapon()
    {
        
        playerController.isSwitching = true;
        playerController.lerpSwitch = true;

        yield return new WaitForSeconds(1);

        playerController.isSwitching = false;
        playerController.lerpSwitch = false;

        int i = 0;

        FindObjectOfType<WeaponCustomisation>().attachmentPoints.Clear();
        FindObjectOfType<CursorController>().SeparateAttachments();
        StartCoroutine(FindObjectOfType<CursorController>().CheckAttachments());

        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
                weapon.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
                weapon.GetChild(0).gameObject.SetActive(false);
            }
            i++;
        }
    }

    private void CheckTransform()
    {
        if (playerController.primaryHolder.activeSelf)
        {
            weaponPosition = playerController.primaryHolder.transform;
        }
        else if (playerController.secondaryHolder.activeSelf)
        {
            weaponPosition = playerController.secondaryHolder.transform;
        }
    }
}
