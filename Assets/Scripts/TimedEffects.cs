using UnityEngine;
using System.Collections;

public class TimedEffects : MonoBehaviour
{
    [Header("Target Sprite")]
    public SpriteRenderer targetSprite;

    [Header("Lightning Audio")]
    public AudioSource lightningAudio;

    [Tooltip("Randomize pitch slightly for variation")]
    public bool randomizePitch = true;

    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;

    [Header("Opacity Settings")]
    [Range(0f, 1f)] public float minOpacity = 0f;
    [Range(0f, 1f)] public float maxOpacity = 1f;

    [Header("Flash Speed")]
    [Tooltip("Higher = faster lightning flashes")]
    public float fadeSpeed = 100f;

    [Tooltip("Minimum random pause between flashes")]
    public float minPause = 0.03f;

    [Tooltip("Maximum random pause between flashes")]
    public float maxPause = 0.12f;

    [Header("Effect Timing")]
    [Tooltip("Time between lightning events")]
    public float effectInterval = 30f;

    private void Start()
    {
        if (targetSprite == null)
            targetSprite = GetComponent<SpriteRenderer>();

        if (lightningAudio == null)
            lightningAudio = GetComponent<AudioSource>();

        SetOpacity(minOpacity);

        StartCoroutine(EffectLoop());
    }

    IEnumerator EffectLoop()
    {
        while (true)
        {
            // Wait before triggering lightning
            yield return new WaitForSeconds(effectInterval);

            // Play lightning flicker sequence
            yield return StartCoroutine(SparkRoutine());
        }
    }

    IEnumerator SparkRoutine()
    {
        // Randomly choose 2–4 flashes
        int flashes = Random.Range(2, 5);

        for (int i = 0; i < flashes; i++)
        {
            // Play sound every flash
            PlayLightningSound();

            // Flash visual
            yield return StartCoroutine(FadeFlash());

            // Small random pause
            yield return new WaitForSeconds(
                Random.Range(minPause, maxPause)
            );
        }

        // Ensure sprite ends invisible
        SetOpacity(minOpacity);
    }

    IEnumerator FadeFlash()
    {
        float t = 0f;

        // Fast fade in
        while (t < 1f)
        {
            t += Time.deltaTime * fadeSpeed;

            SetOpacity(Mathf.Lerp(minOpacity, maxOpacity, t));

            yield return null;
        }

        // Fast fade out
        t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * fadeSpeed;

            SetOpacity(Mathf.Lerp(maxOpacity, minOpacity, t));

            yield return null;
        }
    }

    void PlayLightningSound()
    {
        if (lightningAudio == null) return;

        // Slight pitch variation
        if (randomizePitch)
        {
            lightningAudio.pitch = Random.Range(minPitch, maxPitch);
        }

       lightningAudio.PlayOneShot(lightningAudio.clip);
    }

    void SetOpacity(float alpha)
    {
        if (targetSprite == null) return;

        Color c = targetSprite.color;
        c.a = alpha;
        targetSprite.color = c;
    }
}