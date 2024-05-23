using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionController : MonoBehaviour
{
    public void LoadLevel(string levelName)
    {
        Debug.Log("Loading level: " + levelName);
        SceneManager.LoadScene(levelName);
    }
}
