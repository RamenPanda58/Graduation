using UnityEngine;

public class Reset_Playerprefs : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
