﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

	public static Grid Instance;

	public Room[,] Rooms;
	public Room roomPrefab;
	public Transform roomsRoot;

	private Vector3 gridAnchor;

	public int roomsX;
	public int roomsY;

	public float screenHeight;
	public float screenWidth;

	public float roomHeight;
	public float roomWidth;

	public GameObject topLeftCorner;
	public GameObject topRightCorner;
	public GameObject bottomLeftCorner;
	public GameObject bottomRightCorner;

	// BG
	public GameObject background;
	public Transform tilesRoot;
	public Transform[,] tiles;
	public Transform tilePrefab;

	//DEBUG
	public EnemyManager.EnemyTypes[] enemyArray;

	void Awake(){
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		CalculateScreenSize();
		InitializeRoomSize();
		AdjustScreenHeight(); // To accomodate UI row
		PositionCorners();
		PositionAndScaleBackground();
		//roomPrefab.ResetWallScale();
		SpawnRooms();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CalculateScreenSize(){
		
		screenHeight = Camera.main.orthographicSize * 2;
		screenWidth = screenHeight * Camera.main.aspect;
	}

	public void AdjustScreenHeight(){
		screenHeight -= roomHeight;
	}

	public void PositionCorners(){

		Vector3 cameraPosition = Camera.main.transform.position;

		gridAnchor = cameraPosition + (Vector3.forward * (screenHeight / 2.0f + (roomHeight / 2.0f))) - (Vector3.right * (screenWidth / 2.0f)) - (Vector3.up * Camera.main.transform.position.y);

		topLeftCorner.transform.position = gridAnchor;
		topRightCorner.transform.position = topLeftCorner.transform.position + Vector3.right * screenWidth;
		bottomLeftCorner.transform.position = topLeftCorner.transform.position - Vector3.forward * screenHeight;
		bottomRightCorner.transform.position = bottomLeftCorner.transform.position + Vector3.right * screenWidth;
	}

	public void PositionAndScaleBackground(){
		background.transform.position = ((topLeftCorner.transform.position + topRightCorner.transform.position + bottomLeftCorner.transform.position + bottomRightCorner.transform.position) / 4.0f) - Vector3.up * 10;
		background.transform.localScale = new Vector3(screenWidth, screenHeight, 1);
	}

	public void InitializeRoomSize(){

		roomPrefab = GameObject.Instantiate(roomPrefab) as Room;

		roomHeight = screenHeight / (roomsY + 1); // + 1 because of UI row
		roomWidth = screenWidth / roomsX;

		roomPrefab.transform.localScale = new Vector3(roomWidth, 1, roomHeight);
		tilePrefab.transform.localScale = new Vector3(roomWidth * 0.9f, roomHeight * 0.9f, 1);
	}

	public void SpawnRooms(){

		Rooms = new Room[roomsX, roomsY];
		Room room;

		tiles = new Transform[roomsX, roomsY];
		Transform tile;

		Vector3 roomPlacementAnchor = gridAnchor + new Vector3(roomWidth / 2.0f, 0, -roomHeight / 2.0f);

		for(int i = 0; i < roomsX; i++){
			for(int j = 0; j < roomsY; j++){
				// Room Setup
				room = GameObject.Instantiate(roomPrefab) as Room;
				room.transform.position = roomPlacementAnchor + new Vector3(i * roomWidth, 0, j * -roomHeight);
				room.gameObject.SetActive(false);
				room.transform.parent = roomsRoot;
				//room.Spawn(enemyArray);
				Rooms[i, j] = room;
				room.AssignLocation(i, j);
				// Tile Setup
				tile = GameObject.Instantiate(tilePrefab) as Transform;
				tile.transform.position = room.transform.position;
				tile.gameObject.SetActive(true);
				tile.transform.parent = tilesRoot;
				tiles[i, j] = tile;
			}
		}

		Rooms[1, 1].gameObject.SetActive(true);
		Rooms[1, 1].OpenAllDoors();
		tiles[1, 1].gameObject.SetActive(false);
	}

	public void PlayerExiting(Room room, DirectionHandler.Directions direction){
		if(direction == DirectionHandler.Directions.Right){
			if(LocationInBounds(room.x + 1, room.y) && !RoomActive(room.x + 1, room.y)){
				NewOpenRoom(room.x + 1, room.y, 3);
			}
		}
		else if(direction == DirectionHandler.Directions.Left){
			if(LocationInBounds(room.x - 1, room.y) && !RoomActive(room.x - 1, room.y)){
				NewOpenRoom(room.x - 1, room.y, 2);
			}
		}
		else if(direction == DirectionHandler.Directions.Up){
			if(LocationInBounds(room.x, room.y - 1) && !RoomActive(room.x, room.y - 1)){
				NewOpenRoom(room.x, room.y - 1, 1);
			}
		}
		else if(direction == DirectionHandler.Directions.Down){
			if(LocationInBounds(room.x, room.y + 1) && !RoomActive(room.x, room.y + 1)){
				NewOpenRoom(room.x, room.y + 1, 0);
			}
		}
	}

	public static Vector3 GetRoomPosition(int x, int y){
		return Instance.Rooms[x, y].transform.position;
	}

	public static Room GetRoom(int x, int y){
		return Instance.Rooms[x, y];
	}

	public Room GetNewRoom(){
		return GameObject.Instantiate(roomPrefab) as Room;
	}

	public void NewOpenRoom(int x, int y, int openWall){
		Room newRoom = Grid.GetRoom(x, y);
		WallType[] wallConfig = Card.GetRandomWallConfig();
		wallConfig[openWall] = WallType.Open;
		newRoom.ApplyWallConfiguration(wallConfig);
		newRoom.gameObject.SetActive(true);
	}

	public static Transform GetTile(int x, int y){
		return Instance.tiles[x, y];
	}

	public bool RoomActive(int x, int y){
		return (LocationInBounds(x, y) && GetRoom(x, y).gameObject.activeSelf);
	}

	public bool LocationInBounds(int x, int y){
		return (x >= 0 && x < roomsX) && (y >= 0 && y < roomsY);
	}
}
