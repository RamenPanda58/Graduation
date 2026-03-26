using UnityEngine;

public class BoardAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    public Sprite[] frames;            // Assign your sprites here
    public float framesPerSecond = 5f; // Animation speed
    public float startDelay = 1f;      // Delay before animation starts in seconds

    private SpriteRenderer spriteRenderer;
    private int currentFrame = 0;
    private float timer = 0f;
    private float delayTimer = 0f;
    private bool hasStarted = false;
    private bool isFinished = false;

    // Called as soon as the object exists
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Called whenever the object becomes active (scene load or re-enable)
    void OnEnable()
    {
        // Reset all animation variables
        currentFrame = 0;
        timer = 0f;
        delayTimer = 0f;
        hasStarted = false;
        isFinished = false;

        if (frames.Length == 0)
        {
            Debug.LogError("No frames assigned to BoardAnimation!");
            isFinished = true;
            return;
        }

        // Show first frame immediately
        spriteRenderer.sprite = frames[0];
    }

    void Update()
    {
        if (isFinished) return;

        // Handle start delay
        if (!hasStarted)
        {
            delayTimer += Time.deltaTime;
            if (delayTimer >= startDelay)
            {
                hasStarted = true;
            }
            else
            {
                return; // Still waiting, first frame stays visible
            }
        }

        // Animate frames
        timer += Time.deltaTime;
        if (timer >= 1f / framesPerSecond)
        {
            timer -= 1f / framesPerSecond;
            currentFrame++;

            if (currentFrame < frames.Length)
            {
                spriteRenderer.sprite = frames[currentFrame];
            }
            else
            {
                // Stay on last frame
                spriteRenderer.sprite = frames[frames.Length - 1];
                isFinished = true;
            }
        }
    }
}