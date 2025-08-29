using TMPro;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;
    public TextMeshProUGUI pauseButtonText;

    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        pauseButtonText.text = "RESUME";
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        pauseButtonText.text = "PAUSE";
    }
}
