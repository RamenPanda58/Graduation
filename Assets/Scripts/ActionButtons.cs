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
            Debug.LogError("CharacterChecker missing in scene!");
            return;
        }

        string resultString = result.ToString().ToLower();

        // store result
        CharacterChecker.Instance.SetCharacterResult(characterID, resultString);

        // mark as helped immediately
        CharacterChecker.Instance.MarkHelped(characterID);

        // reaction scene
        string sceneName = characterID + "_" + resultString + "_reaction";

        Debug.Log("Loading scene: " + sceneName);

        SceneManager.LoadScene(sceneName);
    }
}