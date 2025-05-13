using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    private float speed;
    private float damage;
    private Vector3 moveDirection;
    private string shooterTag;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += moveDirection * (speed * Time.deltaTime);
    }

    public void AttackSelectedTarget(GameObject target, float bulletSpeed, float damage, string shooterTag)  {
        speed = bulletSpeed;
        this.shooterTag = shooterTag;
        this.damage = damage;
        if (target != null) {
            moveDirection = (target.transform.position - transform.position).normalized;
        }
    }

    public void ShootInDirection(Vector3 direction, float speed, float damage, string shooterTag) {
        this.speed = speed;
        this.shooterTag = shooterTag;
        this.damage = damage;
        direction.y = 0;
        this.moveDirection = direction;
    }


    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag(shooterTag)) {
            // can't shoot yourself
            return;
        }
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyHP>().TakeDamage(damage);
            Destroy(gameObject);
        }
        if (other.CompareTag("Player")) {
            other.gameObject.GetComponent<PlayerStats>().TakeDamage(damage);
            Destroy(gameObject);
        }

    }
}
