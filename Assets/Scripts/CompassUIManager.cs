using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassUIManager : MonoBehaviour {

	public Image leftBoundry;
	public Image rightBoundry;
	
    public float sonarRange = 40.0f;

    private List<CompassIconSet> compassTargets;

	// Use this for initialization
	void Start () {
		StartCoroutine(SonarPing());
        GameObject[] compassTemp = GameObject.FindGameObjectsWithTag("compassTarget");
        compassTargets = new List<CompassIconSet>();
        for (int i = 0; i < compassTemp.Length; i++){
            compassTargets.Add(compassTemp[i].GetComponent<CompassIconSet>());
        }
	}

	IEnumerator SonarPing()
	{
		while (true)
		{
            yield return new WaitForEndOfFrame();
			Collider[] sonarHits = Physics.OverlapSphere(transform.position, sonarRange, LayerMask.GetMask("sonarDetects"));
			//Debug.Log(sonarHits.Length);
			float nearestFoundDist = sonarRange + 1.0f;

			for (int i = 0; i < sonarHits.Length; i++)
			{
				float thisDist = Vector3.Distance(transform.position, sonarHits[i].transform.position);
				if (thisDist < nearestFoundDist)
				{
					float bearing = Mathf.Atan2(sonarHits[i].transform.position.x - transform.position.x, sonarHits[i].transform.position.z - transform.position.z) * Mathf.Rad2Deg;
					float angleToTarget = Mathf.DeltaAngle(transform.localEulerAngles.y, bearing);
					float compassFOV = 45;
                    CompassIconSet cisScript = sonarHits[i].GetComponent<CompassIconSet>();
                    if (cisScript == null) {
                        Debug.Log("The impossible has happened and something with sonarDetects layer and tag doesn't have CompassIconSet script: " + sonarHits[i].name);
                    }
					if (angleToTarget >= -compassFOV && angleToTarget <= compassFOV)
					{
						float range = compassFOV * 2;
						float angPerc = (angleToTarget - (-compassFOV)) / range;
						//Debug.Log("angle Perc " + angPerc);
						cisScript.myIcon.enabled = true;
						cisScript.myIcon.transform.position = (1.0f - angPerc) * leftBoundry.transform.position + angPerc * rightBoundry.transform.position;
					}
					else
					{
						cisScript.myIcon.enabled = false;
					}
					//Debug.Log("Bearing of thing " + sonarHits[i].name + " is " + bearing + " and angle is " + angleToTarget);

				}
			} // end for loop for sonarHits

			//Debug.Log(sonarHits[nearestHitIndex].name);
		} // end while true
	}  // end SonarPing
}
