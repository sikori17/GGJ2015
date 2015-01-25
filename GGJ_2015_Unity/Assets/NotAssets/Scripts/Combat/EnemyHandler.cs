using UnityEngine;
using System.Collections;

public class EnemyHandler : MonoBehaviour {
	
	public static EnemyHandler Instance;
	
	public enum EnemyTypes { Slug, Wizard, Knight };
	
	public GameObject[] enemyPrefabs;
	
	// Use this for initialization
	void Awake () {
		Instance = this;
	}
	
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public GameObject GetEnemyPrefab (EnemyTypes type) {
		return enemyPrefabs[(int) type];
	}
}
