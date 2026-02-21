using UnityEngine;
using Yarn.Unity;

public class MainCharacterClick : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    public InspectionCamera2D inspectionCamera;
    public string resumeNodeName = "AfterInspection";

    void OnMouseDown()
    {
        // Disable inspection camera
        inspectionCamera.enabled = false;

        // Hide clue UI
        FindObjectOfType<InspectionUIController>().CloseClues();

        // Resume dialogue manually
        dialogueRunner.StartDialogue(resumeNodeName);
    }
}