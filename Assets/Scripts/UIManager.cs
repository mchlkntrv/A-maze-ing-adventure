using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Button nextLevelButton;
    public Button quitButton;
    public Button levelSelectionButton;
    public Button restartLevelButton;
    public AudioManager soundsController;

    private void Start()
    {
        nextLevelButton.onClick.AddListener(OnNextLevelButton);
        quitButton.onClick.AddListener(OnQuitButton);
        levelSelectionButton.onClick.AddListener(OnLevelSelectionButton);
        restartLevelButton.onClick.AddListener(OnRestartLevelButton);
    }

    private void OnNextLevelButton()
    {
        soundsController.PlayButtonClickSound();
        if (LevelManager.instance != null)
        {
            LevelManager.instance.LoadNextLevel();
        }
        else
        {
            Debug.LogError("LevelManager instance is not set!");
        }
    }

    private void OnQuitButton()
    {
        soundsController.PlayButtonClickSound();

        Application.Quit();
        Debug.Log("Application Quit");
    }

    private void OnLevelSelectionButton()
    {
        soundsController.PlayButtonClickSound();

        SceneManager.LoadScene("LevelSelection");
    }

    private void OnRestartLevelButton()
    {
        soundsController.PlayButtonClickSound();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
