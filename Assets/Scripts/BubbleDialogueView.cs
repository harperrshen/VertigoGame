using UnityEngine;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using Yarn.Unity;
using System.Linq;

public class BubbleDialogueView : DialogueViewBase
{
    public GameObject dialogueBubblePrefab;
    public float lettersPerSecond = 20f;

    private Dictionary<Transform, GameObject> activeBubbles = new Dictionary<Transform, GameObject>();
    private HashSet<Transform> validDialogueSources = new HashSet<Transform>();

    private Transform currentSpeakerTransform = null;
    private Coroutine currentTypewriterEffect;

    private DialogueRunner dialogueRunner;
    private Queue<(Transform speaker, string nodeName, BubbleAnimationType animType)> dialogueQueue = new Queue<(Transform, string, BubbleAnimationType)>();
    private bool isDialogueRunning = false;


    private void Awake()
    {
        dialogueRunner = GetComponent<DialogueRunner>();
    }
    private BubbleAnimationType currentBubbleAnimationType;
    public void StartBubbleDialogue(Transform speaker, string nodeName, BubbleAnimationType animationType)
    {
        if (activeBubbles.ContainsKey(speaker))
            return;
        
        if (isDialogueRunning)
        {
            // Queue it if something is running
            dialogueQueue.Enqueue((speaker, nodeName, animationType));
            return;
        }

        currentSpeakerTransform = speaker;
        currentBubbleAnimationType = animationType;

        isDialogueRunning = true;
        dialogueRunner.StartDialogue(nodeName);
    }

    public void StopBubbleDialogue(Transform speaker)
    {
        if (activeBubbles.TryGetValue(speaker, out GameObject bubble))
        {
            Destroy(bubble);
            activeBubbles.Remove(speaker);
        }

        // Also tell DialogueRunner to stop (optional)
        // dialogueRunner.Stop(); // if you want to stop dialogue on exit
    }

    public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
    {
        if (currentTypewriterEffect != null)
        {
            StopCoroutine(currentTypewriterEffect);
        }

        // Spawn a bubble for the current speaking NPC/item
        GameObject newBubble = Instantiate(dialogueBubblePrefab, dialogueBubblePrefab.transform.parent);
        newBubble.SetActive(true);

        var bubbleUI = newBubble.GetComponent<BubbleUI>();
        bubbleUI.SetSpeakerTransform(currentSpeakerTransform);
        bubbleUI.SetText(""); // clear text for typing effect
        bubbleUI.SetAnimationState(currentBubbleAnimationType);
        activeBubbles[currentSpeakerTransform] = newBubble;

        currentTypewriterEffect = StartCoroutine(TypewriterEffect(bubbleUI, dialogueLine.TextWithoutCharacterName.Text, onDialogueLineFinished));
    }

    private IEnumerator TypewriterEffect(BubbleUI bubbleUI, string fullText, Action onFinished)
    {
        var textComponent = bubbleUI.dialogueText;
        textComponent.text = "";

        float delay = 1f / lettersPerSecond;

        foreach (char c in fullText)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(delay);
        }

        currentTypewriterEffect = null;
        onFinished?.Invoke();

        isDialogueRunning = false;
        TryStartNextQueuedDialogue();
    }
    
    private void TryStartNextQueuedDialogue()
    {
         while (dialogueQueue.Count > 0)
        {
            var nextDialogue = dialogueQueue.Dequeue();
            if (validDialogueSources.Contains(nextDialogue.speaker))
            {
                StartBubbleDialogue(nextDialogue.speaker, nextDialogue.nodeName, nextDialogue.animType);
                return;
            }
        }
    }

    public override void RunOptions(DialogueOption[] dialogueOptions, Action<int> onOptionSelected)
    {
        Debug.LogWarning("BubbleDialogueView doesn't support options yet!");
        onOptionSelected(0);
    }

    public void MarkDialogueSourceValid(Transform speaker)
    {
        validDialogueSources.Add(speaker);
    }

    public void MarkDialogueSourceInvalid(Transform speaker)
    {
        validDialogueSources.Remove(speaker);

        // If there's an active bubble, destroy it too
        StopBubbleDialogue(speaker);

        // Remove any queued dialogue by that speaker
        dialogueQueue = new Queue<(Transform, string, BubbleAnimationType)>(
            dialogueQueue.Where(d => d.speaker != speaker)
        );
    }

    
}
