using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFunTestScript : MonoBehaviour {

	public Light myLight;
	public float maxIntensity = 1.0f;
	public float minIntensity = 0.0f;
    public float pulseSpeed = 1.0f;
	private float targetIntensity = 1.0f;
	private float currentIntensity;

	// Update is called once per frame
	void Update()
	{
		if (myLight != null)
		{
			UpdateLight();
		}
	}

	void UpdateLight()
	{
		currentIntensity = Mathf.MoveTowards(myLight.intensity, targetIntensity, Time.deltaTime * pulseSpeed);
		if (currentIntensity >= maxIntensity)
		{
			currentIntensity = maxIntensity;
			targetIntensity = minIntensity;
		}
		else if (currentIntensity <= minIntensity)
		{
			currentIntensity = minIntensity;
			targetIntensity = maxIntensity;
		}
		myLight.intensity = currentIntensity;
	}
}
