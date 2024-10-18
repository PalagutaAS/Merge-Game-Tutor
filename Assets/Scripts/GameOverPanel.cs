using System;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] GameObject _gameOverPanel;
    private void Awake()
    {
        if (_gameOverPanel == null)
        {
            throw new NullReferenceException("Game Over Panel is Required!");
        }
        Timer.IsOver.AddListener(ShowDalay);
    }

    private void ShowDalay()
    {
        Invoke(nameof(Show), 1.5f);
    }    
    
    private void Show()
    {
        _gameOverPanel.SetActive(true);
    }

    
}
