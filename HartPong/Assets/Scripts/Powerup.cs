using UnityEngine;
using System.Collections;

public class Powerup : MonoBehaviour
{
    public enum PowerUpType { IncreaseBallSpeed, IncreasePaddleSize}
    public PowerUpType type;
    //Power up Settings
    public float effectAmount = 1.2f; //speed multiplier
    public float effectDuration = 5f; //to set duration
    public float speed = 15f;
    private Vector3 direction;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        float randomZ = Random.Range(-0.5f, 0.5f);
        float randomX = Random.value < 0.5f ? 1f : -1f;
        direction = new Vector3(randomX, 0f, randomZ).normalized;
    }
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Wall"))
        {
            ContactPoint contact = collision.contacts[0];
            direction = Vector3.Reflect(direction, contact.normal).normalized;
        }
        else if(collision.gameObject.CompareTag("Player"))
        {
            Collider col = GetComponent<Collider>();
            if(col != null)
            {
                col.enabled = false;
            }
            Renderer rend = GetComponent<Renderer>();
            if(rend != null)
            {
                rend.enabled = false;
            }
            if(type == PowerUpType.IncreaseBallSpeed)
            {
                    Ball ballController = FindAnyObjectByType<Ball>();
                    if(ballController != null)
                    {
                        StartCoroutine(ApplyPowerIncrease(ballController));
                    }
            } 
            else if(type == PowerUpType.IncreasePaddleSize)
            {
                StartCoroutine(ApplyPaddleSizeIncrease(collision.gameObject));
            }
        }
    }
    IEnumerator ApplyPaddleSizeIncrease(GameObject paddle)
    {
        Vector3 originalScale = paddle.transform.localScale;
        Vector3 newScale = originalScale;
        newScale.x *= effectAmount;
        paddle.transform.localScale = newScale;
        yield return new WaitForSeconds(effectDuration);
        paddle.transform.localScale = originalScale;
        Destroy(gameObject);
        Debug.Log("Paddle Size Increased");
    }
    IEnumerator ApplyPowerIncrease(Ball ball)
    {
        float normalPower = ball.paddleSpeedIncrease;
        ball.paddleSpeedIncrease *= effectAmount;
        Debug.Log($"Power Increased: {ball.paddleSpeedIncrease}");
        yield return new WaitForSeconds(effectDuration);
        ball.paddleSpeedIncrease = normalPower;
        Debug.Log($"Power decreased: {ball.paddleSpeedIncrease}");
        Destroy(gameObject);
    }
}
