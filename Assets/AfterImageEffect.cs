using UnityEngine;

public class AfterImageEffect : MonoBehaviour
{
    [Header("AfterImage Settings")]
    public SpriteRenderer sourceSprite;
    public Color afterImageColor = Color.white;
    [Range(0f, 1f)] public float afterImageAlpha = 0.35f;
    public int afterImageSortingOrderOffset = -1;

    [Header("Orbit Settings")]
    public float orbitRadius = 1.15f;
    public float orbitSpeed = 0.5f;

    [Header("Optimization")]
    public Transform player;
    public float updateDistance = 30f;
    public float updateInterval = 0.1f;

    private GameObject afterImage;
    private SpriteRenderer afterImageRenderer;
    private float orbitAngle = 0f;
    private float timer = 0f;

    private void Start()
    {
        if (sourceSprite == null)
            sourceSprite = GetComponent<SpriteRenderer>();

        if (sourceSprite == null)
        {
            Debug.LogError("No SpriteRenderer found on this object!");
            return;
        }

        afterImage = new GameObject("AfterImage");
        afterImage.transform.SetParent(transform);
        afterImage.transform.localPosition = Vector3.zero;
        afterImage.transform.localRotation = Quaternion.identity;
        afterImage.transform.localScale = Vector3.one;

        afterImageRenderer = afterImage.AddComponent<SpriteRenderer>();
        afterImageRenderer.sprite = sourceSprite.sprite;
        afterImageRenderer.color = new Color(afterImageColor.r, afterImageColor.g, afterImageColor.b, afterImageAlpha);
        afterImageRenderer.sortingLayerID = sourceSprite.sortingLayerID;
        afterImageRenderer.sortingOrder = sourceSprite.sortingOrder + afterImageSortingOrderOffset;
    }

    private void Update()
    {
        if (player != null && Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(player.position.x, player.position.y)) > updateDistance)
            return;

        timer += Time.deltaTime;
        if (timer >= updateInterval)
        {
            timer = 0f;
            UpdateAfterImage();
        }
    }

    private void UpdateAfterImage()
    {
        if (afterImage == null || sourceSprite == null) return;

        if (afterImageRenderer.sprite != sourceSprite.sprite)
            afterImageRenderer.sprite = sourceSprite.sprite;

        afterImageRenderer.sortingOrder = sourceSprite.sortingOrder + afterImageSortingOrderOffset;
        afterImageRenderer.flipX = sourceSprite.flipX;
        afterImageRenderer.flipY = sourceSprite.flipY;

        orbitAngle += orbitSpeed * updateInterval;
        float x = Mathf.Cos(orbitAngle) * orbitRadius;
        float y = Mathf.Sin(orbitAngle) * orbitRadius;
        afterImage.transform.localPosition = new Vector3(x, y, 0f);
    }
}
