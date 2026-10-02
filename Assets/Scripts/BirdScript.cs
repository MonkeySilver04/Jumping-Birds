using System.ComponentModel;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

//#2E2E2E

public class BirdScript : MonoBehaviour
{
    public Rigidbody2D myRigidBody;
    public float flapStrength;
    public LogicScript logic;
    public bool birdIsAlive = true;
    public AudioSource flapSound;
    public AudioSource deathSound;
    public GameObject JumpInstruction;
    public bool gameStarted = false;
    public float idleMoveAmount = 1f;
    public float idleMoveSpeed = 5f;
    private Vector3 idleStartPosition;

    public GameObject HardModeStartText;
    public GameObject HardModeIndicator;

    public GameObject HardModeVignette;



    private Camera mainCamera;

    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();

        mainCamera = Camera.main;

        myRigidBody.gravityScale = 0;

        idleStartPosition = transform.position;
        myRigidBody.gravityScale = 0;

        if (PlayerPrefs.GetInt("HardMode",0) == 1)
        {
            flapStrength *= 1.06f;

            HardModeStartText.SetActive(true);
            HardModeIndicator.SetActive(true);
            HardModeVignette.SetActive(true);
        }
        else
        {
            HardModeStartText.SetActive(false);
            HardModeIndicator.SetActive(false);
            HardModeVignette.SetActive(false);
        }
    }

void Update()
{
    if (!gameStarted)
        {
            float newY = idleStartPosition.y + Mathf.Sin(Time.time * idleMoveSpeed) * idleMoveAmount;
            transform.position = new Vector3(idleStartPosition.x,newY,idleStartPosition.z);
        }

    if (Input.GetKeyDown(KeyCode.Space) && birdIsAlive)
    {
        if (!gameStarted)
        {
            gameStarted = true;

            myRigidBody.gravityScale = 4.5f;

            JumpInstruction.SetActive(false);
            HardModeStartText.SetActive(false);
        }

        myRigidBody.linearVelocity =
            Vector2.up * flapStrength;

        flapSound.Play();
    }


    if (!gameStarted)
    {
        return;
    }


    Vector3 screenPosition =
        mainCamera.WorldToViewportPoint(transform.position);

    if (screenPosition.y > 1f ||
        screenPosition.y < 0f)
    {
        Die();
    }
}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Die();
    }

    void Die()
    {
        if (birdIsAlive == false)
        {
            return;
        }

        birdIsAlive = false;

        if (deathSound != null)
        {
            deathSound.Play();
        }

        int totalDeaths =
            PlayerPrefs.GetInt("TotalDeaths", 0);

        totalDeaths++;

        PlayerPrefs.SetInt(
            "TotalDeaths",
            totalDeaths
        );

        // HARD MODE DEATH STREAK
        if (PlayerPrefs.GetInt("HardMode", 0) == 1)
        {
            int hardDeathStreak =
                PlayerPrefs.GetInt("HardDeathStreak", 0);

            hardDeathStreak++;

            PlayerPrefs.SetInt(
                "HardDeathStreak",
                hardDeathStreak
            );
        }

        // 200 death unlock
        if (totalDeaths == 200)
        {
            PlayerPrefs.SetInt("NewBirdAvailable", 1);
            PlayerPrefs.SetInt("NewBackgroundAvailable", 1);

            PlayerPrefs.SetInt("BirdSeen_8", 0);
            PlayerPrefs.SetInt("BackgroundSeen_6", 0);
        }

        PlayerPrefs.Save();

        // En son Game Over ekranını aç
        logic.gameOver();

        
    }
}