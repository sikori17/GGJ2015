using UnityEngine;
using System.Collections;

public class RoomHighlight : MonoBehaviour {

	public Color valid;
	public Color invalid;

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

	public void SetValid(){
		SetColor(valid);
	}

	public void SetInvalid(){
		SetColor(invalid);
	}

	public void SetColor(Color color){
		renderer.material.SetColor("_TintColor", color);
	}
}
