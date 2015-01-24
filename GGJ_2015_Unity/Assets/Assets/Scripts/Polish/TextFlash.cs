using UnityEngine;
using System.Collections;

public class TextFlash : MonoBehaviour {

	public Color[] colors;
	public ARLTimer cycleTimer;
	int index;

	// Use this for initialization
	void Start () {
		index = 0;
		GetComponent<TextMesh>().color = colors[index];
	}
	
	// Update is called once per frame
	void Update () {
		if (cycleTimer.IsDone()) {
			index = (index + 1) % colors.Length;
			GetComponent<TextMesh>().color = colors[index];
			cycleTimer.Restart();
		}
	}
}
