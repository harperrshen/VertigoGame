using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FootAnimatorGroup
{
    public Animator outlineAnimator;
    public Animator colorAnimator;
    public List<Animator> effectAnimators; // Changed to a list for multiple effects
}

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float stepDistance = 1f;
    public float rotationSpeed = 360f;
    public float clickThreshold = 0.2f;

    [Header("Foot Animator Groups")]
    public FootAnimatorGroup leftFoot;
    public FootAnimatorGroup rightFoot;

    [Header("Foot Effect Settings")]
    public bool useFootEffect = false;
    public List<float> effectDelays = new List<float>(); // List of delays for effects

    [Header("Wall Detection Settings")]
    public LayerMask obstacleLayerMask; // Choose what counts as a wall
    public float obstacleDetectionDistance = 1f; // How far to check forward

    [Header("Pushback Settings")]
    public float pushbackDistance = 1f;
    public float pushbackSpeed = 2f;
    private bool isBeingPushedBack = false;

    private Quaternion targetRotation;
    private bool isRotating = false;
    private bool isHoldingMouse = false;
    private float mouseHoldTimer = 0f;
    private bool hasStepped = false;
    private bool isLeftNext = true;

    private void Start()
    {
        targetRotation = transform.rotation;
    }

    private void Update()
    {
        HandleInput();
        HandleMove();
        HandleRotate();
    }

    private void HandleInput()
    {
         if (isBeingPushedBack)
        return; 

        if (!isRotating)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                targetRotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + 90f);
                isRotating = true;
                PerformStep(true);
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                targetRotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z - 90f);
                isRotating = true;
                PerformStep(false);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            isHoldingMouse = true;
            mouseHoldTimer = 0f;
            hasStepped = false;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isHoldingMouse = false;

            if (mouseHoldTimer < clickThreshold && !hasStepped && !IsObstacleInFront())
            {
                PerformStep();
            }
        }
    }

    private void HandleMove()
    {
         if (isBeingPushedBack)
        return; 

        if (isHoldingMouse && mouseHoldTimer >= clickThreshold && !isRotating && !IsObstacleInFront())
        {
            transform.position += transform.up * moveSpeed * Time.deltaTime;
            SetWalkingAnimation(true);
        }
        else if (!isRotating && !isHoldingMouse)
        {
            SetWalkingAnimation(false);
        }

        if (isHoldingMouse)
        {
            mouseHoldTimer += Time.deltaTime;
        }
    }

    private void HandleRotate()
    {
        if (!isRotating) return;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
        {
            transform.rotation = targetRotation;
            isRotating = false;
        }
    }

    private void PerformStep()
    {
        if (IsObstacleInFront())
        return; 

        // transform.position += transform.up * stepDistance;

        // if (isLeftNext)
        // {
        //     TriggerFootStep(leftFoot, "StepLeft");
        //     TriggerFootStep(rightFoot, "StepLeft");
        // }
        // else
        // {
        //     TriggerFootStep(leftFoot, "StepRight");
        //     TriggerFootStep(rightFoot, "StepRight");
        // }

        // isLeftNext = !isLeftNext;
        // hasStepped = true;
    }

    private void PerformStep(bool leftTurn)
    {
        // if (leftTurn)
        // {
        //     TriggerFootStep(leftFoot, "StepLeft");
        //     TriggerFootStep(rightFoot, "StepLeft");
        // }
        // else
        // {
        //     TriggerFootStep(leftFoot, "StepRight");
        //     TriggerFootStep(rightFoot, "StepRight");
        // }
    }

    private void SetWalkingAnimation(bool walking)
    {
        SetFootWalking(leftFoot, walking);
        SetFootWalking(rightFoot, walking);
    }

    private void TriggerFootStep(FootAnimatorGroup foot, string triggerName)
    {
        if (foot.outlineAnimator != null)
            foot.outlineAnimator.SetTrigger(triggerName);
        if (foot.colorAnimator != null)
            foot.colorAnimator.SetTrigger(triggerName);

        if (useFootEffect && foot.effectAnimators != null)
        {
            for (int i = 0; i < foot.effectAnimators.Count; i++)
            {
                if (foot.effectAnimators[i] != null)
                {
                    float delay = (i < effectDelays.Count) ? effectDelays[i] : 0f;
                    StartCoroutine(TriggerWithDelay(foot.effectAnimators[i], triggerName, delay));
                }
            }
        }
    }

    private void SetFootWalking(FootAnimatorGroup foot, bool walking)
    {
        if (foot.outlineAnimator != null)
            foot.outlineAnimator.SetBool("isWalking", walking);
        if (foot.colorAnimator != null)
            foot.colorAnimator.SetBool("isWalking", walking);

        if (useFootEffect && foot.effectAnimators != null)
        {
            for (int i = 0; i < foot.effectAnimators.Count; i++)
            {
                if (foot.effectAnimators[i] != null)
                {
                    float delay = (i < effectDelays.Count) ? effectDelays[i] : 0f;
                    StartCoroutine(SetWalkingWithDelay(foot.effectAnimators[i], walking, delay));
                }
            }
        }
    }

    public void StartPushback()
    {
        if (!isBeingPushedBack)
        {
            StartCoroutine(PushbackRoutine());
        }
    }

    private bool IsObstacleInFront()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, obstacleDetectionDistance, obstacleLayerMask);
        return hit.collider != null;
    }

    IEnumerator TriggerWithDelay(Animator animator, string triggerName, float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.SetTrigger(triggerName);
    }

    IEnumerator SetWalkingWithDelay(Animator animator, bool walking, float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.SetBool("isWalking", walking);
    }

    IEnumerator PushbackRoutine()
    {
        isBeingPushedBack = true;
        isHoldingMouse = false;
        
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition - transform.up * pushbackDistance; // Move backwards while facing forward

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * (pushbackSpeed / pushbackDistance);
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            SetWalkingAnimation(true);
            yield return null;
        }

        isBeingPushedBack = false;
        SetWalkingAnimation(false);
    }

}
