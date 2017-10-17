using UnityEngine;

public class TestMapDottedLineBehaviour : MonoBehaviour {
    int distance;
    int maxDistance;
    Renderer rend;
    Material mat;
    TestMapDottedLineBehaviour nextDot;

    private void Start() {
        mat = GetComponent<Material>();
        rend = GetComponent<Renderer>();
        rend.material.color = new Color(1f, 0f, 0f);
        //mat.color = new Color(1f, 0f, 0f);
    }

    public void SetupDot(GameObject next, int maxDots) {
        nextDot = next.GetComponent<TestMapDottedLineBehaviour>();
        maxDistance = maxDots;
        distance = 0;
        nextDot.AddLink();
    }

    public void AddLink() {
        distance += 1;

        if(distance > maxDistance) {
            Destroy(gameObject);
        }

        rend.material.color = new Color((maxDistance - distance) / (float)maxDistance, 0f, distance / (float)maxDistance);
        //mat.color = new Color((maxDistance - distance)/(float)maxDistance, 0f, distance/(float)maxDistance);

        if (nextDot != null) {
            nextDot.AddLink();
        }
    }
}
