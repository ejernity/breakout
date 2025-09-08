using System;
using UnityEngine;

public enum PowerUpType { ExtraLife, BiggerPaddle, SlowBall, FastBall, Fireball }

public class PowerUp : MonoBehaviour
{
    public PowerUpType type;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeadZone"))
        {
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Paddle"))
        {
            PowerUpManager.instance.ApplyPowerUp(type);
            Destroy(gameObject);
        }
    }
}
