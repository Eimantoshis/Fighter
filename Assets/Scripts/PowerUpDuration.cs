using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PowerUpDuration : MonoBehaviour
{
    private float maxDuration;
    private float currentDuration;
    private Image durationImage;

    public void Initialize(float duration) {
        durationImage = GetComponent<Image>();
        maxDuration = duration;
        currentDuration = duration;
        durationImage.fillAmount = 1f;

    }


    // Update is called once per frame
    void Update()
    {
        if (currentDuration > 0) {
            currentDuration -= Time.deltaTime;
            durationImage.fillAmount = currentDuration / maxDuration;
            
            if (currentDuration <= 0) {
                Destroy(transform.parent.gameObject);
            }
        }
    }
}
