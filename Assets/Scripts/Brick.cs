using UnityEngine;

public class Brick : MonoBehaviour
{
    private int currentHitPoints;
    private int totalHitPoints;
    private SpriteRenderer spriteRenderer;

    public GameObject brickExplosionParticleEffect;

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
            DestroyBrick();
        }
        else
        {
            Color color = spriteRenderer.color;
            color.a = (float)currentHitPoints / totalHitPoints;
            spriteRenderer.color = color;
        }
    }

    public void DestroyBrick()
    {
        Instantiate(brickExplosionParticleEffect, this.transform.position, Quaternion.identity);
        GameManager.instance.onBrickDestroyed?.Invoke();
        PowerUpManager.instance.TrySpawnPowerUp(transform.position);
        Destroy(gameObject);
    }
}
