using UnityEngine;
using System.Collections;

public class SpriteFadeSwitcher : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite1Renderer;
    [SerializeField] private SpriteRenderer sprite2Renderer;

    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float visibleDuration = 0.5f;

    private void Start()
    {
        Color c = sprite2Renderer.color;
        c.a = 0f;
        sprite2Renderer.color = c;

        StartCoroutine(FadeLoop());
    }

    private IEnumerator FadeLoop()
    {
        while (true)
        {
            // Fade in
            yield return StartCoroutine(FadeSprite(0f, 1f));

            // Stay visible
            yield return new WaitForSeconds(visibleDuration);

            // Fade out
            yield return StartCoroutine(FadeSprite(1f, 0f));

            // Stay hidden
            yield return new WaitForSeconds(visibleDuration);
        }
    }

    private IEnumerator FadeSprite(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            float alpha = Mathf.Lerp(
                startAlpha,
                endAlpha,
                elapsed / fadeDuration);

            Color c = sprite2Renderer.color;
            c.a = alpha;
            sprite2Renderer.color = c;

            yield return null;
        }

        Color finalColor = sprite2Renderer.color;
        finalColor.a = endAlpha;
        sprite2Renderer.color = finalColor;
    }
}