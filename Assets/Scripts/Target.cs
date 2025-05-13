using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public static readonly HashSet<Target> Entities = new HashSet<Target>();
    // Start is called before the first frame update
    void Awake() {
        Entities.Add(this);
    }

    void OnDestroy() {
        Entities.Remove(this);
    }
}
