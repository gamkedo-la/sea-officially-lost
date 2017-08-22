using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class globalMovement : MonoBehaviour
{
    public GameObject targetToSpawn;
    public GameObject creaturePrefab;
    public static int creatureNumber = 10;
    public static GameObject[] allCreatures = new GameObject[creatureNumber];
    public Vector3 areaSize = new Vector3 (5,5,5);

    public static Vector3 moveToTarget = new Vector3();

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, new Vector3(areaSize.x * 2, areaSize.y * 2, areaSize.z * 2));
        Gizmos.color = new Color(0, 1, 0, 1);
        Gizmos.DrawSphere(moveToTarget, 0.1f);
    }

    // Use this for initialization
    void Start ()
    {
        mySwarm = this;
        moveToTarget = this.transform.position;


		for(int i = 0; i < creatureNumber; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-areaSize.x, areaSize.x),
                                           Random.Range(-areaSize.y, areaSize.y),
                                           Random.Range(-areaSize.z, areaSize.z));
            allCreatures[i] = Instantiate(creaturePrefab, pos, rotation: Quaternion.identity);
            allCreatures[i].GetComponent<SwarmMove>().myManager = this;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Random.Range(0, 10000) < 50)
        {
            moveToTarget = this.transform.position + new Vector3(Random.Range(-areaSize.x,areaSize.x),
                                                                 Random.Range(-areaSize.y, areaSize.y),
                                                                 Random.Range(-areaSize.y, areaSize.y));
        }
	}
}
