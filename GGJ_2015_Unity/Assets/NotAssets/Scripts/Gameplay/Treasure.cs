using UnityEngine;
using System.Collections;

public class Treasure : MonoBehaviour {

	public GameObject[] items;
	public Room room;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "Player"){
			DropLoot();
			room.hasTreasure = false;
			GameObject.Destroy(gameObject);
		}
	}

	public void DropLoot(){
		GameObject item = GameObject.Instantiate(items[Random.Range(0, items.Length)]) as GameObject;
		item.name = item.name.Remove(item.name.Length - 7, 7);
		item.transform.position = transform.position;
	}

	public void AssignRoom(Room room){
		this.room = room;
	}
}
