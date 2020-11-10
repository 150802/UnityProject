using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public float Health = 100;
    public bool hit = false;

    // Start is called before the first frame update
    void Start()
    {
        // In case of a glitch/unexpectancies where the enemy was hit before beeing created this sets it to false.
        hit = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (hit == true) {
            hit = false;
            Health -= 50;
            if (Health <= 0) {
                Destroy(gameObject);
            }
        }
    }
}
