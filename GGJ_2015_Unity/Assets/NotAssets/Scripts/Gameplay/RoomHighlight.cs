using UnityEngine;
using System.Collections;

public class RoomHighlight : MonoBehaviour {

	public static RoomHighlight Instance;

	public Color valid;
	public Color invalid;
	public Color neutral;

	public GameObject death;

	// Use this for initialization
	void Start () {
		Instance = this;
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

	public void SetNeutral(){
		SetColor(neutral);
	}

	public void SetColor(Color color){
		renderer.material.SetColor("_TintColor", color);
	}
}
