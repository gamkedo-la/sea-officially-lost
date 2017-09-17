using UnityEngine;

public class BreathingController : MonoBehaviour {
    public float breathTime;
    private float breathClock;
    private bool breathingIn;
    private ParticleSystem bubbleSystem;

	void Start () {
        breathClock = 0f;
        bubbleSystem = GetComponent<ParticleSystem>();
	}
	
	void Update () {
        breathClock += Time.deltaTime * (breathingIn ? 1 : -1);
        if(breathClock > breathTime) {
            //play breathing out sound for breathTime duration
            bubbleSystem.Play();
            breathingIn = false;
        }
        else if(breathClock < 0) {
            breathingIn = true;
        }
	}
}
