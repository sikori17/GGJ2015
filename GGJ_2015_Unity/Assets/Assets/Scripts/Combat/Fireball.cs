using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour {

	public float speed;
	public Vector3 dir;

	public Color color1, color2;
	public Oscillator flashOscillator;

	// Use this for initialization
	void Start () {
		flashOscillator.Restart();
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(dir * speed * Time.deltaTime);
		renderer.material.color = Color.Lerp(color1, color2, flashOscillator.Displacement());
	}

	public void Launch(Vector3 dir) {
		this.dir = dir;
	}

	void OnCollisionEnter(Collision other) {
		Destroy(gameObject);
	}
}
