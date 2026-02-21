using UnityEngine;
using Yarn.Unity;
using System.Collections;

public class InspectionController : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    public InspectionCamera2D inspectionCamera;

    public GameObject button1;
    public GameObject button2;

    private Vector3 originalPosition;
    private float originalZoom;

    void Start()
    {
        // Register coroutine command
        dialogueRunner.AddCommandHandler("inspect_object", InspectCoroutine);
        dialogueRunner.AddCommandHandler("inspect_button", EnterInspection);
    }

    IEnumerator InspectCoroutine()
    {
        // Save camera state
        originalPosition = inspectionCamera.transform.position;
        originalZoom = inspectionCamera.GetComponent<Camera>().orthographicSize;

        // Enable inspection mode
        inspectionCamera.enabled = true;

        // WAIT until inspection camera is disabled
        while (inspectionCamera.enabled)
            yield return null;

        // Restore camera
        inspectionCamera.transform.position = originalPosition;
        inspectionCamera.GetComponent<Camera>().orthographicSize = originalZoom;

        // Yarn automatically continues after this coroutine ends
    }

    public void EnterInspection()
    {
        button1.SetActive(true);
        button2.SetActive(true);
        
        dialogueRunner.Stop();
    }

    public void FinishInspection()
    {
        inspectionCamera.enabled = false;
        button1.SetActive(false);
        button2.SetActive(false);
        
    }
}  