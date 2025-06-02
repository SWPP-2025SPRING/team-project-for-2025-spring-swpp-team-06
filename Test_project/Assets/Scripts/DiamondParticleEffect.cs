using UnityEngine;
using UnityEngine.UI;

public class DiamondParticleEffect : MonoBehaviour
{
    public float lifetime = 1.5f;
    public float minSpeed = 150f;
    public float maxSpeed = 300f;
    private float speed;
    public Vector2 direction;
    private RectTransform rect;
    private CanvasGroup canvasGroup;

    private float elapsed = 0f;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        direction.Normalize();
        speed = Random.Range(minSpeed, maxSpeed);
    }

    void Update()
    {
        float delta = Time.deltaTime;
        elapsed += delta;

        rect.anchoredPosition += direction * speed * delta;
        canvasGroup.alpha = Mathf.Lerp(1.5f, 0f, elapsed / lifetime);

        if (elapsed >= lifetime)
            Destroy(gameObject);
    }
}
