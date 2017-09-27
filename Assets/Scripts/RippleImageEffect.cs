using UnityEngine;

[ExecuteInEditMode]
public class RippleImageEffect : MonoBehaviour {
    public Material rippleMaterial;
    private Material rippleMaterialInstance;
    private Vector4 rippleProperties;
    private float rippleRadius;
    private float rippleWidth = 40f;
    public float rippleSpeed = 0.01f;
    private bool rippleActive = false;
    public float rippleDuration;
    private float rippleTimer;
    public float maxRippleStrength = 0.03f;
    private float currentRippleStrength;

    private void Start() {
        rippleMaterialInstance = new Material(rippleMaterial);

        StopRipple();
    }

    private void Update() {
        if (rippleActive) {
            rippleTimer += Time.deltaTime;
            rippleProperties.z += Time.deltaTime * rippleSpeed;

            currentRippleStrength = Mathf.Cos((rippleTimer / rippleDuration) * Mathf.PI / 2) * maxRippleStrength;

            rippleMaterialInstance.SetVector("_CircleProperties", rippleProperties);
            rippleMaterialInstance.SetFloat("_EffectPower", currentRippleStrength);

            if (rippleTimer > rippleDuration) {
                StopRipple();
            }
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        Graphics.Blit(source, destination, rippleMaterialInstance);
    }

    public void Ripple(RectTransform rect) {
        Vector2 rectPosition = Camera.main.WorldToScreenPoint(rect.position);
        rectPosition.x /= Camera.main.pixelWidth;
        rectPosition.y /= Camera.main.pixelHeight;

        Ripple(rectPosition.x, rectPosition.y);
    }

    public void Ripple(float x, float y) {
        rippleRadius = 0;

        rippleProperties = new Vector4(x, y, rippleRadius, rippleWidth);

        rippleMaterialInstance.SetVector("_CircleProperties", rippleProperties);
        rippleMaterialInstance.SetFloat("_EffectPower", maxRippleStrength);

        rippleActive = true;
        rippleTimer = 0;
    }

    private void StopRipple() {
        rippleActive = false;
        rippleRadius = 0;

        rippleMaterialInstance.SetFloat("_EffectPower", 0);
        rippleProperties = new Vector4(0.5f, 0.5f, rippleRadius, rippleWidth);
    }
}
