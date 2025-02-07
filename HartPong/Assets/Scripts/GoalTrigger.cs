using UnityEngine;
public class GoalTrigger : MonoBehaviour
{
    // Set this to true if this trigger is for the left goal.
    public bool isLeftGoal;

    private GameManager gameManager;

    void Start()
    {
        // Find the GameManager in the scene.
        gameManager = FindAnyObjectByType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene!");
        }
    }

    // When the ball enters the trigger, count it as a goal.
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            // If this is the left goal trigger, then the left player got scored on.
            gameManager.ScorePoint(isLeftGoal);
        }
    }
}
