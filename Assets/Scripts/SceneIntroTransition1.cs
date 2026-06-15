using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneIntroTransition1 : MonoBehaviour
{
    [Header("Transition Objects")]
    public GameObject mainArt;
    public GameObject topLeftPanel;
    public GameObject topRightPanel;
    public GameObject bottomLeftPanel;
    public GameObject bottomRightPanel;

    [Header("Scene")]
    public string sceneToLoad;

    [Header("Timing")]
    public float panelDelay = 1f;
    public float mainArtVisibleTime = 3f;
    public float fadeDuration = 2f;

    private SpriteRenderer mainArtRenderer;

    private void Awake()
    {
        mainArtRenderer = mainArt.GetComponent<SpriteRenderer>();
    }

    public void StartTransition()
    {
        StartCoroutine(PlayTransition());
    }

    private IEnumerator PlayTransition()
    {
        // Show transition objects
        mainArt.SetActive(true);
        topLeftPanel.SetActive(true);
        topRightPanel.SetActive(true);
        bottomLeftPanel.SetActive(true);
        bottomRightPanel.SetActive(true);

        yield return new WaitForSeconds(panelDelay);
        topLeftPanel.SetActive(false);

        yield return new WaitForSeconds(panelDelay);
        topRightPanel.SetActive(false);

        yield return new WaitForSeconds(panelDelay);
        bottomLeftPanel.SetActive(false);

        yield return new WaitForSeconds(panelDelay);
        bottomRightPanel.SetActive(false);

        yield return new WaitForSeconds(mainArtVisibleTime);

        // Fade artwork
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

        SceneManager.LoadScene(sceneToLoad);
    }
}