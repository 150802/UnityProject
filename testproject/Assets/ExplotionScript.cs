using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplotionScript : MonoBehaviour
{
    public float destroyDelay = 1;
    void Start()
    {
        Destroy(gameObject, destroyDelay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector3 pushDirection = transform.position - other.transform.position;
        other.attachedRigidbody.AddForce(-pushDirection * 3000);
    }
}
