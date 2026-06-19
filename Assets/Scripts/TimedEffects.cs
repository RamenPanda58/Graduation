using UnityEngine;
using System.Collections;

public class TimedEffects : MonoBehaviour
{
    // ---------------- GHOST EVENT ----------------

    [Header("Animation Blending")]
public float animationBlendTime = 0.25f;

[Header("Smell Color Tint (Food Effect)")]
public GameObject farmerTintRoot; // assign farmer root OR character root

public Color smellTintColor = new Color(0.8f, 1f, 0.8f, 1f); // light green

public float smellTintFadeTime = 0.5f;

private SpriteRenderer[] farmerTintSprites;
private Color[] farmerOriginalColors;

[Header("Smell Event (Food Reaction)")]
public float smellInterval = 12f;

public SpriteRenderer smellSprite1;
public SpriteRenderer smellSprite2;
public SpriteRenderer smellSprite3;

public float smellFrameDelay = 0.2f;
public float smellFadeOutTime = 1.5f;

private bool isSmellPlaying = false;


[Header("Smell Reaction (Sailor)")]
public AnimationClip sailorSmellReactionClip;

[Header("Smell Reaction (Farmer)")]
public AnimationClip farmerSmellReactionClip;

    [Header("Ghost Event")]
    public float ghostEventInterval = 30f;
    public float reactionBeforeGhostEnds = 2f;

    [Header("Ghost Animation")]
    public Animator ghostAnimator;
    public AnimationClip ghostFloatClip;

    [Header("Reaction Character")]
    public Animator ghostReactionAnimator;
    public AnimationClip ghostReactionClip;
    public AnimationClip ghostReactionIdleClip;

    [Header("Ghost Reaction Timing")]
    public float ghostReactionDelay = 0.5f;

    private bool isGhostEventPlaying = false;

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
    public AnimationClip twinIdleClip1;
    public AnimationClip twinIdleClip2;
    public int idle2LoopCount = 2;


[Header("Twin Idle Switching")]
public float idleSwitchMinTime = 10f;
public float idleSwitchMaxTime = 30f;

private AnimationClip currentTwinIdle;

    [Header("Twin Ambient Sigh")]
public AnimationClip twinSighClip;

public float sighMinInterval = 15f;
public float sighMaxInterval = 40f;

private bool isTwinReacting = false;
private bool isTwinSighing = false;

    [Header("Shamisen Event")]
    public AudioSource shamisenAudio;
    public float shamisenInterval = 45f;

    [Header("Farmer Reaction (Shamisen)")]
    public Animator farmerAnimator;
    public AnimationClip farmerReactionClip;
    public AnimationClip farmerIdleClip;

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

    private bool isFarmerReacting = false;

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

    currentTwinIdle = twinIdleClip1;

    if (twinAnimator != null && currentTwinIdle != null)
    {
        twinAnimator.Play(currentTwinIdle.name);
    }

    // ---------------- START ALL LOOPS ----------------
    StartCoroutine(EffectLoop());
    StartCoroutine(DoorChimeLoop());
    StartCoroutine(ShamisenLoop());
    StartCoroutine(GhostEventLoop());
    StartCoroutine(TwinIdleSwitchLoop());
    StartCoroutine(SmellEventLoop());   // ✅ FIXED HERE

    if (ghostAnimator != null)
    {
        ghostAnimator.gameObject.SetActive(false);
    }

    if (farmerTintRoot != null)
{
    farmerTintSprites = farmerTintRoot.GetComponentsInChildren<SpriteRenderer>();
    farmerOriginalColors = new Color[farmerTintSprites.Length];

    for (int i = 0; i < farmerTintSprites.Length; i++)
    {
        farmerOriginalColors[i] = farmerTintSprites[i].color;
    }
}
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

    // ---------------- SHAMISEN SYSTEM ----------------

    IEnumerator ShamisenLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(shamisenInterval);

            if (shamisenAudio != null && shamisenAudio.clip != null)
            {
                shamisenAudio.PlayOneShot(shamisenAudio.clip);
            }

