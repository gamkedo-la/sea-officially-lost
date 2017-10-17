using UnityEngine;

public class TestMapToggle : MonoBehaviour {
    public bool mapToggle;
    public GameObject map;
    [Range(0f, 1.0f)]
    public float mapTransparency = 0.5f;
    public UnityEngine.UI.RawImage mapImage;

	void Start () {
        mapImage.color = new Color(mapImage.color.r, mapImage.color.g, mapImage.color.b, mapTransparency);
        map.SetActive(mapToggle);
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.M)) {
            mapToggle = !mapToggle;
            map.SetActive(mapToggle);
        }

        if (Input.GetKey(KeyCode.Minus)) {
            mapTransparency = Mathf.Max(mapTransparency - Time.deltaTime * 0.5f, 0f);
            mapImage.color = new Color(mapImage.color.r, mapImage.color.g, mapImage.color.b, mapTransparency);
        }
        if (Input.GetKey(KeyCode.Equals)) {
            mapTransparency = Mathf.Min(mapTransparency + Time.deltaTime * 0.5f, 1f);
            mapImage.color = new Color(mapImage.color.r, mapImage.color.g, mapImage.color.b, mapTransparency);
        }
    }
}
