using System.Collections;
using UnityEngine;

public class TwinReactionManager : MonoBehaviour
{
    public float animationBlendTime = 0.25f;

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

    void Start()
    {
        if (twinAnimator == null)
        {
            Debug.LogError("Twin Animator is not assigned.");
            return;
        }

        currentTwinIdle = twinIdleClip1;

        if (currentTwinIdle != null)
        {
            twinAnimator.Play(currentTwinIdle.name);
        }

        StartCoroutine(DoorChimeLoop());
        StartCoroutine(TwinIdleSwitchLoop());
        StartCoroutine(TwinSighLoop());
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

            // THIS WAS MISSING
            yield return new WaitForSeconds(chimeInterval);
        }
    }

    IEnumerator DelayedTwinReaction()
    {
        yield return new WaitForSeconds(twinReactionDelay);
        TriggerTwinReaction();
    }

    IEnumerator TwinIdleSwitchLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(
                Random.Range(idleSwitchMinTime, idleSwitchMaxTime)
            );

            if (isTwinReacting || isTwinSighing)
                continue;

            if (twinIdleClip2 == null)
                continue;

            currentTwinIdle = twinIdleClip2;
            twinAnimator.CrossFade(currentTwinIdle.name, animationBlendTime);

            yield return new WaitForSeconds(
                twinIdleClip2.length * idle2LoopCount
            );

            if (twinIdleClip1 != null)
            {
                currentTwinIdle = twinIdleClip1;
                twinAnimator.CrossFade(currentTwinIdle.name, animationBlendTime);
            }
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
        if (twinReactionClip == null)
            yield break;

        isTwinReacting = true;

        twinAnimator.CrossFade(twinReactionClip.name, animationBlendTime);

        yield return new WaitForSeconds(twinReactionClip.length);

        if (twinSighClip != null)
        {
            twinAnimator.CrossFade(twinSighClip.name, animationBlendTime);

            yield return new WaitForSeconds(twinSighClip.length);
        }

        if (currentTwinIdle != null)
        {
            twinAnimator.CrossFade(currentTwinIdle.name, animationBlendTime);
        }

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
        if (twinSighClip == null)
            yield break;

        isTwinSighing = true;

        twinAnimator.CrossFade(twinSighClip.name, animationBlendTime);

        yield return new WaitForSeconds(twinSighClip.length);

        if (currentTwinIdle != null)
        {
            twinAnimator.CrossFade(currentTwinIdle.name, animationBlendTime);
        }

        isTwinSighing = false;
    }
}