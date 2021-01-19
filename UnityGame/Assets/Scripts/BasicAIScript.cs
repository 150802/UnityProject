using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAIScript : MonoBehaviour
{
    private GameObject player;
    
    private void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(player.transform.position);
    }
}
