using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BubbleUI : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;

    private Transform speakerTransform;
    private Camera mainCamera;
    public Vector3 offset = new Vector3(0f, 0.2f, 0f); // above head
    private float margin = 50f; // screen margin pixels

    private RectTransform rectTransform;
    private Animator animator;
    private Vector3 targetScreenPos;
    private float smoothingSpeed = 10f;
    private void Start()
    {
        mainCamera = Camera.main;
        rectTransform = GetComponent<RectTransform>();
        animator = GetComponent<Animator>();
    }

    public void SetSpeakerTransform(Transform speaker)
    {
        speakerTransform = speaker;

        if (animator == null){animator = GetComponentInChildren<Animator>();}
    }

    public void SetText(string text)
    {
        dialogueText.text = text;

        if (animator == null){animator = GetComponentInChildren<Animator>();}
    }

    public void SetAnimationState(BubbleAnimationType animationType)
    {
        if (animator == null)
            return;

        string stateName = animationType.ToString();
        animator.Play(stateName);
    }

    private void Update()
    {
        if (speakerTransform == null)
            return;

        Vector2 desiredScreenPos = mainCamera.WorldToScreenPoint(speakerTransform.position + offset);

        // 2. Clamp position
        Vector2 clampedScreenPos = desiredScreenPos;

        clampedScreenPos.x = Mathf.Clamp(clampedScreenPos.x, margin, Screen.width - margin);
        clampedScreenPos.y = Mathf.Clamp(clampedScreenPos.y, margin, Screen.height - margin);

        // 3. Smoothly move towards clamped position
        targetScreenPos = clampedScreenPos;

        rectTransform.position = Vector3.Lerp(rectTransform.position, targetScreenPos, Time.deltaTime * smoothingSpeed);
    }
}
