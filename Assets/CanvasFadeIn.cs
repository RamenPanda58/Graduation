using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasFadeIn : MonoBehaviour
{
    public float delay = 1f;
    public float fadeDuration = 1f;

    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        // start fully invisible
        canvasGroup.alpha = 0f;
    }

    void Start()
    {
        StartCoroutine(FadeInRoutine());
    }

    IEnumerator FadeInRoutine()
    {
        // wait 1 second after scene loads
        yield return new WaitForSeconds(delay);

        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(t / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }
}