using System; 
using UnityEngine;
using UnityEngine.InputSystem;

public class DemoPaddle : MonoBehaviour
{

    public float maxPaddleSpeed = 1f;
    public float paddleForce = 1f;

    public bool isLeft;
    public bool isRight;

    private Vector2 movementInput;

    private Rigidbody rb;
    private float movementX;
    private float movementY;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();  
        BoxCollider c = GetComponent<BoxCollider>();
        float max = c.bounds.max.z;
        float min = c.bounds.min.z;
        Debug.Log($"Max: {max}, Min: {min}");
    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>(); 
        movementX = movementInput.x; 
        movementY = movementInput.y; 
    }

    // void FixedUpdate()
    // {

    //     Vector3 movement = new Vector3 (movementX, 0.0f, movementY);
    //     rb.AddForce(movement * maxPaddleSpeed); 
 
    // }

    // Update is called once per frame
    void FixedUpdate()
    {
        float movementAxis = 0f;

        if(isLeft){
            movementAxis = Input.GetAxis("LeftPaddle");
        }
        else if(isRight){
            movementAxis = Input.GetAxis("RightPaddle");
        }

        Vector3 force = new Vector3(0f, 0f, 1f) * movementAxis * paddleForce;

        Transform paddleTransform = GetComponent<Transform>();

        Vector3 newPosition =  paddleTransform.position + new Vector3(0f, 0f, movementAxis * maxPaddleSpeed * Time.deltaTime);
        //newPosition.z = Math.Clamp(newPosition.z, -2.5f, 2.5f);

        paddleTransform.position = newPosition;
        
    }
}
