using UnityEngine;

public class Magnifier : MonoBehaviour
{

    public RectTransform magnifierUI;
    public Camera zoomCamera; 

    void Update()
    {
        // UI follow 
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            magnifierUI.parent as RectTransform, 
            (Vector2)Input.mousePosition, 
            cam: null, 
            out localPos);
            magnifierUI.anchoredPosition = localPos; 

            // world follow
            Vector3 world = Camera.main.ScreenToWorldPoint(position: new Vector3(Input.mousePosition.x, Input.mousePosition.y, z:10f));
            zoomCamera.transform.position = new Vector3(world.x, world.y, zoomCamera.transform.position.z);
    }
}
