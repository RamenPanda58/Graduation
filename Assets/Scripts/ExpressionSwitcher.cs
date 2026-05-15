using UnityEngine;

public class ExpressionSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject[] expressions;

    private int currentExpression = -1;

    public void SetExpression(int index)
    {
        // Prevent invalid indexes
        if (index < 0 || index >= expressions.Length)
            return;

        // Skip if already active
        if (currentExpression == index)
            return;

        // Disable all
        for (int i = 0; i < expressions.Length; i++)
        {
            expressions[i].SetActive(false);
        }

        // Enable selected
        expressions[index].SetActive(true);

        currentExpression = index;
    }

    private void Start()
    {
        // Optional: activate first expression by default
        if (expressions.Length > 0)
        {
            SetExpression(0);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetExpression(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetExpression(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetExpression(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetExpression(3);
        }
    }
}
