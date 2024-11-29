using UnityEngine;

public class Ball : MonoBehaviour
{
    public float initialSpeed = 10f;   // Initial speed of the ball
    public float maxSpeed = 20f;       // Max speed limit for the ball
    public float speedIncreaseFactor = 1.05f;  // Factor by which speed will increase

    private float currentSpeed;
    private Rigidbody2D rb;
    private bool isLaunched = false;
    private Paddle paddle; // Reference to Paddle to sync movement

    private GameManager gameManager;
    private Timer timer;  // Timer to track the remaining time

    // Declare the aimLine (LineRenderer) to be used for aiming the ball's launch direction
    public LineRenderer aimLine; // This will allow you to draw the trajectory line for the ball

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        currentSpeed = initialSpeed;
        aimLine.enabled = true;  // Make sure aimLine is enabled for rendering the trajectory

        paddle = FindObjectOfType<Paddle>();
        gameManager = FindObjectOfType<GameManager>();
        timer = FindObjectOfType<Timer>();

        if (paddle == null) Debug.LogError("Paddle not found in the scene.");
        if (gameManager == null) Debug.LogError("GameManager not found in the scene.");
        if (timer == null) Debug.LogError("Timer not found in the scene.");
    }

    void Update()
    {
        if (!isLaunched)
        {
            transform.position = paddle.transform.position + new Vector3(0, 0.5f, 0);
            AimAndLaunch();
        }

        // Update ball speed based on the remaining time and score
        AdjustSpeedBasedOnTimeAndScore();
    }

    void AimAndLaunch()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector2 direction = (mousePosition - transform.position).normalized;

        aimLine.SetPosition(0, transform.position);  // Set the start point of the aim line
        aimLine.SetPosition(1, transform.position + (Vector3)direction * 5);  // Set the end point to show the direction

        if (Input.GetMouseButtonDown(0))
        {
            rb.isKinematic = false;
            rb.velocity = direction * currentSpeed;
            aimLine.enabled = false;  // Disable the aim line after the ball is launched
            isLaunched = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Brick"))
        {
            ResetBall();
        }
    }

    public void ResetBall()
    {
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        isLaunched = false;
        aimLine.enabled = true;  // Enable the aim line again after resetting the ball
        transform.position = paddle.transform.position + new Vector3(0, 0.5f, 0);
    }

    void AdjustSpeedBasedOnTimeAndScore()
    {
        // Increase speed based on remaining time (faster as time decreases)
        if (timer != null)
        {
            float timeRemaining = timer.GetTimeRemaining(); // Assuming timer.GetTimeRemaining() returns time left
            if (timeRemaining < 30f && currentSpeed < maxSpeed)
            {
                IncreaseBallSpeed(); // Increase speed if time is running out
            }
        }

        // Increase speed based on score (for example, every 100 points increase speed)
        if (gameManager != null)
        {
            int score = gameManager.GetScore();
            if (score >= 100 && currentSpeed < maxSpeed)
            {
                IncreaseBallSpeed(); // Increase speed if score threshold is met
            }
        }
    }

    void IncreaseBallSpeed()
    {
        if (currentSpeed < maxSpeed)
        {
            currentSpeed *= speedIncreaseFactor; // Increase speed by a factor
        }
    }
}
