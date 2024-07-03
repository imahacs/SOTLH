using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    void Start()
    {

    }

    // Load the main menu scene
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }


    // Load the settings menu scene
    public void LoadSettingsMenu()
    {
        SceneManager.LoadScene("SettingsMenuScene");
    }

    // Load the tutorial scene
    public void LoadTutorial()
    {
        SceneManager.LoadScene("TutorialScene");
    }

    // Load the game scene
    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene"); // Replace with your actual game scene name
    }

    // Quit the application
    public void QuitGame()
    {
        Application.Quit();
    }


}