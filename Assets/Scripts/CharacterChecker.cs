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
        if (Instance != null)
        {
            Debug.LogError("DUPLICATE CharacterChecker DESTROYED");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Debug.Log("CharacterChecker CREATED");
    }

    // =========================
    // SET RESULT (SAFE)
    // =========================
    public void SetCharacterResult(string characterName, string result)
    {
        Debug.Log("SET RESULT: " + characterName + " = " + result);

        // store / overwrite safely
        characterResults[characterName] = result;

        // score only ONCE per character
        if (!scoredCharacters.Contains(characterName))
        {
            if (result == "completed" || result == "nearly")
            {
                score += 1;
                Debug.Log("SCORE +1 (from " + characterName + ")");
            }

            scoredCharacters.Add(characterName);
        }
    }

    // =========================
    // HELPED SYSTEM
    // =========================
    public void MarkHelped(string characterName)
    {
        helpedCharacters.Add(characterName);
        Debug.Log("HELPED: " + characterName + " | total = " + helpedCharacters.Count);
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

    // =========================
    // GETTERS
    // =========================
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

    public int DebugHelpedCount()
    {
        return helpedCharacters.Count;
    }

    // =========================
    // DEBUG FULL STATE
    // =========================
    public void PrintFinalState()
    {
        Debug.Log("===== FINAL STATE =====");

        foreach (var kv in characterResults)
        {
            Debug.Log(kv.Key + " = " + kv.Value);
        }

        Debug.Log("FINAL SCORE = " + score);
        Debug.Log("HELPED COUNT = " + helpedCharacters.Count);
    }
}