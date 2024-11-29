using System.Collections;
using UnityEngine;
using TMPro;

public class Brick : MonoBehaviour
{
    public int points = 100;
    public string wordPart;   // The half-word assigned to this brick
    public TextMeshPro wordText;  // Reference to the TextMeshPro component

    private bool isFlipped = false;
    private GameManager gameManager;
    private float displayTime = 1f;   // Time to show the word before hiding

    // Floating variables
    private Rigidbody2D rb;
    public float floatingSpeed = 1f;   // Speed of floating up and down
    public float floatingAmplitude = 0.1f; // The height of the floating movement
    private float initialYPosition; // The starting Y position for each brick's float

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene.");
        }

        // Initialize the Rigidbody2D for floating effect
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;  // Disable gravity for floating effect
        rb.freezeRotation = true;  // Prevent brick rotation (important!)

        // Store the initial Y position of the brick for reference
        initialYPosition = transform.position.y;

        if (wordText != null)
        {
            wordText.text = ""; // Hide text initially
        }
    }

    void Update()
    {
        // Apply the floating effect (sine wave)
        if (rb != null)
        {
            // Using sine wave to create a gentle up-and-down floating effect within the limited range
            float floatMovement = Mathf.Sin(Time.time * floatingSpeed) * floatingAmplitude;

            // Keep the brick floating around its initial Y position (not too much up/down)
            float newYPosition = initialYPosition + floatMovement;

            // Update the position with limited Y movement
            rb.MovePosition(new Vector2(transform.position.x, newYPosition));
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            if (!isFlipped)
            {
                ShowWordPart();
                gameManager.CheckForCompleteWord(this); // Check for word completion
            }
        }
    }

    private void ShowWordPart()
    {
        isFlipped = true;
        // Display the word part on the front of the brick
        if (wordText != null)
        {
            wordText.text = wordPart;
            wordText.gameObject.SetActive(true);
            StartCoroutine(HideWordPartAfterTime(displayTime));
        }
    }

    private IEnumerator HideWordPartAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        wordText.gameObject.SetActive(false);
        isFlipped = false;
    }

    public void Highlight()
    {
        GetComponent<SpriteRenderer>().color = Color.green; // Highlight brick when matched
    }
}
