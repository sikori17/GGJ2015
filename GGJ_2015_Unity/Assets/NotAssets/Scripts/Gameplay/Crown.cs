using UnityEngine;
using System.Collections;

public class Crown : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "Player"){
			// End game
			Application.LoadLevel("RestartScene_AdventurerWin");
		}
	}
}
