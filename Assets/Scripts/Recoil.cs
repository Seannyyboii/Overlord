using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    private bool isAiming;

    //Rotations 
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    //Hipfire Recoil
    [SerializeField] private float recoilX;
    [SerializeField] private float recoilY;
    [SerializeField] private float recoilZ;

    //ADS Recoil
    [SerializeField] private float aimRecoilX;
    [SerializeField] private float aimRecoilY;
    [SerializeField] private float aimRecoilZ;

    //Settings
    [SerializeField] private float snap;
    [SerializeField] private float returnSpeed;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isAiming = playerController.aiming;
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snap * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void RecoilFire()
    {
        if(isAiming) targetRotation += new Vector3(aimRecoilX, Random.Range(-aimRecoilY, aimRecoilY), Random.Range(-aimRecoilZ, aimRecoilZ));
        else targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }
}
