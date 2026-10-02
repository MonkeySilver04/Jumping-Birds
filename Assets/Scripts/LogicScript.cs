using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class LogicScript : MonoBehaviour
{

    public int playerScore;
    public int testStartingScore;
    public Text scoreText;
    public TextMeshProUGUI HighScoreText;
    public GameObject gameOverScreen;
    public GameObject SwitchToNormalModeButton;
    public UnlockNotification unlockNotification;
    public Animator highScoreAnimator;
    public Image deadBirdImage;
    public Sprite[] deadBirdSprites;
    public float[] deadBirdScales;
    public GameObject NewUnlockGameOverText;
    public GameObject[] gameStar;

    

    [ContextMenu("Increase Score")]

    void Start()
    {
        playerScore = testStartingScore;
        scoreText.text = playerScore.ToString();

        string highScoreKey = GetHighScoreKey();

        int highScore =
            PlayerPrefs.GetInt(highScoreKey, 0);

        if (PlayerPrefs.GetInt("HardMode", 0) == 1)
        {
            HighScoreText.text =
                "Hard Mode High Score: " + highScore;
        }
        else
        {
            HighScoreText.text =
                "High Score: " + highScore;
        }

        UpdateGameStars();
    }
    

    public void addScore(int scoreToAdd)
    {
        playerScore += scoreToAdd;
        scoreText.text = playerScore.ToString();

        string highScoreKey = GetHighScoreKey();

        int highScore =
            PlayerPrefs.GetInt(highScoreKey, 0);

        if (playerScore > highScore)
        {
            PlayerPrefs.SetInt(
                highScoreKey,
                playerScore
            );

            PlayerPrefs.Save();

            HighScoreText.text =
                "NEW HIGH SCORE!";

            highScoreAnimator.SetTrigger(
                "NewHighScore"
            );
        }

        unlockNotification.CheckUnlocks(playerScore);

        UpdateGameStars();
    }

    public void restartGame()
    {
        Invoke("ReloadGame",0.1f);
    }
    void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void gameOver()
    {
        bool isHardMode = PlayerPrefs.GetInt("HardMode",0) == 1;

        int hardDeathStreak = PlayerPrefs.GetInt("HardDeathStreak",0);
        
        if (isHardMode && hardDeathStreak >= 7)
        {
            SwitchToNormalModeButton.SetActive(true);
        }
        else
        {
            SwitchToNormalModeButton.SetActive(false);
        }

        int NewUnlock = PlayerPrefs.GetInt("NewUnlockGameOver",0);
        if (NewUnlock == 1)
        {
            NewUnlockGameOverText.SetActive(true);
            PlayerPrefs.SetInt("NewUnlockGameOver",0);
            PlayerPrefs.Save();
        }
        else
        {
            NewUnlockGameOverText.SetActive(false);
        }

        string highScoreKey = GetHighScoreKey();

        int highScore =
            PlayerPrefs.GetInt(highScoreKey, 0);

        if (playerScore > highScore)
        {
            highScore = playerScore;

            PlayerPrefs.SetInt(
                highScoreKey,
                highScore
            );

            PlayerPrefs.Save();
        }

        if (PlayerPrefs.GetInt("HardMode", 0) == 1)
        {
            HighScoreText.text =
                "Hard Mode High Score: " + highScore;
        }
        else
        {
            HighScoreText.text =
                "High Score: " + highScore;
        }

        int selectedBird =
            PlayerPrefs.GetInt("SelectedBird", 0);

        deadBirdImage.sprite =
            deadBirdSprites[selectedBird];

        deadBirdImage.rectTransform.localScale =
            Vector3.one *
            deadBirdScales[selectedBird];

        gameOverScreen.SetActive(true);
    }

    void Update()
    {
        if (gameOverScreen.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            restartGame();
        }  
    }

    public void SwitchToNormalMode()
    {
        PlayerPrefs.SetInt("HardMode",0);
        PlayerPrefs.SetInt("HardDeathStreak",0);
        PlayerPrefs.Save();

        SceneManager.LoadScene("Game");
    }

    string GetHighScoreKey()
    {
        if (PlayerPrefs.GetInt("HardMode", 0) == 1)
        {
            return "HardHighScore";
        }

        return "HighScore";
    }

void UpdateGameStars()
    {
        int normalHighScore = PlayerPrefs.GetInt("HighScore",0);
        int HardHighScore = PlayerPrefs.GetInt("HardHighScore",0);

        if (PlayerPrefs.GetInt("HardMode",0) == 1)
        {
            if (playerScore > HardHighScore)
                HardHighScore = playerScore;
        }
        else
        {
            if (playerScore > normalHighScore)
                normalHighScore = playerScore;
        }

        bool normalCompleted = normalHighScore >= 120;
        bool hardCompleted = HardHighScore >= 100;

        int starCount = 0;

        if (normalCompleted || hardCompleted)
        {
            starCount = 1;
        }
        if (normalCompleted && hardCompleted)
        {
            starCount = 2;
        }
        if (normalCompleted && hardCompleted && HardHighScore >= 150)
        {
            starCount = 3;
        }

        for (int i = 0; i < gameStar.Length; i++)
        {
            gameStar[i].SetActive(i < starCount);
        }
    }

    [ContextMenu("Set High Score to 10")]
    public void SetHighScore10()
    {
        PlayerPrefs.SetInt("HighScore",10);
        PlayerPrefs.Save();
    }
    [ContextMenu("Set High Score to 20")]
    public void SetHighScore20()
    {
        PlayerPrefs.SetInt("HighScore",20);
        PlayerPrefs.Save();
    }
    [ContextMenu("Set High Score to 30")]
    public void SetHighScore30()
    {
        PlayerPrefs.SetInt("HighScore",30);
        PlayerPrefs.Save();
    }
    [ContextMenu("Set High Score to 100")]
    public void SetHighScore100()
    {
        PlayerPrefs.SetInt("HighScore",100);
        PlayerPrefs.Save();
    }
    [ContextMenu("Set High Score to 120")]
    public void SetHighScore120()
    {
        PlayerPrefs.SetInt("HighScore",120);
        PlayerPrefs.Save();
    }
    [ContextMenu("Reset High Score")]
    public void ResetHighScore()
    {
        PlayerPrefs.SetInt("HighScore",0);
        PlayerPrefs.Save();
    }

    [ContextMenu("Set Death Count to 200")]
    public void SetDeathScore200()
    {
        PlayerPrefs.SetInt("TotalDeaths",200);
        PlayerPrefs.Save();
    }
    [ContextMenu("Set Death Count to 199")]
    public void SetDeathScore199()
    {
        PlayerPrefs.SetInt("TotalDeaths",199);
        PlayerPrefs.Save();
    }
    [ContextMenu("Reset Death Count")]
    public void ResetDeathCount()
    {
        PlayerPrefs.SetInt("TotalDeaths",0);
        PlayerPrefs.Save();
    }
    
    [ContextMenu("Reset Hard Death Streak")]
    public void ResetHardDeathStreak()
    {
        PlayerPrefs.SetInt("HardDeathStreak",0);
        PlayerPrefs.Save();

        Debug.Log("Hard Death Streak reset!");
    }

}
