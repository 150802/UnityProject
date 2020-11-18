using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject myPrefab;
    public int enemyCount;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i <= enemyCount; i++)
        {
            Instantiate(myPrefab, this.gameObject.transform.position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
