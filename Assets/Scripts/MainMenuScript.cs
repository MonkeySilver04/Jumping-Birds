using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class MainMenuScript : MonoBehaviour
{
    public TextMeshProUGUI HighScoreText;
    public TextMeshProUGUI TotalDeathsText;
    public GameObject BirdSelectionPanel;

    public GameObject musicMutedLine;

    public Image BirdImage;
    public TextMeshProUGUI BirdName;
    public Animator changeBirdAnimation;
    public GameObject newBirdIndicator;
    public Animator changeBackgroundAnimation;
    public GameObject newBackgroundIndicator;

    public int deathUnlockedBirdIndex = 8;
    public int requiredDeathsForBird = 200;
    public int deathUnlockedBackgroundIndex = 6;
    public int requiredDeathsForBackground = 200;

    public int firstHardBirdIndex = 9;
    public int finalBirdIndex = 13;
    public int[] hardBirdRequieredScores =
    {
        5,
        20,
        50,
        70,
    };

    public int firstHardBackgroundIndex = 7;
    public int[] hardBackgroundRequiredScore =
    {
        0,
        20,
        60,
        70,
        100
    };

    public TextMeshProUGUI HardHighScoreText;

    public GameObject HardModebutton;
    public TextMeshProUGUI HardModeText;
    public GameObject HardModeTitle;
    

    public float[] selectedBirdScales;
    public float[] birdSelectionScales;

    public Sprite[] birdSprites;
    public Sprite[] lockedBirdSprites;
    public string[] birdNames;
    public int[] requiredScores;

    public Image SelectedBirdImage;
    public GameObject SelectedFrame;

    public GameObject newBirdSelectionText;
    public GameObject newBackgroundSelectionText;

    private int currentBirdIndex = 0;

    public GameObject[] unlockedStars;
    public Animator[] achievementStarAnimators;
    public Animator star1Animator;
    public Animator star2Animator;
    public Animator star3Animator;



    // BACKGROUND SELECTION
    public Sprite[] backgroundSprites;
    public Sprite[] lockedBackgroundSprites;
    public string[] backgroundNames;
    public int[] backgroundRequiredScores;

    public Image BackgroundImage;
    public TextMeshProUGUI BackgroundName;

    public Image SelectedBackgroundImage;
    public GameObject SelectedBackgroundFrame;
    public GameObject BackgroundSelectionPanel;

    public GameObject starInfoPanel;
    public TextMeshProUGUI starInfoTitle;
    public TextMeshProUGUI starInfoDescription;
    public Button playNormalButton;
    public Button playHardButton;
    public TextMeshProUGUI playNormalButtonText;
    public TextMeshProUGUI playHardButtonText;
    public GameObject hardModeRequirementText;
    public GameObject starCompletedText;
    

    private int currentBackgroundIndex = 0;

    [Header("DEBUG")]
    public int debugNormalHighScore = 70;
    public int debugHardHighScore = 0;


    void Start()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        HighScoreText.text = "High Score: " + highScore;

        int totalDeaths = PlayerPrefs.GetInt("TotalDeaths",0);
        TotalDeathsText.text = "Deaths: " + totalDeaths;


        int selectedBird = PlayerPrefs.GetInt("SelectedBird", 0);

        SelectedBirdImage.sprite = birdSprites[selectedBird];

        UpdateSelectedBirdScale();

        int hardModeUnlocked = PlayerPrefs.GetInt("HardModeUnlocked",0);
        if (hardModeUnlocked == 1)
        {
            int HardHighScore = PlayerPrefs.GetInt("HardHighScore",0);
            HardHighScoreText.gameObject.SetActive(true);
            HardHighScoreText.text = "Hard Mode High Score: " + HardHighScore;
        }
        else
        {
            HardHighScoreText.gameObject.SetActive(false);
        }


        int selectedBackground =
            PlayerPrefs.GetInt("SelectedBackground", 0);

        SelectedBackgroundImage.sprite =
            backgroundSprites[selectedBackground];

        int newBird = PlayerPrefs.GetInt("NewBirdAvailable",0);

        if (newBird == 1)
        {
            changeBirdAnimation.enabled = true;
            newBirdIndicator.SetActive(true);
        }
        else
        {
            changeBirdAnimation.enabled = false;
            newBirdIndicator.SetActive(false);
        }

        int newBackground = PlayerPrefs.GetInt("NewBackgroundAvailable",0);

        if (newBackground == 1)
        {
            changeBackgroundAnimation.enabled = true;
            newBackgroundIndicator.SetActive(true);
        }
        else
        {
            changeBackgroundAnimation.enabled = false;
            newBackgroundIndicator.SetActive(false);
        }

        bool isMusicMuted = PlayerPrefs.GetInt("MusicMuted",0) == 1;
        musicMutedLine.SetActive(isMusicMuted);

        int hardMode = PlayerPrefs.GetInt("HardMode",0);

        if (highScore >= 70 && hardModeUnlocked == 0)
        {
            PlayerPrefs.SetInt("HardModeUnlocked",1);
            PlayerPrefs.Save();

            hardModeUnlocked = 1;
        }

        if (hardModeUnlocked == 1)
        {
            HardModebutton.SetActive(true);

            if (hardMode == 1)
            {
                HardModeText.text = "SWITCH TO\nNORMAL MODE";
            }
            else
            {
                HardModeText.text = "SWITCH TO\nHARD MODE";
            }
            HardHighScoreText.gameObject.SetActive(true);

            int hardHighScore = PlayerPrefs.GetInt("HardHighScore",0);

            HardHighScoreText.text = "Hard Mode High Score: " + hardHighScore;

        }
        else
        {
            HardModebutton.SetActive(true);
            HardModeText.text = "HARD MODE\nReach Score 70";
            HardHighScoreText.gameObject.SetActive(false);
        }

        HardModeTitle.SetActive(hardMode == 1);

        UpdateAchievementStars();

        CheckFinalBirdUnlock(); 
        UpdateAchievementStars();
    }


    public void ToggleHardMode()
    {
        int hardModeUnlocked = PlayerPrefs.GetInt("HardModeUnlocked",0);
        
        if (hardModeUnlocked == 0)
        {
            return;
        }
        int hardMode = PlayerPrefs.GetInt("HardMode",0);
        
        if (hardMode == 0)
        {
            PlayerPrefs.SetInt("HardMode", 1);
            HardModeTitle.SetActive(true);

            HardModeText.text = "SWITCH TO\nNORMAL MODE";

            // Hard Mode'a hayatında ilk kez giriyorsa
            if (PlayerPrefs.GetInt("EnteredHardModeBefore", 0) == 0)
            {
                PlayerPrefs.SetInt(
                    "SelectedBackground",
                    firstHardBackgroundIndex
                );

                PlayerPrefs.SetInt(
                    "EnteredHardModeBefore",
                    1
                );

                SelectedBackgroundImage.sprite =
                    backgroundSprites[firstHardBackgroundIndex];

                MusicManager musicManager =
                    FindFirstObjectByType<MusicManager>();

                if (musicManager != null)
                {
                    musicManager.UpdateMusic();
                }
            }
}
else
{
    PlayerPrefs.SetInt("HardMode", 0);
    HardModeTitle.SetActive(false);

    HardModeText.text = "SWITCH TO\nHARD MODE";
}

PlayerPrefs.Save();
    }

    public void ToggleMusicButton()
    {
        MusicManager musicManager = FindFirstObjectByType<MusicManager>();

        if (musicManager != null)
        {
            musicManager.ToggleMusic();
            musicMutedLine.SetActive(musicManager.isMusicMuted);
        }
    }


    public void PlayGame()
    {
        Invoke("LoadGame",0.1f);   
    }
    void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }


    public void MainMenu()
    {
        Invoke("LoadMainMenu",0.1f);
    }
    void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }


    public void OpenBirdSelection()
    {
        PlayerPrefs.SetInt("NewBirdAvailable",0);
        PlayerPrefs.Save();

        changeBirdAnimation.enabled = false;
        newBirdIndicator.SetActive(false);


        BirdSelectionPanel.SetActive(true);

        currentBirdIndex =
            PlayerPrefs.GetInt("SelectedBird", 0);

        UpdateBirdDisplay();
    }


    public void CloseBirdSelection()
    {
        MarkCurrentBirdAsSeen();

        BirdSelectionPanel.SetActive(false);
    }


    public void NextBird()
    {
        MarkCurrentBirdAsSeen();

        currentBirdIndex++;

        if (currentBirdIndex >= birdSprites.Length)
        {
            currentBirdIndex = 0;
        }

        UpdateBirdDisplay();
    }


    public void PreviousBird()
    {
        MarkCurrentBirdAsSeen();

        currentBirdIndex--;

        if (currentBirdIndex < 0)
        {
            currentBirdIndex = birdSprites.Length - 1;
        }

        UpdateBirdDisplay();
    }


