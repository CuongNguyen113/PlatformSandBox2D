using UnityEngine;

public class PlatformShadow : MonoBehaviour {
    // Serialized fields (giữ nguyên)
    [SerializeField] private SpriteRenderer pixelSprite;
    [SerializeField] private float baseWidth = 0.2f;
    [SerializeField] private float maxHeight = 0.5f;
    [SerializeField][Range(0, 1)] private float shrinkFactor = 0.25f;
    [SerializeField][Range(0, 3)] private int chamfer = 1;
    [SerializeField] private Color darkestShade = Color.black;
    [SerializeField] private Color lightestShade = Color.black;
    [SerializeField] private LayerMask collisionLayers;
    [SerializeField] private float edgeDropTolerance = 0.03f;
    [SerializeField] private bool continueOffEdge;

    // Cache variables
    private ShadowPixel[] pixels;
    private RaycastHit2D[] raycastHits;
    private Vector2 rayStartOffset;
    private Vector2 tempPos;
    private Vector3 tempScale;
    private Color tempColor;
    private float invMaxHeight;
    private float shrinkWidth;
    private float chamferThreshold;

    private void Start() {
        raycastHits = new RaycastHit2D[1];
        InitializeShadow();
        CacheConstants();
    }

    private void CacheConstants() {
        invMaxHeight = 1f / maxHeight;
        shrinkWidth = baseWidth * shrinkFactor;
        chamferThreshold = chamfer;
    }

    private void InitializeShadow() {
        int pixelWidth = Mathf.CeilToInt(baseWidth * pixelSprite.sprite.pixelsPerUnit);
        pixels = new ShadowPixel[pixelWidth];
        rayStartOffset = Vector2.up * (baseWidth * 0.5f);

        float pixelSize = 1f / pixelSprite.sprite.pixelsPerUnit;
        float startX = -(baseWidth * 0.5f) + pixelSize * 0.5f;

        for (int i = 0; i < pixelWidth; i++) {
            tempPos.Set(startX + i * pixelSize, 0f);
            SpriteRenderer sr = Instantiate(pixelSprite, transform);
            sr.gameObject.name = $"Pixel_{i}";
            sr.transform.localPosition = tempPos;
            sr.gameObject.SetActive(true);
            pixels[i] = new ShadowPixel(tempPos, sr);
        }
    }

    private void LateUpdate() {
        UpdateShadow();
        transform.rotation = Quaternion.identity;
    }

    private void UpdateShadow() {
        SetBaseShadow();
        if (!continueOffEdge && pixels.Length > 0) {
            CullShadow();
        }
    }

    private void CullShadow() {
        bool leftToRight = pixels[0].initLocalPos.y > pixels[pixels.Length - 1].initLocalPos.y;
        int increment = leftToRight ? 1 : -1;
        int start = leftToRight ? 0 : pixels.Length - 1;
        int end = leftToRight ? pixels.Length : -1;
        bool shouldCull = false;
        float toleranceSquared = edgeDropTolerance * edgeDropTolerance;

        for (int i = start; i != end; i += increment) {
            if (!shouldCull && i + increment != end) {
                if (pixels[i].renderer.enabled) {
                    float diff = pixels[i].initLocalPos.y - pixels[i + increment].initLocalPos.y;
                    shouldCull = diff * diff > toleranceSquared;
                }
            }

            if (shouldCull) {
                pixels[i].renderer.enabled = false;
            }
        }
    }
    private void SetBaseShadow() {
        float scaleX = transform.lossyScale.x;
        float halfPpuWidth = pixels.Length / 2;
        float pixelShrinkBase = (baseWidth - shrinkWidth) * pixelSprite.sprite.pixelsPerUnit;

        for (int i = 0; i < pixels.Length; i++) {
            ShadowPixel sp = pixels[i];
            Vector2 rayOrigin = (Vector2)transform.position + sp.initLocalPos + rayStartOffset;

            int hitCount = Physics2D.RaycastNonAlloc(rayOrigin, Vector2.down, raycastHits, maxHeight * 2, collisionLayers);
            bool isValidHit = hitCount > 0 && raycastHits[0].distance > 0;
            sp.renderer.enabled = isValidHit;

            if (!isValidHit) continue;

            float distance = raycastHits[0].distance - rayStartOffset.magnitude;

            // Position update
            tempPos.x = sp.initLocalPos.x * scaleX;
            tempPos.y = -distance;
            sp.transform.localPosition = tempPos;

            // Distance calculations
            float distanceFromCenter = Mathf.Abs((pixels.Length / 2 - 0.5f) - i);
            float distanceFromEdge = halfPpuWidth - distanceFromCenter;
            float normalizedEdgeDistance = distanceFromEdge / halfPpuWidth;

            if (normalizedEdgeDistance > shrinkFactor) {
                float pixelShrinkDistance = pixelShrinkBase * (distance * invMaxHeight);
                if (distanceFromEdge < pixelShrinkDistance) {
                    sp.renderer.enabled = false;
                    continue;
                }
            }

            // Color update
            float t = distance * invMaxHeight;
            tempColor.r = darkestShade.r + (lightestShade.r - darkestShade.r) * t;
            tempColor.g = darkestShade.g + (lightestShade.g - darkestShade.g) * t;
            tempColor.b = darkestShade.b + (lightestShade.b - darkestShade.b) * t;
            tempColor.a = darkestShade.a + (lightestShade.a - darkestShade.a) * t;
            sp.renderer.color = tempColor;

            // Scale update
            tempScale.Set(1, (distanceFromEdge - chamferThreshold < 0) ? 1 : 2, 1);
            sp.transform.localScale = tempScale;
        }
    }

    // Phần còn lại giữ nguyên...
}

public readonly struct ShadowPixel {
    public readonly SpriteRenderer renderer;
    public readonly Transform transform;
    public readonly Vector2 initLocalPos;

    public ShadowPixel(Vector2 _initLocalPos, SpriteRenderer _renderer) {
        initLocalPos = _initLocalPos;
        renderer = _renderer;
        transform = _renderer.transform;
    }
}