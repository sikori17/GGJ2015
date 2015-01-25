using UnityEngine;
using System.Collections;

public class Treasure : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "Player"){
			DropLoot();
			GameObject.Destroy(gameObject);
		}
	}

	public void DropLoot(){

	}
}
