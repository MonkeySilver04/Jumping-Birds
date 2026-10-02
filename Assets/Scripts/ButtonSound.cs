using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public AudioSource buttonSound;

    public void PlayButtonSound()
    {
        if (buttonSound != null && buttonSound.clip != null)
        {
            AudioSource.PlayClipAtPoint(
                buttonSound.clip,
                Camera.main.transform.position,
                buttonSound.volume
            );
        }
    }
}