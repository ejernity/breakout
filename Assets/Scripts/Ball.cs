using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Vector3 startingPosition;
    private float anglePercentage = 0.67f;
    private float moveSpeed = 8f;
    private float powerUpFactor = 1f;
    public float minXVelocity = 2f;      // Ελάχιστη ταχύτητα στον Χ για να μην κολλάει κάθετα
    public float minΥVelocity = 2f;      // Ελάχιστη ταχύτητα στον Χ για να μην κολλάει κάθετα
    private bool isMoving;

    private void Awake()
    {
        isMoving = false;
        // Συνδέουμε τη μέθοδο μας με το event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        //Debug.Log($"Ball:Start:Level{SceneManager.GetActiveScene().name}");
        startingPosition = transform.position;
        GameManager.instance.onLostLife += ResetBall;
        GameManager.instance.onFastBallPowerup += ApplyFastBall;
        GameManager.instance.onSlowBallPowerup += ApplySlowBall;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log($"Scene {scene.name} loaded...");
        if (this == null) return; // check if the Ball still exists

        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) && !isMoving)
        {
            InitialPush();
        }
    }

    private void InitialPush()
    {
        Vector2 vector2 = Vector2.up;
        vector2.x = Random.Range(-anglePercentage, anglePercentage);
        rb2d.linearVelocity = vector2 * moveSpeed;
        isMoving = true;
    }

    private void ResetBall()
    {
        isMoving = false;
        rb2d.linearVelocity = Vector2.zero;
        transform.position = startingPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log($"Trigger with {collision.name}");

        DeadZone deadZone = collision.GetComponent<DeadZone>();
        if (deadZone)
        {
            powerUpFactor = 1f;
            GameManager.instance.OnDeadZoneEnter();
            GameManager.instance.onLostLife?.Invoke();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ---- 0. Λογική αν χτυπήσει στο Brick ----
        Brick brick = collision.collider.GetComponent<Brick>();
        if (brick != null)
        {
            brick.OnBrickHit();
        }

        // ---- 1. Σπάσιμο κάθετης τροχιάς ----
        Vector2 v = rb2d.linearVelocity;

        if (Mathf.Abs(v.x) < minXVelocity)
        {
            // Αν η μπάλα πάει πολύ κάθετα, δίνουμε ένα bump στον Χ
            v.x = Mathf.Sign(v.x) * minXVelocity;
            rb2d.linearVelocity = v.normalized * moveSpeed * powerUpFactor;
        }

        if (Mathf.Abs(v.y) < minΥVelocity)
        {
            v.y = Mathf.Sign(v.y) * minΥVelocity;
            rb2d.linearVelocity = v.normalized * moveSpeed * powerUpFactor;
        }

        // ---- 2. Ειδική λογική αν χτυπήσει στο Paddle ----
        if (collision.gameObject.CompareTag("Paddle"))
        {
            float paddleX = collision.transform.position.x;
            float hitPointX = collision.GetContact(0).point.x;

            // Offset του χτυπήματος σε σχέση με το κέντρο του paddle
            float offset = hitPointX - paddleX;
            float width = collision.collider.bounds.size.x / 2;

            float normalizedOffset = offset / width; // από -1 (αριστερά) έως 1 (δεξιά)

            // Υπολογίζουμε νέα κατεύθυνση
            Vector2 dir = new Vector2(normalizedOffset, 1).normalized;
            rb2d.linearVelocity = dir * moveSpeed * powerUpFactor;

            // Παίξε τον ήχο του collision σε paddle
            GameManager.instance.audioManager.PlayPaddleHitSound();
        }
    }

    public bool IsMoving { get { return isMoving; } }

    private void ApplyFastBall()
    {
        powerUpFactor += 0.1f;
    }

    private void ApplySlowBall()
    {
        powerUpFactor -= 0.1f;
    }
    private void OnDestroy()
    {
        //Debug.Log("Ball destroyed!");
        SceneManager.sceneLoaded -= OnSceneLoaded;
        GameManager.instance.onLostLife -= ResetBall;
        GameManager.instance.onFastBallPowerup -= ApplyFastBall;
        GameManager.instance.onSlowBallPowerup -= ApplySlowBall;
    }
}
