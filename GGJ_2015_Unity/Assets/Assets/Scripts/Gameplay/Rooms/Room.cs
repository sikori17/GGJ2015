using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour {

	public Wall northWall;
	public Wall southWall;
	public Wall eastWall;
	public Wall westWall;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Spawn(List<EnemyManager.EnemyTypes> enemies) {
		Vector3 roomPos = transform.position;
		Vector3 roomScale = new Vector3(Grid.Instance.roomWidth, 0, Grid.Instance.roomHeight);

		foreach (EnemyManager.EnemyTypes e in enemies) {
			GameObject newEnemy = Instantiate(EnemyManager.Instance.GetEnemyPrefab(e)) as GameObject;
			newEnemy.GetComponent<Enemy>().Spawn(roomPos, roomScale);
		}
	}

	public void ApplyCard(Card card){
		Debug.Log("GGG " + (card == null).ToString());
		if(card.format == CardFormat.Room){
			ApplyRoomCard(card);
		}
		else{
			ApplyEffectCard(card);
		}
	}

	public void ApplyRoomCard(Card card){
		ApplyWallConfiguration(card.wallTypes);
	}

	public void ApplyWallConfiguration(WallType[] wallTypes){
		northWall.ApplyType(wallTypes[0]);
		southWall.ApplyType(wallTypes[1]);
		eastWall.ApplyType(wallTypes[2]);
		westWall.ApplyType(wallTypes[3]);
	}

	public void ApplyEffectCard(Card card){

	}
}
