using UnityEngine;
using System.Collections;

public enum WallType{
	Open,
	Closed,
	Key,
	Length
}

public class Wall : MonoBehaviour {

	public WallType type;

	public GameObject closedWall;
	public GameObject openWall;
	public GameObject lockedDoor;

	public RoomExit exit;

	public void ApplyType(WallType type){

		this.type = type;

		if(type == WallType.Open){
			closedWall.gameObject.SetActive(false);
			openWall.gameObject.SetActive(true);
			lockedDoor.gameObject.SetActive(false);
			exit.gameObject.SetActive(true);
		}
		else if(type == WallType.Closed){
			openWall.gameObject.SetActive(false);
			closedWall.gameObject.SetActive(true);
			lockedDoor.gameObject.SetActive(false);
			exit.gameObject.SetActive(false);
		}
		else if(type == WallType.Key){
			closedWall.gameObject.SetActive(false);
			openWall.gameObject.SetActive(true);
			lockedDoor.gameObject.SetActive(true);
			exit.gameObject.SetActive(false);
		}
	}

	public void Lock(){
		if(type == WallType.Open){
			ApplyType(WallType.Key);
		}
	}
}
