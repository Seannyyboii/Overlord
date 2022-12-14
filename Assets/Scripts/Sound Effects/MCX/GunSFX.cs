using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSFX : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] shootSounds;
    [SerializeField] private AudioClip[] AimSound;
    [SerializeField] private AudioClip[] UnAimSound;
    [SerializeField] private AudioClip[] reloadStartSounds;
    [SerializeField] private AudioClip[] reloadEndSounds;
    [SerializeField] private AudioClip[] reloadClickSound;
    [SerializeField] private AudioClip[] drawSound;
    [SerializeField] private AudioClip[] withdrawSound;

    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Shoot()
    {
        if (FindObjectOfType<Muzzle>().targetMesh.name == "No Muzzle")
        {
            AudioClip shoot = shootSounds[0];
            audioSource.PlayOneShot(shoot);
        }
        else
        {
            AudioClip shoot = shootSounds[1];
            audioSource.PlayOneShot(shoot);
        }
    }

    private void ReloadStart()
    {
        // Sets a footstep variable as a random audio clip in the footstepSounds AudioClip array and plays the audio clip
        AudioClip reload = reloadStartSounds[Random.Range(0, reloadStartSounds.Length)];
        audioSource.PlayOneShot(reload);
    }

    private void ReloadEnd()
    {
        // Sets a footstep variable as a random audio clip in the footstepSounds AudioClip array and plays the audio clip
        AudioClip reload = reloadEndSounds[Random.Range(0, reloadEndSounds.Length)];
        audioSource.PlayOneShot(reload);
    }

    private void ReloadClick()
    {
        // Sets a footstep variable as a random audio clip in the footstepSounds AudioClip array and plays the audio clip
        AudioClip reloadClick = reloadClickSound[Random.Range(0, reloadClickSound.Length)];
        audioSource.PlayOneShot(reloadClick);
    }

    private void Draw()
    {
        // Sets a footstep variable as a random audio clip in the footstepSounds AudioClip array and plays the audio clip
        AudioClip draw = drawSound[Random.Range(0, drawSound.Length)];
        audioSource.PlayOneShot(draw);
    }

    public void Withdraw()
    {
        // Sets a footstep variable as a random audio clip in the footstepSounds AudioClip array and plays the audio clip
        AudioClip withdraw = withdrawSound[Random.Range(0, withdrawSound.Length)];
        audioSource.PlayOneShot(withdraw);
    }

    public void Aim()
    {
        // Sets a footstep variable as a random audio clip in the footstepSounds AudioClip array and plays the audio clip
        AudioClip aim = AimSound[Random.Range(0, AimSound.Length)];
        audioSource.PlayOneShot(aim);
    }

    public void UnAim()
    {
        // Sets a footstep variable as a random audio clip in the footstepSounds AudioClip array and plays the audio clip
        AudioClip unAim = UnAimSound[Random.Range(0, UnAimSound.Length)];
        audioSource.PlayOneShot(unAim);
    }
}
