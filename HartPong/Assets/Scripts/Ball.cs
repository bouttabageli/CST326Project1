using UnityEngine;

public class Ball : MonoBehaviour
{
    //speed settings
    public float speed = 10f;
    public float paddleSpeedIncrease = 0.5f;
    //movement variables
    private Vector3 direction;
    private Rigidbody rb;
    //sound settings
    public AudioClip paddleHitSound;
    private AudioSource audioSource;
    //used to adjust pitch relative to starting speed
    private float baseSpeed;
    private float basePower;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        baseSpeed = speed;
        basePower = paddleSpeedIncrease;
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
            float pitch = 1f + ((speed - baseSpeed)/baseSpeed) * 0.5f;
            pitch = Mathf.Clamp(pitch, 0.8f, 1.5f);
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(paddleHitSound);
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
    public float getBasePower()
    {
        return basePower;
    }
}