void UpdateBirdDisplay()
{
    int highScore =
        PlayerPrefs.GetInt("HighScore", 0);

    int hardHighScore = 
        PlayerPrefs.GetInt("HardHighScore",0);

    int totalDeaths =
        PlayerPrefs.GetInt("TotalDeaths", 0);

    bool isDeathBird =
        currentBirdIndex == deathUnlockedBirdIndex;

    bool isFinalBird =
        currentBirdIndex == finalBirdIndex;
    
    bool isHardBird = 
        currentBirdIndex >= firstHardBirdIndex && currentBirdIndex < finalBirdIndex;

    bool isUnlocked;

    if (isFinalBird)
        {
            isUnlocked = 
                highScore >= 120 && hardHighScore >= 150;
        }
    else if (isDeathBird)
        {
            isUnlocked = totalDeaths >= requiredDeathsForBird;
        }
    else if (isHardBird)
        {
            int hardBirdIndex = 
                currentBirdIndex - firstHardBirdIndex;
            isUnlocked = 
                hardHighScore >= hardBirdRequieredScores[hardBirdIndex];
        }
    else
    {
        isUnlocked =
            highScore >= requiredScores[currentBirdIndex];
    }


    // Kilitliyse silüet göster
    if (!isUnlocked && currentBirdIndex >= 1)
    {
        BirdImage.sprite =
            lockedBirdSprites[currentBirdIndex - 1];
    }
    else
    {
        BirdImage.sprite =
            birdSprites[currentBirdIndex];
    }


    // Kuş adı ve kilit durumu
    if (isUnlocked)
    {
        BirdName.text =
            birdNames[currentBirdIndex]
            + "\nUNLOCKED";
    }
    else
    {
        if (isFinalBird)
            {
                BirdName.text = 
                    birdNames[currentBirdIndex]
                    + "\nLOCKED - Earn\nAll 3 stars";    
            }
            
        else if (isDeathBird)
        {
            BirdName.text =
                birdNames[currentBirdIndex]
                + "\nLOCKED - Die\n "
                + requiredDeathsForBird
                + " Times";
        }
        else if (isHardBird)
            {
                int hardBirdIndex = 
                    currentBirdIndex - firstHardBirdIndex;
                
                BirdName.text =
                    birdNames[currentBirdIndex]
                    + "\nLOCKED - Reach\n "
                    + hardBirdRequieredScores[hardBirdIndex]
                    + " in Hard Mode";
            }
        else
        {
            BirdName.text =
                birdNames[currentBirdIndex]
                + "\nLOCKED - Reach\n "
                + requiredScores[currentBirdIndex]
                + " High Score";
        }
    }
    if (isUnlocked && IsBirdNew(currentBirdIndex))
        {
            newBirdSelectionText.SetActive(true);
        }
    else
        {
            newBirdSelectionText.SetActive(false);
        }

    // Seçili kuş çerçevesi
    int selectedBird =
        PlayerPrefs.GetInt("SelectedBird", 0);

    if (currentBirdIndex == selectedBird)
    {
        SelectedFrame.SetActive(true);
    }
    else
    {
        SelectedFrame.SetActive(false);
    }


    BirdImage.rectTransform.localScale =
        Vector3.one *
        birdSelectionScales[currentBirdIndex];
}


