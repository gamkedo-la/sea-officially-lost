using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class globalSwarm : MonoBehaviour {

    public GameObject creaturePrefab;
    public static int areaSize = 5;

    public static Vector3 targetPosition = Vector3.zero;

    static int creatureNumber = 10;
    public static GameObject[] allCreatures = new GameObject[creatureNumber];
    
    
    // Use this for initialization
	void Start ()
    {
		for(int i = 0; i < creatureNumber; i++)
        {
            Vector3 position = new Vector3(Random.Range(-areaSize,areaSize),
                                           Random.Range(-areaSize,areaSize),
                                           Random.Range(-areaSize,areaSize)
                                           );
            allCreatures[i] = (GameObject)Instantiate(creaturePrefab, position, Quaternion.identity);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Random.Range(0,10000) < 50)
        {
            targetPosition = new Vector3(Random.Range(-areaSize, areaSize),
                                           Random.Range(-areaSize, areaSize),
                                           Random.Range(-areaSize, areaSize)
                                           );
        }
	}
}
