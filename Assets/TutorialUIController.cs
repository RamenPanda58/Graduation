using System.Collections;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    public CanvasGroup tutorialPanel;
    public CanvasGroup exitButton;

    public GameObject tutorialCanvas;

    public float fadeSpeed = 2f;
    public float delayBeforeExit = 1f;

    public static string TUTORIAL_KEY = "Tutorial_Shown_TeahouseView";

    void Start()
    {
        if (PlayerPrefs.GetInt(TUTORIAL_KEY, 0) != 1)
        {
            Debug.Log("not closed before");
            StartCoroutine(Run());
            return;
        }
        else
        {
            tutorialCanvas.SetActive(false);
        }
    }

    void Update()
    {

    }

    IEnumerator Run()
    {
        tutorialPanel.alpha = 0;
        exitButton.alpha = 0;

        tutorialPanel.interactable = false;
        exitButton.interactable = false;

        tutorialPanel.blocksRaycasts = false;
        exitButton.blocksRaycasts = false;

        yield return FadeIn(tutorialPanel);
        yield return new WaitForSeconds(delayBeforeExit);
        yield return FadeIn(exitButton);
    }

    IEnumerator FadeIn(CanvasGroup cg)
    {
        cg.interactable = true;
        cg.blocksRaycasts = true;

        while (cg.alpha < 1)
        {
            cg.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }

        cg.alpha = 1;
    }

    public void CloseTutorial()
    {
        // ✅ SAVE THAT IT HAS BEEN SHOWN
        PlayerPrefs.SetInt(TUTORIAL_KEY, 1);
        PlayerPrefs.Save();

        Debug.Log("Closed");

        if (tutorialCanvas != null)
            tutorialCanvas.SetActive(false);
    }
}