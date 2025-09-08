using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI lifesText;
    public TextMeshProUGUI levelText;

    [Header("Audio Reference")]
    public AudioManager audioManager;

    [Header("Basic Events")]
    public Action onBrickHit;
    public Action onBrickDestroyed;
    public Action onLostLife;
    public Action onPointWon;

    [Header("PowerUps Events")]
    public Action onFastBallPowerup;
    public Action onSlowBallPowerup;
    public Action onBiggerPaddlePowerup;
    public Action onFireballPowerupBegin;
    public Action onFireballPowerupEnd;

    private int score = 0;
    private int lifes = 3;
    private int maxLifes = 3;

    private int totalBricks;
    private int totalScenes;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        // Κάνε subscribe στο sceneLoaded event για να ξαναφτιάχνεις refs όταν αλλάζει σκηνή
        SceneManager.sceneLoaded += OnSceneLoaded;
        onBrickHit += OnBrickHitSound;
        onBrickDestroyed += OnBrickDestroyed;
    }

    private void Start()
    {
        totalScenes = SceneManager.sceneCountInBuildSettings;
        InitLevel();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Ψάξε ξανά UI elements, γιατί στο νέο scene αλλάζουν!
        scoreText = GameObject.Find("ScoreText")?.GetComponent<TextMeshProUGUI>();
        lifesText = GameObject.Find("LifesText")?.GetComponent<TextMeshProUGUI>();
        levelText = GameObject.Find("LevelText")?.GetComponent<TextMeshProUGUI>();

        InitLevel();
    }

    private void InitLevel()
    {
        totalBricks = GameObject.FindGameObjectsWithTag("Brick").Length;

        UpdateUI();
    }

    public void OnDeadZoneEnter()
    {
        lifes--;

        if (lifes <= 0)
        {
            //Debug.Log("Game Over!");
            SceneManager.LoadScene("GameOver");
            SaveHighscore();
            return;
        }

        UpdateUI();
    }

    public int GetCurrentSceneBuildIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public void OnBrickDestroyed()
    {
        score++;
        onPointWon?.Invoke();

        audioManager.PlayBrickDestroyedSound();

        totalBricks--;

        if (totalBricks <= 0)
        {
            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            if (currentIndex + 1 < totalScenes)
            {
                SceneManager.LoadScene(currentIndex + 1);
            }
            else
            {
                Debug.Log("You Win!");
                SaveHighscore();
                SceneManager.LoadScene("WinScene");
            }
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (scoreText) scoreText.text = $"Score: {score}";
        if (lifesText) lifesText.text = $"Lives: {lifes}";
        if (levelText) levelText.text = $"Level {SceneManager.GetActiveScene().buildIndex + 1}";
    }

    public void ResetGame()
    {
        //Debug.Log("Let's play again!");
        score = 0;
        lifes = 3;
        SceneManager.LoadScene(0);
    }

    private void SaveHighscore()
    {
        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save(); // αποθηκεύει στη συσκευή
        }
    }

    public void QuitGame()
    {
        Application.Quit();

        // Σημείωση: Στο Editor δεν "κλείνει" το παιχνίδι.
        // Αν θες να δουλεύει και στο Editor, βάλε:
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void OnBrickHitSound()
    {
        audioManager.PlayBrickHitSound();
    }

    public void AddExtraLife()
    {
        if (lifes < maxLifes)
            lifes++;
    }
}
