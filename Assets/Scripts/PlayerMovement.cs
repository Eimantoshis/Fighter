using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeedFront = 5f;
    [SerializeField] private float movementSpeedBack = 2f;
    private float movementSpeed;
    private float horizontalInput;
    private float verticalInput;


    [SerializeField] Transform camera;
    [SerializeField] private float dashCDMax = 5f;
    [SerializeField] private float dashDuration = 0.2f;
    private float dashTimer;
    private float dashCD;
    private float dashMultiplier = 1f;
    private ParticleSystem dashEffect;

    private const int MAX_DASH_CHARGES = 2;
    private int currentDashCharges = MAX_DASH_CHARGES;
    private float secondChargeTimer;

    private PlayerUI playerUI;

    private bool speedBuffOn = false;
    
    private void Start() {
        dashEffect = gameObject.GetComponent<ParticleSystem>();
        playerUI = gameObject.GetComponent<PlayerUI>();
        dashCD = dashCDMax;
        
    }
    
    void Update()
    {
        Move();   
        Dash();
    }


    private void Dash() {
        float fillAmount = dashCD / dashCDMax;
        if (currentDashCharges < MAX_DASH_CHARGES) {
            dashCD -= Time.deltaTime;

            
            if (dashCD <= 0) {
                currentDashCharges++;
                dashCD = dashCDMax;
            }


        }
        playerUI.UpdateDashUI(currentDashCharges, fillAmount);


        SetMovementInput();
        bool isMoving = Mathf.Abs(horizontalInput) > 0.1 || Mathf.Abs(verticalInput) > 0.1f;


        if (currentDashCharges > 0 && Input.GetKeyDown(InputManager.Instance.Dash) && isMoving) {
            UseDash();
            playerUI.UpdateDashUI(currentDashCharges, fillAmount);
        }

        if (dashTimer > 0) {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0) {
                dashMultiplier = 1f;
                dashEffect.Stop();
            }
        }
    }

    private void UseDash() {
        dashMultiplier = 4f;
        dashTimer = dashDuration;
        currentDashCharges--;
        
        
        AudioManager.Instance.PlayDashSound();
        if (dashEffect != null) {
            dashEffect.Play();
        }
    }



    private void Move() {
        SetMovementInput();
        if (!speedBuffOn) { // always go fast with buff
            if (verticalInput >= 0.1f) {
                // Forward
                movementSpeed = movementSpeedFront;
            } else if (verticalInput <= -0.1f) {
                // Backward
                movementSpeed = movementSpeedBack;
            } else if (Mathf.Abs(horizontalInput) > 0.1f) {
                // Sideways
                movementSpeed = movementSpeedBack; 
            } else {
                // Idle
                movementSpeed = movementSpeedFront;
            }
        }


        Vector3 camForward = camera.forward;
        Vector3 camRight = camera.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = camForward * verticalInput + camRight * horizontalInput;

        if (moveDir.magnitude > 1f) {
            moveDir.Normalize();
        }

        transform.position += moveDir * (movementSpeed * Time.deltaTime * dashMultiplier);
    }

    private void SetMovementInput() {
        horizontalInput = 0f;
        verticalInput = 0f;
        
        if (Input.GetKey(InputManager.Instance.MoveUp)) verticalInput += 1f;
        if (Input.GetKey(InputManager.Instance.MoveDown)) verticalInput -= 1f;
        if (Input.GetKey(InputManager.Instance.MoveLeft)) horizontalInput -= 1f;
        if (Input.GetKey(InputManager.Instance.MoveRight)) horizontalInput += 1f;
    }

    public void ApplySpeedBoost() {
        speedBuffOn = true;
        movementSpeed = movementSpeedFront;
    }

    public void RemoveSpeedBoost() {
        speedBuffOn = false;
    }
    

    public void IncreaseStatsOnLevel(float addSpeed) {
        movementSpeedFront += addSpeed;
        movementSpeedBack += addSpeed;
    }

    public float GetDashCD() {
        return dashCD;
    }

    public float GetDashCDMax() {
        return dashCDMax;
    }

    public int GetMaxDashCharges() {
        return MAX_DASH_CHARGES;
    }
}
