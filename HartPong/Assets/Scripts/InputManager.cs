using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public GameObject paddle1;
    public GameObject paddle2;
    public InputActionAsset InputAsset;
    private PaddleController paddle1Controller;
    private PaddleController paddle2Controller;
    private InputAction player1MoveAction;
    private InputAction player2MoveAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player1MoveAction = InputAsset.FindActionMap("PlayerControls").FindAction("Player1Move");
        player2MoveAction = InputAsset.FindActionMap("PlayerControls").FindAction("Player2Move");
        paddle1Controller = paddle1.GetComponent<PaddleController>();
        paddle2Controller = paddle2.GetComponent<PaddleController>();
        if(paddle1Controller != null){
            paddle1Controller.Initialize(player1MoveAction);
        }
        if(paddle2Controller != null){
            paddle2Controller.Initialize(player2MoveAction);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
