using UnityEngine;
using TMPro;

public class DemoBall : MonoBehaviour
{

    public float initialSpeed = 1f;
    public float maxSpeed = 20f;
    public float increment = 0.5f;
    public int yellowThreshold = 5;
    public int redThreshold = 9;
    

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI winnerText;
    public GameObject winnerTextObject;


    [HideInInspector]
    public float speedMultiplier = 1f;

    int leftScore = 0;
    int rightScore = 0;

    private Vector3 direction;
    private Rigidbody rb;
    private float currentSpeed;
    private string lastScored;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ResetBall();
        winnerTextObject.SetActive(false);
        scoreText.color = Color.green;
    }

    // Update is called once per frame
    void Update()
    {
        // Vector3 up = new Vector3(0f, 1f, 0f);
        // Quaternion posRotation = Quaternion.Euler(45f, 0f, 0f);
        // Quaternion negRotation = Quaternion.Euler(-45f, 0f, 0f);
        // Vector3 posVector = posRotation * up;
        // Vector3 negVector = negRotation * up;


        // Debug.DrawRay(transform.position, posVector * 2f, Color.red);
        // Debug.DrawRay(transform.position, negVector * 2f, Color.blue);
        Debug.DrawRay(transform.position, rb.linearVelocity.normalized * 2f, Color.green);

    }

    void ResetBall(){
        currentSpeed = initialSpeed;
        speedMultiplier = 1f;

        GameObject ground = GameObject.FindGameObjectWithTag("Ground"); 

        if (ground != null){
            // Get the center position of the ground
            Bounds groundBounds = ground.GetComponent<Collider>().bounds;
            Vector3 groundCenter = groundBounds.center;

            // Set ball position to ground center
            transform.position = new Vector3(groundCenter.x, transform.position.y, groundCenter.z);
        }

        Debug.Log(lastScored);

        // Determines who scored last
        if(string.IsNullOrEmpty(lastScored)){
            direction = (Random.value < 0.5f) ? Vector3.left : Vector3.right;
        }
        else{
            direction = (lastScored == "LeftPaddle") ? Vector3.left : Vector3.right;
        }

        // Add a little randomness
        direction += new Vector3(0f, 0f, Random.Range(-0.5f, 0.5f));
        direction.Normalize();

        rb.linearVelocity = direction * currentSpeed * speedMultiplier;

    }

    void OnCollisionEnter(Collision other){
        Debug.Log($"Made contact with {other.gameObject.name}");

        // Determines the Balls speed and trajectory
        if(other.gameObject.CompareTag("Paddle")){

            float hitFactor = (transform.position.z - other.transform.position.z)/other.collider.bounds.size.z;
            Vector3 newDirection;

            // Determines which paddle hit the ball and if it should go left or right
            if(other.gameObject.name.Contains("Left")){
                newDirection = new Vector3(1, 0, hitFactor).normalized;
            }
            else{
                newDirection = new Vector3(-1, 0, hitFactor).normalized;
            }

            currentSpeed = Mathf.Min(currentSpeed + increment, maxSpeed);
            rb.linearVelocity = newDirection * currentSpeed * speedMultiplier;
        }
        else if (other.gameObject.CompareTag("Walls")){
            //Reflects the ball off the wall
            Vector3 contactNormal = other.contacts[0].normal;
            Vector3 reflectedVelocity = Vector3.Reflect(rb.linearVelocity, contactNormal);

            rb.linearVelocity = reflectedVelocity.normalized * currentSpeed;
        }
        else if (other.gameObject.CompareTag("Goal")){

            if(other.gameObject.name.Contains("Left")){
                lastScored = "LeftPaddle";
                rightScore += 1;
                scoreText.text = $"{leftScore}:{rightScore}";
                Debug.Log($"Score! The Right Player has scored! The score is {leftScore}-{rightScore}");
                UpdateScoreUI();
            }
            else if(other.gameObject.name.Contains("Right")){
                lastScored = "RightPaddle";
                leftScore += 1; 
                scoreText.text = $"{leftScore}:{rightScore}";
                Debug.Log($"Score! The Left Player has scored! The score is {leftScore}-{rightScore}");
                UpdateScoreUI();
            }

            ResetBall();
        }

        // Ends the game and displays text on the screen and in the console depending on who wins
        if(leftScore >= 11){
            Debug.Log($"Game Over! Left Player Wins!");
            winnerText.text = $"Game Over! Left Player Wins!";
            winnerTextObject.SetActive(true);
            Destroy(gameObject);
        }
        else if(rightScore >= 11){
            Debug.Log($"Game Over! Right Player Wins!");
            winnerText.text = $"Game Over! Right Player Wins!";
            winnerTextObject.SetActive(true);
            Destroy(gameObject);
        }

    }


void UpdateScoreUI(){

    

    if(leftScore >= yellowThreshold || rightScore >= yellowThreshold){
        scoreText.color = Color.yellow;
    }
    if (leftScore >= redThreshold || rightScore >= redThreshold){
        scoreText.color = Color.red;
     }
    }

}
