using System; 
using UnityEngine;
using UnityEngine.InputSystem;

public class DemoPaddle : MonoBehaviour
{

    public float maxPaddleSpeed = 1f;
    public float paddleForce = 1f;

    public bool isLeft;
    public bool isRight;

    public AudioClip paddleHitClip;
    public float speedDivisor = 10f;

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

    void OnCollisionEnter(Collision collision){

        if(collision.gameObject.CompareTag("Ball")){


            BoxCollider paddleCollider = GetComponent<BoxCollider>();
            float paddleHalf = paddleCollider.bounds.size.z/2;
            float hitFactor = (collision.transform.position.z - transform.position.z)/paddleHalf;

            Rigidbody ballRb = collision.gameObject.GetComponent<Rigidbody>();
            float ballSpeed = ballRb ? ballRb.linearVelocity.magnitude : 0f;

            float speedFactor = ballSpeed/speedDivisor;

            float pitch = 1.0f + (Mathf.Abs(hitFactor) * 0.3f) + (speedFactor * 0.3f);

            if(paddleHitClip != null){
            AudioSource audioSrc = GetComponent<AudioSource>();
            audioSrc.clip = paddleHitClip;
            audioSrc.pitch = pitch;
            audioSrc.Play();
            }
        }

    }
}
