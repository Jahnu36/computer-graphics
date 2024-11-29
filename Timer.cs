using UnityEngine;

public class Timer : MonoBehaviour
{
    public float timeLimit = 60f;  // Set the timer limit (e.g., 60 seconds)
    private float timeRemaining;

    void Start()
    {
        timeRemaining = timeLimit;
    }

    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            timeRemaining = 0;
        }
    }

    public float GetTimeRemaining()
    {
        return timeRemaining;  // Return the remaining time
    }
}
