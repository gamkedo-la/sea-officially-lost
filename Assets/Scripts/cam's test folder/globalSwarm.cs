using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class globalSwarm : MonoBehaviour {

    public globalSwarm thisSwarm;
    public GameObject creaturePrefab;
    public Vector3 swimLimits = new Vector3(10,10,10);

    public Vector3 targetPosition = Vector3.zero;


    public int creatureNumber = 10;
    [HideInInspector]
    public GameObject[] allCreatures;

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
        allCreatures = new GameObject[creatureNumber];
        thisSwarm = this;
        targetPosition = this.transform.position;


		for(int i = 0; i < creatureNumber; i++)
        {
            Vector3 position =this.transform.position +  new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
                                           Random.Range(-swimLimits.y, swimLimits.y),
                                           Random.Range(-swimLimits.z, swimLimits.z));
            allCreatures[i] = (GameObject)Instantiate(creaturePrefab, position, Quaternion.identity);
            allCreatures[i].GetComponent<Swarm>().myManager = this;
            allCreatures[i].transform.SetParent(transform);
        }
        StartCoroutine(changeTargets());

	}
	
	// Update is called once per frame
	IEnumerator changeTargets ()
    {
        while(true)
        {
            targetPosition = this.transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
                                           Random.Range(-swimLimits.y, swimLimits.y),
                                           Random.Range(-swimLimits.z, swimLimits.z)
                                           );
            yield return new WaitForSeconds(Random.Range(1.0f, 4.0f));
      
        }
	}
}
