using UnityEngine;

public class PipeSpawnScript : MonoBehaviour
{

    public GameObject pipe;
    public float spawnRate = 3;
    private float timer = 0;
    public float heightOffset = 7;
    public float minHeight = -5;
    public float maxHeight = 5;
    private BirdScript bird;
 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bird = FindFirstObjectByType<BirdScript>();

        if (PlayerPrefs.GetInt("HardMode",0) == 1)
        {
            spawnRate = 1.7f;
        }
        spawnPipe();
    }

    // Update is called once per frame
    void Update()
    {
        if (bird == null || !bird.gameStarted)
        {
            return;
        }

        if (timer < spawnRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            spawnPipe();
            timer = 0;
        }
        
    }

    void spawnPipe()
    {
        float randomY = Random.Range(minHeight,maxHeight);

        Instantiate(
            pipe,
            new Vector3(transform.position.x,randomY,0),
            transform.rotation
        );
    }
}
