using UnityEngine;
using UnityEngine.InputSystem;
public class Playable : MonoBehaviour
{
    [SerializeField] private float currentSpeed = 0.0f;
    public float acceleration = 1.0f;
    public float deceleration = 1.0f;
    public float maxSpeed = 5.0f;
    Vector3 moveDirection = Vector3.zero;
    public InputAction MoveLeft;
    public InputAction MoveRight;


    private void OnEnable()
    {

        MoveLeft.performed += ctx =>
        {
            moveDirection += Vector3.left;
        };
        MoveRight.performed += ctx =>
        {
            moveDirection += Vector3.right;
        };

        MoveLeft.canceled += ctx =>
        {
            moveDirection -= Vector3.left;
        };
        MoveRight.canceled += ctx =>
        {
            moveDirection -= Vector3.right;
        };

        MoveLeft.Enable();
        MoveRight.Enable();
    }

    private void OnDisable()
    {
        MoveLeft.Disable();
        MoveRight.Disable();
    }
    void Update()
    {
        currentSpeed = Movement.Move(this.transform, moveDirection, currentSpeed, maxSpeed, acceleration, deceleration);
    }


}

