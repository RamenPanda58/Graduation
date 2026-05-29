using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndDayButton : MonoBehaviour
{
    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        button.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        CheckState();
    }

    void Start()
    {
        CheckState();
        
    }

    void CheckState()
    {
        if (CharacterChecker.Instance == null) return;

        if (CharacterChecker.Instance.AllCharactersHelped())
        {
            Debug.Log("ALL CHARACTERS HELPED → SHOW BUTTON");
            button.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("NOT READY YET");
        }
    }

    public void GoToResult()
    {
        int score = CharacterChecker.Instance.GetScore();
        SceneManager.LoadScene("Result_" + score);
    }
}