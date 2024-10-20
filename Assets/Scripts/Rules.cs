using UnityEngine;
using UnityEngine.SceneManagement;

public class Rules : MonoBehaviour
{

    void Awake()
    {
        Timer.IsOver.AddListener(GameOver);
        Cursor.visible = false;
    }

    private void GameOver()
    {
        SetCursorVisible(true);
    }

    public void RestartGame() 
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void SetCursorVisible(bool visible)
    {
        Cursor.visible = visible;
    }

}