public void SelectBird()
{
    int highScore =
        PlayerPrefs.GetInt("HighScore", 0);

    int hardHighScore = 
        PlayerPrefs.GetInt("HardHighScore",0);

    int totalDeaths =
        PlayerPrefs.GetInt("TotalDeaths", 0);

    bool isDeathBird =
        currentBirdIndex == deathUnlockedBirdIndex;

    bool isFinalBird = 
        currentBirdIndex == finalBirdIndex;

    bool isHardBird =
        currentBirdIndex >= firstHardBirdIndex && currentBirdIndex < finalBirdIndex;

    bool isUnlocked;

    if (isFinalBird)
        {
            isUnlocked = highScore >= 120 && hardHighScore >= 150;
        }
    
    else if (isDeathBird)
    {
        isUnlocked =
            totalDeaths >= requiredDeathsForBird;
    }
    else if (isHardBird)
        {
            int hardBirdIndex =
                currentBirdIndex - firstHardBirdIndex;

            isUnlocked = hardHighScore >= hardBirdRequieredScores[hardBirdIndex];
        }
    else
    {
        isUnlocked =
            highScore >= requiredScores[currentBirdIndex];
    }


    if (isUnlocked)
    {
        MarkCurrentBirdAsSeen();

        PlayerPrefs.SetInt(
            "SelectedBird",
            currentBirdIndex
        );

        PlayerPrefs.Save();


        SelectedBirdImage.sprite =
            birdSprites[currentBirdIndex];

        UpdateSelectedBirdScale();


        BirdSelectionPanel.SetActive(false);
    }
}

    void UpdateSelectedBirdScale()
    {
        int selectedBird = PlayerPrefs.GetInt("SelectedBird", 0);

        SelectedBirdImage.rectTransform.localScale =
            Vector3.one * selectedBirdScales[selectedBird];
    }


    // =========================
    // BACKGROUND SELECTION
    // =========================

    public void OpenBackgroundSelection()
    {
        PlayerPrefs.SetInt("NewBackgroundAvailable",0);
        PlayerPrefs.Save();

        changeBackgroundAnimation.enabled = false;
        newBackgroundIndicator.SetActive(false);


        BackgroundSelectionPanel.SetActive(true);

        currentBackgroundIndex =
            PlayerPrefs.GetInt("SelectedBackground", 0);

        UpdateBackgroundDisplay();
    }


    public void CloseBackgroundSelection()
    {
        MarkCurrentBackgroundAsSeen();

        BackgroundSelectionPanel.SetActive(false);
    }


    public void NextBackground()
    {
        MarkCurrentBackgroundAsSeen();

        currentBackgroundIndex++;

        if (currentBackgroundIndex >= backgroundSprites.Length)
        {
            currentBackgroundIndex = 0;
        }

        UpdateBackgroundDisplay();
    }


    public void PreviousBackground()
    {
        MarkCurrentBackgroundAsSeen();

        currentBackgroundIndex--;

        if (currentBackgroundIndex < 0)
        {
            currentBackgroundIndex =
                backgroundSprites.Length - 1;
        }

        UpdateBackgroundDisplay();
    }


