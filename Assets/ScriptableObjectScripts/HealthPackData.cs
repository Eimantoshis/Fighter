using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHealthPackData", menuName = "Items/HealthPack")]
public class HealthPackData : ScriptableObject
{
    public string packName;
    public int healAmount;
    public Color color;
    //public Color glowColor;
    //public AudioClip pickupSound;
}
