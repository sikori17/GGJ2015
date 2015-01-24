using UnityEngine;
using System.Collections;

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

	void Awake(){
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		CalculateScreenSize();
		InitializeRoomSize();
		AdjustScreenHeight(); // To accomodate UI row
		PositionCorners();
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

	public void InitializeRoomSize(){

		roomPrefab = GameObject.Instantiate(roomPrefab) as Room;

		roomHeight = screenHeight / (roomsY + 1); // + 1 because of UI row
		roomWidth = screenWidth / roomsX;

		roomPrefab.transform.localScale = new Vector3(roomWidth, 1, roomHeight);
	}

	public void SpawnRooms(){

		Rooms = new Room[roomsX, roomsY];
		Room room;

		Vector3 roomPlacementAnchor = gridAnchor + new Vector3(roomWidth / 2.0f, 0, -roomHeight / 2.0f);

		for(int i = 0; i < roomsX; i++){
			for(int j = 0; j < roomsY; j++){
				room = GameObject.Instantiate(roomPrefab) as Room;
				room.transform.position = roomPlacementAnchor + new Vector3(i * roomWidth, 0, j * -roomHeight);
				room.gameObject.SetActive(true);
				room.transform.parent = roomsRoot;
				Rooms[i, j] = room;
			}
		}
	}

	public static Vector3 GetRoomPosition(int x, int y){
		return Instance.Rooms[x, y].transform.position;
	}
}
