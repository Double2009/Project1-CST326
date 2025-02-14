using UnityEngine;

public enum PowerUpType
{
    IncreaseBallSpeed,
    ChangeBallDirection
}

public class PowerUps : MonoBehaviour
{

    public PowerUpType powerUpType;

    public float multiplier = 1.2f;


    private void OnTriggerEnter(Collider collider){
        
        Debug.Log("PowerUp triggered by: " + collider.gameObject.name);

        if (collider.CompareTag("Ball")){

            DemoBall ballScript = collider.GetComponent<DemoBall>();
            Rigidbody ballRb = collider.GetComponent<Rigidbody>();

            if(ballScript != null && ballRb != null){
                if (powerUpType == PowerUpType.IncreaseBallSpeed){
                   
                    ballScript.speedMultiplier *= multiplier;

                    ballRb.linearVelocity = ballRb.linearVelocity * multiplier;
                    Debug.Log("PowerUp Applied: Increased Ball Speed. New speed multiplier: " + ballScript.speedMultiplier);
                }
                else if (powerUpType == PowerUpType.ChangeBallDirection){

                    float currentSpeed = ballRb.linearVelocity.magnitude;
                    Vector3 newDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
                    ballRb.linearVelocity = newDirection * currentSpeed;
                    Debug.Log("PowerUp Applied: Changed Ball Direction");
                }
            }
            Destroy(gameObject);
        }

    }


}
