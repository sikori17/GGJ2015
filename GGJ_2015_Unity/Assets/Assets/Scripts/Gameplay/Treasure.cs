using UnityEngine;
using System.Collections;

public class Treasure : MonoBehaviour {

	public GameObject[] items;

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
		GameObject item = GameObject.Instantiate(items[Random.Range(0, items.Length)]) as GameObject;
		item.transform.position = transform.position;
	}
}
