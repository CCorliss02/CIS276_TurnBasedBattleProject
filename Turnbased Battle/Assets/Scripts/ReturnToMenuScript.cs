using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReturnToMenuScript : MonoBehaviour
{
    public Button startButton;
    private string gameSceneName = "MainMenuScene";

    private void Start()
    {
        startButton.onClick.AddListener(LoadGameScene);
    }

    public void LoadGameScene()
    {
        GameSceneManager.Instance.LoadScene(gameSceneName);
    }
}
