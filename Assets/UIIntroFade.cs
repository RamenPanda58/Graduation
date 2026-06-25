using System.Collections;
using UnityEngine;

public class UIIntroFade : MonoBehaviour
{
    [Header("References")]
    public CanvasGroup textGroup;
    public CanvasGroup buttonGroup;

    [Header("Timing")]
    public float textFadeDuration = 1.5f;
    public float delayAfterText = 2f;
    public float buttonFadeDuration = 1f;

    private void Start()
    {
        textGroup.alpha = 0f;
        buttonGroup.alpha = 0f;

        StartCoroutine(FadeSequence());
    }

    private IEnumerator FadeSequence()
    {
        // Fade in text
        yield return StartCoroutine(FadeCanvasGroup(textGroup, 0f, 1f, textFadeDuration));

        // Wait 2 seconds
        yield return new WaitForSeconds(delayAfterText);

        // Fade in buttons
        yield return StartCoroutine(FadeCanvasGroup(buttonGroup, 0f, 1f, buttonFadeDuration));

        // Enable button interaction
        buttonGroup.interactable = true;
        buttonGroup.blocksRaycasts = true;
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup group, float start, float end, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            group.alpha = Mathf.Lerp(start, end, elapsed / duration);
            yield return null;
        }

        group.alpha = end;
    }
}