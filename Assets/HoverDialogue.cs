using System.Collections;
using UnityEngine;
using Yarn.Unity;

public class HoverDialogue : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private DialogueRunner dialogueRunner;
    [SerializeField] private string nodeName;

    private Coroutine running;

    private void Start()
    {
        dialoguePanel.SetActive(false);
    }

    private void OnMouseEnter()
    {
        dialoguePanel.SetActive(true);

        if (running != null)
            StopCoroutine(running);

        running = StartCoroutine(PlayNode());
    }

    private void OnMouseExit()
    {
        if (running != null)
            StopCoroutine(running);
        running = null;

        dialogueRunner.Stop();
        dialoguePanel.SetActive(false);
    }

    private IEnumerator PlayNode()
    {
        // Hard reset any previous state
        dialogueRunner.Stop();

        // Wait 1 frame so Yarn fully clears internal state
        yield return null;

        // Start fresh every time
        dialogueRunner.StartDialogue(nodeName);
    }
}