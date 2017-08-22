using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmMove : MonoBehaviour
{
    public float speed = 0.5f;
    float rotationSpeed = 4f;
    Vector3 averageHeading;
    Vector3 averagePosition;
    float neighbourDistance = 0.5f;

    bool turning = false;

	// Use this for initialization
	void Start ()
    {
        speed = Random.Range(0.5f, 2f);
	}
	
	// Update is called once per frame
	void Update ()
    {
        Bounds b = new Bounds(myManager.transform.position, myManager.areaSize * 2);

        if (!b.Contains(transform.position))
        {
            turning = true;
        }

        if (Random.Range(0,5)<1)
            ApplyRules();
        transform.Translate(0, 0, Time.deltaTime * speed);
        
	}

    void ApplyRules()
    {
        GameObject[] gos;
        gos = globalMovement.allCreatures;
        Vector3 vcenter = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.1f;

        Vector3 targetPos = globalMovement.moveToTarget;

        float dist;

        int groupSize = 0;
        foreach (GameObject go in gos)
        {
            if (go != this.gameObject)
            {
                dist = Vector3.Distance(go.transform.position, this.transform.position);
                if(dist <= neighbourDistance)
                {
                    vcenter += go.transform.position;
                    groupSize++;
                    if (dist < 1.0f)
                    {
                        vavoid = vavoid + (this.transform.position - go.transform.position);
                    }

                    SwarmMove anotherSwarm = go.GetComponent<SwarmMove>();
                    gSpeed = gSpeed + anotherSwarm.speed;
                }
            }
        }

        if (groupSize > 0 )
        {
            vcenter = vcenter / groupSize + (targetPos - this.transform.position);
            speed = gSpeed / groupSize;

            Vector3 direction = (vcenter + vavoid) - transform.position;
            if(direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation,
                                                        Quaternion.LookRotation(direction),
                                                        rotationSpeed * Time.deltaTime);
            }
        }
    }
}
