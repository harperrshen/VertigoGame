using UnityEngine;

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

        // TODO: Add fade/credits logic if needed
    }
}
