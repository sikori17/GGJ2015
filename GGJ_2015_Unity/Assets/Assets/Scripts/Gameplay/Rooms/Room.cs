using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour {

	public int x;
	public int y;

	public Wall northWall;
	public Wall southWall;
	public Wall eastWall;
	public Wall westWall;

	public Transform cornersRoot;
	public Transform northEastCorner;
	public Transform southEastCorner;
	public Transform southWestCorner;
	public Transform northWestCorner;

	public bool locked;
	public List<Wall> lockedWalls;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AssignLocation(int x, int y){
		this.x = x;
		this.y = y;
	}

	public void Spawn(EnemyManager.EnemyTypes[] enemies) {
		Vector3 roomPos = transform.position;
		Vector3 roomScale = new Vector3(Grid.Instance.roomWidth, 0, Grid.Instance.roomHeight);

		foreach (EnemyManager.EnemyTypes e in enemies) {
			GameObject newEnemy = Instantiate(EnemyManager.Instance.GetEnemyPrefab(e)) as GameObject;
			newEnemy.GetComponent<Enemy>().Spawn(roomPos, roomScale);
		}
	}

	public void ApplyCard(Card card){

		if(card.format == CardFormat.Room){
			ApplyRoomCard(card);
		}
		else{
			ApplyEffectCard(card);
		}
	}

	public void ApplyRoomCard(Card card){
		ApplyWallConfiguration(card.wallTypes);
		Spawn(card.enemySpawnList);
	}

	public void ApplyWallConfiguration(WallType[] wallTypes){
		northWall.ApplyType(wallTypes[0]);
		southWall.ApplyType(wallTypes[1]);
		eastWall.ApplyType(wallTypes[2]);
		westWall.ApplyType(wallTypes[3]);
	}

	public void ApplyEffectCard(Card card){
		if (card.effect == Effect.SpawnEnemy) {
			Spawn(card.enemySpawnList);
		}
	}

	public void ResetWallScale(){

		//northWall.transform.parent = null;
		//northWall.transform.localScale = new Vector3(northWall.transform.localScale.x, 1, northWall.transform.localScale.z * (Grid.Instance.roomWidth / Grid.Instance.roomHeight));
		//northWall.transform.parent = transform;

		//southWall.transform.parent = null;
		//southWall.transform.localScale = new Vector3(Grid.Instance.roomWidth - 0.2f, 1, 1);
		//southWall.transform.parent = transform;

		//eastWall.transform.parent = null;
		eastWall.transform.localScale = new Vector3(eastWall.transform.localScale.x * (Grid.Instance.roomWidth / Grid.Instance.roomHeight), Grid.Instance.roomHeight - 1, 1);
		//eastWall.transform.parent = transform;

		westWall.transform.parent = null;
		westWall.transform.localScale = new Vector3(1, Grid.Instance.roomHeight - 0.2f, 1);
		westWall.transform.parent = transform;

		cornersRoot.parent = null;
		cornersRoot.localScale = Vector3.one;
		cornersRoot.parent = transform;

		northEastCorner.parent = null;
		northEastCorner.localScale = new Vector3(1, 1, 1);
		northEastCorner.transform.position = transform.position + new Vector3(Grid.Instance.roomWidth / 2.0f, 1, Grid.Instance.roomHeight / 2.0f);
		northEastCorner.transform.parent = cornersRoot;
	}

	public void OpenAllDoors(){
		northWall.ApplyType(WallType.Open);
		southWall.ApplyType(WallType.Open);
		eastWall.ApplyType(WallType.Open);
		westWall.ApplyType(WallType.Open);
	}

	public bool IsDoorOpen(DirectionHandler.Directions direction){
		if(direction == DirectionHandler.Directions.Left){
			return westWall.type == WallType.Open;
		}
		else if(direction == DirectionHandler.Directions.Right){
			return eastWall.type == WallType.Open;
		}
		else if(direction == DirectionHandler.Directions.Up){
			return northWall.type == WallType.Open;
		}
		else if(direction == DirectionHandler.Directions.Down){
			return southWall.type == WallType.Open;
		}
		else{
			return false;
		}
	}

	public void PlayerEntered(){
		if(false && locked){

			lockedWalls = new List<Wall>();

			if(northWall.type == WallType.Open){
				northWall.Lock();
				lockedWalls.Add(northWall);
			}

			if(southWall.type == WallType.Open){
				southWall.Lock();
				lockedWalls.Add(southWall);
			}

			if(eastWall.type == WallType.Open){
				eastWall.Lock();
				lockedWalls.Add(eastWall);
			}

			if(westWall.type == WallType.Open){
				westWall.Lock();
				lockedWalls.Add(westWall);
			}

			locked = false;
		}
	}
}
