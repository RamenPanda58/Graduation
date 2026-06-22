using UnityEngine;

public class StartScreenBackgroundAnimation : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private SpriteRenderer backgroundRenderer;
    [SerializeField] private Sprite[] animationFrames;
    [SerializeField] private float frameDuration = 0.2f; // Seconds per frame

    [Header("UI")]
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject quitButton;

    private void Start()
    {
        // Hide buttons at scene start
        startButton.SetActive(false);
        quitButton.SetActive(false);

        StartCoroutine(PlayIntroAnimation());
    }

  private System.Collections.IEnumerator PlayIntroAnimation()
{
    for (int i = 0; i < animationFrames.Length - 1; i++)
    {
        backgroundRenderer.sprite = animationFrames[i];
        yield return new WaitForSeconds(frameDuration);
    }

    // Show final frame
    backgroundRenderer.sprite = animationFrames[animationFrames.Length - 1];

    // Buttons appear immediately
    startButton.SetActive(true);
    quitButton.SetActive(true);
}
}