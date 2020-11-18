using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public float Health = 100;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Health <= 0) {
            Destroy(gameObject);
        }
    }

    public void Hit(int damage) {
        Health -= damage;
    }
}
