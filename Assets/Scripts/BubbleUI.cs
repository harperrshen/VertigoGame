using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BubbleUI : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    private Transform speakerTransform;
    private Transform playerTransform;
    private Camera mainCamera;
    public Vector3 offset = new Vector3(0f, 0.2f, 0f); // above head
    private float margin = 50f; // screen margin pixels

    private RectTransform rectTransform;
    private RectTransform bubbleRect;
    private Animator animator;
    private Vector3 targetScreenPos;
    private float smoothingSpeed = 10f;

    bool isOutLeft = false;
    bool isOutRight = false;
    bool isOutTop = false;
    bool isOutBottom = false;

    private void Start()
    {
        mainCamera = Camera.main;
        ResetOffset();
        rectTransform = GetComponent<RectTransform>();
        animator = GetComponent<Animator>();
    }

    public void SetSpeakerTransform(Transform speaker)
    {
        speakerTransform = speaker;

        if (animator == null){animator = GetComponentInChildren<Animator>();}
        if (bubbleRect == null && transform.childCount > 0)
        {
            bubbleRect = transform.GetChild(0).GetComponent<RectTransform>();
        }
    }

    public void SetText(string text)
    {
        dialogueText.text = text;

        if (animator == null){animator = GetComponentInChildren<Animator>();}
        if (bubbleRect == null && transform.childCount > 0)
        {
            bubbleRect = transform.GetChild(0).GetComponent<RectTransform>();
        }
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
        
        // bool isOutLeft = bubbleRect.position.x < margin;
        // bool isOutRight = bubbleRect.position.x > Screen.width - margin;
        // bool isOutBottom = bubbleRect.position.y < margin;
        // bool isOutTop = bubbleRect.position.y > Screen.height - margin;
        
        // if(isOutLeft)
        // {
        //     offset.x+=0.2f;
        // }
        // if(isOutRight)
        // {
        //     offset.x-=0.2f;
        // }
        // if(isOutBottom)
        // {
        //     offset.y+=0.2f;
        // }
        // if(isOutTop)
        // {
        //     offset.y-=0.2f;
        // }

        Vector2 desiredScreenPos = mainCamera.WorldToScreenPoint(speakerTransform.position + offset);

        // 2. Clamp position
        Vector2 clampedScreenPos = desiredScreenPos;

        clampedScreenPos.x = Mathf.Clamp(clampedScreenPos.x, margin, Screen.width - margin);
        clampedScreenPos.y = Mathf.Clamp(clampedScreenPos.y, margin, Screen.height - margin);

        // 3. Smoothly move towards clamped position
        targetScreenPos = clampedScreenPos;

        rectTransform.position = Vector3.Lerp(rectTransform.position, targetScreenPos, Time.deltaTime * smoothingSpeed);
    }
    public void ResetOffset()
    {
        offset = Vector3.zero;
    }

 //      private void Update()
// {
//     if (speakerTransform == null)
//         return;

//     Vector3 bubbleScreenPos = mainCamera.WorldToScreenPoint(bubbleRect.position);

//     float nudgeAmount = 0.4f;

//     float playerRotation = playerTransform.eulerAngles.z;

//     // Normalize angle (convert -90 to 270 for consistency)
//     playerRotation = Mathf.Round(playerRotation) % 360f;
//     if (playerRotation < 0f) playerRotation += 360f;

//     // Check if rotation changed
//     if (!Mathf.Approximately(playerRotation, lastPlayerRotation)) {
//         ResetOffset();
//         lastPlayerRotation = playerRotation;
//     }

//     switch(playerRotation)
//     {
//         case 0f: //facing front  
//             isOutLeft = bubbleRect.position.x < margin;
//             isOutRight = bubbleRect.position.x > Screen.width - margin;
//             isOutBottom = bubbleRect.position.y < margin;
//             isOutTop = bubbleRect.position.y > Screen.height - margin;

//             if (isOutLeft) offset.x += nudgeAmount;
//             if (isOutRight) offset.x -= nudgeAmount;
//             if (isOutBottom) offset.y += nudgeAmount;
//             if (isOutTop) offset.y -= nudgeAmount;
//         break;

//         case 90f: //facing left
//             isOutBottom = bubbleScreenPos.x < margin;
//             isOutTop = bubbleScreenPos.x > Screen.width - margin;
//             isOutLeft = bubbleScreenPos.y < margin;
//             isOutRight = bubbleScreenPos.y > Screen.height - margin;

//             if (isOutLeft) offset.y += nudgeAmount;
//             if (isOutRight) offset.y -= nudgeAmount;
//             if (isOutBottom) offset.x += nudgeAmount;
//             if (isOutTop) offset.x -= nudgeAmount;
//         break;

//         case 180f://facing right
//             isOutRight = bubbleScreenPos.x < margin;
//             isOutLeft = bubbleScreenPos.x > Screen.width - margin;
//             isOutTop = bubbleScreenPos.y < margin;
//             isOutBottom = bubbleScreenPos.y > Screen.height - margin;

//             if (isOutLeft) offset.x += nudgeAmount;
//             if (isOutRight) offset.x -= nudgeAmount;
//             if (isOutTop) offset.y += nudgeAmount;
//             if (isOutBottom) offset.y -= nudgeAmount;
//         break;
//         case 270f:
//             isOutTop = bubbleScreenPos.x < margin;
//             isOutBottom = bubbleScreenPos.x > Screen.width - margin;
//             isOutRight = bubbleScreenPos.y < margin;
//             isOutLeft = bubbleScreenPos.y > Screen.height - margin;

//             if (isOutLeft) offset.y += nudgeAmount;
//             if (isOutRight) offset.y -= nudgeAmount;
//             if (isOutTop) offset.x += nudgeAmount;
//             if (isOutBottom) offset.x -= nudgeAmount;
//         break;
//     }

//     Vector2 desiredScreenPos = mainCamera.WorldToScreenPoint(speakerTransform.position + offset);

//     // 2. Clamp position
//     Vector2 clampedScreenPos = desiredScreenPos;

//     clampedScreenPos.x = Mathf.Clamp(clampedScreenPos.x, margin, Screen.width - margin);
//     clampedScreenPos.y = Mathf.Clamp(clampedScreenPos.y, margin, Screen.height - margin);

//     // 3. Smoothly move towards clamped position
//     targetScreenPos = clampedScreenPos;

//     rectTransform.position = Vector3.Lerp(rectTransform.position, targetScreenPos, Time.deltaTime * smoothingSpeed);
// }

}
