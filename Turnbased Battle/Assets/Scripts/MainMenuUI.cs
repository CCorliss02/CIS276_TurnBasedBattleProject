using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public Button startButton;
    private string gameSceneName = "BattleScene";

    private void Start()
    {
        startButton.onClick.AddListener(LoadGameScene);
    }

    public void LoadGameScene()
    {
        GameSceneManager.Instance.LoadScene(gameSceneName);
    }
}