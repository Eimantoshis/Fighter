using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider XPBar;
    [SerializeField] private GameObject XPCanvas;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private GameObject dashCDObject;
    [SerializeField] private TextMeshProUGUI dashChargeText;
    [SerializeField] private GameObject bulletCDObject;
    private GameObject healthFillObject;
    private GameObject XPFillObject;
    

    private PlayerMovement playerMovement;
    private PlayerAttack playerAttack;
    private Image dashCDImage;
    private Image bulletCDImage;
    
    // for performance
    private float lastDashFill = -1f;
    private float lastBulletFill = -1f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        playerAttack = gameObject.GetComponent<PlayerAttack>();
        dashCDImage = dashCDObject.gameObject.GetComponent<Image>();
        bulletCDImage = bulletCDObject.gameObject.GetComponent<Image>();
        levelText.SetText(gameObject.GetComponent<PlayerStats>().GetLevel().ToString());
        UpdateXPBar();
        SetHealthFillObject();

    }

    // Update is called once per frame
    void Update()
    {

        UpdateBulletCD();
    }

    public void UpdateDashUI(int charges, float fillAmount)
    {
        if (charges >= 1) {
            dashChargeText.gameObject.SetActive(true);
            dashChargeText.SetText(charges.ToString());
        }
        else {
            dashChargeText.gameObject.SetActive(false);
        }

        int MAXCHARGES = playerMovement.GetMaxDashCharges();
        if (charges == MAXCHARGES) {
            fillAmount = 0f;
        }
        dashCDImage.fillAmount = fillAmount;
        
        Color currentColor = dashCDImage.color;
        currentColor.a = (charges == 0) ? 1f : 0.5f;
        dashCDImage.color = currentColor;
    }



    private void UpdateBulletCD()
    {
        float bulletCD = playerAttack.GetShootTime();
        float bulletCDMax = playerAttack.GetShootTimeMax();
        float fill = bulletCD / bulletCDMax;
        
        // to reduce filling
        if (!Mathf.Approximately(fill, lastBulletFill)) {
            bulletCDImage.fillAmount = fill;
            lastBulletFill = fill;
        }
    }





    public void UpdateHealthBar() {
        
        float health = gameObject.GetComponent<PlayerStats>().GetHealth();
        float maxHealth = gameObject.GetComponent<PlayerStats>().GetMaxHealth();
        float hp = health / maxHealth;
        healthBar.value = hp;
        if (hp >= 0.65) {
            healthFillObject.GetComponent<Image>().color = Color.green;
        } else if (hp > 0.3 && hp <0.65){
            healthFillObject.GetComponent<Image>().color = Color.yellow;
        } else {
            healthFillObject.GetComponent<Image>().color = Color.red;
        }
    }
    public void UpdateXPBar() {
        float XP = gameObject.GetComponent<PlayerStats>().GetXP();
        XPBar.value = XP / 100;
    }

    private void SetHealthFillObject() {
        healthFillObject = healthBar.transform.Find("Fill Area/Fill").gameObject;
        if (healthFillObject == null) {
            Debug.Log("Could not find healthba Fill object");
        }
    }
    private void SetXPFillObject() {
        healthFillObject = XPBar.transform.Find("Fill Area/Fill").gameObject;
        if (healthFillObject == null) {
            Debug.Log("Could not find healthba Fill object");
        }
    }

    public void LevelUp() {
        levelText.SetText(gameObject.GetComponent<PlayerStats>().GetLevel().ToString());
        XPCanvas.GetComponent<Animator>().Play("TextFade", -1, 0);
    }
}
