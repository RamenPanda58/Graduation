using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ButtonManager : MonoBehaviour
{
    [Header("General UI")]
    [SerializeField] private GameObject buttonObject;

    [Header("End Day UI")]
    [SerializeField] private GameObject endDayButtonObject;

    [Header("Tutorial UI")]
    [SerializeField] private GameObject tutorialUI;
    [SerializeField] private GameObject tutorialButtonObject;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private CanvasGroup tutorialButtonCanvasGroup;

    [Header("Scene Transition (OLD)")]
    [SerializeField] private GameObject mainArt;
    [SerializeField] private GameObject topLeftPanel;
    [SerializeField] private GameObject topRightPanel;
    [SerializeField] private GameObject bottomLeftPanel;
    [SerializeField] private GameObject bottomRightPanel;

    [SerializeField] private float timeWaitAnimation = 3f;

    private static bool tutorialButtonShown = false;

    [Header("Main Scene Voice Over (NEW)")]
    [SerializeField] private GameObject introBackground;
    [SerializeField] private GameObject introTextObject;
    [SerializeField] private AudioSource narrationAudio;

    private float introDuration =>
        narrationAudio != null && narrationAudio.clip != null
        ? narrationAudio.clip.length
        : 27f;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        // OLD system cleanup
        if (mainArt != null)
            mainArt.SetActive(false);

        if (tutorialUI != null)
            tutorialUI.SetActive(false);

        if (exitButton != null)
            exitButton.SetActive(false);

        if (tutorialButtonObject != null)
            tutorialButtonObject.SetActive(false);

        if (endDayButtonObject != null)
            endDayButtonObject.SetActive(false);

        // NEW system cleanup
        if (introBackground != null)
            introBackground.SetActive(false);

        if (introTextObject != null)
            introTextObject.SetActive(false);

        if (!tutorialButtonShown)
        {
            StartCoroutine(ShowTutorialButtonAfterDelay());
        }

        CheckEndDay();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckEndDay();
    }

    private void CheckEndDay()
    {
        if (endDayButtonObject == null) return;
        if (CharacterChecker.Instance == null) return;

        if (CharacterChecker.Instance.AllCharactersHelped())
            endDayButtonObject.SetActive(true);
        else
            endDayButtonObject.SetActive(false);
    }

    // =========================
    // TUTORIAL BUTTON
    // =========================

    private IEnumerator ShowTutorialButtonAfterDelay()
    {
        yield return new WaitForSeconds(2f);

        if (tutorialButtonObject != null)
            tutorialButtonObject.SetActive(true);

        if (tutorialButtonCanvasGroup != null)
            tutorialButtonCanvasGroup.alpha = 0f;

        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            if (tutorialButtonCanvasGroup != null)
                tutorialButtonCanvasGroup.alpha =
                    Mathf.Lerp(0f, 1f, elapsed / duration);

            yield return null;
        }

        if (tutorialButtonCanvasGroup != null)
            tutorialButtonCanvasGroup.alpha = 1f;

        tutorialButtonShown = true;
    }

    // =========================
    // OLD SCENE TRANSITION
    // =========================

    public void MainSceneAnimation()
    {
        StartCoroutine(TransitionAndLoad());
    }

    private IEnumerator TransitionAndLoad()
    {
        if (mainArt != null)
            mainArt.SetActive(true);

        if (topLeftPanel != null) topLeftPanel.SetActive(true);
        if (topRightPanel != null) topRightPanel.SetActive(true);
        if (bottomLeftPanel != null) bottomLeftPanel.SetActive(true);
        if (bottomRightPanel != null) bottomRightPanel.SetActive(true);

        yield return new WaitForSeconds(1f);
        if (topLeftPanel != null)
            topLeftPanel.SetActive(false);

        yield return new WaitForSeconds(1f);
        if (topRightPanel != null)
            topRightPanel.SetActive(false);

        yield return new WaitForSeconds(1f);
        if (bottomLeftPanel != null)
            bottomLeftPanel.SetActive(false);

        yield return new WaitForSeconds(1f);
        if (bottomRightPanel != null)
            bottomRightPanel.SetActive(false);

        yield return new WaitForSeconds(timeWaitAnimation);

        SceneManager.LoadScene("TeahouseView");
    }

    // =========================
    // NEW VOICE OVER INTRO
    // =========================

    public void MainSceneVoiceOver()
    {
        StartCoroutine(MainSceneVoiceOverRoutine());
    }

    private IEnumerator MainSceneVoiceOverRoutine()
    {
        if (introBackground != null)
            introBackground.SetActive(true);

        if (introTextObject != null)
            introTextObject.SetActive(true);

        if (narrationAudio != null)
            narrationAudio.Play();

        yield return new WaitForSeconds(introDuration);

        SceneManager.LoadScene("TeahouseView");
    }

    // =========================
    // END DAY RESULT
    // =========================

    public void GoToResult()
    {
        if (CharacterChecker.Instance == null) return;

        int score = CharacterChecker.Instance.GetScore();
        SceneManager.LoadScene("Result_" + score);
    }

    // =========================
    // UI
    // =========================

    public void OpenTutorial()
    {
        if (tutorialUI != null)
            tutorialUI.SetActive(true);

        if (exitButton != null)
            exitButton.SetActive(true);
    }

    public void CloseTutorial()
    {
        if (tutorialUI != null)
            tutorialUI.SetActive(false);

        if (exitButton != null)
            exitButton.SetActive(false);
    }

    public void ShowButtonHome()
    {
        if (buttonObject != null)
            buttonObject.SetActive(true);
    }

    // =========================
    // GAME
    // =========================

    public void QuitGame() => Application.Quit();

    public void RestartGame() => SceneManager.LoadScene("StartScene");

    // =========================
    // CHARACTER SCENES
    // =========================

    public void TwinInspect() => SceneManager.LoadScene("Twin_inspect");
    public void SailorInspect() => SceneManager.LoadScene("Sailor_inspect");
    public void AnxLadyInspect() => SceneManager.LoadScene("AnxLady_inspect");
    public void FarmerInspect() => SceneManager.LoadScene("Farmer_inspect");

    public void AnxLadyAction() => SceneManager.LoadScene("AnxLady_action");
    public void FarmerAction() => SceneManager.LoadScene("Farmer_action");
    public void SailorAction() => SceneManager.LoadScene("Sailor_action");
    public void TwinAction() => SceneManager.LoadScene("Twin_action");

    public void ActionSceneButton() => SceneManager.LoadScene("ActionScene");
    public void RightAnswer() => SceneManager.LoadScene("ReactionScene");
    public void CloseForTheDay() => SceneManager.LoadScene("ClosedScene");
    public void Home() => SceneManager.LoadScene("TeahouseView");
}