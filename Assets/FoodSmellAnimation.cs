using System.Collections;
using UnityEngine;

public class FoodSmellAnimation : MonoBehaviour
{
    [Header("Sprites In Order")]
    public GameObject sprite1;
    public GameObject sprite2;
    public GameObject sprite3;

    [Header("Timing")]
    public float playEverySeconds = 5f;
    public float spriteDuration = 0.2f;

    [Header("Fade")]
    public float fadeDuration = 1f;

    private SpriteRenderer sprite3Renderer;

    void Start()
    {
        if (sprite3 != null)
            sprite3Renderer = sprite3.GetComponent<SpriteRenderer>();

        HideAll();

        StartCoroutine(AnimationLoop());
    }

    IEnumerator AnimationLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(playEverySeconds);

            yield return StartCoroutine(PlaySmellAnimation());
        }
    }

    IEnumerator PlaySmellAnimation()
{
    HideAll();

    // Sprite 1
    sprite1.SetActive(true);
    yield return new WaitForSeconds(spriteDuration);

    sprite1.SetActive(false);

    // Sprite 2
    sprite2.SetActive(true);
    yield return new WaitForSeconds(spriteDuration);

    sprite2.SetActive(false);

    // Sprite 3
    sprite3.SetActive(true);

    if (sprite3Renderer != null)
    {
        Color c = sprite3Renderer.color;

        // Make sure alpha starts at 1
        sprite3Renderer.color = new Color(c.r, c.g, c.b, 1f);

        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);

            sprite3Renderer.color = new Color(
                c.r,
                c.g,
                c.b,
                alpha
            );

            yield return null;
        }
    }

    sprite3.SetActive(false);

    // Reset alpha for next play
    if (sprite3Renderer != null)
    {
        Color c = sprite3Renderer.color;
        sprite3Renderer.color = new Color(c.r, c.g, c.b, 1f);
    }
}

    void HideAll()
    {
        if (sprite1 != null) sprite1.SetActive(false);
        if (sprite2 != null) sprite2.SetActive(false);
        if (sprite3 != null) sprite3.SetActive(false);
    }
}