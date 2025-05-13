using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "PowerUpData", menuName = "Items/PowerUp")]
public class PowerUpData : ScriptableObject {
    public string powerUpName;
    public float duration;
    //public Color Color;
    //public GameObject visualEffect;
    public Sprite imagePU;
    public PowerUpType type;
    public float effectIncrease;
    
    public enum PowerUpType {
        RapidFire,
        DamageIncrease,
        SpeedBoost,
        Shield
    }

}
