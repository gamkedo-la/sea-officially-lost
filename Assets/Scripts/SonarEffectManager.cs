using UnityEngine;

public class SonarEffectManager : MonoBehaviour {
    public SonarScanEffect[] scanners;
    public Camera cam;
    private bool rendered;
    private RenderTexture[] textures;
    private Resolution res;

    private void Start() {
        res = Screen.currentResolution;

        textures = new RenderTexture[scanners.Length];
        InitializeTextures();
    }

    private void OnEnable() {
        cam.depthTextureMode = DepthTextureMode.Depth;
    }

    private void Update() {
        // If the screen resolution changes, modify the scan textures to match it
        if(!res.Equals(Screen.currentResolution)) {
            res = Screen.currentResolution;
            InitializeTextures();
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        textures[0] = source;
        int texIndex = 0;

        int scanCount = 0;
        foreach (SonarScanEffect scanner in scanners) {
            scanCount += scanner.Scanning ? 1 : 0;
        }

        if (scanCount > 0) {
            for (int i = 0; i < scanners.Length; i++) {
                if (scanners[i].Scanning && scanCount > 1) {
                    RaycastCornerBlit(textures[texIndex], textures[texIndex + 1], scanners[i].EffectMaterial);

                    scanCount--;
                    texIndex++;
                }
                if (scanners[i].Scanning && scanCount == 1) {
                    RaycastCornerBlit(textures[texIndex], destination, scanners[i].EffectMaterial);
                }
            }
        }
        else {
            Graphics.Blit(source, destination);
        }
    }

    // Custom blit courtesy of Dan Moran of "Makin' Stuff Look Good in Unity"
    void RaycastCornerBlit(RenderTexture source, RenderTexture dest, Material mat) {
        // Compute Frustum Corners
        float camFar = cam.farClipPlane;
        float camFov = cam.fieldOfView;
        float camAspect = cam.aspect;

        float fovWHalf = camFov * 0.5f;

        Vector3 toRight = cam.transform.right * Mathf.Tan(fovWHalf * Mathf.Deg2Rad) * camAspect;
        Vector3 toTop = cam.transform.up * Mathf.Tan(fovWHalf * Mathf.Deg2Rad);

        Vector3 topLeft = (cam.transform.forward - toRight + toTop);
        float camScale = topLeft.magnitude * camFar;

        topLeft.Normalize();
        topLeft *= camScale;

        Vector3 topRight = (cam.transform.forward + toRight + toTop);
        topRight.Normalize();
        topRight *= camScale;

        Vector3 bottomRight = (cam.transform.forward + toRight - toTop);
        bottomRight.Normalize();
        bottomRight *= camScale;

        Vector3 bottomLeft = (cam.transform.forward - toRight - toTop);
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

    private void InitializeTextures() {
        for (int i = 1; i < textures.Length; i++) {
            textures[i] = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
            textures[i].Create();
        }
    }
}
