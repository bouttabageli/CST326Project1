using UnityEngine;
using UnityEngine.InputSystem;

public class PaddleController : MonoBehaviour
{
    public float speed = 5f;
    public float boundary = 4.5f;
    private InputAction moveAction;
    private Rigidbody rb;
    public void Initialize(InputAction action){
        moveAction = action;
        moveAction.Enable();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if(rb == null){
            rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
        }
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotationY;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(moveAction == null) return;
        Vector2 movementValue = moveAction.ReadValue<Vector2>();
        Vector3 movement = new Vector3(0, 0, movementValue.x);
        rb.linearVelocity = movement * speed;
        Vector3 clampedPosition = transform.position;
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, -boundary, boundary);
        transform.position = clampedPosition;
    }
    void OnDestroy(){
        if(moveAction != null){
            moveAction.Disable();
        }
    }
}
