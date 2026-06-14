using System.Collections;
using UnityEngine;

public class SceneIntroTransition : MonoBehaviour
{
    [Header("Sprites")]
    public GameObject mainArt;
    public GameObject topLeftPanel;
    public GameObject topRightPanel;
    public GameObject bottomLeftPanel;
    public GameObject bottomRightPanel;

    [Header("Canvases To Show After Transition")]
    public GameObject dialogueCanvas;
    public GameObject heartCanvas;

    [Header("Timing")]
    public float panelDelay = 1f;
    public float mainArtVisibleTime = 3f;
    public float fadeDuration = 2f;

    private SpriteRenderer mainArtRenderer;

    private void Start()
    {
        // Hide gameplay/UI canvases initially
        if (dialogueCanvas != null)
            dialogueCanvas.SetActive(false);

        if (heartCanvas != null)
            heartCanvas.SetActive(false);

        mainArtRenderer = mainArt.GetComponent<SpriteRenderer>();

        if (mainArtRenderer == null)
        {
            Debug.LogError("Main Art is missing a SpriteRenderer!");
            return;
        }

        StartCoroutine(PlayTransition());
    }

    private IEnumerator PlayTransition()
    {
        // Ensure everything starts visible
        mainArt.SetActive(true);
        topLeftPanel.SetActive(true);
        topRightPanel.SetActive(true);
        bottomLeftPanel.SetActive(true);
        bottomRightPanel.SetActive(true);

        // Reveal artwork one panel at a time
        yield return new WaitForSeconds(panelDelay);
        topLeftPanel.SetActive(false);

        yield return new WaitForSeconds(panelDelay);
        topRightPanel.SetActive(false);

        yield return new WaitForSeconds(panelDelay);
        bottomLeftPanel.SetActive(false);

        yield return new WaitForSeconds(panelDelay);
        bottomRightPanel.SetActive(false);

        // Keep the artwork fully visible for a while
        yield return new WaitForSeconds(mainArtVisibleTime);

        // Fade out the artwork
        Color startColor = mainArtRenderer.color;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            Color c = startColor;
            c.a = Mathf.Lerp(startColor.a, 0f, elapsed / fadeDuration);

            mainArtRenderer.color = c;

            yield return null;
        }

        // Ensure fully transparent
        Color finalColor = mainArtRenderer.color;
        finalColor.a = 0f;
        mainArtRenderer.color = finalColor;

        mainArt.SetActive(false);

        // Show the UI after the transition is complete
        if (dialogueCanvas != null)
            dialogueCanvas.SetActive(true);

        if (heartCanvas != null)
            heartCanvas.SetActive(true);
    }
}