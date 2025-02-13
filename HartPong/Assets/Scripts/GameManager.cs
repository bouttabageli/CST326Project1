using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

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
    public GameObject[] powerUpPrefabs;
    public float minSpawnTime = 5f;
    public float maxSpawnTime = 15f;
    public Vector2 spawnXRange = new Vector2(-8f, 8f);
    public Vector2 spawnZRange = new Vector2(-4f, 4f);

    void Start()
    {
        StartCoroutine(SpawnPowerUps());
    }

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
        {
            float t = Mathf.Clamp01((float)leftPlayerScore/10f);
            leftScoreText.color = Color.Lerp(Color.white, Color.red, t);
            leftScoreText.text = leftPlayerScore.ToString();
        }
        if (rightScoreText != null)
        {
            float t = Mathf.Clamp01((float)rightPlayerScore/10f);
            rightScoreText.color = Color.Lerp(Color.white, Color.red, t);
            rightScoreText.text = rightPlayerScore.ToString();
        }
    }

    public void ResetBall()
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
    IEnumerator SpawnPowerUps()
    {
        while(true)
        {
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);
            if(powerUpPrefabs.Length > 0)
            {
                GameObject prefab = powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)];
                float randomX = Random.Range(spawnXRange.x, spawnXRange.y);
                float randomZ = Random.Range(spawnZRange.x, spawnZRange.y);
                Vector3 spawnPosition = new Vector3(randomX, 0f, randomZ);
                Instantiate(prefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}
