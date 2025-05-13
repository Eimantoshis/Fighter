using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    [SerializeField] private float defaultDamage = 50;
    private float damage;
    [SerializeField] private GameObject bullet;
    private float bulletSpeed = 12f;
    [SerializeField] private float defaultShootTimerMax = 2f;
    private float shootTimerMax;
    private float shootTime = 0f;
    private Animator animator;
    private float maxRange = 30f;

    private bool isAutoFire;

    // Start is called before the first frame update
    void Start()
    {
       damage = defaultDamage;
       shootTimerMax = defaultShootTimerMax;
       animator = gameObject.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        Attack();
        ChangeAutoFire();
    }

    public float GetShootTime() {
        return shootTime;
    }

    public float GetShootTimeMax() {
        return shootTimerMax;
    }
    

    public void SetShootTime(float time) {
        shootTime = time;
    }

    private void ChangeAutoFire() {
        if (Input.GetKeyDown(InputManager.Instance.AutoFire)) {
            isAutoFire = !isAutoFire;
        }

    }

    private void Attack() {
        shootTime -= Time.deltaTime;
        if (shootTime <= 0) {
            if (isAutoFire) {
                BaseAttackAuto();

            }
            else {
                if (Input.GetMouseButtonDown(0) && Time.timeScale == 1f) {
                    BaseAttackManual();
                }

            }


        }
    }

    private void BaseAttackAuto() {
        GameObject nearEnem = NearestEnemy();
        if (nearEnem != null) {
            GameObject shot = Instantiate(bullet, this.transform.position, Quaternion.identity);
            BaseBullet bulletScript = shot.GetComponent<BaseBullet>();
            if (bulletScript != null) {
                bulletScript.AttackSelectedTarget(nearEnem, bulletSpeed, damage, this.gameObject.tag);
                shootTime = shootTimerMax;
                animator.Play("PlayerShoot", -1, 0);
                AudioManager.Instance.PlayShootSound();
            }
        }
            
    }

    private void BaseAttackManual() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit, 100f)) {
            targetPoint = hit.point;
        }
        else {
            targetPoint = ray.origin + ray.direction * 50f;
        }

        if (!IsPointInCone(targetPoint)) return;
        
        Vector3 direction = (targetPoint - transform.position).normalized;
        
        
        GameObject shot = Instantiate(bullet, transform.position, Quaternion.identity);
        BaseBullet bulletScript = shot.GetComponent<BaseBullet>();

        if (bulletScript != null) {
            bulletScript.ShootInDirection(direction, bulletSpeed, damage, this.gameObject.tag);
            shootTime = shootTimerMax;
            animator.Play("PlayerShoot", -1, 0);
            AudioManager.Instance.PlayShootSound();
        }
    }

    public void IncreaseStatsOnLevel(float damage, float bulletSpeed, float shootIncrease) {
        if (defaultDamage == damage) { // No PowerUp In Use
            this.defaultDamage += damage;
            this.damage = this.defaultDamage;
        }
        else {
            this.defaultDamage += damage;
        }
        

        this.bulletSpeed += bulletSpeed;

        if (shootTimerMax == defaultShootTimerMax) { // No PowerUp In Use
            this.defaultShootTimerMax -= shootIncrease;
            this.shootTimerMax = this.defaultShootTimerMax;
        }
        else {
            this.defaultShootTimerMax -= shootIncrease;
        }

    }

    private GameObject NearestEnemy() {
        Vector3 pos = transform.position;

        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        float dist = float.PositiveInfinity;
        Target target = null;
        foreach(var targ in Target.Entities) {


            Vector3 toTarget = targ.transform.position - pos;
            float sqrDist = toTarget.sqrMagnitude;
            
            if (sqrDist > maxRange * maxRange) continue; // target is too far away

            Vector3 dirToTarget = toTarget.normalized;
            dirToTarget.y = 0f; // ignore vertical difference

            float dot = Vector3.Dot(cameraForward, dirToTarget);
            // 0.5f = 120 degrees in front of player
            if (dot > 0.5f) { // 1 = directly forward, 0 = 90 degrees to the side, -1 = behind
                if (sqrDist < dist) {
                    target = targ;
                    dist = sqrDist;
                }
            }
        }
        return target?.gameObject;
    }

    public void ApplyDamageIncrease(float increase) {
        Debug.Log("Damage Increase:");
        damage += increase;
    }
    public void RemoveDamageIncrease() {
        damage = defaultDamage;
    }
    
    public void ApplyRapidFire(float increase) {
        Debug.Log("Rapid Fire Increase:");
        shootTimerMax /= increase;
    }
    public void RemoveRapidFire() {
        shootTimerMax = defaultShootTimerMax;
    }

    private bool IsPointInCone(Vector3 point) {
        Vector3 pos = transform.position;
        Vector3 toPoint = point - pos;
        
        Vector3 dirToPoint = toPoint.normalized;
        dirToPoint.y = 0f;
        
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();
        
        float dot = Vector3.Dot(cameraForward, dirToPoint);

        return dot > 0.5f;
    }

}
