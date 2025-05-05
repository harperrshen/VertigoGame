using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class FacingLeftTrigger : MonoBehaviour
{
    public GameObject autoTurnTrigger;          // Reference to Trigger 2's GameObject
    public PlayerMovement playerMovement;
    public Animator cameraAnimator;
    public string cameraZoomTrigger = "ZoomOut"; 
    public float delayAfterTurn = 2f;

    private bool hasTriggered = false;
    private bool playerInside = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            Debug.Log("Player entered the trigger zone.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            Debug.Log("Player exited the trigger zone.");
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (hasTriggered || !playerInside || !other.CompareTag("Player"))
            return;

        float rotZ = playerMovement.transform.eulerAngles.z;
        rotZ = (rotZ + 360f) % 360f;

        if (Mathf.Approximately(rotZ, 270f))
        {
            hasTriggered = true;
            Debug.Log("Player is facing left. Triggering end sequence.");
            StartCoroutine(PlayEndSequence());

            if (autoTurnTrigger != null)
                Destroy(autoTurnTrigger);
        }
    }
    public AudioSource bgmAudioSource;   
    public float fadeDuration = 4f; 
    
    public GameObject CreditsText;
    private System.Collections.IEnumerator PlayEndSequence()
    {
        playerMovement.isEnding = true;
        playerMovement.SetWalkingAnimation(false);

        yield return new WaitForSeconds(delayAfterTurn);

        if (cameraAnimator != null)
        {
            cameraAnimator.SetTrigger(cameraZoomTrigger);
        }

        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null)
        {
            audio.Play();
        }
        if (bgmAudioSource != null)
        StartCoroutine(FadeOutAudio(bgmAudioSource, fadeDuration));

        if (cameraAnimator != null)
            {
                // Wait until the animator is playing the correct state
                while (!cameraAnimator.GetCurrentAnimatorStateInfo(0).IsName("FinalCamPo"))
                {
                    yield return null;
                }
            }

        // Activate credits
        if (CreditsText != null && !CreditsText.activeSelf)
        {
            CreditsText.SetActive(true);
        }
        
    }

        private IEnumerator FadeOutAudio(AudioSource audioSource, float duration)
       {
        float startVolume = audioSource.volume;

            float time = 0f;
            while (time < duration)
            {
                time += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(startVolume, 0.1f, time / duration);
                yield return null;
            }


        }

}
