using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMindHive : MonoBehaviour
{
    public GameObject player;
    public float AICoverCheckCooldown;
    public int coverRadius;
    public LayerMask coverLayers;
    public List<GameObject> coverLocations = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AICoverCheckCooldown += Time.deltaTime;
        if(AICoverCheckCooldown >= 5)
        {
            //Reset the cooldown
            AICoverCheckCooldown = 0;

            //Check for the closest cover spot
            Collider[] Locations = Physics.OverlapSphere(player.transform.position, coverRadius, coverLayers);

            //Clear the outdated list
            coverLocations.Clear();

            foreach(Collider cover in Locations)
            {
                //Put the distances in variables to make it more readable
                float distBack = Vector3.Distance(player.transform.position, cover.gameObject.transform.GetChild(0).position);
                float distFront = Vector3.Distance(player.transform.position, cover.gameObject.transform.GetChild(1).position);

                //Compare the distances and add the proper gameobject to the list of coverLocations
                if (distFront <= distBack)
                {
                    coverLocations.Add(cover.gameObject.transform.GetChild(0).gameObject);
                    //print("front cover further!");
                } else {
                    coverLocations.Add(cover.gameObject.transform.GetChild(1).gameObject);
                    //print("back cover further!");
                }
            }

            //print("AI relocate");
        }
    }
}
