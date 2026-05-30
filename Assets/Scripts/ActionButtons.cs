using UnityEngine;
using UnityEngine.SceneManagement;

public enum ResultType
{
    Completed,
    Nearly,
    Failed
}

public class ActionButtons : MonoBehaviour
{
    public string characterID;
    public ResultType result;

    public void Submit()
    {
        if (CharacterChecker.Instance == null)
        {
            Debug.LogError("CharacterChecker missing!");
            return;
        }

        string resultString = result.ToString().ToLower();

        CharacterChecker.Instance.SetCharacterResult(characterID, resultString);
        CharacterChecker.Instance.MarkHelped(characterID);

        string sceneName = characterID + "_" + resultString + "_reaction";

        Debug.Log("Loading: " + sceneName);

        SceneManager.LoadScene(sceneName);
    }
}