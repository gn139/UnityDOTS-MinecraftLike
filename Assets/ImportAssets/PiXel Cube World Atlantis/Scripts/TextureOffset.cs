using UnityEngine;
using System.Collections;

public class TextureOffset: MonoBehaviour {
	public float scrollSpeed = 0.5F;
	public Renderer rend;
	void Start() {
		rend = GetComponent<Renderer>();
	}
	void Update() {
		float offset = Time.time * scrollSpeed;
		rend.material.SetTextureOffset("_MainTex", new Vector2(0,offset));
	}
}