using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterState : MonoBehaviour
{
    public string characterName;
    public Button characterButton;

    private SpriteRenderer[] sprites;
    private Color[] originalColors;

    private const string KEY_PREFIX = "Character_";

    private bool isCompletedForever = false;

    private void Awake()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>();

        originalColors = new Color[sprites.Length];

        for (int i = 0; i < sprites.Length; i++)
        {
            originalColors[i] = sprites[i].color;
        }

        // 🔥 IMPORTANT: instant skip BEFORE first render logic
        if (PlayerPrefs.GetInt(KEY_PREFIX + characterName, 0) == 1)
        {
            isCompletedForever = true;
            characterButton.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        if (isCompletedForever)
            return;

        StartCoroutine(DelayedApplyState());
    }

    IEnumerator DelayedApplyState()
    {
        yield return null;
        ApplyState();
    }

    void ApplyState()
    {
        if (CharacterChecker.Instance == null)
            return;

        string result = CharacterChecker.Instance.GetCharacterResult(characterName);

        if (result == "completed" || result == "nearly")
        {
            StartCoroutine(FadeOut(2f));

            if (characterButton != null)
                characterButton.gameObject.SetActive(false);
        }
        else if (result == "failed")
        {
            ApplyGray();

            if (characterButton != null)
                characterButton.gameObject.SetActive(false);
        }
    }

    void ApplyGray()
    {
        foreach (var s in sprites)
        {
            if (s != null)
                s.color = Color.gray;
        }
    }

    IEnumerator FadeOut(float duration)
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;

            for (int i = 0; i < sprites.Length; i++)
            {
                if (sprites[i] == null) continue;

                Color c = originalColors[i];
                c.a = Mathf.Lerp(originalColors[i].a, 0f, t);
                sprites[i].color = c;
            }

            yield return null;
        }

        // 🔥 SAVE STATE
        PlayerPrefs.SetInt(KEY_PREFIX + characterName, 1);
        PlayerPrefs.Save();

        gameObject.SetActive(false);
    }
}