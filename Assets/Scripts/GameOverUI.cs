using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public Button resetButton;
    public Button quitGameButton;
    public TextMeshProUGUI highScoreText;

    private void Start()
    {
        resetButton.onClick.AddListener(() =>
        {
            GameManager.instance.ResetGame();
        });

        quitGameButton.onClick.AddListener(() =>
        {
            GameManager.instance.QuitGame();
        });

        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = $"High Score: {highScore}";

    }
}
