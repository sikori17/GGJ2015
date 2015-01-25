using UnityEngine;
using System.Collections;

public class RoomEnter : MonoBehaviour {

	public Room room;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "Player"){
			room.PlayerEntered();
		}
	}
}
