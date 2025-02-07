using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Ball ball;
    // The initial speed of the ball when it is reset.
    public float initialBallSpeed = 10f;
    // Delay before resetting the ball after a goal.
    public float resetDelay = 1f;

    // Scores for each player.
    public int leftPlayerScore = 0;
    public int rightPlayerScore = 0;

    public TextMeshProUGUI leftScoreText;
    public TextMeshProUGUI rightScoreText;

    // This vector will store the direction the ball should be served.
    private Vector3 serveDirection;

    /// Call this when a goal is scored.
    /// Parameter: scoredOnLeft is true if the left goal trigger was hit,
    /// meaning the left player was scored on (so the right player earns a point).
    public void ScorePoint(bool scoredOnLeft)
    {
        if (scoredOnLeft)
        {
            // Left goal hit: left player got scored on, so right player earns a point.
            rightPlayerScore++;
            Debug.Log($"Player 2 Scored. Score: {leftPlayerScore}-{rightPlayerScore}");
            // Serve the ball toward the left (negative x) so it heads to the player who missed.
            serveDirection = new Vector3(1f, 0f, Random.Range(-0.5f, 0.5f)).normalized;
        }
        else
        {
            // Right goal hit: right player got scored on, so left player earns a point.
            leftPlayerScore++;
            Debug.Log($"Player 1 Scored. Score: {leftPlayerScore}-{rightPlayerScore}");
            // Serve the ball toward the right (positive x).
            serveDirection = new Vector3(-1f, 0f, Random.Range(-0.5f, 0.5f)).normalized;
        }
        if(CheckWin(leftPlayerScore, rightPlayerScore))
        {
            leftPlayerScore = 0;
            rightPlayerScore = 0;
        }
        UpdateScoreUI();

        // Reset the ball after a short delay.
        Invoke("ResetBall", resetDelay);
    }

    void UpdateScoreUI()
    {
        if (leftScoreText != null)
            leftScoreText.text = leftPlayerScore.ToString();
        if (rightScoreText != null)
            rightScoreText.text = rightPlayerScore.ToString();
    }

    void ResetBall()
    {
        ball.ResetBall(serveDirection, initialBallSpeed);
    }
    bool CheckWin(int leftPlayerScore, int rightPlayerScore)
    {
        if(leftPlayerScore >= 11)
        {
            Debug.Log("Game Over. Player 1 Wins");
            return true;
        }
        else if(rightPlayerScore >= 11){
            Debug.Log("Game Over. Player 2 Wins");
            return true;
        }
        else
        {
            return false;
        }
    }
}
