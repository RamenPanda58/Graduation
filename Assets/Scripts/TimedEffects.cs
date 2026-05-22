using UnityEngine;
using System.Collections;

public class TimedEffects : MonoBehaviour
{
    [Header("Target Sprite (Lightning Flash)")]
    public SpriteRenderer targetSprite;

    [Header("Dark Overlay / Rectangle")]
    public SpriteRenderer darkOverlay;

    [Tooltip("Normal darkness opacity")]
    [Range(0f, 1f)]
    public float normalOverlayOpacity = 0.7f;

    [Tooltip("How transparent the darkness becomes during lightning")]
    [Range(0f, 1f)]
    public float lightningOverlayOpacity = 0.2f;

    [Header("Lightning Audio")]
    public AudioSource lightningAudio;

    [Tooltip("Randomize pitch slightly")]
    public bool randomizePitch = true;
    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;

    [Header("Door Chime Audio")]
    public AudioSource doorChimeAudio;

    [Tooltip("Delay before first chime plays")]
    public float firstChimeDelay = 0.5f;

    [Tooltip("Exact time between chimes (e.g. 20 seconds)")]
    public float chimeInterval = 20f;

    [Header("Lightning Sprite Opacity")]
    [Range(0f, 1f)] public float minOpacity = 0f;
    [Range(0f, 1f)] public float maxOpacity = 1f;

    [Header("Flash Speed")]
    [Tooltip("Higher = faster lightning")]
    public float fadeSpeed = 100f;

    [Tooltip("Minimum pause between flashes")]
    public float minPause = 0.03f;

    [Tooltip("Maximum pause between flashes")]
    public float maxPause = 0.12f;

    [Header("Effect Timing")]
    public float effectInterval = 30f;

    private void Start()
    {
        if (targetSprite == null)
            targetSprite = GetComponent<SpriteRenderer>();

        if (lightningAudio == null)
            lightningAudio = GetComponent<AudioSource>();

        SetOpacity(minOpacity);
        SetOverlayOpacity(normalOverlayOpacity);

        StartCoroutine(EffectLoop());
        StartCoroutine(DoorChimeLoop());
    }

    IEnumerator EffectLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(effectInterval);
            yield return StartCoroutine(SparkRoutine());
        }
    }

    IEnumerator DoorChimeLoop()
    {
        yield return new WaitForSeconds(firstChimeDelay);

        while (true)
        {
            if (doorChimeAudio != null && doorChimeAudio.clip != null)
            {
                doorChimeAudio.pitch = Random.Range(0.95f, 1.05f);
                doorChimeAudio.PlayOneShot(doorChimeAudio.clip);
            }

            yield return new WaitForSeconds(chimeInterval);
        }
    }

    IEnumerator SparkRoutine()
    {
        int flashes = Random.Range(2, 5);

        for (int i = 0; i < flashes; i++)
        {
            PlayLightningSound();

            yield return StartCoroutine(FadeFlash());

            yield return new WaitForSeconds(
                Random.Range(minPause, maxPause)
            );
        }

        SetOpacity(minOpacity);
        SetOverlayOpacity(normalOverlayOpacity);
    }

    IEnumerator FadeFlash()
    {
        float t = 0f;

        // Fade IN
        while (t < 1f)
        {
            t += Time.deltaTime * fadeSpeed;

            float flashAlpha = Mathf.Lerp(minOpacity, maxOpacity, t);
            SetOpacity(flashAlpha);

            float overlayAlpha = Mathf.Lerp(
                normalOverlayOpacity,
                lightningOverlayOpacity,
                t
            );
            SetOverlayOpacity(overlayAlpha);

            yield return null;
        }

        // Fade OUT
        t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * fadeSpeed;

            float flashAlpha = Mathf.Lerp(maxOpacity, minOpacity, t);
            SetOpacity(flashAlpha);

            float overlayAlpha = Mathf.Lerp(
                lightningOverlayOpacity,
                normalOverlayOpacity,
                t
            );
            SetOverlayOpacity(overlayAlpha);

            yield return null;
        }
    }

    void PlayLightningSound()
    {
        if (lightningAudio == null) return;

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

    void SetOverlayOpacity(float alpha)
    {
        if (darkOverlay == null) return;

        Color c = darkOverlay.color;
        c.a = alpha;
        darkOverlay.color = c;
    }
}