using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class MainCamera : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private int z = -3;
    [SerializeField] private int y = 7;
    [SerializeField] private float smoothTime = 0.15f;

    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private float shakeMagnitude = 0.3f;

    [SerializeField] private float vignetteIntensity = 0.4f;
    [SerializeField] private float fadeSpeed = 1f;
    [SerializeField] private PostProcessVolume postProcVol;
    private Vignette vignette;
    private PlayerAttack playerAttack;

    private Vector3 velocity = Vector3.zero;
    private Vector3 cameraOffsetDirection = Vector3.forward;

    private bool isShaking = false;

    private void Start() {
        if (!postProcVol.profile.TryGetSettings(out vignette) || vignette == null) {
            Debug.LogError("Vignette not found in Post Processing Profile!");
        }

        playerAttack = player.GetComponent<PlayerAttack>();
    }

    private void Awake() {
        PlayerStats.onPlayerDamaged += OnPlayerDamaged;
    }
    
    private void OnDestroy() {
        PlayerStats.onPlayerDamaged -= OnPlayerDamaged;
    }

    void Update()
    {
        HandleCameraRotationInput();
        FollowPlayer();
    }

    private void OnPlayerDamaged(object sender, EventArgs e)
    {
        if (!isShaking){
            StartCoroutine(ShakeCamera());
        }
        StartCoroutine(FlashVignette());
    }

    private void FollowPlayer() {
        Vector3 flatDirection = new Vector3(cameraOffsetDirection.x, 0, cameraOffsetDirection.z).normalized;
        Vector3 targetPosition = player.position - flatDirection * Mathf.Abs(z) + Vector3.up * y;

        if (!isShaking){
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }


        Vector3 lookTarget = player.position + Vector3.up * 2f;
        Quaternion targetRotation = Quaternion.LookRotation(lookTarget - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    private void HandleCameraRotationInput() {
        if (Input.GetKeyDown(InputManager.Instance.CameraLeft))
        {
            cameraOffsetDirection = Quaternion.Euler(0, -90, 0) * cameraOffsetDirection;
            if (playerAttack.GetShootTime() <= 0.7f) { // avoid insta-shooting with camera spamming
                playerAttack.SetShootTime(0.7f);
            }
        }
        else if (Input.GetKeyDown(InputManager.Instance.CameraRight))
        {
            cameraOffsetDirection = Quaternion.Euler(0, 90, 0) * cameraOffsetDirection;
            if (playerAttack.GetShootTime() <= 0.7f) { // avoid insta-shooting with camera spamming
                playerAttack.SetShootTime(0.7f);
            }
        }
    }

    private IEnumerator ShakeCamera() {
        isShaking = true;
        Vector3 originalPos = transform.position;

        float elapsed = 0f;

        while (elapsed < shakeDuration) {
            while (Time.timeScale == 0f) {
                yield return null; // wait until unpaused
            }
            Vector3 randomOffset = UnityEngine.Random.insideUnitSphere * shakeMagnitude;
            randomOffset.y = 0;
            transform.position = originalPos + randomOffset;

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        isShaking = false;
    }

    private IEnumerator FlashVignette() {
        vignette.intensity.value = vignetteIntensity;

        while (vignette.intensity.value > 0f) {

            while (Time.timeScale == 0f) {
                yield return null;
            }
            
            vignette.intensity.value -= Time.unscaledDeltaTime * fadeSpeed;
            yield return null;
        }

        vignette.intensity.value = 0f;
    }
}

