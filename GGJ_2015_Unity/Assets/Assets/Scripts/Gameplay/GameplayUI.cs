using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameplayUI : MonoBehaviour {

	public static GameplayUI Instance;
	
	// Adventurer
	public Image heartOne;
	public Image heartTwo;
	public Image heartThree;
	public Scrollbar adventurerXP;

	// Dungeon Master
	public Scrollbar deckPoints;
	public Text draws;
	public Room displayRoom;
	public Image effectImage;

	// Hand
	public RectTransform cardOne;
	public RectTransform cardTwo;
	public RectTransform cardThree;
	public RectTransform cardFour;

	private Vector2 cardOneAnchor;
	private Vector2 cardTwoAnchor;
	private Vector2 cardThreeAnchor;
	private Vector2 cardFourAnchor;

	// Image assets

	void Awake(){
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		InitializeDisplayRoom();
		InitializeHandVariables();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void InitializeDisplayRoom(){
		displayRoom = Grid.Instance.GetNewRoom();
		displayRoom.gameObject.name = "Display_Room";
		displayRoom.gameObject.SetActive(true);
		displayRoom.transform.position = Grid.GetRoomPosition(Grid.Instance.roomsX - 1, Grid.Instance.roomsY - 1) + new Vector3(0, 0, -Grid.Instance.roomHeight);
		displayRoom.transform.localScale *= 0.9f;
	}

	public void InitializeHandVariables(){
		cardOneAnchor = cardOne.anchoredPosition;
		cardTwoAnchor = cardTwo.anchoredPosition;
		cardThreeAnchor = cardThree.anchoredPosition;
		cardFourAnchor = cardFour.anchoredPosition;
	}

	public void SetXP(float norm){
		adventurerXP.size = norm;
	}

	public void SetDeckPoints(float norm){
		deckPoints.size = norm;
	}

	public void SetDrawCount(int count){
		draws.text = count.ToString();
	}

	public void SetHealth(int health){
		if(health == 0){
			heartOne.gameObject.SetActive(false);
			heartTwo.gameObject.SetActive(false);
			heartThree.gameObject.SetActive(false);
		}
		else if(health == 1){
			heartOne.gameObject.SetActive(true);
			heartTwo.gameObject.SetActive(false);
			heartThree.gameObject.SetActive(false);
		}
		else if(health == 2){
			heartOne.gameObject.SetActive(true);
			heartTwo.gameObject.SetActive(true);
			heartThree.gameObject.SetActive(false);
		}
		else if(health == 3){
			heartOne.gameObject.SetActive(true);
			heartTwo.gameObject.SetActive(true);
			heartThree.gameObject.SetActive(true);
		}
	}

	public void SetDisplayRoom(Card card){
		if(card.format == CardFormat.Room){
			displayRoom.gameObject.SetActive(true);
			displayRoom.ApplyWallConfiguration(card.wallTypes);
		}
		else{
			displayRoom.gameObject.SetActive(false);
		}

	}
}
