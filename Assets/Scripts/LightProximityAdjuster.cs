using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightProximityAdjuster : MonoBehaviour {
    private Light lamp;
    public Transform distanceFrom;
    private enum Mode {
        Linear, Exponential, ExponentialSquared
    };
    [SerializeField] private Mode mode;
    public float minIntensity, maxIntensity;
    public float minDistance, maxDistance;

    private void Start() {
        lamp = GetComponent<Light>();
    }

    void Update () {
        float intensity = CalculateIntensity();
        lamp.intensity = Mathf.Lerp(minIntensity, maxIntensity, intensity);
	}

    private float CalculateIntensity() {
        float t = Mathf.InverseLerp(minDistance, maxDistance, Vector3.Distance(transform.position, distanceFrom.position));
//        if (mode == Mode.Linear) {
            return t;
/*       }
        else if(mode == Mode.Exponential) {
            return Mathf.Exp(t);
        }
        else {

        }
        return 0f;
*/
    }
}
