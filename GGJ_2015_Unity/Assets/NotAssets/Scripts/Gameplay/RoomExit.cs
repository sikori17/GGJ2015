using UnityEngine;
using System.Collections;

public class RoomExit : MonoBehaviour {

	public Room room;
	public DirectionHandler.Directions direction;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "Player"){
			Grid.Instance.PlayerExiting(room, direction);
		}
	}
}
