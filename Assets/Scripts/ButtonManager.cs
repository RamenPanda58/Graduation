using UnityEngine;
using UnityEngine.SceneManagement;  // For scene management

public class ButtonManager : MonoBehaviour
{

    [SerializeField] private GameObject buttonObject;
    // Function for the Start button
    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No next scene in the build settings.");
        }
    }

    // Function for the Quit button
    public void QuitGame()
    {
        Debug.Log("Quit button clicked!");
        Application.Quit();
    }

    // Function for the Restart button (if you have one)
    public void RestartGame()
    {
        Debug.Log("Restart button clicked!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Function to go to the scene with index 2 (InspectTwin)
    public void InspectTwin()
    {
        int targetSceneIndex = 2;

        // Check if the scene with index 2 exists in the build settings
        if (targetSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(targetSceneIndex);
        }
        else
        {
            Debug.LogError("Scene with index 2 not found in the build settings.");
        }
    }

       // NEW: Function to load the "ActionScene"
    public void ActionSceneButton()
    {
        SceneManager.LoadScene("ActionScene");
    }

    // Function to go to the scene called "ReactionScene"
public void RightAnswer()
{
    SceneManager.LoadScene("ReactionScene");
}

// Function to go to the scene called "ClosingScene"
public void CloseForTheDay()
{
    SceneManager.LoadScene("ClosedScene");
}

public void Home()
{
    SceneManager.LoadScene("TeahouseView");
}


  public void ShowButtonHome()
    {
        buttonObject.SetActive(true);
    }

   public void TwinInspect()
{
    SceneManager.LoadScene("Twin_inspect");
}

   public void SailorInspect()
{
    SceneManager.LoadScene("Sailor_inspect");
}
   public void AnxLadyInspect()
{
    SceneManager.LoadScene("AnxLady_inspect");
}
   public void FarmerInspect()
{
    SceneManager.LoadScene("Farmer_inspect");
}


   public void AnxLadyAction()
{
    SceneManager.LoadScene("AnxLady_action");
}

   public void FarmerAction()
{
    SceneManager.LoadScene("Farmer_action");
}

   public void SailorAction()
{
    SceneManager.LoadScene("Sailor_action");
}

   public void TwinAction()
{
    SceneManager.LoadScene("Twin_action");
}
}