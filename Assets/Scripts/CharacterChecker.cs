using System.Collections.Generic;
using UnityEngine;

public class CharacterChecker : MonoBehaviour
{
    public static CharacterChecker Instance;

    // stores state for each character
    private Dictionary<string, string> characterResults =
        new Dictionary<string, string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCharacterResult(string characterName, string result)
    {
        characterResults[characterName] = result;
    }

    public string GetCharacterResult(string characterName)
    {
        if (characterResults.ContainsKey(characterName))
        {
            return characterResults[characterName];
        }

        return "active";
    }
}