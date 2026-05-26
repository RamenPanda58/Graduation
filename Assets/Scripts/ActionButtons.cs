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
        // store result
        CharacterChecker.Instance.SetCharacterResult(
            characterID,
            result.ToString().ToLower()
        );

        // build next scene name
        string sceneName =
            characterID + "_" + result.ToString().ToLower() + "_reaction";

        // DEBUG LOG (this is what you wanted)
        Debug.Log("Loading scene: " + sceneName);

        SceneManager.LoadScene(sceneName);
    }
}