using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class globalSwarm : MonoBehaviour {

    private bool isSpawned = false;
    public float rangeToSpawn = 10.0f;
    public globalSwarm thisSwarm;
    public GameObject creaturePrefab;
    public Vector3 swimLimits = new Vector3(10,10,10);

    public Vector3 targetPosition = Vector3.zero;

    private UnityEngine.Coroutine targetCoroutine;
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

    void RemoveAllCreatures()
    {
        for (int i = 0; i < allCreatures.Length; i++)
        {
            Destroy(allCreatures[i]);
        }
        StopCoroutine(targetCoroutine);
    }
    void SpawnCreatures ()
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
        targetCoroutine = StartCoroutine(changeTargets());

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

    public void Update()
    {
        float distToPlayer = Vector3.Distance(transform.position, PlayerController.instance.transform.position);

        if(distToPlayer < rangeToSpawn)
        {
            if (isSpawned == false)
            {
                Debug.Log("spawning fish from "+ gameObject.name);
                SpawnCreatures();
                isSpawned = true;
            }
        }
        else
        {
            if (isSpawned)
            {
                Debug.Log("removing fish for " + gameObject.name);
                RemoveAllCreatures();
                isSpawned = false;
            }
        }
    }
}
