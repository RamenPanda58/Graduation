using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Blinking : MonoBehaviour
{
    public static Blinking Instance;

    public SpriteRenderer fadeRect;
    public float fadeTime = 0.12f;
    public float holdTime = 0.10f;

    public GameObject[] lightObjects;

    private bool isTransitioning = false;
    private string targetScene;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GoToScene(string sceneName)
    {
        if (isTransitioning) return;

        targetScene = sceneName;
        StartCoroutine(Transition());
    }

    IEnumerator Transition()
    {
        isTransitioning = true;

        SetLights(false);

        // BLINK 1
        yield return Fade(0f, 1f);
        yield return new WaitForSeconds(holdTime);
        yield return Fade(1f, 0f);

        // gap between blinks
        yield return new WaitForSeconds(holdTime);

        // BLINK 2 (go to black)
        yield return Fade(0f, 1f);

        // 🔥 IMPORTANT: force full black hold BEFORE scene swap
        yield return new WaitForSeconds(0.15f);

        AsyncOperation op = SceneManager.LoadSceneAsync(targetScene);
        op.allowSceneActivation = false;

        // wait until scene is ready
        while (op.progress < 0.9f)
            yield return null;

        // 🔥 keep screen black while swapping scene
        yield return new WaitForSeconds(0.1f);

        op.allowSceneActivation = true;

        // wait for scene to fully load
        while (!op.isDone)
            yield return null;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(PostLoad());
    }

    IEnumerator PostLoad()
    {
        yield return null; // let scene initialize

        FindFadeRect();

        // fade OUT of black into new scene
        yield return Fade(1f, 0f);

        SetLights(true);

        isTransitioning = false;
    }

    IEnumerator Fade(float from, float to)
    {
        FindFadeRect();

        if (fadeRect == null)
            yield break;

        float t = 0f;
        Color c = fadeRect.color;

        while (t < fadeTime)
        {
            t += Time.deltaTime;

            float p = Mathf.SmoothStep(0f, 1f, t / fadeTime);
            c.a = Mathf.Lerp(from, to, p);

            fadeRect.color = c;

            yield return null;
        }

        c.a = to;
        fadeRect.color = c;
    }

    void FindFadeRect()
    {
        if (fadeRect != null) return;

        GameObject obj = GameObject.FindGameObjectWithTag("FadeRect");

        if (obj != null)
            fadeRect = obj.GetComponent<SpriteRenderer>();
    }

    void SetLights(bool state)
    {
        if (lightObjects == null) return;

        foreach (var obj in lightObjects)
        {
            if (obj != null)
                obj.SetActive(state);
        }
    }
}