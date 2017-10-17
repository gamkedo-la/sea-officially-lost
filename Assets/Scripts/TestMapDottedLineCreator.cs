using UnityEngine;

public class TestMapDottedLineCreator : MonoBehaviour {
    public GameObject dot;
    private GameObject closestDot;
    public float dotDistance;
    public int maxDots;

    void Update () {
        if (closestDot == null) {
            GameObject newDot = Instantiate(dot, transform.position, transform.rotation);
            closestDot = newDot;
        }

        if (Vector3.Distance(transform.position, closestDot.transform.position) > dotDistance) {
            GameObject newDot = Instantiate(dot, transform.position, transform.rotation);
            newDot.GetComponent<TestMapDottedLineBehaviour>().SetupDot(closestDot, maxDots);
            closestDot = newDot;
        }
    }
}
