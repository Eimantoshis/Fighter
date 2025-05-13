using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberEnemy : MonoBehaviour
{
    [SerializeField] private GameObject bomb;
    private GameObject player;
    private float shootTimerMax = 3.5f;
    private float shootTimer;
    private float speed = 3f;
    private float bombArcHeight = 5f;
    
    
    private Vector3 lastPlayerPosition;
    private Vector3 estimatedPlayerVelocity;
    private float velocitySampleInterval = 0.2f;
    private float velocityTimer;
    private float shootTimeVariance = 1.5f;
    
    private float strafeSpeed = 1f;
    private float strafeDirectionChangeTime = 2f;
    private float strafeTimer;
    private Vector3 strafeDirection;

    // Start is called before the first frame update
    void Start()
    {
        FindPlayerReference();
        shootTimer = shootTimerMax + Random.Range(-shootTimeVariance, shootTimeVariance);

        if (player != null) {
            lastPlayerPosition = player.transform.position;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
        if (player == null) return;

        velocityTimer += Time.deltaTime;
        if (velocityTimer >= velocitySampleInterval)
        {
            Vector3 currentPosition = player.transform.position;
            estimatedPlayerVelocity = (currentPosition - lastPlayerPosition) / velocityTimer;
            lastPlayerPosition = currentPosition;
            velocityTimer = 0f;
        }
        
        Move();
    }

    private void Move() {
        shootTimer -= Time.deltaTime;
        strafeTimer -= Time.deltaTime;
        
        if (strafeTimer <= 0) {
            strafeTimer = strafeDirectionChangeTime;
            strafeDirection = Random.insideUnitCircle.normalized;
        }

        Vector3 toPlayer = (player.transform.position - transform.position).normalized;
        Vector3 strafe = Vector3.Cross(toPlayer, Vector3.up).normalized * strafeDirection.x;
        
        
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance > 20f){
            // do not shoot if too far away
            Vector3 moveToward = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            transform.position = moveToward + strafe * (strafeSpeed * 0.3f * Time.deltaTime); // reduced strafe 
        } else if (distance < 15f){
            // too close
            Vector3 dirAway = (transform.position - player.transform.position).normalized;
            transform.position += dirAway * (speed * Time.deltaTime) + strafe * (strafeSpeed * 0.6f * Time.deltaTime); // medium strafe
            Shoot();
        } else {

            
            transform.position += strafe * (strafeSpeed * Time.deltaTime); // full strafe
            Shoot();
        }
    }

    private void Shoot()
    {
        if (shootTimer > 0) return;

        shootTimer = shootTimerMax + Random.Range(-shootTimeVariance, shootTimeVariance);
        GameObject newBomb = Instantiate(bomb, transform.position, Quaternion.identity);
        Bomb bombScript = newBomb.GetComponent<Bomb>();

        if (bombScript != null)
        {
            Vector3 playerPos = player.transform.position;
            Vector3 toPlayer = playerPos - transform.position;
            Vector3 horizontalDir = new Vector3(toPlayer.x, 0, toPlayer.z);

            float gravity = Mathf.Abs(Physics.gravity.y);
            float timeToPeak = Mathf.Sqrt(2 * bombArcHeight / gravity);
            float timeToFall = Mathf.Sqrt(2 * Mathf.Max(bombArcHeight + toPlayer.y, 0) / gravity);
            float totalTime = timeToPeak + timeToFall;

            Vector3 predictedPos = playerPos + estimatedPlayerVelocity * totalTime;

            float maxPredictionDistance = 10f;
            if ((predictedPos - playerPos).magnitude > maxPredictionDistance)
            {
                predictedPos = playerPos + (predictedPos - playerPos).normalized * maxPredictionDistance;
            }

            bombScript.Launch(predictedPos, bombArcHeight);
        }
    }

    private void FindPlayerReference() {
        player = GameObject.FindWithTag("Player");
        if (player == null) {
            Destroy(gameObject);
        }
    }
}
