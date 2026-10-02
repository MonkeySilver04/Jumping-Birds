using UnityEngine;

public class PipeAppearance : MonoBehaviour
{
    public Sprite[] pipeSprites;

    private SpriteRenderer topRenderer;
    private SpriteRenderer bottomRenderer;

    void Start()
    {
        topRenderer = transform.Find("Top Pipe").GetComponent<SpriteRenderer>();
        bottomRenderer = transform.Find("Bottom Pipe").GetComponent<SpriteRenderer>();

        int selectedBackground = PlayerPrefs.GetInt("SelectedBackground",0);

        topRenderer.sprite = pipeSprites[selectedBackground];

        bottomRenderer.sprite = pipeSprites[selectedBackground];

        if (PlayerPrefs.GetInt("HardMode",0) == 1)
        {
            Transform topPipe = transform.Find("Top Pipe");
            Transform bottomPipe = transform.Find("Bottom Pipe");

            topPipe.localPosition += Vector3.down * 0.2f;
            bottomPipe.localPosition += Vector3.up * 0.2f;
        }
    }
}