void UpdateBackgroundDisplay()
{
    int highScore =
        PlayerPrefs.GetInt("HighScore", 0);

    int hardHighScore =
        PlayerPrefs.GetInt("HardHighScore", 0);

    int totalDeaths =
        PlayerPrefs.GetInt("TotalDeaths", 0);


    bool isDeathBackground =
        currentBackgroundIndex == deathUnlockedBackgroundIndex;

    bool isHardBackground =
        currentBackgroundIndex >= firstHardBackgroundIndex;

    bool isUnlocked;


    // =========================
    // UNLOCK KONTROLÜ
    // =========================

    if (isDeathBackground)
    {
        isUnlocked =
            totalDeaths >= requiredDeathsForBackground;
    }
    else if (isHardBackground)
    {
        int hardBackgroundIndex =
            currentBackgroundIndex - firstHardBackgroundIndex;

        // İlk Hard background:
        // Hard Mode açıldığı anda unlock olur.
        if (hardBackgroundIndex == 0)
        {
            isUnlocked =
                PlayerPrefs.GetInt("HardModeUnlocked", 0) == 1;
        }
        else
        {
            // Diğer Hard backgroundlar:
            // Hard High Score ile açılır.
            isUnlocked =
                hardHighScore >=
                hardBackgroundRequiredScore[hardBackgroundIndex];
        }
    }
    else
    {
        // Normal backgroundlar:
        // Normal High Score ile açılır.
        isUnlocked =
            highScore >=
            backgroundRequiredScores[currentBackgroundIndex];
    }


    // =========================
    // BACKGROUND GÖRSELİ
    // =========================

    if (!isUnlocked && currentBackgroundIndex >= 1)
    {
        BackgroundImage.sprite =
            lockedBackgroundSprites[0];
    }
    else
    {
        BackgroundImage.sprite =
            backgroundSprites[currentBackgroundIndex];
    }


    // =========================
    // İSİM VE KİLİT YAZISI
    // =========================

    if (isUnlocked)
    {
        BackgroundName.text =
            backgroundNames[currentBackgroundIndex]
            + "\nUNLOCKED";
    }
    else
    {
        // 200 Death background
        if (isDeathBackground)
        {
            BackgroundName.text =
                backgroundNames[currentBackgroundIndex]
                + "\nLOCKED - Die\n "
                + requiredDeathsForBackground
                + " Times";
        }

        // Hard Mode background
        else if (isHardBackground)
        {
            int hardBackgroundIndex =
                currentBackgroundIndex - firstHardBackgroundIndex;

            // Default Hard background
            if (hardBackgroundIndex == 0)
            {
                BackgroundName.text =
                    backgroundNames[currentBackgroundIndex]
                    + "\nLOCKED\nUnlock Hard Mode";
            }

            // Puanla açılan Hard backgroundlar
            else
            {
                BackgroundName.text =
                    backgroundNames[currentBackgroundIndex]
                    + "\nLOCKED - Reach\n "
                    + hardBackgroundRequiredScore[hardBackgroundIndex]
                    + " in Hard Mode";
            }
        }

        // Normal background
        else
        {
            BackgroundName.text =
                backgroundNames[currentBackgroundIndex]
                + "\nLOCKED - Reach\n "
                + backgroundRequiredScores[currentBackgroundIndex]
                + " High Score";
        }
    }

    if (isUnlocked && IsBackgroundNew(currentBackgroundIndex))
        {
            newBackgroundSelectionText.SetActive(true);
        }
    else
        {
            newBackgroundSelectionText.SetActive(false);
        }

    // =========================
    // SELECTED FRAME
    // =========================

    int selectedBackground =
        PlayerPrefs.GetInt("SelectedBackground", 0);

    if (currentBackgroundIndex == selectedBackground)
    {
        SelectedBackgroundFrame.SetActive(true);
    }
    else
    {
        SelectedBackgroundFrame.SetActive(false);
    }
}


