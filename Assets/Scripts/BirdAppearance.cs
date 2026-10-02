using UnityEngine;

public class BirdAppearance : MonoBehaviour
{
    public Sprite[] birdSprites;

    private SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        int SelectedBird = PlayerPrefs.GetInt("SelectedBird",0);

        spriteRenderer.sprite = birdSprites[SelectedBird];
    }
}
