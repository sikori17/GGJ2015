using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour {

	public static EnemyManager Instance;

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
