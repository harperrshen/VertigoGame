using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    public enum BubbleAnimationType{CircularMotion,HorizontalMotion, Idling}public class DialogueTrigger : MonoBehaviour
{
    public string yarnStartNodeName; // The Yarn node this trigger will start
    public BubbleAnimationType bubbleAnimationType;
    private bool playerInZone = false;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;

            var dialogueView = FindObjectOfType<BubbleDialogueView>();
            dialogueView.StartBubbleDialogue(this.transform, yarnStartNodeName,bubbleAnimationType);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;

            var dialogueView = FindObjectOfType<BubbleDialogueView>();
            dialogueView.StopBubbleDialogue(this.transform);
        }
    }
}
