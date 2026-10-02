using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;

    public bool isMusicMuted = false;

    public AudioClip[] backgroundMusic;

    private AudioSource audioSource;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        isMusicMuted = PlayerPrefs.GetInt("MusicMuted",0) == 1;

        UpdateMusic();

        audioSource.mute = isMusicMuted;
    }

    public void ToggleMusic()
    {
        isMusicMuted = !isMusicMuted;

        audioSource.mute = isMusicMuted;

        PlayerPrefs.SetInt(
            "MusicMuted",isMusicMuted ? 1 :0
        );

        PlayerPrefs.Save();
    }

    public void UpdateMusic()
    {
        int selectedBackground =
            PlayerPrefs.GetInt("SelectedBackground", 0);

        if (selectedBackground < 0 ||
            selectedBackground >= backgroundMusic.Length)
        {
            selectedBackground = 0;
        }

        audioSource.clip = backgroundMusic[selectedBackground];
        audioSource.Play();
    }
}