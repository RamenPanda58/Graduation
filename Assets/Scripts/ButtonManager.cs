using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ButtonManager : MonoBehaviour
{
    [Header("Tutorial UI")]
    [SerializeField] private GameObject tutorialUI;
    [SerializeField] private GameObject tutorialButtonObject;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private CanvasGroup tutorialButtonCanvasGroup;

    // Prevent tutorial button appearing every scene load
    private static bool tutorialButtonShown = false;

    private void Start()
    {
        // Hide tutorial UI and exit button at start
        tutorialUI.SetActive(false);
        exitButton.SetActive(false);

        // Hide tutorial button initially
        tutorialButtonObject.SetActive(false);

        // Only show tutorial button once during gameplay
        if (!tutorialButtonShown)
        {
            StartCoroutine(ShowTutorialButtonAfterDelay());
        }
    }

    private IEnumerator ShowTutorialButtonAfterDelay()
    {
        // Wait 2 seconds
        yield return new WaitForSeconds(2f);

        // Show tutorial button
        tutorialButtonObject.SetActive(true);

        // Fade in setup
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

        // Mark as shown so it won't appear again
        tutorialButtonShown = true;
    }

    // ==========================
    // Tutorial UI Functions
    // ==========================

    // Open tutorial UI
    public void OpenTutorial()
    {
        tutorialUI.SetActive(true);
        exitButton.SetActive(true);
    }

    // Close tutorial UI
    public void CloseTutorial()
    {
        tutorialUI.SetActive(false);
        exitButton.SetActive(false);
    }

    // ==========================
    // Scene Navigation
    // ==========================

    public void LoadNextScene()
    {
        int currentSceneIndex =
            SceneManager.GetActiveScene().buildIndex;

        int nextSceneIndex =
            currentSceneIndex + 1;

        if (nextSceneIndex <
            SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No next scene in the build settings.");
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

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    // Scene index 2
    public void InspectTwin()
    {
        int targetSceneIndex = 2;

        if (targetSceneIndex <
            SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(targetSceneIndex);
        }
        else
        {
            Debug.LogError(
                "Scene with index 2 not found in build settings."
            );
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

    // ==========================
    // Inspect Scenes
    // ==========================

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

    // ==========================
    // Action Scenes
    // ==========================

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