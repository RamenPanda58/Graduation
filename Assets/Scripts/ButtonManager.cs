using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ButtonManager : MonoBehaviour
{
    [Header("General UI")]
    [SerializeField] private GameObject buttonObject;

    [Header("Tutorial UI")]
    [SerializeField] private GameObject tutorialUI;
    [SerializeField] private GameObject tutorialButtonObject;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private CanvasGroup tutorialButtonCanvasGroup;

    private static bool tutorialButtonShown = false;

    private void Start()
    {
        // Safety defaults
        if (tutorialUI != null)
            tutorialUI.SetActive(false);

        if (exitButton != null)
            exitButton.SetActive(false);

        if (tutorialButtonObject != null)
            tutorialButtonObject.SetActive(false);

        // Show tutorial button only once per gameplay session
        if (!tutorialButtonShown)
        {
            StartCoroutine(ShowTutorialButtonAfterDelay());
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
    // Global UI Helper (IMPORTANT)
    // =========================

    public void ShowButtonHome()
    {
        buttonObject.SetActive(true);
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

    public void InspectTwin()
    {
        int targetSceneIndex = 2;

        if (targetSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(targetSceneIndex);
        }
        else
        {
            Debug.LogError("Scene index 2 not found in build settings.");
        }
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
}