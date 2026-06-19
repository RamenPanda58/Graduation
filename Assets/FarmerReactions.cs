using UnityEngine;
using System.Collections;

public class FarmerReactions : MonoBehaviour
{
    [Header("Animator")]
    public Animator farmerAnimator;

    [Header("Animation Clips")]
    public AnimationClip idleClip;
    public AnimationClip farmerSmellReactionClip;
    public AnimationClip farmerShamisenReactionClip;

    [Header("Animation Settings")]
    public float animationBlendTime = 0.25f;

    // =====================================================
    // SMELL EVENT
    // =====================================================

    [Header("Smell Event")]
    public float smellInterval = 12f;

    [Header("Smell Sprites")]
    public SpriteRenderer smellSprite1;
    public SpriteRenderer smellSprite2;
    public SpriteRenderer smellSprite3;

    public float smellFrameDelay = 0.2f;
    public float smellFadeOutTime = 1.5f;

    // =====================================================
    // GREEN TINT EFFECT
    // =====================================================

    [Header("Smell Tint Effect")]
    public GameObject farmerTintRoot;

    public Color smellTintColor = new Color(0.6f, 1f, 0.6f, 1f);
    public float smellTintFadeTime = 0.5f;

    private SpriteRenderer[] farmerTintSprites;
    private Color[] farmerOriginalColors;

    // =====================================================
    // SHAMISEN REACTION
    // =====================================================

    [Header("Shamisen")]
    public AudioSource shamisenAudioSource;

    // =====================================================
    // INTERNAL
    // =====================================================

    public bool isSmellReacting = false;
    public bool isShamisenReacting = false;
   
[Header("Shamisen Timing")]
public float shamisenInterval = 20f;
    void Start()
    {
        
        // Auto-find all farmer sprites
        if (farmerTintRoot != null)
        {
            farmerTintSprites =
                farmerTintRoot.GetComponentsInChildren<SpriteRenderer>(true);

            // Exclude smell sprites from tinting
            farmerTintSprites = System.Array.FindAll(
                farmerTintSprites,
                sr =>
                    sr != smellSprite1 &&
                    sr != smellSprite2 &&
                    sr != smellSprite3
            );

            farmerOriginalColors =
                new Color[farmerTintSprites.Length];

            for (int i = 0; i < farmerTintSprites.Length; i++)
            {
                farmerOriginalColors[i] =
                    farmerTintSprites[i].color;
            }
        }

        SetSmellSprites(false, false, false);

        StartCoroutine(SmellLoop());
        StartCoroutine(ShamisenLoop());
    }

    

    // =====================================================
    // SMELL LOOP
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
        if (isSmellReacting)
            yield break;

        isSmellReacting = true;

        if (farmerAnimator != null && farmerSmellReactionClip != null)
        {
            farmerAnimator.CrossFade(farmerSmellReactionClip.name, animationBlendTime);
        }

        StartCoroutine(PlaySmellTintEffect());
        yield return StartCoroutine(SmellSpritesRoutine());

        var ctrl = farmerAnimator.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;
        #if UNITY_EDITOR
        foreach (var l in ctrl.layers)
        {
            foreach (var s in l.stateMachine.states)
                Debug.Log($"REAL STATE (top): '{s.state.name}'");

            foreach (var sub in l.stateMachine.stateMachines)
                foreach (var s in sub.stateMachine.states)
                    Debug.Log($"REAL STATE (sub '{sub.stateMachine.name}'): '{s.state.name}'");
        }
        #endif
        Debug.Log($"idleClip.name = '{idleClip.name}'");

        if (farmerAnimator != null && idleClip != null)
        {
            Debug.Log(farmerAnimator.HasState(0, Animator.StringToHash("Idle_Farmer")));
            farmerAnimator.CrossFade(idleClip.name, animationBlendTime);
        }

