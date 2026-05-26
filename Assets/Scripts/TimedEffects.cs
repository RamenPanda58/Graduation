using UnityEngine;
using System.Collections;

public class TimedEffects : MonoBehaviour
{
    [Header("Target Sprite (Lightning Flash)")]
    public SpriteRenderer targetSprite;

    [Header("Dark Overlay / Rectangle")]
    public SpriteRenderer darkOverlay;

    [Range(0f, 1f)]
    public float normalOverlayOpacity = 0.7f;

    [Range(0f, 1f)]
    public float lightningOverlayOpacity = 0.2f;

    [Header("Lightning Audio")]
    public AudioSource lightningAudio;
    public bool randomizePitch = true;
    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;

    [Header("Door Chime Audio")]
    public AudioSource doorChimeAudio;
    public float firstChimeDelay = 0.5f;
    public float chimeInterval = 20f;

    [Header("Twin Reaction Timing")]
public float twinReactionDelay = 2f;

    [Header("Twin Reaction (Chime Response)")]
    public Animator twinAnimator;
    public AnimationClip twinReactionClip;
    public AnimationClip twinIdleClip;

    [Header("Character Root (Fade In Group)")]
    public GameObject characterRoot;
    public float characterFadeDuration = 2f;

    [Header("Lightning Sprite Opacity")]
    [Range(0f, 1f)] public float minOpacity = 0f;
    [Range(0f, 1f)] public float maxOpacity = 1f;

    [Header("Flash Speed")]
    public float fadeSpeed = 100f;
    public float minPause = 0.03f;
    public float maxPause = 0.12f;

    [Header("Effect Timing")]
    public float effectInterval = 30f;

    // ---------------- JUMP SCARE SYSTEM ----------------
    [Header("Sailor Jump Scare")]
    public Animator sailorAnimator;
    public AnimationClip jumpScareClip;
    public AnimationClip idleClip;

    public float jumpScareCooldown = 2f;
    private float lastJumpScareTime = 0f;
    // ---------------------------------------------------

    private static bool hasFadedInCharacters = false;

    private SpriteRenderer[] characterSprites;
    private Color[] originalColors;

    void Start()
    {
        if (targetSprite == null)
            targetSprite = GetComponent<SpriteRenderer>();

        if (lightningAudio == null)
            lightningAudio = GetComponent<AudioSource>();

        SetOpacity(minOpacity);
        SetOverlayOpacity(normalOverlayOpacity);

        if (characterRoot != null)
        {
            characterSprites = characterRoot.GetComponentsInChildren<SpriteRenderer>();
            originalColors = new Color[characterSprites.Length];

            for (int i = 0; i < characterSprites.Length; i++)
            {
                originalColors[i] = characterSprites[i].color;

                if (!hasFadedInCharacters)
                {
                    Color c = characterSprites[i].color;
                    c.a = 0f;
                    characterSprites[i].color = c;
                }
            }
        }

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

            // 👇 DELAYED TWIN REACTION (2 seconds after chime starts)
            StartCoroutine(DelayedTwinReaction());

            if (!hasFadedInCharacters)
            {
                hasFadedInCharacters = true;

                if (characterRoot != null)
                    StartCoroutine(FadeInCharacters());
            }

            yield return new WaitForSeconds(chimeInterval);
        }
    }

    IEnumerator DelayedTwinReaction()
    {
       yield return new WaitForSeconds(twinReactionDelay);
        TriggerTwinReaction();
    }

    IEnumerator FadeInCharacters()
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / characterFadeDuration;

            for (int i = 0; i < characterSprites.Length; i++)
            {
                Color original = originalColors[i];
                Color c = original;
                c.a = Mathf.Lerp(0f, original.a, t);
                characterSprites[i].color = c;
            }

            yield return null;
        }

        for (int i = 0; i < characterSprites.Length; i++)
        {
            characterSprites[i].color = originalColors[i];
        }
    }

    IEnumerator SparkRoutine()
    {
        int flashes = Random.Range(2, 5);

        for (int i = 0; i < flashes; i++)
        {
            PlayLightningSound();
            TriggerJumpScare();

            yield return StartCoroutine(FadeFlash());
            yield return new WaitForSeconds(Random.Range(minPause, maxPause));
        }

        SetOpacity(minOpacity);
        SetOverlayOpacity(normalOverlayOpacity);
    }

    IEnumerator FadeFlash()
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * fadeSpeed;

            SetOpacity(Mathf.Lerp(minOpacity, maxOpacity, t));
            SetOverlayOpacity(Mathf.Lerp(normalOverlayOpacity, lightningOverlayOpacity, t));

            yield return null;
        }

        t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * fadeSpeed;

            SetOpacity(Mathf.Lerp(maxOpacity, minOpacity, t));
            SetOverlayOpacity(Mathf.Lerp(lightningOverlayOpacity, normalOverlayOpacity, t));

            yield return null;
        }
    }

    // ---------------- JUMP SCARE ----------------

    void TriggerJumpScare()
    {
        if (sailorAnimator == null) return;
        if (jumpScareClip == null) return;
        if (idleClip == null) return;

        if (Time.time < lastJumpScareTime + jumpScareCooldown) return;

        lastJumpScareTime = Time.time;
        StartCoroutine(PlayJumpScareThenIdle());
    }

    IEnumerator PlayJumpScareThenIdle()
    {
        sailorAnimator.Play(jumpScareClip.name);
        yield return new WaitForSeconds(jumpScareClip.length);
        sailorAnimator.Play(idleClip.name);
    }

    // ---------------- TWIN REACTION ----------------

    void TriggerTwinReaction()
    {
        if (twinAnimator == null) return;
        if (twinReactionClip == null) return;
        if (twinIdleClip == null) return;

        StartCoroutine(PlayTwinReactionThenIdle());
    }

    IEnumerator PlayTwinReactionThenIdle()
    {
        twinAnimator.Play(twinReactionClip.name);
        yield return new WaitForSeconds(twinReactionClip.length);
        twinAnimator.Play(twinIdleClip.name);
    }

    // --------------------------------------------

    void PlayLightningSound()
    {
        if (lightningAudio == null || lightningAudio.clip == null)
            return;

        if (randomizePitch)
            lightningAudio.pitch = Random.Range(minPitch, maxPitch);

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