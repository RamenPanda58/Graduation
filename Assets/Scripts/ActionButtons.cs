using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum ResultType
{
    Completed,
    Nearly,
    Failed
}

public class ActionButtons : MonoBehaviour
{
    [Header("Character Result")]
    public string characterID;
    public ResultType result;

    [Header("Transition UI")]
    public GameObject mainArt;
    public GameObject topLeftPanel;
    public GameObject topRightPanel;
    public GameObject bottomLeftPanel;
    public GameObject bottomRightPanel;

    [Header("Timing")]
    public float panelDelay = 1f;
    public float mainArtVisibleTime = 3f;

    private bool hasBeenPressed = false;

    public void Submit()
    {
        if (hasBeenPressed)
            return;

        hasBeenPressed = true;

        // Disable button to prevent double-clicks
        Button button = GetComponent<Button>();
        if (button != null)
            button.interactable = false;

        if (CharacterChecker.Instance == null)
        {
            Debug.LogError("CharacterChecker missing!");
            return;
        }

        string resultString = result.ToString().ToLower();

        CharacterChecker.Instance.SetCharacterResult(characterID, resultString);
        CharacterChecker.Instance.MarkHelped(characterID);

        string sceneName = characterID + "_" + resultString + "_reaction";

        Debug.Log("Loading after transition: " + sceneName);

        StartCoroutine(PlayTransitionAndLoad(sceneName));
    }

    private IEnumerator PlayTransitionAndLoad(string sceneName)
    {
        // Show transition elements
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

        // Keep the artwork visible
        yield return new WaitForSeconds(mainArtVisibleTime);

        // Load the next scene immediately
        SceneManager.LoadScene(sceneName);
    }
}