using UnityEngine;

public class SonarScanEffect : MonoBehaviour {
    public Transform scannerOrigin;
    public Material effectMaterial;
    private Material localEffectMaterial;
    public float maxScanDistance;
    public float scanDistance;
    public float scanSpeed;
    public float scanWidth;
    public float cooldownTime;
    private float cooldownCounter;

    private bool scanning;
    private float timeElapsed;

    public Material EffectMaterial {
        get {
            return localEffectMaterial;
        }
    }

    public bool Scanning {
        get {
            return scanning;
        }
    }

    void Update() {
        if (scanning) {
            localEffectMaterial.SetVector("_WorldSpaceScannerPos", scannerOrigin.position);
            localEffectMaterial.SetFloat("_ScanDistance", scanDistance);
            scanDistance += Time.deltaTime * scanSpeed;
            //            scanDistance += Time.deltaTime * timeElapsed * scanSpeed;
            //		timeElapsed += Time.deltaTime;
            if (scanDistance >= maxScanDistance + scanWidth) {
                scanning = false;
                scanDistance = 0;
            }
        }

        if(cooldownCounter > 0) {
            cooldownCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.C) && cooldownCounter <= 0) {
            Scan();
        }
    }

    void OnEnable() {
        if (scannerOrigin == null) {
            scannerOrigin = transform;
        }

        cooldownCounter = 0;

        localEffectMaterial = Material.Instantiate(effectMaterial);
    }

    public void Scan() {
        cooldownCounter = cooldownTime;
        scanning = true;
        scanDistance = 0;
        timeElapsed = 0.0f;

        localEffectMaterial.SetFloat("_ScanWidth", scanWidth);
        localEffectMaterial.SetFloat("_Range", 1 / maxScanDistance);

        localEffectMaterial.SetVector("_WorldSpaceScannerPos", scannerOrigin.position);
        localEffectMaterial.SetFloat("_ScanDistance", scanDistance);
    }
}
