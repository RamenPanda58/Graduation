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

    private void Start()
    {
        StartCoroutine(DelayedApplyState());
    }

    IEnumerator DelayedApplyState()
    {
        // wait one frame so CharacterChecker initializes first
        yield return null;

        ApplyState();
    }

    void ApplyState()
    {
        if (CharacterChecker.Instance == null)
        {
            Debug.LogError("CharacterChecker Instance is STILL NULL");
            return;
        }

        string result =
            CharacterChecker.Instance.GetCharacterResult(characterName);

        Debug.Log(characterName + " result = " + result);

        if (result == "completed" || result == "nearly")
        {
            StartCoroutine(FadeOut(2f));

            if (characterButton != null)
            {
                characterButton.gameObject.SetActive(false);
            }
        }
        else if (result == "failed")
        {
            ApplyGray();

            if (characterButton != null)
            {
                characterButton.gameObject.SetActive(false);
            }
        }
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

        foreach (var s in sprites)
        {
            Color c = s.color;
            c.a = 0f;
            s.color = c;
        }
    }
}