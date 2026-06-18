using UnityEngine;
using System.Collections;

public class SailorReactions : MonoBehaviour
{
    [Header("Animation Blending")]
    public float animationBlendTime = 0.25f;

    // =====================================================
    // SMELL SYSTEM
    // =====================================================
    [Header("Smell Event (Food Reaction)")]
    public float smellInterval = 12f;

    public SpriteRenderer smellSprite1;
    public SpriteRenderer smellSprite2;
    public SpriteRenderer smellSprite3;

    public float smellFrameDelay = 0.2f;
    public float smellFadeOutTime = 1.5f;

    public AnimationClip sailorSmellReactionClip;
    public AnimationClip idleClip;

    private bool isSmellPlaying = false;

    // =====================================================
    // STOMACH AUDIO (SIMPLIFIED)
    // =====================================================
    [Header("Stomach Audio Reaction")]
    public AudioSource stomachAudioSource;

    public float stomachDelay = 2f;

    public bool randomizeStomachPitch = true;
    public float minStomachPitch = 0.95f;
    public float maxStomachPitch = 1.05f;

    // =====================================================
    // DARK OVERLAY
    // =====================================================
    [Header("Dark Overlay / Rectangle")]
    public SpriteRenderer darkOverlay;

    [Range(0f, 1f)]
    public float normalOverlayOpacity = 0.7f;

    [Range(0f, 1f)]
    public float lightningOverlayOpacity = 0.2f;

    // =====================================================
    // WHITE LIGHTNING FLASH
    // =====================================================
    [Header("Lightning Flash Overlay")]
    public SpriteRenderer lightFlashOverlay;

    [Range(0f, 1f)]
    public float flashMaxOpacity = 1f;

    public float flashInSpeed = 40f;
    public float flashOutSpeed = 25f;

    // =====================================================
    // LIGHTNING EFFECTS
    // =====================================================
    [Header("Flash Timing")]
    public float fadeSpeed = 100f;
    public float minPause = 0.03f;
    public float maxPause = 0.12f;

    [Header("Effect Timing")]
    public float effectInterval = 30f;

    [Header("Lightning Audio")]
    public AudioSource lightningAudio;
    public bool randomizePitch = true;
    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;

    // =====================================================
    // JUMP SCARE
    // =====================================================
    [Header("Sailor Jump Scare")]
    public Animator sailorAnimator;
    public AnimationClip jumpScareClip;

    public float jumpScareCooldown = 2f;
    private float lastJumpScareTime = 0f;

    // =====================================================
    // INTERNAL
    // =====================================================
    private SpriteRenderer targetSprite;

    void Start()
    {
        targetSprite = GetComponent<SpriteRenderer>();

        if (lightningAudio == null)
            lightningAudio = GetComponent<AudioSource>();

        SetOpacity(1f);
        SetOverlayOpacity(normalOverlayOpacity);

        if (lightFlashOverlay != null)
            SetFlashOpacity(0f);

        StartCoroutine(EffectLoop());
        StartCoroutine(SmellLoop());
    }

    // =====================================================
    // LIGHTNING LOOP
    // =====================================================

