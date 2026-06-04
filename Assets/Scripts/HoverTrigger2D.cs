using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HoverTrigger2D : MonoBehaviour
{
    [SerializeField] private Image uiImage;
    [SerializeField] private float fadeDuration = 0.25f;

    private Coroutine fadeRoutine;

    private void Start()
    {
        SetAlpha(0f);
    }

    private void OnMouseEnter()
    {
        FadeTo(1f);
    }

    private void OnMouseExit()
    {
        FadeTo(0f);
    }

    private void FadeTo(float targetAlpha)
    {
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(Fade(targetAlpha));
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = uiImage.color.a;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            Color c = uiImage.color;
            c.a = Mathf.Lerp(startAlpha, targetAlpha, timer / fadeDuration);
            uiImage.color = c;

            yield return null;
        }

        SetAlpha(targetAlpha);
    }

    private void SetAlpha(float alpha)
    {
        Color c = uiImage.color;
        c.a = alpha;
        uiImage.color = c;
    }
}