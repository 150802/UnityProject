using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAIScript : MonoBehaviour
{
    private GameObject envroiment;
    private GameObject player;
    private bool hasTarget = false;

    private void Start()
    {
        player = GameObject.Find("Player");
        envroiment = GameObject.Find("Envroiment");
    }

    // Update is called once per frame
    void Update()
    {
        List<GameObject> extCoverLocations = envroiment.GetComponent<AIMindHive>().coverLocations;

        if (extCoverLocations.Count != 0)
        {
            int chosenCover = Random.Range(0, extCoverLocations.Count);

            while (hasTarget != true)
            {
                if (envroiment.GetComponent<AIMindHive>().coverLocations[chosenCover].GetComponent<CoverCounter>().CoverCount <= 5)
                {
                    envroiment.GetComponent<AIMindHive>().coverLocations[chosenCover].GetComponent<CoverCounter>().CoverCount++;
                    this.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(envroiment.GetComponent<AIMindHive>().coverLocations[chosenCover].transform.position);
                    hasTarget = true;
                }
                else
                {
                    if(chosenCover < extCoverLocations.Count)
                    {
                        chosenCover++;
                    } else {
                        this.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(player.transform.position);
                        hasTarget = true;
                    }
                }
                print(chosenCover);
            }
        }

        print(extCoverLocations.Count);
    }
}
