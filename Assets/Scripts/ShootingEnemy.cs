using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private GameObject bullet;
    private float bulletSpeed = 12f;
    private float bulletDamage = 15f;
    private float speed = 3f;
    private float shootTimerMax = 4f;
    private float shootTimer;

    private float strafeSpeed = 2f;
    private float strafeDirectionChangeTime = 2f;
    private float strafeTimer;
    private Vector3 strafeDirection;

    private float shootTimeVariance = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        FindPlayerReference();
        shootTimer = shootTimerMax + Random.Range(-shootTimeVariance, shootTimeVariance);
        strafeTimer = strafeDirectionChangeTime;
        strafeDirection = Random.insideUnitCircle.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        //Shoot();
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
            transform.position = moveToward + strafe * (strafeSpeed * 0.7f * Time.deltaTime); // reduced strafe 
        } else if (distance < 15f){
            // too close
           Vector3 dirAway = (transform.position - player.transform.position).normalized;
           transform.position += dirAway * (speed * Time.deltaTime) + strafe * (strafeSpeed * 0.8f * Time.deltaTime); // medium strafe
           Shoot();
        } else {

            
            transform.position += strafe * (strafeSpeed * Time.deltaTime); // full strafe
            Shoot();
        }
    }

    private void Shoot() {

        if (shootTimer <= 0) {
            shootTimer = shootTimerMax + Random.Range(-shootTimeVariance, shootTimeVariance);
            GameObject shot = Instantiate(bullet, this.transform.position, Quaternion.identity);
            BaseBullet bulletScript = shot.GetComponent<BaseBullet>();
            if (bulletScript != null) {
                bulletScript.AttackSelectedTarget(player,bulletSpeed, bulletDamage,this.gameObject.tag);
            }
        }
    }

    private void FindPlayerReference() {
        player = GameObject.FindWithTag("Player");
        if (player == null) {
            Destroy(gameObject);
        }
    }
}
