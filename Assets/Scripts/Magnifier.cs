using UnityEngine;
using TMPro;

public class Magnifier : MonoBehaviour
{
    public RectTransform magnifierUI;
    public Camera zoomCamera;
    public TMP_Text clueTextUI;

    Clue currentClue;

    void Update()
    {
        // UI follow
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            magnifierUI.parent as RectTransform,
            Input.mousePosition,
            null,
            out localPos);

        magnifierUI.anchoredPosition = localPos;

        // world follow
        Vector3 world = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));

        zoomCamera.transform.position =
            new Vector3(world.x, world.y, zoomCamera.transform.position.z);

        CheckForClue();
    }

   void CheckForClue()
{
    Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    RaycastHit2D hit = Physics2D.Raycast(mouseWorld, Vector2.zero);

    if (hit.collider != null)
    {
        Clue clue = hit.collider.GetComponent<Clue>();

        if (clue != null)
        {
            clueTextUI.text = clue.clueText;
            clueTextUI.gameObject.SetActive(true);
            return;
        }
    }

    clueTextUI.gameObject.SetActive(false);
}
}