using UnityEngine;

public class GameBackground : MonoBehaviour
{
    public Sprite[] backgroundSprites;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        int selectedBackground = PlayerPrefs.GetInt("SelectedBackground",0);

        spriteRenderer.sprite = backgroundSprites[selectedBackground];
    }
}