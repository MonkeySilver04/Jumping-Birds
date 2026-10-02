using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class UnlockNotification : MonoBehaviour
{
    public TextMeshProUGUI notificationText;

    private Coroutine notificationCoroutine;

    private Queue<string> notificationQueue = new Queue<string>();

public void CheckUnlocks(int currentScore)
{
    // =========================
    // NORMAL UNLOCKS
    // Normal ve Hard Mode'da açılabilir
    // =========================

    if (currentScore == 15)
    {
        UnlockBird(
            "BirdUnlocked_0",
            "COOL BIRD",1
        );
    }

    if (currentScore == 30)
    {
        UnlockBird(
            "BirdUnlocked_1",
            "CAP BIRD",2
        );

        UnlockBackground(
            "BackgroundUnlocked_0",
            "Sunset",1
        );
    }

    if (currentScore == 40)
    {
        UnlockBird(
            "BirdUnlocked_2",
            "GHOST BIRD",3
        );
    }

    if (currentScore == 60)
    {
        UnlockBird(
            "BirdUnlocked_3",
            "MS.BIRD",4
        );

        UnlockBackground(
            "BackgroundUnlocked_1",
            "Japan",2
        );
    }

    if (currentScore == 70)
    {
        UnlockBackground(
            "BackgroundUnlocked_2",
            "Retrowave",3
        );
    }

    if (currentScore == 80)
    {
        UnlockBird(
            "BirdUnlocked_4",
            "ASTRONAUT BIRD",5
        );

        UnlockBackground(
            "BackgroundUnlocked_3",
            "Space",4
        );
    }

    if (currentScore == 100)
    {
        UnlockBird(
            "BirdUnlocked_5",
            "CYBORG BIRD",6
        );

        UnlockBackground(
            "BackgroundUnlocked_4",
            "Cyberpunk",5
        );
    }

    if (currentScore == 120)
    {
        UnlockBird(
            "BirdUnlocked_6",
            "OG BIRD",7
        );
    }


    // =========================
    // HARD MODE UNLOCKS
    // =========================

    bool isHardMode =
        PlayerPrefs.GetInt("HardMode", 0) == 1;

    // Normal Mode ise burada bitir.
    if (!isHardMode)
    {
        return;
    }


    // 5 → Hard Bird 1
    if (currentScore == 5)
    {
        UnlockBird(
            "HardBirdUnlocked_0",
            "ASSASIN BIRD",9
        );
    }


    // 20 → Hard Bird 2 + Background
    if (currentScore == 20)
    {
        UnlockBird(
            "HardBirdUnlocked_1",
            "COWBOY BIRD",10
        );

        UnlockBackground(
            "HardBackgroundUnlocked_1",
            "WESTERN",8
        );
    }


    // 50 → Hard Bird 3
    if (currentScore == 50)
    {
        UnlockBird(
            "HardBirdUnlocked_2",
            "SKELETON BIRD",11
        );
    }


    // 60 → Hard Background
    if (currentScore == 60)
    {
        UnlockBackground(
            "HardBackgroundUnlocked_2",
            "DRAGON PEACKS",9
        );
    }


    // 70 → Hard Bird 4 + Background
    if (currentScore == 70)
    {
        UnlockBird(
            "HardBirdUnlocked_3",
            "PIRATE BIRD",12
        );

        UnlockBackground(
            "HardBackgroundUnlocked_3",
            "PIRATE DECK",10
        );
    }


    // 100 → Final Hard Background
    if (currentScore == 100)
    {
        UnlockBackground(
            "HardBackgroundUnlocked_4",
            "VOID",11
        );
    }
}

    void UnlockBird(
        string unlockKey,
        string birdName,
        int birdIndex)
    {
        if (PlayerPrefs.GetInt(unlockKey, 0) == 1)
        {
            return;
        }

        PlayerPrefs.SetInt(unlockKey, 1);

        // Bu kuş yeni açıldı.
        PlayerPrefs.SetInt(
            "BirdSeen_" + birdIndex,
            0
        );

        PlayerPrefs.SetInt("NewBirdAvailable", 1);
        PlayerPrefs.SetInt("NewUnlockGameOver",1);

        PlayerPrefs.Save();

        notificationQueue.Enqueue(
            "NEW BIRD UNLOCKED!\n" + birdName
        );

        ShowNextNotification();
    }

    void UnlockBackground(
        string unlockKey,
        string backgroundName,
        int backgroundIndex)
    {
        if (PlayerPrefs.GetInt(unlockKey, 0) == 1)
        {
            return;
        }

        PlayerPrefs.SetInt(unlockKey, 1);

        // Bu background yeni açıldı.
        PlayerPrefs.SetInt(
            "BackgroundSeen_" + backgroundIndex,
            0
        );

        PlayerPrefs.SetInt("NewBackgroundAvailable",1);
        PlayerPrefs.SetInt("NewUnlockGameOver",1);

        PlayerPrefs.Save();

        notificationQueue.Enqueue(
            "NEW BACKGROUND UNLOCKED!\n"
            + backgroundName
        );

        ShowNextNotification();
    }

    void ShowNextNotification()
    {
        if (notificationCoroutine != null)
        {
            return;
        }

        if (notificationQueue.Count == 0)
        {
            return;
        }

        string message = notificationQueue.Dequeue();

        notificationText.text = message;

        gameObject.SetActive(true);

        notificationCoroutine =
            StartCoroutine(HideNotification());
    }

    IEnumerator HideNotification()
    {
        yield return new WaitForSeconds(3f);

        gameObject.SetActive(false);

        notificationCoroutine = null;

        ShowNextNotification();
    }


    [ContextMenu("Reset Bird Unlocks")]
    public void ResetBirdUnlocks()
    {
        PlayerPrefs.DeleteKey("BirdUnlocked_0");
        PlayerPrefs.DeleteKey("BirdUnlocked_1");
        PlayerPrefs.DeleteKey("BirdUnlocked_2");
        PlayerPrefs.DeleteKey("BirdUnlocked_3");
        PlayerPrefs.DeleteKey("BirdUnlocked_4");
        PlayerPrefs.DeleteKey("BirdUnlocked_5");
        PlayerPrefs.DeleteKey("BirdUnlocked_6");

        PlayerPrefs.DeleteKey("HardBirdUnlocked_0");
        PlayerPrefs.DeleteKey("HardBirdUnlocked_1");
        PlayerPrefs.DeleteKey("HardBirdUnlocked_2");
        PlayerPrefs.DeleteKey("HardBirdUnlocked_3");

        PlayerPrefs.DeleteKey("NewBirdAvailable");

        PlayerPrefs.Save();

        Debug.Log("Bird unlocks reset!");
    }


    [ContextMenu("Reset Background Unlocks")]
    public void ResetBackgroundUnlocks()
    {
        PlayerPrefs.DeleteKey("BackgroundUnlocked_0");
        PlayerPrefs.DeleteKey("BackgroundUnlocked_1");
        PlayerPrefs.DeleteKey("BackgroundUnlocked_2");
        PlayerPrefs.DeleteKey("BackgroundUnlocked_3");
        PlayerPrefs.DeleteKey("BackgroundUnlocked_4");

        PlayerPrefs.DeleteKey("HardBackgroundUnlocked_1");
        PlayerPrefs.DeleteKey("HardBackgroundUnlocked_2");
        PlayerPrefs.DeleteKey("HardBackgroundUnlocked_3");
        PlayerPrefs.DeleteKey("HardBackgroundUnlocked_4");

        PlayerPrefs.DeleteKey("NewBackgroundAvailable");

        PlayerPrefs.Save();

        Debug.Log("Background unlocks reset!");
    }
}