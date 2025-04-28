using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageWalker : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    private Vector2Int currentPosition = Vector2Int.zero;

    private float moveDelay = 0.1f;       // How fast to move once holding
    private float holdThreshold = 0.05f;   // Time before auto-repeat starts
    private float keyHoldTime = 0f;

    private float repeatTimer = 0f;
    private bool isHolding = false;
    private Vector2Int heldDirection = Vector2Int.zero;


    void Start()
    {
        UpdateDisplay();
    }

    void Update()
    {
        Vector2Int input = GetHeldDirection();

        if (input != Vector2Int.zero)
        {
            if (!isHolding)
            {
                // First frame: register move immediately
                TryMove(input);
                isHolding = true;
                heldDirection = input;
                keyHoldTime = 0f;
                repeatTimer = moveDelay;
            }
            else
            {
                keyHoldTime += Time.deltaTime;

                if (keyHoldTime >= holdThreshold)
                {
                    // Start repeating once hold threshold is passed
                    repeatTimer -= Time.deltaTime;
                    if (repeatTimer <= 0f)
                    {
                        TryMove(heldDirection);
                        repeatTimer = moveDelay;
                    }
                }
            }
        }
        else
        {
            isHolding = false;
            heldDirection = Vector2Int.zero;
            keyHoldTime = 0f;
        }
    }


    void TryMove(Vector2Int direction)
    {
        Vector2Int targetPos = currentPosition + direction;
        if (HasImageAt(targetPos))
        {
            currentPosition = targetPos;
            UpdateDisplay();
        }
}

    Vector2Int GetHeldDirection()
    {
        Vector2Int dir = Vector2Int.zero;
        if (Input.GetKey(KeyCode.W)) dir.y += 1;
        if (Input.GetKey(KeyCode.S)) dir.y -= 1;
        if (Input.GetKey(KeyCode.A)) dir.x -= 1;
        if (Input.GetKey(KeyCode.D)) dir.x += 1;
        return dir;
    }

    void UpdateDisplay()
    {
        string spritePath = $"Sprites/img_{currentPosition.x}_{currentPosition.y}";
        Sprite sprite = Resources.Load<Sprite>(spritePath);

        if (sprite != null)
        {
            spriteRenderer.sprite = sprite;
        }
        else
        {
            Debug.LogWarning($"No image at {currentPosition} — looking for: {spritePath}");
            spriteRenderer.sprite = null;
        }
    }

    bool HasImageAt(Vector2Int pos)
    {
        string spritePath = $"Sprites/img_{pos.x}_{pos.y}";
        return Resources.Load<Sprite>(spritePath) != null;
    }

}
