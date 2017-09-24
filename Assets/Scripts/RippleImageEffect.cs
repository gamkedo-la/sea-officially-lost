using UnityEngine;

[ExecuteInEditMode]
public class RippleImageEffect : MonoBehaviour {
    public Material rippleEffect;
    private Vector4 rippleProperties;
    private float rippleRadius;
    private float rippleWidth = 40f;
    public float rippleSpeed = 0.01f;
    private bool rippleActive = false;
    public float rippleDuration;
    private float rippleTimer;
    private float rippleStrength = 0.03f;

    private void Start() {
        rippleRadius = 0;
        rippleProperties = new Vector4(0.5f, 0.5f, rippleRadius, rippleWidth);
    }

    private void Update() {
        if (rippleActive) {
            rippleTimer += Time.deltaTime;
            rippleProperties.z += Time.deltaTime * rippleSpeed;
            rippleEffect.SetVector("_CircleProperties", rippleProperties);
            rippleEffect.SetFloat("_EffectPower", Mathf.Cos((rippleTimer / rippleDuration)*Mathf.PI/2) * rippleStrength);
            if (rippleTimer > rippleDuration) {
                rippleActive = false;
                rippleEffect.SetFloat("_EffectPower", 0);
            }
        }
    }

    public void Ripple(RectTransform rect) {
        Vector2 rectPosition = Camera.main.WorldToScreenPoint(rect.position);
        rectPosition.x /= Camera.main.pixelWidth;
        rectPosition.y /= Camera.main.pixelHeight;

        rippleRadius = 0;
        rippleProperties = new Vector4(rectPosition.x, rectPosition.y, rippleRadius, rippleWidth);
        rippleEffect.SetVector("_CircleProperties", rippleProperties);
        rippleEffect.SetFloat("_EffectPower", rippleStrength);
        rippleActive = true;
        rippleTimer = 0;
    }

    public void Ripple(float x, float y) {
        rippleRadius = 0;
        rippleProperties = new Vector4(x, y, rippleRadius, rippleWidth);
        rippleEffect.SetVector("_CircleProperties", rippleProperties);
        rippleEffect.SetFloat("_EffectPower", rippleStrength);
        rippleActive = true;
        rippleTimer = 0;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        Graphics.Blit(source, destination, rippleEffect);
    }
}
