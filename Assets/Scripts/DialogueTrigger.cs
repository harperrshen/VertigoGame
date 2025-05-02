using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    public enum BubbleAnimationType{CircularMotion,HorizontalMotion, Idling}
    public class DialogueTrigger : MonoBehaviour
{
    public string yarnStartNodeName; // The Yarn node this trigger will start
    public BubbleAnimationType bubbleAnimationType;
    public BubbleDialogueView bubbleDialogueView;
    private bool playerInZone = false;

    private void Start()
    {
        bubbleDialogueView = FindObjectOfType<BubbleDialogueView>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            var dialogueView = FindObjectOfType<BubbleDialogueView>();
            bubbleDialogueView.MarkDialogueSourceValid(transform); // "this" NPC/item
            dialogueView.StartBubbleDialogue(this.transform, yarnStartNodeName,bubbleAnimationType);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;

            var dialogueView = FindObjectOfType<BubbleDialogueView>();
            bubbleDialogueView.MarkDialogueSourceInvalid(transform); 
            dialogueView.StopBubbleDialogue(this.transform);
        }
    }
}
