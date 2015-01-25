using UnityEngine;
using System.Collections;

public class DestroyEnemyOnContact : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision other) {
		if (other.gameObject.GetComponent<Enemy>() != null) {
			other.gameObject.GetComponent<Enemy>().TakeDamage(100, DirectionHandler.Directions.Down);
		}
	}
}
