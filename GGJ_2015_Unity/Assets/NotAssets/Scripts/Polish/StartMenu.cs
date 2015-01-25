using UnityEngine;
using System.Collections;

public class StartMenu : MonoBehaviour {

	public float zStartPos;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Avatar.Instance.transform.position.z > zStartPos) {
			Application.LoadLevel("DevScene");
		}
	}
}
