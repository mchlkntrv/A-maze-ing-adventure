using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Button nextLevelButton;
    public Button quitButton;
    public Button levelSelectionButton;
    public Button restartLevelButton;
    public Button muteButton;

    private void Start()
    {
        nextLevelButton.onClick.AddListener(OnNextLevelButton);
        quitButton.onClick.AddListener(OnQuitButton);
        levelSelectionButton.onClick.AddListener(OnLevelSelectionButton);
        restartLevelButton.onClick.AddListener(OnRestartLevelButton);

        muteButton = GameObject.Find("MuteButton")?.GetComponent<Button>();
        if (muteButton != null)
        {
            muteButton.onClick.RemoveAllListeners();
            muteButton.onClick.AddListener(OnMuteButton);
        }
    }

    private void OnMuteButton()
    {
        AudioManager.instance.PlayBackgroundMusic();
    }

    private void OnNextLevelButton()
    {
        AudioManager.instance.PlayButtonClickSound();
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
        AudioManager.instance.PlayButtonClickSound();

        Application.Quit();
        Debug.Log("Application Quit");
    }

    private void OnLevelSelectionButton()
    {
        AudioManager.instance.PlayButtonClickSound();

        SceneManager.LoadScene("LevelSelection");
    }

    private void OnRestartLevelButton()
    {
        AudioManager.instance.PlayButtonClickSound();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
