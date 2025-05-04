using UnityEngine;

public class AfterImageEffect : MonoBehaviour
{
    [Header("AfterImage Settings")]
    public SpriteRenderer sourceSprite;      // The original object's sprite
    public Color afterImageColor = Color.white;
    [Range(0f, 1f)] public float afterImageAlpha = 0.35f;
    public int afterImageSortingOrderOffset = -1;

    [Header("Orbit Settings")]
    public float orbitRadius = 1.15f;
    public float orbitSpeed = 0.5f;

    private GameObject afterImage;
    private SpriteRenderer afterImageRenderer;
    private float orbitAngle = 0f;

    private void Start()
    {
        if (sourceSprite == null)
        {
            sourceSprite = GetComponent<SpriteRenderer>();
        }

        if (sourceSprite == null)
        {
            Debug.LogError("No SpriteRenderer found on this object!");
            return;
        }

        // Create afterimage GameObject
        afterImage = new GameObject("AfterImage");
        afterImage.transform.SetParent(transform); // Inherit rotation/scale
        afterImage.transform.localPosition = Vector3.zero;
        afterImage.transform.localRotation = Quaternion.identity;
        afterImage.transform.localScale = Vector3.one;

        // Add SpriteRenderer
        afterImageRenderer = afterImage.AddComponent<SpriteRenderer>();
        afterImageRenderer.sprite = sourceSprite.sprite;
        afterImageRenderer.color = new Color(afterImageColor.r, afterImageColor.g, afterImageColor.b, afterImageAlpha);
        afterImageRenderer.sortingLayerID = sourceSprite.sortingLayerID;
        afterImageRenderer.sortingOrder = sourceSprite.sortingOrder + afterImageSortingOrderOffset;
    }

    private void Update()
    {
        if (afterImage == null || sourceSprite == null) return;

        // Sync sprite, sorting, and flip
        afterImageRenderer.sprite = sourceSprite.sprite;
        afterImageRenderer.sortingOrder = sourceSprite.sortingOrder + afterImageSortingOrderOffset;
        afterImageRenderer.flipX = sourceSprite.flipX;
        afterImageRenderer.flipY = sourceSprite.flipY;

        // Orbit offset in local space
        orbitAngle += orbitSpeed * Time.deltaTime;
        float x = Mathf.Cos(orbitAngle) * orbitRadius;
        float y = Mathf.Sin(orbitAngle) * orbitRadius;
        afterImage.transform.localPosition = new Vector3(x, y, 0f);
    }

}
