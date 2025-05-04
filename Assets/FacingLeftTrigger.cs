using UnityEngine;

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

    private System.Collections.IEnumerator PlayEndSequence()
    {
        playerMovement.isEnding = true;
        playerMovement.SetWalkingAnimation(false);

        yield return new WaitForSeconds(delayAfterTurn);

        if (cameraAnimator != null)
        {
            cameraAnimator.SetTrigger(cameraZoomTrigger);
        }

        // Optional: fade to black, show credits, etc.
    }
}
