using UnityEngine;

public class SFXManager : Singleton<SFXManager>
{
    AudioSource _audio;
    [Header("AudioClip")]
    [SerializeField] AudioClip clickSound;
    [SerializeField] AudioClip exitSound;
    [SerializeField] AudioClip buySound;
    [SerializeField] AudioClip denySound;

    void Start()
    {
        _audio = GetComponent<AudioSource>();
    }
    public void PlayClickSound()
    {
        _audio.PlayOneShot(clickSound);
    }

    public void PlayExitSound()
    {
        _audio.PlayOneShot(exitSound);
    }

    public void PlayBuySound()
    {
        _audio.PlayOneShot(buySound);
    }

    public void PlayDenySound()
    {
        _audio.PlayOneShot(denySound);
    }
    
}
