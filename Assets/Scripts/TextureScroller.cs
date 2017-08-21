using UnityEngine;
using System.Collections;

public class TextureScroller : MonoBehaviour {
    
	public float scrollSpeed = 0.5F;
    
	private Renderer rend;
	private Material mat;
	
    void Start() {
        rend = GetComponent<Renderer>();
		mat = rend.material;
    }
    void Update() {
        if (!rend) return;
		float offset = Time.time * scrollSpeed;
        mat.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}