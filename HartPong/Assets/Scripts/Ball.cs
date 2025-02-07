using UnityEngine;

public class Ball : MonoBehaviour
{
    public float speed = 10f;
    public float paddleSpeedIncrease = 0.5f;
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
        rb.MovePosition(rb.position + direction * speed  * Time.fixedDeltaTime);
    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Wall"))
        {
            //Debug.Log("Hit Wall");
            ContactPoint contact = collision.contacts[0];
            direction = Vector3.Reflect(direction, contact.normal); 
        }
        else if(collision.gameObject.CompareTag("Player"))
        {
            speed += paddleSpeedIncrease;
            ContactPoint contact = collision.contacts[0];
            Transform paddleTransform = collision.transform;
            Collider paddleCollider = collision.collider;
            float hitPointZ = contact.point.z;
            float paddleCenterZ = paddleTransform.position.z;
            float offset = hitPointZ - paddleCenterZ;
            float paddleHeight = paddleCollider.bounds.size.z;
            float normalizedOffset = offset / (paddleHeight / 2f);
            normalizedOffset = Mathf.Clamp(normalizedOffset, -1f, 1f);
            direction.x = -direction.x;
            direction.z = normalizedOffset;
            direction = direction.normalized;
        }
    }
    public void ResetBall(Vector3 newDirection, float newSpeed)
    {
        transform.position = Vector3.zero;
        speed = newSpeed;
        direction = newDirection.normalized;
    }
}
