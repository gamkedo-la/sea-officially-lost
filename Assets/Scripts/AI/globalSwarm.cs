using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class globalSwarm : MonoBehaviour {

    public globalSwarm thisSwarm;
    public GameObject creaturePrefab;
    public Vector3 swimLimits = new Vector3(10,10,10);

    public Vector3 targetPosition = Vector3.zero;

    static int creatureNumber = 10;
    public GameObject[] allCreatures = new GameObject[creatureNumber];

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, new Vector3(swimLimits.x * 2, swimLimits.y * 2, swimLimits.z * 2));
        Gizmos.color = new Color(0, 1, 0, 1);
        Gizmos.DrawSphere(targetPosition, 0.1f);
    }


    // Use this for initialization
    void Start ()
    {
        thisSwarm = this;
        targetPosition = this.transform.position;


		for(int i = 0; i < creatureNumber; i++)
        {
            Vector3 position = new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
                                           Random.Range(-swimLimits.y, swimLimits.y),
                                           Random.Range(-swimLimits.y, swimLimits.y)
                                           );
            allCreatures[i] = (GameObject)Instantiate(creaturePrefab, position, Quaternion.identity);
            allCreatures[i].GetComponent<Swarm>().myManager = this;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Random.Range(0,10000) < 50)
        {
            targetPosition = this.transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
                                           Random.Range(-swimLimits.y, swimLimits.y),
                                           Random.Range(-swimLimits.y, swimLimits.y)
                                           );
        }
	}
}
