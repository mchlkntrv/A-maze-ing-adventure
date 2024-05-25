using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectionController : MonoBehaviour
{
    public void LoadLevel(string levelName)
    {
        AudioManager.instance.PlayButtonClickSound();
        Debug.Log("Loading level: " + levelName);
        SceneManager.LoadScene(levelName);
    }
}
