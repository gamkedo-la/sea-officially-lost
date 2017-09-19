using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightProximityAdjuster : MonoBehaviour {
    private Light lamp;
    public Transform distanceFrom;
    public float minIntensity, maxIntensity;
    public float minDistance, maxDistance;

    private void Start() {
        lamp = GetComponent<Light>();
    }

    void Update () {
        float intensity = Mathf.InverseLerp(minDistance, maxDistance, Vector3.Distance(transform.position, distanceFrom.position));
        lamp.intensity = Mathf.Lerp(minIntensity, maxIntensity, intensity);
	}
}