public void SelectBackground()
{
    int highScore =
        PlayerPrefs.GetInt("HighScore", 0);

    int hardHighScore = 
        PlayerPrefs.GetInt("HardHighScore",0);

    int totalDeaths =
        PlayerPrefs.GetInt("TotalDeaths", 0);

    bool isDeathBackground =
        currentBackgroundIndex == deathUnlockedBackgroundIndex;

    bool isHardBackground =
        currentBackgroundIndex >= firstHardBackgroundIndex;

    bool isUnlocked;

    if (isDeathBackground)
    {
        isUnlocked =
            totalDeaths >= requiredDeathsForBackground;
    }
    else if (isHardBackground)
        {
            int hardBackgroundIndex = currentBackgroundIndex - firstHardBackgroundIndex;

            if (hardBackgroundIndex == 0)
            {
                isUnlocked = PlayerPrefs.GetInt("HardModeUnlocked",0) == 1;
            }
            else
            {
                isUnlocked = hardHighScore >= hardBackgroundRequiredScore[hardBackgroundIndex];
            }

            
        }
    else
    {
        isUnlocked =
            highScore >= backgroundRequiredScores[currentBackgroundIndex];
    }


    if (isUnlocked)
    {
        MarkCurrentBackgroundAsSeen();

        PlayerPrefs.SetInt(
            "SelectedBackground",
            currentBackgroundIndex
        );

        PlayerPrefs.Save();


        SelectedBackgroundImage.sprite =
            backgroundSprites[currentBackgroundIndex];


        MusicManager musicManager =
            FindFirstObjectByType<MusicManager>();

        if (musicManager != null)
        {
            musicManager.UpdateMusic();
        }


        BackgroundSelectionPanel.SetActive(false);
    }
}

bool IsBirdNew(int birdIndex)
    {
        if (birdIndex == 0)
        {
            return false;
        }
        string key = "BirdSeen_" + birdIndex;

        if (!PlayerPrefs.HasKey(key))
        {
            return false;
        }

        return PlayerPrefs.GetInt(key) == 0;
    }

