using UnityEngine;

public class BreathingController : MonoBehaviour {
    public float breathingTimeScale = 1f;
    public float breathTime;
    private float breathClock;
    private bool breathingIn;
    private ParticleSystem bubbleSystem;

	void Start () {
        breathClock = 0f;
        bubbleSystem = GetComponent<ParticleSystem>();
	}
	
	void Update () {
        breathClock += Time.deltaTime * breathingTimeScale * (breathingIn ? 1 : -1);
        if(breathClock > breathTime) {
            //play breathing out sound for breathTime duration
            bubbleSystem.Play();
            breathingIn = false;
        }
        else if(breathClock < 0) {
            breathingIn = true;
        }
	}

    public void BreathIn() {

    }

    public void BreathOut(float duration) {
        breathingTimeScale = 0;
        //TODO quick fade between sound clips or some kind of method to stop a beath sound short
        //TODO play breath sound of correct type for duration

        var main = bubbleSystem.main;
        main.duration = duration / 2;
    }
}