            StartCoroutine(PlayFarmerReaction());
        }
    }

    IEnumerator PlayFarmerReaction()
    {
        if (isFarmerReacting) yield break;

        if (farmerAnimator == null) yield break;
        if (farmerReactionClip == null) yield break;
        if (farmerIdleClip == null) yield break;

        isFarmerReacting = true;

        farmerAnimator.Play(farmerReactionClip.name, 0, 0f);

        yield return new WaitForSeconds(farmerReactionClip.length);

        farmerAnimator.Play(farmerIdleClip.name, 0, 0f);

        isFarmerReacting = false;
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
        sailorAnimator.CrossFade(jumpScareClip.name, animationBlendTime);
        yield return new WaitForSeconds(jumpScareClip.length);
        sailorAnimator.CrossFade(idleClip.name, animationBlendTime);
    }

    // ---------------- TWIN REACTION ----------------
IEnumerator TwinIdleSwitchLoop()
{
    while (true)
    {
        yield return new WaitForSeconds(
            Random.Range(idleSwitchMinTime, idleSwitchMaxTime)
        );

        if (isTwinReacting || isTwinSighing)
            continue;

        // Play Idle 2
        currentTwinIdle = twinIdleClip2;
        twinAnimator.CrossFade(currentTwinIdle.name, animationBlendTime);

        // Wait for Idle 2 to loop twice
        yield return new WaitForSeconds(
            twinIdleClip2.length * idle2LoopCount
        );

        // Return to Idle 1
        currentTwinIdle = twinIdleClip1;
        twinAnimator.CrossFade(currentTwinIdle.name, animationBlendTime);
    }
}

   void TriggerTwinReaction()
{


    if (isTwinReacting || isTwinSighing)
        return;

    StartCoroutine(PlayTwinReactionThenIdle());
}

IEnumerator PlayTwinReactionThenIdle()
{
    isTwinReacting = true;

    // Play reaction
    twinAnimator.CrossFade(twinReactionClip.name, animationBlendTime);

    yield return new WaitForSeconds(twinReactionClip.length);

    // Play sigh immediately after
    if (twinSighClip != null)
    {
        twinAnimator.CrossFade(twinSighClip.name, animationBlendTime);

        yield return new WaitForSeconds(twinSighClip.length);
    }

    // Return to current idle
    twinAnimator.CrossFade(currentTwinIdle.name, animationBlendTime);

    isTwinReacting = false;
}

IEnumerator TwinSighLoop()
{
    while (true)
    {
        float waitTime = Random.Range(sighMinInterval, sighMaxInterval);

        yield return new WaitForSeconds(waitTime);

        if (isTwinReacting || isTwinSighing)
            continue;

        StartCoroutine(PlayTwinSighThenIdle());
    }
}

IEnumerator PlayTwinSighThenIdle()
{
    

    isTwinSighing = true;

    twinAnimator.CrossFade(twinSighClip.name, animationBlendTime);

    yield return new WaitForSeconds(twinSighClip.length);

    twinAnimator.CrossFade(currentTwinIdle.name, animationBlendTime);

    isTwinSighing = false;
}

    // ---------------- GHOST EVENT ----------------

    IEnumerator GhostEventLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(ghostEventInterval);
            StartCoroutine(PlayGhostEvent());
        }
    }

    IEnumerator PlayGhostEvent()
{
    if (isGhostEventPlaying)
        yield break;

    if (ghostAnimator == null || ghostFloatClip == null)
        yield break;

    isGhostEventPlaying = true;

    // Store start position once (optional safety check)
    Vector3 startPos = ghostAnimator.transform.position;

    // Enable ghost
    ghostAnimator.gameObject.SetActive(true);

    // Reset position so animation always starts correctly
    ghostAnimator.transform.position = startPos;

    // Play ghost animation
    ghostAnimator.Play(ghostFloatClip.name, 0, 0f);

    // Reaction timing (starts slightly before ghost finishes if you use your system)
    if (ghostReactionAnimator != null &&
        ghostReactionClip != null)
    {
        float reactionTime = Mathf.Max(
            0f,
            ghostFloatClip.length - reactionBeforeGhostEnds
        );

        StartCoroutine(PlayReactionAtTime(reactionTime));
    }

    // Wait for ghost animation to finish
    yield return new WaitForSeconds(ghostFloatClip.length);

    // Disable ghost again (fully hidden)
    ghostAnimator.gameObject.SetActive(false);

    isGhostEventPlaying = false;
}

  IEnumerator PlayReactionAtTime(float delay)
{
    yield return new WaitForSeconds(delay);

    if (ghostReactionAnimator == null || ghostReactionClip == null)
        yield break;

    ghostReactionAnimator.Play(ghostReactionClip.name, 0, 0f);

    yield return new WaitForSeconds(ghostReactionClip.length);

    if (ghostReactionIdleClip != null)
    {
        ghostReactionAnimator.Play(ghostReactionIdleClip.name, 0, 0f);
    }
}

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

    IEnumerator SmellEventLoop()
{
    yield return new WaitForSeconds(2f); // optional initial delay

    while (true)
    {
        yield return new WaitForSeconds(smellInterval);

        if (!isSmellPlaying)
        {
            StartCoroutine(PlaySmellEvent());
        }
    }
}

