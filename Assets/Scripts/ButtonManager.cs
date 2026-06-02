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
        // Hide tutorial UI at start
        if (tutorialUI != null)
            tutorialUI.SetActive(false);

        // Hide exit button at start
        if (exitButton != null)
            exitButton.SetActive(false);

        // Hide tutorial button initially
        if (tutorialButtonObject != null)
            tutorialButtonObject.SetActive(false);

        // Hide end day button initially
        if (endDayButtonObject != null)
            endDayButtonObject.SetActive(false);

        // Show tutorial button only once
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
            Debug.Log("ALL CHARACTERS HELPED → SHOW END DAY");
            endDayButtonObject.SetActive(true);
        }
        else
        {
            endDayButtonObject.SetActive(false);
        }
    }

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
            {
                tutorialButtonCanvasGroup.alpha =
                    Mathf.Lerp(0f, 1f, elapsed / duration);
            }

            yield return null;
        }

        if (tutorialButtonCanvasGroup != null)
            tutorialButtonCanvasGroup.alpha = 1f;

        tutorialButtonShown = true;
    }

    // =========================
    // END DAY RESULT
    // =========================
    public void GoToResult()
    {
        if (CharacterChecker.Instance == null) return;

        int score = CharacterChecker.Instance.GetScore();

        Debug.Log("FINAL SCORE: " + score);

        SceneManager.LoadScene("Result_" + score);
    }

    // =========================
    // UI
    // =========================
    public void OpenTutorial()
    {
        if (tutorialUI != null)
            tutorialUI.SetActive(true);

        // Show exit button
        if (exitButton != null)
            exitButton.SetActive(true);
    }

    public void CloseTutorial()
    {
        if (tutorialUI != null)
            tutorialUI.SetActive(false);

        // Hide exit button
        if (exitButton != null)
            exitButton.SetActive(false);
    }

    public void ShowButtonHome()
    {
        if (buttonObject != null)
            buttonObject.SetActive(true);
    }

    // =========================
    // SCENES
    // =========================
    public void LoadNextScene()
    {
        int next = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(next);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void TwinInspect()
    {
        SceneManager.LoadScene("Twin_inspect");
    }

    public void SailorInspect()
    {
        SceneManager.LoadScene("Sailor_inspect");
    }

    public void AnxLadyInspect()
    {
        SceneManager.LoadScene("AnxLady_inspect");
    }

    public void FarmerInspect()
    {
        SceneManager.LoadScene("Farmer_inspect");
    }

    public void AnxLadyAction()
    {
        SceneManager.LoadScene("AnxLady_action");
    }

    public void FarmerAction()
    {
        SceneManager.LoadScene("Farmer_action");
    }

    public void SailorAction()
    {
        SceneManager.LoadScene("Sailor_action");
    }

    public void TwinAction()
    {
        SceneManager.LoadScene("Twin_action");
    }

    public void ActionSceneButton()
    {
        SceneManager.LoadScene("ActionScene");
    }

    public void RightAnswer()
    {
        SceneManager.LoadScene("ReactionScene");
    }

    public void CloseForTheDay()
    {
        SceneManager.LoadScene("ClosedScene");
    }

    public void Home()
    {
        SceneManager.LoadScene("TeahouseView");
    }
}