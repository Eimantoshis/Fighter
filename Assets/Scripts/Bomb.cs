using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bomb : MonoBehaviour
{

    [SerializeField] private float damage = 30f;
    private bool canHit;
    private Animator animator;
    private Rigidbody rb;
    [SerializeField] private float bombSpeedMultiplier = 1.5f;
    [SerializeField] private ParticleSystem preExplosionParticles;
    [SerializeField] private ParticleSystem postExplosionParticles;
    //public GameObject explosionEffect;

    private void Start() {
        animator = gameObject.GetComponent<Animator>();
        
        Destroy(gameObject, 5f); // if it manages to fly out of the map somehow
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    // Launch the bomb with an arc
    public void Launch(Vector3 target, float arcHeight)
    {
        Vector3 direction = target - transform.position;
        Vector3 horizontal = new Vector3(direction.x, 0, direction.z);
        float distance = horizontal.magnitude;

        // Set initial velocity to reach the target with an arc
        float height = arcHeight;
        float gravity = Mathf.Abs(Physics.gravity.y);
        float time = Mathf.Sqrt(2 * height / gravity) + Mathf.Sqrt(2 * (height - direction.y) / gravity);
        Vector3 velocity = horizontal / time;
        velocity.y = Mathf.Sqrt(2 * gravity * height);

        rb.velocity = velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            Explode();
        }
        if (canHit && other.gameObject.CompareTag("Player")) {
            other.gameObject.GetComponent<PlayerStats>().TakeDamage(damage);
        }
    }

    private void Explode()
    {
        preExplosionParticles.Stop();
        postExplosionParticles.Play();
        canHit = true;
        rb.velocity = Vector3.zero;

        // so that explosion seems more like a sphere
        transform.position += Vector3.up * 0.33f;

        animator.Play("BombExplosion");
        AudioManager.Instance.PlayExplode();
        Destroy(gameObject,0.5f); // 0.5f should be animation ("BombExplosion") duration
    }
}
