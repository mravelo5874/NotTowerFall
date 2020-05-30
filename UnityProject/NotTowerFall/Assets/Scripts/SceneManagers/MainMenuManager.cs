using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void OnlineMultButton()
    {
        Debug.Log("OnlineMultButton: Pressed");
    }

    public void LocalMultButton()
    {
        Debug.Log("LocalMultButton: Pressed");
        SceneManager.LoadScene("DefaultScene");
    }

    public void SettingsButton()
    {
        Debug.Log("SettingsButton: Pressed");
    }

    public void ExitButton()
    {
        Debug.Log("ExitButton: Pressed");
        Application.Quit();
    }
}
