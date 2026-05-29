using System.Collections.Generic;
using UnityEngine;

public class CharacterChecker : MonoBehaviour
{
    public static CharacterChecker Instance;

    private Dictionary<string, string> characterResults = new Dictionary<string, string>();
    private HashSet<string> helpedCharacters = new HashSet<string>();
    private HashSet<string> scoredCharacters = new HashSet<string>();

    private int score = 0;

    private readonly string[] allCharacters =
    {
        "AnxLady",
        "Twin",
        "Farmer",
        "Sailor"
    };

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

    // -------------------------
    // RESULT + SCORING
    // -------------------------
    public void SetCharacterResult(string characterName, string result)
    {
        characterResults[characterName] = result;

        // score only once per character
        if (!scoredCharacters.Contains(characterName))
        {
            if (result == "completed" || result == "nearly")
            {
                score += 1;
            }

            scoredCharacters.Add(characterName);
        }
    }

    // -------------------------
    // HELPED SYSTEM
    // -------------------------
    public void MarkHelped(string characterName)
{
    helpedCharacters.Add(characterName);
    Debug.Log("HELPED ADDED: " + characterName + " | Total: " + helpedCharacters.Count);
}

    public bool AllCharactersHelped()
    {
        foreach (var c in allCharacters)
        {
            if (!helpedCharacters.Contains(c))
                return false;
        }
        return true;
    }

    // -------------------------
    // GETTERS
    // -------------------------
    public string GetCharacterResult(string characterName)
    {
        if (characterResults.ContainsKey(characterName))
            return characterResults[characterName];

        return "active";
    }

    public int GetScore()
    {
        return score;
    }

    // DEBUG (optional)
    public int DebugHelpedCount()
    {
        return helpedCharacters.Count;
    }
}