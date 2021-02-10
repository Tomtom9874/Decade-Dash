using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioClip walkingSound;
    [SerializeField] private AudioClip jumpingSound;

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayWalkingSound()
    {
        if (_audioSource.clip == walkingSound) return;
        _audioSource.clip = walkingSound;
        _audioSource.loop = true;
        _audioSource.Play();
        Debug.Log("PlayWalkingSound");
    }

    public void PlayJumpSound()
    {
        _audioSource.clip = jumpingSound;
        _audioSource.loop = false;
        _audioSource.Play();
    }

    public void StopSound()
    {
        _audioSource.Stop();
    }
}
