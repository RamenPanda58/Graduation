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

    private static bool tutorialButtonShown = false;

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
        // Safety defaults
        if (tutorialUI != null)
            tutorialUI.SetActive(false);

        if (exitButton != null)
            exitButton.SetActive(false);

        if (tutorialButtonObject != null)
            tutorialButtonObject.SetActive(false);

        if (endDayButtonObject != null)
            endDayButtonObject.SetActive(false);

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
        {
            Debug.Log("ALL CHARACTERS HELPED → SHOW END DAY BUTTON");
            endDayButtonObject.SetActive(true);
        }
        else
        {
            Debug.Log("NOT READY FOR END DAY");
            endDayButtonObject.SetActive(false);
        }
    }

    private IEnumerator ShowTutorialButtonAfterDelay()
    {
        yield return new WaitForSeconds(2f);

        tutorialButtonObject.SetActive(true);
        tutorialButtonCanvasGroup.alpha = 0f;

        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            tutorialButtonCanvasGroup.alpha =
                Mathf.Lerp(0f, 1f, elapsed / duration);

            yield return null;
        }

        tutorialButtonCanvasGroup.alpha = 1f;
        tutorialButtonShown = true;
    }

    // =========================
    // Tutorial System
    // =========================

    public void OpenTutorial()
    {
        tutorialUI.SetActive(true);
        exitButton.SetActive(true);
    }

    public void CloseTutorial()
    {
        tutorialUI.SetActive(false);
        exitButton.SetActive(false);
    }

    // =========================
    // Global UI Helper
    // =========================

    public void ShowButtonHome()
    {
        buttonObject.SetActive(true);
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
    // Scene Management
    // =========================

    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No next scene in build settings.");
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quit button clicked!");
        Application.Quit();
    }

    public void RestartGame()
    {
        Debug.Log("Restart button clicked!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

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