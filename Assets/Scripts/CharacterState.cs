using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterState : MonoBehaviour
{
    public string characterName;
    public Button characterButton;

    private SpriteRenderer[] sprites;
    private Color[] originalColors;

    private void Awake()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>();

        originalColors = new Color[sprites.Length];
        for (int i = 0; i < sprites.Length; i++)
        {
            originalColors[i] = sprites[i].color;
        }
    }

    private void OnEnable()
    {
        ApplyState();
    }

    void ApplyState()
{
    string result = CharacterChecker.Instance.GetCharacterResult(characterName);

    if (result == "completed" || result == "nearly")
    {
        StartCoroutine(FadeOut(2f));
        characterButton.gameObject.SetActive(false);
    }
    else if (result == "failed")
    {
        ApplyGray();
        characterButton.gameObject.SetActive(false);
    }

    // mark as helped visually/logic-safe (optional redundancy)
    CharacterChecker.Instance.SetCharacterResult(characterName, result);
}

    void ApplyGray()
    {
        foreach (var s in sprites)
        {
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
                Color c = originalColors[i];
                c.a = Mathf.Lerp(originalColors[i].a, 0f, t);
                sprites[i].color = c;
            }

            yield return null;
        }

        // fully invisible
        foreach (var s in sprites)
        {
            Color c = s.color;
            c.a = 0f;
            s.color = c;
        }
    }
}