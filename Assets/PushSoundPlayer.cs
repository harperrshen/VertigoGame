using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(Rigidbody2D))]
public class PushSoundPlayer : MonoBehaviour
{
    public string playerTag = "Player";
    private AudioSource audioSource;
    private bool hasPlayedThisPush = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(playerTag) && !hasPlayedThisPush)
        {
            audioSource.Play();
            hasPlayedThisPush = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(playerTag))
        {
            hasPlayedThisPush = false;
        }
    }
}
