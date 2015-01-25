using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	public float startSize, endSize;
	public Color startColor, endColor;
	public ARLTimer timer;

	// Use this for initialization
	void Start () {
		transform.localScale = Vector3.one * startSize;
		renderer.material.color = startColor;

		timer.Restart();
	}
	
	// Update is called once per frame
	void Update () {
		transform.localScale = Vector3.one * Mathf.Lerp(startSize, endSize, timer.PercentDone());
		renderer.material.color = Color.Lerp(startColor, endColor, timer.PercentDone());

		if (timer.IsDone()) {
			Destroy(gameObject);
		}
	}
}