    IEnumerator EffectLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(effectInterval);
            yield return StartCoroutine(SparkRoutine());
        }
    }

    IEnumerator SparkRoutine()
    {
        int flashes = Random.Range(2, 5);

        for (int i = 0; i < flashes; i++)
        {
            PlayLightningSound();
            TriggerJumpScare();

            PlayFlashOverlay();

            yield return StartCoroutine(FadeFlash());
            yield return new WaitForSeconds(Random.Range(minPause, maxPause));
        }

        SetOverlayOpacity(normalOverlayOpacity);
    }

    IEnumerator FadeFlash()
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * fadeSpeed;
            SetOverlayOpacity(Mathf.Lerp(normalOverlayOpacity, lightningOverlayOpacity, t));
            yield return null;
        }

        t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * fadeSpeed;
            SetOverlayOpacity(Mathf.Lerp(lightningOverlayOpacity, normalOverlayOpacity, t));
            yield return null;
        }
    }

    // =====================================================
    // WHITE FLASH OVERLAY
    // =====================================================

    void PlayFlashOverlay()
    {
        if (lightFlashOverlay != null)
            StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * flashInSpeed;
            SetFlashOpacity(Mathf.Lerp(0f, flashMaxOpacity, t));
            yield return null;
        }

        t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * flashOutSpeed;
            SetFlashOpacity(Mathf.Lerp(flashMaxOpacity, 0f, t));
            yield return null;
        }
    }

    void SetFlashOpacity(float alpha)
    {
        if (lightFlashOverlay == null) return;

        Color c = lightFlashOverlay.color;
        c.a = alpha;
        lightFlashOverlay.color = c;
    }

    // =====================================================
    // LIGHTNING AUDIO
    // =====================================================

    void PlayLightningSound()
    {
        if (lightningAudio == null || lightningAudio.clip == null)
            return;

        if (randomizePitch)
            lightningAudio.pitch = Random.Range(minPitch, maxPitch);

        lightningAudio.PlayOneShot(lightningAudio.clip);
    }

    // =====================================================
    // JUMP SCARE
    // =====================================================

    void TriggerJumpScare()
    {
        if (sailorAnimator == null || jumpScareClip == null) return;
        if (Time.time < lastJumpScareTime + jumpScareCooldown) return;

        lastJumpScareTime = Time.time;
        StartCoroutine(JumpScareRoutine());
    }

    IEnumerator JumpScareRoutine()
    {
        sailorAnimator.CrossFade(jumpScareClip.name, animationBlendTime);
        yield return new WaitForSeconds(jumpScareClip.length);
        sailorAnimator.CrossFade(idleClip.name, animationBlendTime);
    }

    // =====================================================
    // SMELL SYSTEM
    // =====================================================

    IEnumerator SmellLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(smellInterval);
            yield return StartCoroutine(SmellRoutine());
        }
    }

    IEnumerator SmellRoutine()
    {
        if (isSmellPlaying) yield break;
        isSmellPlaying = true;

        if (sailorAnimator != null && sailorSmellReactionClip != null)
            sailorAnimator.CrossFade(sailorSmellReactionClip.name, animationBlendTime);

        StartCoroutine(PlayStomachGurgle());

        yield return StartCoroutine(SmellSpritesRoutine());

        if (sailorAnimator != null && idleClip != null)
            sailorAnimator.CrossFade(idleClip.name, animationBlendTime);

        isSmellPlaying = false;
    }

    IEnumerator PlayStomachGurgle()
    {
        yield return new WaitForSeconds(stomachDelay);

        if (stomachAudioSource == null || stomachAudioSource.clip == null)
            yield break;

        if (randomizeStomachPitch)
            stomachAudioSource.pitch = Random.Range(minStomachPitch, maxStomachPitch);

        stomachAudioSource.PlayOneShot(stomachAudioSource.clip);
    }

    IEnumerator SmellSpritesRoutine()
    {
        SetSmellActive(false, false, false);

        if (smellSprite1 != null)
        {
            SetSmellActive(true, false, false);
            yield return new WaitForSeconds(smellFrameDelay);
        }

        if (smellSprite2 != null)
        {
            SetSmellActive(false, true, false);
            yield return new WaitForSeconds(smellFrameDelay);
        }

        if (smellSprite3 != null)
        {
            SetSmellActive(false, false, true);
            yield return StartCoroutine(FadeOutSprite(smellSprite3, smellFadeOutTime));
        }

        SetSmellActive(false, false, false);
        ResetSpriteAlpha(smellSprite3);
    }

    IEnumerator FadeOutSprite(SpriteRenderer sr, float duration)
    {
        if (sr == null) yield break;

        float t = 0f;
        Color c = sr.color;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            c.a = Mathf.Lerp(1f, 0f, t);
            sr.color = c;
            yield return null;
        }

        c.a = 0f;
        sr.color = c;
    }

    void SetSmellActive(bool a, bool b, bool c)
    {
        if (smellSprite1) smellSprite1.gameObject.SetActive(a);
        if (smellSprite2) smellSprite2.gameObject.SetActive(b);
        if (smellSprite3) smellSprite3.gameObject.SetActive(c);
    }

    void ResetSpriteAlpha(SpriteRenderer sr)
    {
        if (sr == null) return;

        Color c = sr.color;
        c.a = 1f;
        sr.color = c;
    }

    // =====================================================
    // UTILITIES
    // =====================================================

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