        isSmellReacting = false;
    }

    IEnumerator SmellSpritesRoutine()
    {
        SetSmellSprites(false, false, false);

        // Sprite 1
        if (smellSprite1 != null)
        {
            SetSmellSprites(true, false, false);
            yield return new WaitForSeconds(smellFrameDelay);
        }

        // Sprite 2
        if (smellSprite2 != null)
        {
            SetSmellSprites(false, true, false);
            yield return new WaitForSeconds(smellFrameDelay);
        }

        // Sprite 3
        if (smellSprite3 != null)
        {
            SetSmellSprites(false, false, true);

            Color c = smellSprite3.color;
            float t = 0f;

            while (t < smellFadeOutTime)
            {
                t += Time.deltaTime;

                float alpha = Mathf.Lerp(
                    1f,
                    0f,
                    t / smellFadeOutTime
                );

                smellSprite3.color =
                    new Color(c.r, c.g, c.b, alpha);

                yield return null;
            }

            smellSprite3.gameObject.SetActive(false);

            smellSprite3.color =
                new Color(c.r, c.g, c.b, 1f);
        }

        SetSmellSprites(false, false, false);
    }

    // =====================================================
    // GREEN TINT EFFECT
    // =====================================================

    IEnumerator PlaySmellTintEffect()
    {
        if (farmerTintSprites == null ||
            farmerTintSprites.Length == 0)
        {
            yield break;
        }

        float t = 0f;

        // Fade to green
        while (t < smellTintFadeTime)
        {
            t += Time.deltaTime;

            float lerp = t / smellTintFadeTime;

            for (int i = 0; i < farmerTintSprites.Length; i++)
            {
                Color original =
                    farmerOriginalColors[i];

                Color result = Color.Lerp(
                    original,
                    smellTintColor,
                    lerp
                );

                result.a = original.a;

                farmerTintSprites[i].color =
                    result;
            }

            yield return null;
        }

        yield return new WaitForSeconds(0.3f);

        t = 0f;

        // Fade back
        while (t < smellTintFadeTime)
        {
            t += Time.deltaTime;

            float lerp = t / smellTintFadeTime;

            for (int i = 0; i < farmerTintSprites.Length; i++)
            {
                Color original =
                    farmerOriginalColors[i];

                Color result = Color.Lerp(
                    smellTintColor,
                    original,
                    lerp
                );

                result.a = original.a;

                farmerTintSprites[i].color =
                    result;
            }

            yield return null;
        }

        // Hard reset
        for (int i = 0; i < farmerTintSprites.Length; i++)
        {
            farmerTintSprites[i].color =
                farmerOriginalColors[i];
        }
    }

    // =====================================================
    // SHAMISEN REACTION
    // =====================================================

 IEnumerator ShamisenRoutine()
{
    if (isSmellReacting || isShamisenReacting)
        yield break;

    isShamisenReacting = true;

    if (farmerAnimator != null && farmerShamisenReactionClip != null)
    {
        farmerAnimator.CrossFade(farmerShamisenReactionClip.name, animationBlendTime);

        if (shamisenAudioSource != null)
        {
            shamisenAudioSource.Play();
        }

        yield return new WaitForSeconds(farmerShamisenReactionClip.length);
    }

    if (farmerAnimator != null && idleClip != null)
    {
        farmerAnimator.CrossFade(idleClip.name, animationBlendTime);
    }

    isShamisenReacting = false;
}

    IEnumerator ShamisenLoop()
{
    while (true)
    {
        yield return new WaitForSeconds(shamisenInterval);
        while (isSmellReacting)
    yield return null;

yield return StartCoroutine(ShamisenRoutine());
    }
}

    // =====================================================
    // HELPERS
    // =====================================================

    void SetSmellSprites(bool a, bool b, bool c)
    {
        if (smellSprite1)
            smellSprite1.gameObject.SetActive(a);

        if (smellSprite2)
            smellSprite2.gameObject.SetActive(b);

        if (smellSprite3)
            smellSprite3.gameObject.SetActive(c);
    }


    

}