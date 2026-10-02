using UnityEngine;

public class PipeMoveScript : MonoBehaviour
{
    public float moveSpeed = 5;
    public float deadZone = -45;
    private BirdScript bird;

    void Start()
    {
        bird = FindFirstObjectByType<BirdScript>();

        if (PlayerPrefs.GetInt("HardMode",0) == 1)
        {
            moveSpeed *= 1.15f;
        }
    }

    // Update is called once per frame
void Update()
{
    if (bird == null || !bird.gameStarted)
    {
        return;
    }

    transform.position =
        transform.position +
        (Vector3.left * moveSpeed) * Time.deltaTime;

    if (transform.position.x < deadZone)
    {
        Debug.Log("Pipe deleted.");
        Destroy(gameObject);
    }
}
}
