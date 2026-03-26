using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanOnce : MonoBehaviour
{
    [Header("Pan Settings")]
    public float distance = 0.5f;
    public float duration = 2f;

    [Header("UI Fade Settings")]
    public TextMeshProUGUI textUI; // Assign your TextMeshProUGUI here
    public Image imageUI;           // Assign your Image here
    public Button buttonUI;         // Assign your Button here
    public float fadeDuration = 2f;

    private Vector3 startPos;
    private Vector3 targetPos;
    private float timeElapsed = 0f;
    private bool isPanning = true;
    private bool isFading = false;
    private float fadeTimer = 0f;

    void Start()
    {
        startPos = transform.position;
        targetPos = startPos + new Vector3(distance, 0f, 0f);

        // Make text, image, and button invisible at start
        if (textUI != null)
        {
            Color c = textUI.color;
            c.a = 0f;
            textUI.color = c;
        }
        if (imageUI != null)
        {
            Color c = imageUI.color;
            c.a = 0f;
            imageUI.color = c;
        }
        if (buttonUI != null)
        {
            Image buttonImage = buttonUI.GetComponent<Image>();
            if (buttonImage != null)
            {
                Color c = buttonImage.color;
                c.a = 0f;
                buttonImage.color = c;
            }

            // Optional: fade button text if it exists
            TextMeshProUGUI buttonText = buttonUI.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                Color c = buttonText.color;
                c.a = 0f;
                buttonText.color = c;
            }
        }
    }

    void Update()
    {
        // --- PANNING ---
        if (isPanning)
        {
            timeElapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, timeElapsed / duration);
            transform.position = Vector3.Lerp(startPos, targetPos, t);

            if (t >= 1f)
            {
                transform.position = targetPos;
                isPanning = false;
                isFading = true;
            }
        }

        // --- FADING ---
        if (isFading)
        {
            fadeTimer += Time.deltaTime;
            float t = fadeTimer / fadeDuration;

            // Fade text UI
            if (textUI != null)
            {
                Color c = textUI.color;
                c.a = Mathf.Lerp(0f, 1f, t);
                textUI.color = c;
            }

            // Fade image UI
            if (imageUI != null)
            {
                Color c = imageUI.color;
                c.a = Mathf.Lerp(0f, 1f, t);
                imageUI.color = c;
            }

            // Fade button UI
            if (buttonUI != null)
            {
                Image buttonImage = buttonUI.GetComponent<Image>();
                if (buttonImage != null)
                {
                    Color c = buttonImage.color;
                    c.a = Mathf.Lerp(0f, 1f, t);
                    buttonImage.color = c;
                }

                TextMeshProUGUI buttonText = buttonUI.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    Color c = buttonText.color;
                    c.a = Mathf.Lerp(0f, 1f, t);
                    buttonText.color = c;
                }
            }

            if (t >= 1f)
            {
                isFading = false;
            }
        }
    }
}