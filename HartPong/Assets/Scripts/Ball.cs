using UnityEngine;

public class Ball : MonoBehaviour
{
    public float initialSpeed = 5f;
    private Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Launch();
    }
    public void Launch()
    {
        float xDirection = Random.Range(-1f, 1f);
        while(xDirection > -.2f && xDirection < .2f)
        {
            xDirection = Random.Range(-1f, 1f);
        }
        float zDirection = Random.Range(-1f, 1f);
        Vector3 direction = new Vector3(xDirection, 0, zDirection).normalized;
        rb.linearVelocity = direction * initialSpeed;
    }
    void ResetBall()
    {
        transform.position = Vector3.zero;
        Launch();
    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            float relativeIntersectZ = (transform.position.z - collision.transform.position.z) / collision.collider.bounds.size.z;
            Debug.Log($"relativeIntersectZ: {relativeIntersectZ}");
            float maxReflectionAngle = 75f;
            float reflectionAngle = relativeIntersectZ * maxReflectionAngle;
            Debug.Log($"reflection Angle: {reflectionAngle}");
            Vector3 paddleForward = collision.transform.right;
            Vector3 reflectionNormal = Quaternion.AngleAxis(-reflectionAngle, Vector3.up) * paddleForward;
            Debug.Log($"reflection normal: {reflectionNormal}");
            float originalSpeed = previousVelocity.magnitude;
            Vector3 currentDirection = previousVelocity.normalized;
            Vector3 newDirection = Vector3.Reflect(currentDirection, reflectionNormal);
            Debug.Log($"newDirection: {newDirection}");
            rb.linearVelocity = newDirection.normalized * (originalSpeed * 2);
            rb.AddForce(newDirection.normalized * 0.5f, ForceMode.Impulse);
        }
        else if(collision.gameObject.CompareTag("Wall"))
        {
            Vector3 normal = collision.contacts[0].normal;
            Vector3 newDirection = Vector3.Reflect(previousVelocity.normalized, normal);
            float newSpeed = previousVelocity.magnitude;
            rb.linearVelocity = newDirection.normalized * newSpeed; 
            rb.AddForce(newDirection.normalized * 0.3f, ForceMode.Impulse);
        }
    }
    private Vector3 previousVelocity;
    void FixedUpdate()
    {
        previousVelocity = rb.linearVelocity;
        if(transform.position.x > 25 || transform.position.x < -25)
        {
            ResetBall();
        }
    }
}
