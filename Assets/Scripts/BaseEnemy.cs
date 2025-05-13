using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseEnemy : MonoBehaviour
{
    private GameObject player;
    private float damageTimerMax = 1.5f;
    private float damageTimer;
    private float damage = 20f;
    private float speed = 4f;

    // Start is called before the first frame update
    void Start()
    {

        damageTimer = damageTimerMax;
        FindPlayerReference();
    }

    // Update is called once per frame
    void Update()
    {
        damageTimer -= Time.deltaTime;
        FollowPlayer();

    }
    private void FindPlayerReference() {
        player = GameObject.FindWithTag("Player");
        if (player == null) {
            Destroy(gameObject);
        }
    }

    private void FollowPlayer() {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }


    

    private void OnCollisionStay(Collision other) {
        if (other.gameObject.CompareTag("Player") && damageTimer < 0) {
            player.GetComponent<PlayerStats>().TakeDamage(damage);
            damageTimer = damageTimerMax;
        }
    }



}
