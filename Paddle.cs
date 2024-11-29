using UnityEngine;

public class Paddle : MonoBehaviour
{
    public float speed = 10f;
    private float screenHalfWidthInWorldUnits;

    void Start()
    {
        float halfPaddleWidth = transform.localScale.x / 2f;
        screenHalfWidthInWorldUnits = Camera.main.aspect * Camera.main.orthographicSize - halfPaddleWidth;
    }

    void Update()
    {
        float input = Input.GetAxis("Horizontal");
        Vector3 position = transform.position;
        position.x += input * speed * Time.deltaTime;

        // Clamp the paddle position within screen bounds
        position.x = Mathf.Clamp(position.x, -screenHalfWidthInWorldUnits, screenHalfWidthInWorldUnits);
        transform.position = position;
    }
}
