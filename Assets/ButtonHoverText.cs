using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text actionText; // Drag your ActionText here
    [TextArea]
    public string hoverMessage; // Set custom phrase per button

    public void OnPointerEnter(PointerEventData eventData)
    {
        actionText.text = hoverMessage;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        actionText.text = "";
    }
}