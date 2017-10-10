using UnityEngine;

[ExecuteInEditMode]
public class SonarScanEffect : MonoBehaviour {

    public Transform scannerOrigin;
    public Material effectMaterial;
    private Material localEffectMaterial;
    public float maxScanDistance; //TODO make this variable meaningful
    private float scanDistance;
    public float scanSpeed;
    public float scanWidth;
    public float cooldownTime;
    private float cooldownCounter;

    private Camera _camera;
    private bool _scanning;
    private float TimeElapsed;

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        if (_scanning) {
            scanDistance += Time.deltaTime * scanSpeed;
            //            ScanDistance += Time.deltaTime * TimeElapsed * scanSpeed;
            //		TimeElapsed += Time.deltaTime;
            if (scanDistance > 1000) {
                _scanning = false;
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
        cooldownCounter = 0;

        localEffectMaterial = Material.Instantiate(effectMaterial);

        localEffectMaterial.SetFloat("_ScanWidth", scanWidth);

        _camera = GetComponent<Camera>();
        _camera.depthTextureMode = DepthTextureMode.Depth;
        if (scannerOrigin == null) {
            scannerOrigin = _camera.transform;
        }
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst) {
        localEffectMaterial.SetVector("_WorldSpaceScannerPos", scannerOrigin.position);
        localEffectMaterial.SetFloat("_ScanDistance", scanDistance);
        RaycastCornerBlit(src, dst, localEffectMaterial);
    }


    void RaycastCornerBlit(RenderTexture source, RenderTexture dest, Material mat) {
        // Compute Frustum Corners
        float camFar = _camera.farClipPlane;
        float camFov = _camera.fieldOfView;
        float camAspect = _camera.aspect;

        float fovWHalf = camFov * 0.5f;

        Vector3 toRight = _camera.transform.right * Mathf.Tan(fovWHalf * Mathf.Deg2Rad) * camAspect;
        Vector3 toTop = _camera.transform.up * Mathf.Tan(fovWHalf * Mathf.Deg2Rad);

        Vector3 topLeft = (_camera.transform.forward - toRight + toTop);
        float camScale = topLeft.magnitude * camFar;

        topLeft.Normalize();
        topLeft *= camScale;

        Vector3 topRight = (_camera.transform.forward + toRight + toTop);
        topRight.Normalize();
        topRight *= camScale;

        Vector3 bottomRight = (_camera.transform.forward + toRight - toTop);
        bottomRight.Normalize();
        bottomRight *= camScale;

        Vector3 bottomLeft = (_camera.transform.forward - toRight - toTop);
        bottomLeft.Normalize();
        bottomLeft *= camScale;

        // Custom Blit, encoding Frustum Corners as additional Texture Coordinates
        RenderTexture.active = dest;

        mat.SetTexture("_MainTex", source);

        GL.PushMatrix();
        GL.LoadOrtho();

        mat.SetPass(0);

        GL.Begin(GL.QUADS);

        GL.MultiTexCoord2(0, 0.0f, 0.0f);
        GL.MultiTexCoord(1, bottomLeft);
        GL.Vertex3(0.0f, 0.0f, 0.0f);

        GL.MultiTexCoord2(0, 1.0f, 0.0f);
        GL.MultiTexCoord(1, bottomRight);
        GL.Vertex3(1.0f, 0.0f, 0.0f);

        GL.MultiTexCoord2(0, 1.0f, 1.0f);
        GL.MultiTexCoord(1, topRight);
        GL.Vertex3(1.0f, 1.0f, 0.0f);

        GL.MultiTexCoord2(0, 0.0f, 1.0f);
        GL.MultiTexCoord(1, topLeft);
        GL.Vertex3(0.0f, 1.0f, 0.0f);

        GL.End();
        GL.PopMatrix();
    }

    public void Scan() {
        cooldownCounter = cooldownTime;
        _scanning = true;
        scanDistance = 0;
        TimeElapsed = 0.0f;
    }

}
