using UnityEngine;

public class Paddle : MonoBehaviour
{
    public Ball ball;

    private Rigidbody2D rb2d;
    private Vector3 startingPosition;
    private float moveSpeed = 8f;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        GameManager.instance.onLostLife += ResetPaddle;
        startingPosition = transform.position;
    }

    void Update()
    {
        float movement = Input.GetAxis("Horizontal");
        Move(movement);
    }

    private void Move(float movement)
    {
        Vector2 vector2 = rb2d.linearVelocity;
        vector2.x = movement * moveSpeed;
        rb2d.linearVelocity = vector2;

        if (ball.IsMoving == false)
        {
            MoveBall(movement);
        }
    }

    private void MoveBall(float movement)
    {
        float ballPositionY = ball.transform.position.y;
        Vector3 vector3 = transform.position;
        vector3.y = ballPositionY;
        ball.transform.position = vector3;
    }

    private void ResetPaddle()
    {
        transform.position = startingPosition;
    }

    private void OnDestroy()
    {
        GameManager.instance.onLostLife -= ResetPaddle;
    }
}
