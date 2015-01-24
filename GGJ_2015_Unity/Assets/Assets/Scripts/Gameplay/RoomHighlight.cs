using UnityEngine;
using System.Collections;

public class RoomHighlight : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Resize();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Resize(){
		transform.localScale = new Vector3(Grid.Instance.roomWidth, Grid.Instance.roomHeight, 1);
	}
}
