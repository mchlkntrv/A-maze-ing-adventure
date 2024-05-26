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

        if (GameObject.Find("MuteButton").TryGetComponent<Button>(out muteButton))
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
        LevelManager.instance.LoadNextLevel();
    }

    private void OnQuitButton()
    {
        PlayerPrefs.DeleteAll();
        AudioManager.instance.PlayButtonClickSound();
        Application.Quit();
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
