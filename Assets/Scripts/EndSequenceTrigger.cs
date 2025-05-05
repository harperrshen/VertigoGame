using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class EndSequenceTrigger : MonoBehaviour
{
    public GameObject manualTurnTrigger;  
    public PlayerMovement playerMovement;
    public Animator cameraAnimator;
    public string cameraZoomTrigger = "ZoomOut"; 
    public float delayBeforeTurn = 1f;
    public float delayAfterTurn = 2f;

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;
            StartCoroutine(PlayEndSequence());
            
            if (manualTurnTrigger != null)
                Destroy(manualTurnTrigger);
        }

        
    }
    public AudioSource bgmAudioSource;   
    public float fadeDuration = 4f; 
   
    public GameObject CreditsText;
    private System.Collections.IEnumerator PlayEndSequence()
    {
        // Disable input
        playerMovement.isEnding = true;

        // Stop animation immediately
        playerMovement.SetWalkingAnimation(false);

        // Wait before rotation
        yield return new WaitForSeconds(delayBeforeTurn);

        // Set rotation direction (left)
        Quaternion finalRotation = Quaternion.Euler(0, 0, playerMovement.transform.eulerAngles.z + 90f);
        playerMovement.ForceRotate(finalRotation);

        // Wait for rotation to complete
        yield return new WaitForSeconds(delayAfterTurn);

        // Trigger camera zoom
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
