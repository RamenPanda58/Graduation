using UnityEngine;

public class BackgroundChanger : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;  // Reference to the SpriteRenderer component
    public Sprite image1;  // First image
    public Sprite image2;  // Second image
    public float switchInterval = 1f;  // Interval to switch images (in seconds)

    private bool isImage1 = true;  // Track which image is currently displayed

    private void Start()
    {
        if (spriteRenderer == null || image1 == null || image2 == null)
        {
            Debug.LogError("Please assign the SpriteRenderer and sprites in the Inspector.");
            return;
        }

        // Start the background switch loop
        InvokeRepeating("SwitchBackground", 0f, switchInterval);
    }

    private void SwitchBackground()
    {
        // Toggle between the two sprites
        if (isImage1)
        {
            spriteRenderer.sprite = image1;
        }
        else
        {
            spriteRenderer.sprite = image2;
        }

        // Flip the flag for the next switch
        isImage1 = !isImage1;
    }
}