using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameplayUI : MonoBehaviour {

	public static GameplayUI Instance;
	
	// Adventurer
	public Image heartOne;
	public Image heartTwo;
	public Image heartThree;
	public Scrollbar adventurerXP;

	// Dungeon Master
	public Room displayRoom;
	public Image effectImage;

	// Deck
	public RectTransform deck;
	public Scrollbar deckPoints;
	public Text draws;

	// Hand
	public RectTransform cardOne;
	public RectTransform cardTwo;
	public RectTransform cardThree;
	public RectTransform cardFour;

	public List<CardUI> cards;

	private Vector3 deckAnchor;
	private Vector3 cardOneAnchor;
	private Vector3 cardTwoAnchor;
	private Vector3 cardThreeAnchor;
	private Vector3 cardFourAnchor;

	// Times
	public float drawTime;

	// Image assets

	void Awake(){
		Instance = this;
		Debug.Log("1_" + cardOne.transform.position);
	}

	// Use this for initialization
	void Start () {
		InitializeDisplayRoom();
		Debug.Log("2_" + cardOne.transform.position);
		InitializeVariables();
		Debug.Log("3_" + cardOne.transform.position);
		Debug.Log("C " + cardOneAnchor);
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

	public void InitializeVariables(){
		deckAnchor = deck.transform.position;
		cardOneAnchor = cardOne.transform.position;
		cardTwoAnchor = cardTwo.transform.position;
		cardThreeAnchor = cardThree.transform.position;
		cardFourAnchor = cardFour.transform.position;
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
			effectImage.gameObject.SetActive(true);
		}
		else{
			displayRoom.gameObject.SetActive(false);
			effectImage.gameObject.SetActive(true);
		}

	}

	public void ClearCard(Button button){

		RectTransform target = null;

		if(button == Button.Xbox_A){
			target = cardOne;
		}
		else if(button == Button.Xbox_B){
			target = cardTwo;
		}
		else if(button == Button.Xbox_X){
			target = cardThree;
		}
		else if(button == Button.Xbox_Y){
			target = cardFour;
		}

		target.gameObject.SetActive(false);
		displayRoom.gameObject.SetActive(false);
		effectImage.gameObject.SetActive(false);
	}

	#region Animation

	public void AnimateDraw(Card card, Button button){

		RectTransform target = null;
		Vector3 targetPos = Vector2.zero;

		if(button == Button.Xbox_A){
			target = cardOne;
			targetPos = cardOneAnchor;
		}
		else if(button == Button.Xbox_B){
			target = cardTwo;
			targetPos = cardTwoAnchor;
		}
		else if(button == Button.Xbox_X){
			target = cardThree;
			targetPos = cardThreeAnchor;
		}
		else if(button == Button.Xbox_Y){
			target = cardFour;
			targetPos = cardFourAnchor;
		}

		target.gameObject.SetActive(true);
		StartCoroutine(Animate(target, deckAnchor, targetPos, drawTime));
	}

	public IEnumerator Animate(RectTransform rect, Vector3 startPos, Vector3 endPos, float time){
		Timer timer = new Timer(time);
		rect.transform.position = startPos;
		while(!timer.StopwatchDone()){
			Debug.Log("A " + rect.transform.position);
			rect.transform.position = Vector3.Lerp(startPos, endPos, timer.GetNormalizedTime());
			yield return null;
		}
		Debug.Log("K");
		rect.transform.position = endPos;
	}

	#endregion
}
