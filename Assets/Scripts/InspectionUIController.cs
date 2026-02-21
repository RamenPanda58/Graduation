using UnityEngine;
using UnityEngine.UI;

public class InspectionUIController : MonoBehaviour
{
    public GameObject clueImage1;
    public GameObject clueImage2;
    public GameObject closeButton;

    public void ShowClue1()
    {
        clueImage1.SetActive(true);
        closeButton.SetActive(true);
    }

    public void ShowClue2()
    {
        clueImage2.SetActive(true);
        closeButton.SetActive(true);
    }

    public void CloseClues()
    {
        clueImage1.SetActive(false);
        clueImage2.SetActive(false);
        closeButton.SetActive(false);
    }
}