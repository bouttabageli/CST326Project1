using UnityEngine;

public class OOBTrigger : MonoBehaviour
{
    private GameManager gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        if(gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene!");
        }
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ball"))
        {
            gameManager.ResetBall();
        } 
        else if(other.CompareTag("PowerUp"))
        {
            Destroy(other.gameObject);
        }
    }
}
