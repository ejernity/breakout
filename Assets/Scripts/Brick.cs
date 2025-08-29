using UnityEngine;
using UnityEngine.SceneManagement;

public class Brick : MonoBehaviour
{
    private int currentHitPoints;
    private int totalHitPoints;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        int currentSceneBuildIndex = GameManager.instance.GetCurrentSceneBuildIndex();
        totalHitPoints = Random.Range(1, currentSceneBuildIndex + 1);
        currentHitPoints = totalHitPoints;
    }

    public void OnBrickHit()
    {
        GameManager.instance.onBrickHit?.Invoke();
        currentHitPoints--;

        if (currentHitPoints <= 0)
        {
            GameManager.instance.onBrickDestroyed?.Invoke();
            Destroy(gameObject);
        }
        else
        {
            Color color = spriteRenderer.color;
            color.a = (float) currentHitPoints / totalHitPoints;
            spriteRenderer.color = color;
        }
    }
}
