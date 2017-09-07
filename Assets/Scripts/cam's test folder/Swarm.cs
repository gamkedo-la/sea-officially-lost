using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swarm : MonoBehaviour {

    public float speed = 2.5f;
    float rotationSPeed = 4.0f;
    Vector3 averageDirection;
    Vector3 averageDistance;
    public float groupPullPower = 2.0f; //how close they must be in order to group up

    private float visionRange = 5.0f;
    private float maxAngryTime = 2.0f;

    bool turning = false;
    float timeStillAlerted = 0.0f;

    [HideInInspector]
    public globalSwarm myManager;
    // Use this for initialization
    void Start ()
    {
        StartCoroutine(AICheckForPlayer());

        speed = Random.Range(0.5f,3);
	}

	IEnumerator AICheckForPlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.5f, 1.0f));
            float distToPlayer = Vector3.Distance(transform.position, PlayerController.instance.transform.position);
           // Debug.Log(distToPlayer);
           if(distToPlayer < visionRange)
            {
                timeStillAlerted = maxAngryTime;
            }
        }
    }
    private void FixedUpdate()
    {
        if(timeStillAlerted > 0.0f)
        {
            Quaternion angleToPlayer = Quaternion.LookRotation(PlayerController.instance.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, angleToPlayer, 5.0f * Time.deltaTime);
            //transform.LookAt(PlayerController.instance.transform.position); //snaps instantly to target


        }
    }
    // Update is called once per frame
    void Update ()
    {
        if (timeStillAlerted > 0.0f) // aware of player, chasing, ignoring boundaries
        {
            timeStillAlerted -= Time.deltaTime;
        }
        else // unaware of player, wandering within boundary
        {
            Bounds box = new Bounds(myManager.transform.position, myManager.swimLimits * 2);


            if (!box.Contains(point: transform.position))
            {
                turning = true;
            }
            else
                turning = false;
            if (turning)
            {
                Vector3 direction = myManager.transform.position - transform.position;
                transform.rotation = Quaternion.Slerp(transform.rotation,
                                                      Quaternion.LookRotation(direction),
                                                      rotationSPeed * Time.deltaTime);
                speed = Random.Range(0.5f, 1);
            }
            else
            {
                if (Random.Range(0, 5) < 1)
                    ApplyRules();
            }
        }
        transform.Translate(0, 0, Time.deltaTime * speed);
        
	}

    void ApplyRules()  //when swarming creatures will try to be in the center while avoiding other creatures
    {
        GameObject[] gos;
        gos = myManager.allCreatures;

        Vector3 vCentre = Vector3.zero;
        Vector3 vAvoid = Vector3.zero;
        float gSpeed = 0.1f;

        Vector3 targetPosition = myManager.targetPosition;

        float distance;

        int groupSize = 0;
        foreach(GameObject go in gos)
        {
            if(go != this.gameObject)
            {
                distance = Vector3.Distance(go.transform.position, this.transform.position);
                if(distance <= groupPullPower)
                {
                    vCentre += go.transform.position;
                    groupSize++;
                    if(distance < 1.0f)
                    {
                        vAvoid = vAvoid + (this.transform.position - go.transform.position);
                    }

                    Swarm anotherSwarm = go.GetComponent<Swarm>();
                    gSpeed = gSpeed + anotherSwarm.speed;
                }
            }
        }

        if(groupSize > 0)
        {
            vCentre = vCentre / groupSize + (targetPosition - this.transform.position);
            speed = gSpeed / groupSize;

            Vector3 direction = (vCentre + vAvoid) - transform.position;
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation,
                                                      Quaternion.LookRotation(direction),
                                                      rotationSPeed * Time.deltaTime);
        }
    }
}