bool IsBackgroundNew(int backgroundIndex)
    {
        if (backgroundIndex == 0)
        {
            return false;
        }
        string key = "BackgroundSeen_" + backgroundIndex;

        if (!PlayerPrefs.HasKey(key))
        {
            return false;
        }
        return PlayerPrefs.GetInt(key) == 0;
    }

void MarkCurrentBirdAsSeen()
    {
        if (currentBirdIndex == 0)
        {
            return;
        }

        PlayerPrefs.SetInt(
            "BirdSeen_" + currentBirdIndex,
            1
        );
        PlayerPrefs.Save();
    }
void MarkCurrentBackgroundAsSeen()
    {
        if (currentBackgroundIndex == 0)
        {
            return;
        }
        string key = "BackgroundSeen_" + currentBackgroundIndex;

        if (!PlayerPrefs.HasKey(key))
        {
            return;
        }
        PlayerPrefs.SetInt(key,1);
        PlayerPrefs.Save();
    }


void UpdateAchievementStars()
{
    int normalHighScore =
        PlayerPrefs.GetInt("HighScore", 0);

    int hardHighScore =
        PlayerPrefs.GetInt("HardHighScore", 0);

    bool normalCompleted =
        normalHighScore >= 120;

    bool hardCompleted =
        hardHighScore >= 100;

    int starCount = 0;

    if (normalCompleted || hardCompleted)
    {
        starCount = 1;
    }

    if (normalCompleted && hardCompleted)
    {
        starCount = 2;
    }

    if (normalCompleted &&
        hardCompleted &&
        hardHighScore >= 150)
    {
        starCount = 3;
    }

    for (int i = 0; i < unlockedStars.Length; i++)
    {
        bool earned = i < starCount;

        unlockedStars[i].SetActive(earned);
    }
    //YILDIZ 1
    if (starCount >= 1 && PlayerPrefs.GetInt("AchievementStar1Shown",0) == 0)
        {
            star1Animator.Play("StarEarnAnimation",0,0f);

            PlayerPrefs.SetInt("AchievementStar1Shown",1);
            PlayerPrefs.Save();
        }
    //YILDIZ 2
    if (starCount >= 2 && PlayerPrefs.GetInt("AchievementStar2Shown", 0) == 0)
    {
        star2Animator.Play("StarEarnAnimation", 0, 0f);

        PlayerPrefs.SetInt("AchievementStar2Shown", 1);
        PlayerPrefs.Save();
    } 
    //YILDIZ 3
    if (starCount >= 3 && PlayerPrefs.GetInt("AchievementStar3Shown",0) == 0)
        {
            star3Animator.Play("StarEarnAnimation",0,0f);

            PlayerPrefs.SetInt("AchievementStar3Shown",1);
            PlayerPrefs.Save();
        }
}
void CheckFinalBirdUnlock()
{
    
        int normalHighScore = PlayerPrefs.GetInt("HighScore",0);
        int hardHighScore = PlayerPrefs.GetInt("HardHighScore",0);

        bool finalBirdUnlock = 
            normalHighScore >= 120 && hardHighScore >= 150; 
        
        if (finalBirdUnlock && 
            PlayerPrefs.GetInt("FinalBirdUnlocked",0) == 0)
        {
            PlayerPrefs.SetInt("FinalBirdUnlocked",1);

            PlayerPrefs.SetInt("BirdSeen_" + finalBirdIndex,0 );

            PlayerPrefs.SetInt("NewBirdAvailable",1);

            PlayerPrefs.Save();

            changeBirdAnimation.enabled = true;
            newBirdIndicator.SetActive(true);
        }
}


public void OpenStar1Info()
    {
        starInfoPanel.SetActive(true);
        UpdateStarPlayButtons();
        starInfoTitle.text = "STAR 1";
        
        starInfoDescription.text = 
        "Reach 120 High Score in Normal Mode\n" + 
        "OR\n" +
        "Reach 100 High Score in Hard Mode";

        int normalHighScore = PlayerPrefs.GetInt("HighScore",0);
        int hardHighScore = PlayerPrefs.GetInt("HardHighScore",0);
        
        bool completed = normalHighScore >= 120 || hardHighScore >= 100;
        starCompletedText.SetActive(completed);
    }