IEnumerator PlaySmellEvent()
{
    isSmellPlaying = true;

    if (sailorAnimator != null && sailorSmellReactionClip != null)
        sailorAnimator.CrossFade(sailorSmellReactionClip.name, animationBlendTime);

    if (farmerAnimator != null && farmerSmellReactionClip != null)
        farmerAnimator.CrossFade(farmerSmellReactionClip.name, animationBlendTime);

    StartCoroutine(PlaySmellTintEffect());

    smellSprite1.gameObject.SetActive(true);
    smellSprite2.gameObject.SetActive(false);
    smellSprite3.gameObject.SetActive(false);

    yield return new WaitForSeconds(smellFrameDelay);

    smellSprite1.gameObject.SetActive(false);
    smellSprite2.gameObject.SetActive(true);

    yield return new WaitForSeconds(smellFrameDelay);

    smellSprite2.gameObject.SetActive(false);
    smellSprite3.gameObject.SetActive(true);

    Color c = smellSprite3.color;
    float t = 0f;

    while (t < smellFadeOutTime)
    {
        t += Time.deltaTime;
        float alpha = Mathf.Lerp(1f, 0f, t / smellFadeOutTime);
        smellSprite3.color = new Color(c.r, c.g, c.b, alpha);
        yield return null;
    }

    smellSprite3.gameObject.SetActive(false);
    smellSprite3.color = new Color(c.r, c.g, c.b, 1f);

    // Return both characters to idle
    if (sailorAnimator != null && idleClip != null)
        sailorAnimator.CrossFade(idleClip.name, animationBlendTime);

    if (farmerAnimator != null && farmerIdleClip != null)
        farmerAnimator.CrossFade(farmerIdleClip.name, animationBlendTime);

    isSmellPlaying = false;
}

IEnumerator PlaySmellTintEffect()
{
    if (farmerTintSprites == null || farmerTintSprites.Length == 0)
        yield break;

    float t = 0f;

    while (t < smellTintFadeTime)
    {
        t += Time.deltaTime;

        float lerp = t / smellTintFadeTime;

        for (int i = 0; i < farmerTintSprites.Length; i++)
        {
            Color original = farmerOriginalColors[i];
            original.a = 1f;

            Color tinted = smellTintColor;
            tinted.a = 1f;

            Color result = Color.Lerp(original, tinted, lerp);
            result.a = 1f; // 🔥 FORCE VISIBILITY ALWAYS

            farmerTintSprites[i].color = result;
        }

        yield return null;
    }

    yield return new WaitForSeconds(0.3f);

    t = 0f;

    while (t < smellTintFadeTime)
    {
        t += Time.deltaTime;

        float lerp = t / smellTintFadeTime;

        for (int i = 0; i < farmerTintSprites.Length; i++)
        {
            Color original = farmerOriginalColors[i];
            original.a = 1f;

            Color tinted = smellTintColor;
            tinted.a = 1f;

            Color result = Color.Lerp(tinted, original, lerp);
            result.a = 1f; // 🔥 NEVER LET IT FADE OUT

            farmerTintSprites[i].color = result;
        }

        yield return null;
    }

    // FINAL HARD RESET
    for (int i = 0; i < farmerTintSprites.Length; i++)
    {
        Color c = farmerOriginalColors[i];
        c.a = 1f;
        farmerTintSprites[i].color = c;
    }
}
}