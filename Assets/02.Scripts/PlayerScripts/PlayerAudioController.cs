using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    AudioSource _audio;

    [Header("Jump & Land")]
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip landSound;

    [Header("Hit")]
    [SerializeField] AudioClip hitSound;

    void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    public void PlayJumpSound()
    {
        _audio.PlayOneShot(jumpSound);
    }

    public void PlayLandSound()
    {
        _audio.PlayOneShot(landSound);
    }

    public void PlayHitSound()
    {
        _audio.PlayOneShot(hitSound);
    }
}
