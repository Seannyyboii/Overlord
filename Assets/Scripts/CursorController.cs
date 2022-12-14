using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.MouseActions mouse;
    private Camera mainCamera;

    public List<GameObject> IronSights;
    public List<GameObject> FirstOptics;
    public List<GameObject> SecondOptics;
    public List<GameObject> CantedOptics;
    public List<GameObject> Flashlights;
    public List<GameObject> Lasers;
    public List<GameObject> Muzzles;
    public List<GameObject> Grips;

    public GameObject activeWeapon;
    public GameObject lightSource;

    int noOfClicks = 0;

    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInput();
        mouse = playerInput.Mouse;
        mainCamera = Camera.main;
    }

    void Start()
    {
        mouse.Click.performed += _ => Click();
        CheckActiveWeapon();
        CheckActiveAttachments();
        SeparateAttachments();
    }

    // Update is called once per frame
    void Update()
    {
        CheckActiveWeapon();
        CheckActiveAttachments();
        //SeparateAttachments();
        DeactivateAttachments();
    }

    private void OnEnable()
    {
        mouse.Enable();
    }

    private void OnDisable()
    {
        mouse.Disable();
    }

    void DetectObject()
    {
        Ray ray = mainCamera.ScreenPointToRay(mouse.Position.ReadValue<Vector2>());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                Debug.Log(hit.collider.name);
                

                if (hit.collider.name == "SelectFirstOptic" || hit.collider.name == "SelectOptic")
                {
                    CycleThroughAttachments(FirstOptics);
                }

                if (hit.collider.name == "SelectSecondOptic")
                {
                    CycleThroughAttachments(SecondOptics);
                }

                if (hit.collider.name == "SelectCantedOptic")
                {
                    CycleThroughAttachments(CantedOptics);
                }

                if (hit.collider.name == "SelectFlashlight")
                {
                    CycleThroughAttachments(Flashlights);
                }

                if (hit.collider.name == "SelectLaser")
                {
                    CycleThroughAttachments(Lasers);
                }

                if (hit.collider.name == "SelectMuzzle")
                {
                    CycleThroughAttachments(Muzzles);
                }

                if (hit.collider.name == "SelectGrip")
                {
                    CycleThroughAttachments(Grips);
                }
            }
        }
    }

    void Click()
    {
        DetectObject();
    }

    void CheckActiveWeapon()
    {
        if (FindObjectOfType<PlayerController>().primaryHolder.activeSelf)
        {
            activeWeapon = GameObject.FindGameObjectWithTag("Primary");
            CheckIronSights();
            CheckFlashlight();
        }
        else
        {
            activeWeapon = GameObject.FindGameObjectWithTag("Secondary");
        }
    }

    public void SeparateAttachments()
    {
        foreach (Transform child in activeWeapon.GetComponentsInChildren<Transform>())
        {
            foreach (Transform childObject in child)
            {
                if (FindObjectOfType<PlayerController>().primaryHolder.activeSelf)
                {
                    //CheckIronSights();
                    //CheckFlashlight();

                    if (childObject.CompareTag("AttachmentType/IronSight") && IronSights.Count < GameObject.Find("Iron Sights").transform.childCount)
                    {
                        //Debug.Log("Attached Iron Sight");
                        IronSights.Add(childObject.gameObject);
                    }

                    if (childObject.CompareTag("AttachmentType/FirstOptic") && FirstOptics.Count < GameObject.Find("FirstOpticHolder").transform.childCount)
                    {
                        //Debug.Log("Attached Optic");
                        FirstOptics.Add(childObject.gameObject);
                    }

                    if (childObject.CompareTag("AttachmentType/SecondOptic") && SecondOptics.Count < GameObject.Find("SecondOpticHolder").transform.childCount)
                    {
                        //Debug.Log("Attached Optic");
                        SecondOptics.Add(childObject.gameObject);
                    }

                    if (childObject.CompareTag("AttachmentType/CantedOptic") && CantedOptics.Count < GameObject.Find("CantedOpticHolder").transform.childCount)
                    {
                        //Debug.Log("Attached Optic");
                        CantedOptics.Add(childObject.gameObject);
                    }

                    if (childObject.CompareTag("AttachmentType/Flashlight") && Flashlights.Count < GameObject.Find("FlashlightHolder").transform.childCount)
                    {
                        //Debug.Log("Attached Optic");
                        Flashlights.Add(childObject.gameObject);
                    }

                    if (childObject.CompareTag("AttachmentType/Grip") && Grips.Count < GameObject.Find("GripHolder").transform.childCount)
                    {
                        //Debug.Log("Attached Grip");
                        Grips.Add(childObject.gameObject);
                    }
                }
                else if (!FindObjectOfType<PlayerController>().primaryHolder.activeSelf)
                {
                    if (childObject.CompareTag("AttachmentType/FirstOptic") && FirstOptics.Count < GameObject.Find("OpticHolder").transform.childCount)
                    {
                        //Debug.Log("Attached Optic");
                        FirstOptics.Add(childObject.gameObject);
                    }
                }

                if (childObject.CompareTag("AttachmentType/Laser") && Lasers.Count < GameObject.Find("LaserHolder").transform.childCount)
                {
                    //Debug.Log("Attached Laser");
                    Lasers.Add(childObject.gameObject);
                }

                if (childObject.CompareTag("AttachmentType/Muzzle") && Muzzles.Count < GameObject.Find("MuzzleHolder").transform.childCount)
                {
                    //Debug.Log("Attached Muzzle");
                    Muzzles.Add(childObject.gameObject);
                }


            }
        }
    }

    void CheckActiveAttachments()
    {
        foreach (Transform child in activeWeapon.GetComponentsInChildren<Transform>())
        {
            foreach (Transform childObject in child)
            {
                if (PlayerPrefs.GetInt(childObject.name, 0) == 1 && childObject.CompareTag("AttachmentType/FirstOptic") && activeWeapon.activeSelf)
                {
                    FindObjectOfType<FirstOptic>().targetMesh = childObject.gameObject;
                }
                if (PlayerPrefs.GetInt(childObject.name, 0) == 1 && childObject.CompareTag("AttachmentType/SecondOptic") && activeWeapon.activeSelf)
                {
                    FindObjectOfType<SecondOptic>().targetMesh = childObject.gameObject;
                }
                if (PlayerPrefs.GetInt(childObject.name, 0) == 1 && childObject.CompareTag("AttachmentType/CantedOptic") && activeWeapon.activeSelf)
                {
                    FindObjectOfType<CantedOptic>().targetMesh = childObject.gameObject;
                }
                if (PlayerPrefs.GetInt(childObject.name, 0) == 1 && childObject.CompareTag("AttachmentType/Flashlight") && activeWeapon.activeSelf)
                {
                    FindObjectOfType<Flashlight>().targetMesh = childObject.gameObject;
                }
                if (PlayerPrefs.GetInt(childObject.name, 0) == 1 && childObject.CompareTag("AttachmentType/Laser") && activeWeapon.activeSelf)
                {
                    FindObjectOfType<Laser>().targetMesh = childObject.gameObject;
                }
                if (PlayerPrefs.GetInt(childObject.name, 0) == 1 && childObject.CompareTag("AttachmentType/Muzzle") && activeWeapon.activeSelf)
                {
                    FindObjectOfType<Muzzle>().targetMesh = childObject.gameObject;
                }
                if (PlayerPrefs.GetInt(childObject.name, 0) == 1 && childObject.CompareTag("AttachmentType/Grip") && activeWeapon.activeSelf)
                {
                    FindObjectOfType<Grip>().targetMesh = childObject.gameObject;
                }
            }
        }
    }

    void CycleThroughAttachments(List<GameObject> attachmentList)
    {
        if (noOfClicks >= attachmentList.Count)
        {
            noOfClicks = 0;
        }

        attachmentList[noOfClicks].SetActive(false);

        PlayerPrefs.SetInt(attachmentList[noOfClicks].name, 0);

        Debug.Log(attachmentList[noOfClicks].name + " integer set to " + PlayerPrefs.GetInt(attachmentList[noOfClicks].name));

        noOfClicks++;

        if (noOfClicks >= attachmentList.Count)
        {
            noOfClicks = 0;
        }

        attachmentList[noOfClicks].SetActive(true);

        PlayerPrefs.SetInt(attachmentList[noOfClicks].name, 1);

        Debug.Log(attachmentList[noOfClicks].name + " integer set to " + PlayerPrefs.GetInt(attachmentList[noOfClicks].name));

        //FindObjectOfType<IronSights>().targetMesh = attachmentList[noOfClicks];
        ShowCorrectAttachment(attachmentList);
    }
    public void ClearAttachments()
    {
        IronSights.Clear();
        FirstOptics.Clear();
        SecondOptics.Clear();
        CantedOptics.Clear();
        Flashlights.Clear();
        Lasers.Clear();
        Muzzles.Clear();
        Grips.Clear();
    }

    public IEnumerator CheckAttachments()
    {
        ClearAttachments();
        yield return new WaitForSeconds(0.1f);
        SeparateAttachments();
    }

    void CheckIronSights()
    {
        if (FindObjectOfType<FirstOptic>().targetMesh.name == "No First Optic" && FindObjectOfType<SecondOptic>().targetMesh.name == "No Second Optic" || FindObjectOfType<FirstOptic>().targetMesh.name == "No First Optic" && FindObjectOfType<SecondOptic>().targetMesh.name == "Canted Rail")
        {
            foreach (GameObject Sight in IronSights)
            {
                Sight.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
        }
        else
        {
            foreach (GameObject Sight in IronSights)
            {
                Sight.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
            }
        }
    }

    void CheckFlashlight()
    {
        foreach (Transform child in activeWeapon.GetComponentsInChildren<Transform>())
        {
            foreach (Transform childObject in child)
            {
                if (childObject.CompareTag("Light"))
                {
                    lightSource = childObject.gameObject;
                }
            }
        }
    }

    void ShowCorrectAttachment(List<GameObject> attachmentList)
    {
        foreach (GameObject attachment in attachmentList)
        {
            if (attachment.name == attachmentList[noOfClicks].name)
            {
                PlayerPrefs.SetInt(attachment.name, 1);
                attachment.SetActive(true);
            }
            else
            {
                PlayerPrefs.SetInt(attachment.name, 0);
                attachment.SetActive(false);
            }
        }
    }

    void DeactivateAttachments()
    {
        if (FindObjectOfType<PlayerController>().primaryHolder.activeSelf)
        {
            foreach (GameObject attachment in FirstOptics)
            {
                if (FindObjectOfType<SecondOptic>().targetMesh.name == "Eotech")
                {
                    attachment.SetActive(false);
                }
                else
                {
                    if (attachment.name == FindObjectOfType<FirstOptic>().name)
                    {
                        attachment.SetActive(true);
                    }
                }
            }
        }
    }
}
