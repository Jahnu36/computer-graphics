using UnityEngine;
using System;
using System.Collections;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TMPro;

public class WordValidationManager : MonoBehaviour
{
    private const string GROQ_API_KEY = "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";
    private const string GROQ_API_ENDPOINT = "https://api.groq.com/v1/chat/completions";
    private GameManager gameManager;
    public TextMeshProUGUI timerText;           // Display the timer
    public TextMeshProUGUI scoreText;           // Display the score
    public TextMeshProUGUI matchedWordText;     // Display the matched word
    public GameObject gameOverPanel;            // Panel shown when the game ends

    private float timer = 0f;                   // Timer to track elapsed time
    private int score = 0;                      // Player's score
    private List<string> flippedWords = new List<string>(); // Track flipped bricks' word parts
    private int consecutiveIncorrectMatches = 0; // Track consecutive incorrect matches
    private const int maxIncorrectMatches = 3;  // Game ends after 3 consecutive incorrect matches

    private int consecutiveMatches = 0;         // Count of consecutive correct matches
    private int scoreMultiplier = 1;            // Score multiplier based on consecutive matches
    private const int bonusScoreForCombo = 20;  // Bonus points for consecutive matches (2)
    private const int maxMultiplier = 5;        // Max multiplier (optional)


    [Header("Word Validation Settings")]
    public TextMeshProUGUI validationFeedbackText;
    public float feedbackDisplayDuration = 2f;

    void Start()
    {
        gameManager = GetComponent<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("WordValidationManager requires a GameManager component on the same GameObject.");
        }

        UpdateScoreText();      // Initialize score display
        UpdateTimerText();      // Initialize timer display

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false); // Hide game-over panel at the start
        else
            Debug.LogError("GameOverPanel is not assigned in the Inspector.");

        if (matchedWordText != null)
            matchedWordText.text = ""; // Clear matched word display at the start
        else
            Debug.LogError("MatchedWordText is not assigned in the Inspector.");
    }
  

    void Update()
    {
        timer += Time.deltaTime;  // Increment the timer
        UpdateTimerText();        // Update the timer display
    }

    public void AddScore(int points)
    {
        // Add score with multiplier
        score += points * scoreMultiplier;          // Apply score multiplier
        UpdateScoreText();        // Update the score display
    }

    public async void ValidateWordCombination(string firstWord, string secondWord)
    {

            // If one wordPart is already flipped
            if (flippedWords.Count == 1)
            {
                string firstWordPart = flippedWords[0].Trim(); // Trim any extra spaces
                string secondWordPart = brick.wordPart.Trim(); // Trim any extra spaces

                // Log the word parts to check if they are correct
                Debug.Log("First Word Part: " + firstWordPart);
                Debug.Log("Second Word Part: " + secondWordPart);

                // Combine the two parts into a full word
                string combinedWord = firstWordPart + secondWordPart;

                // Log the combined word
                Debug.Log("Combined Word: " + combinedWord);

                // Normalize the combined word and validWords to ensure no case or whitespace issues
                combinedWord = combinedWord.ToLower().Trim(); // Ensure lower case and no spaces

                // Log the validWords list for debugging
                Debug.Log("Valid Words List: ");
                foreach (var validWord in validWords)
                {
                    Debug.Log(validWord);
                }

                // Check if the combined word is valid (case-insensitive and no spaces)
                if (validWords.Exists(word => word.ToLower().Trim() == combinedWord))
                {
                    Debug.Log("Matched Word: " + combinedWord);
                    matchedWordText.text = "Matched Word: " + combinedWord; // Display the matched word
                    AddScore(100); // Award points for matching
                    consecutiveIncorrectMatches = 0; // Reset incorrect match counter on a correct match

                    // Increment consecutive matches and apply combo scoring
                    consecutiveMatches++;
                    if (consecutiveMatches == 2)
                    {
                        // Apply bonus score after 2 consecutive matches
                        AddScore(bonusScoreForCombo);  // Add 20 bonus points for consecutive matches
                        Debug.Log("Bonus for 2 consecutive matches: " + bonusScoreForCombo);
                        consecutiveMatches = 0;  // Reset after awarding the bonus
                    }

                    // Apply score multiplier (increase with each correct match)
                    if (consecutiveMatches > 1 && scoreMultiplier < maxMultiplier)
                    {
                        scoreMultiplier++;
                    }
                }
                else
                {
                    // Log that the combined word is not valid
                    Debug.Log("Not a valid match: " + combinedWord);

                    consecutiveIncorrectMatches++; // Increment the consecutive incorrect match counter

                    // Check if consecutive incorrect matches exceed the limit
                    if (consecutiveIncorrectMatches >= maxIncorrectMatches)
                    {
                        EndGame(); // End the game after 3 consecutive incorrect matches
                        return;
                    }

                    // Reset combo and multiplier if the player makes an incorrect match
                    consecutiveMatches = 0;
                    scoreMultiplier = 1; // Reset multiplier after wrong match
                }

                // Clear flipped words for the next attempt
                flippedWords.Clear();
            }
            else
            {
                // Add the first flipped word part to flippedWords
                flippedWords.Add(brick.wordPart);
            }


    }

    private async Task<bool> CheckWordValidityWithGroq(string word)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {GROQ_API_KEY}");

                var requestBody = new
                {
                    model = "mixtral-8x7b-32768",
                    messages = new[]
                    {
                        new {
                            role = "system",
                            content = "You are a word validation assistant. Strictly determine if the given word is a valid English word."
                        },
                        new {
                            role = "user",
                            content = $"Is '{word}' a valid English word? Respond with only 'YES' or 'NO'."
                        }
                    },
                    temperature = 0,
                    max_tokens = 10
                };

                var response = await client.PostAsync(
                    GROQ_API_ENDPOINT,
                    new StringContent(JsonConvert.SerializeObject(requestBody), System.Text.Encoding.UTF8, "application/json")
                );

                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<dynamic>(responseContent);

                string result = responseObject.choices[0].message.content.ToString().Trim().ToUpper();
                return result == "YES";
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Word Validation Error: {e.Message}");
            return false;
        }
    }

    private void DisplayValidationFeedback(string message, Color feedbackColor)
    {
        if (validationFeedbackText != null)
        {
            validationFeedbackText.text = message;
            validationFeedbackText.color = feedbackColor;
            StartCoroutine(ClearFeedbackAfterDelay());
        }
    }

    private IEnumerator ClearFeedbackAfterDelay()
    {
        yield return new WaitForSeconds(feedbackDisplayDuration);
        if (validationFeedbackText != null)
        {
            validationFeedbackText.text = "";
        }
    }
    private void UpdateTimerText()
    {
        timerText.text = "Timer: " + timer.ToString("F2"); // Format the timer display
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score; // Update the score display
    }

    private void EndGame()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true); // Show the game-over panel
            Time.timeScale = 0;           // Pause the game
            Debug.Log("Game Over! Too many consecutive incorrect matches.");
        }
    }

    // Optionally add a bonus score based on time taken
    public void AddBonusForTime(float timeLeft)
    {
        int bonus = Mathf.FloorToInt(timeLeft); // Bonus score based on remaining time
        AddScore(bonus); // Add the time-based bonus score
    }

    // Get the current score
    public int GetScore()
    {
        return score;
    }
}