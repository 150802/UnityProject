using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPScript : MonoBehaviour
{
    //public GameObject otherSpawn;
    //public GameObject Player;
    public GameObject winText;
    public GameObject cheatText;
    public GameObject[] gos;
    private int gosLength;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter() {
        gosLength = 0;

        print("test print for teleportation");
        //Player.transform.position.Set(otherSpawn.transform.position.x, otherSpawn.transform.position.y, otherSpawn.transform.position.z);
        //Player.transform.position = otherSpawn.transform.position;
        winText.active = true;

        gos = GameObject.FindGameObjectsWithTag("enemy");
        foreach (GameObject item in gos)
        {
            print("enemy test count");
            gosLength++;
        }
        
        if (gosLength != 0) 
        {
            // Do Something
            cheatText.active = true;
        }
    }

    private void OnTriggerExit() {
        winText.active = false;
        cheatText.active = false;
    }
}