using UnityEngine;
using UnityEngine.SceneManagement;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager instance;

    public float createPowerUpPercentage = 0.2f;

    public GameObject[] powerUpPrefabs;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < powerUpPrefabs.Length; i++)
        {
            Debug.Log($"Prefab {powerUpPrefabs[i].name} loaded...");
        }
    }

    public void TrySpawnPowerUp(Vector3 powerUpPosition)
    {
        if (Random.value < createPowerUpPercentage)
        {
            int index = Random.Range(0, powerUpPrefabs.Length);
            Debug.Log($"Instantiate object {powerUpPrefabs[index].name}");
            Instantiate(powerUpPrefabs[index], powerUpPosition, Quaternion.identity);
        }
    }

    public void ApplyPowerUp(PowerUpType type)
    {
        Debug.Log($"Applying powerup of type {type}");

        switch(type)
        {
            case PowerUpType.ExtraLife:
                ApplyExtraLife();
                break;
            case PowerUpType.SlowBall:
                ApplySlowBall();
                break;
            case PowerUpType.FastBall:
                ApplyFastBall();
                break;
            case PowerUpType.BiggerPaddle:
                ApplyBiggerPaddle();
                break;
        }
    }

    private void ApplyExtraLife()
    {
        GameManager.instance.AddExtraLife();
    }

    private void ApplySlowBall()
    {
        GameManager.instance.onSlowBallPowerup?.Invoke();
    }

    private void ApplyFastBall()
    {
        GameManager.instance.onFastBallPowerup?.Invoke();
    }

    private void ApplyBiggerPaddle()
    {
        GameManager.instance.onBiggerPaddlePowerup?.Invoke();
    }
}