public void OpenStar2Info()
    {
        starInfoPanel.SetActive(true);
        UpdateStarPlayButtons();
        starInfoTitle.text = "STAR 2";
        
        starInfoDescription.text = 
        "Reach 120 High Score in Normal Mode\n" + 
        "AND\n" +
        "Reach 100 High Score in Hard Mode";

        int normalHighScore = PlayerPrefs.GetInt("HighScore",0);
        int hardHighScore = PlayerPrefs.GetInt("HardHighScore",0);
        
        bool completed = normalHighScore >= 120 && hardHighScore >= 100;
        starCompletedText.SetActive(completed);

    }

public void OpenStar3Info()
    {
        starInfoPanel.SetActive(true);
        UpdateStarPlayButtons();
        starInfoTitle.text = "STAR 3";
        
        starInfoDescription.text = 
        "Earn other stars\n" + 
        "AND\n" +
        "Reach 150 High Score in Hard Mode";
    
        int normalHighScore = PlayerPrefs.GetInt("HighScore",0);
        int hardHighScore = PlayerPrefs.GetInt("HardHighScore",0);
        
        bool completed = normalHighScore >= 120 && hardHighScore >= 150;
        starCompletedText.SetActive(completed);
    }

public void CloseStarInfo()
    {
        starInfoPanel.SetActive(false);
    }

public void PlayNormalFromStarPanel()
    {
        PlayerPrefs.SetInt("HardMode",0);
        PlayerPrefs.Save();

        SceneManager.LoadScene("Game");
    }
public void PlayHardFromStarPanel()
    {
        int highScore = PlayerPrefs.GetInt("HighScore",0);

        if (highScore < 70)
        {
            return;
        }

        PlayerPrefs.SetInt("HardModeUnlocked",1);
        PlayerPrefs.SetInt("HardMode",1);
        PlayerPrefs.Save();

        SceneManager.LoadScene("Game");
    }

void UpdateStarPlayButtons()
    {
        int highScore = PlayerPrefs.GetInt("HighScore",0);

        playNormalButton.interactable = true;
        playNormalButtonText.text = "PLAY NORMAL MODE";

        if (highScore >= 70)
        {
            playHardButton.interactable = true;
            playHardButtonText.text = "PLAY HARD MODE";
            hardModeRequirementText.SetActive(false);
        }
        else
        {
            playHardButton.interactable = false;
            playHardButtonText.text = "HARD MODE LOCKED";
            hardModeRequirementText.SetActive(true);
        }
    }


public void DebugSetNormalHighScore()
    {
        PlayerPrefs.SetInt("HighScore", debugNormalHighScore);
        PlayerPrefs.Save();

        HighScoreText.text = "High Score: " + debugNormalHighScore;

        Debug.Log("Normal High Score set to: "+ debugNormalHighScore);
    }
public void DebugSetHardHighScore()
    {
        PlayerPrefs.SetInt("HardHighScore", debugHardHighScore);
        PlayerPrefs.Save();

        HardHighScoreText.text = "Hard Mode High Score: " + debugHardHighScore;

        Debug.Log("Hard High Score set to: "+ debugHardHighScore);
    }
public void DebugResetHardMode()
    {
        PlayerPrefs.SetInt("HardMode",0);

        PlayerPrefs.SetInt("HardModeUnlocked",0);

        PlayerPrefs.SetInt("EnteredHardModeBefore",0);

        PlayerPrefs.SetInt("HardHighScore",0);

        PlayerPrefs.Save();

        Debug.Log("Hard Mode debug data reset.");
    }

[ContextMenu("Reset Achievement Star Animations")]
public void ResetAchievementStarAnimations()
    {
        PlayerPrefs.DeleteKey("AchievementStar1Shown");
        PlayerPrefs.DeleteKey("AchievementStar2Shown");
        PlayerPrefs.DeleteKey("AchievementStar3Shown");

        PlayerPrefs.Save();

        Debug.Log("Achievement star animations reset!");
    }
    
[ContextMenu("Final Bird Reset")]
public void FinalBirdReset()
    {
        PlayerPrefs.DeleteKey("FinalBirdUnlocked");
        PlayerPrefs.DeleteKey("BirdSeen_13");
        PlayerPrefs.DeleteKey("NewBirdAvailable");

        PlayerPrefs.Save();

        Debug.Log("Final Bird unlock reset!");
    }
}