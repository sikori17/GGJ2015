using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameplayUI : MonoBehaviour {

	public static GameplayUI Instance;
	
	// Adventurer
	public Image[] heartImages;
	public Scrollbar adventurerXP;
	public Image itemButtonA, itemButtonB;

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

	public Image promptA;
	public Image promptB;
	public Image promptX;
	public Image promptY;

	public RectTransform cardHighlight;

	// Times
	public float drawTime;

	// Image assets
	public Sprite[] cardImages;
	public Sprite treasureSprite;

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
		displayRoom.transform.localScale *= 0.75f;
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

		if (norm <= 0f) {
			adventurerXP.gameObject.SetActive(false);
		}
		else {
			adventurerXP.gameObject.SetActive(true);
		}
	}

	public void SetDeckPoints(float norm){
		deckPoints.size = norm;

		/*
		if (norm <= 0f) {
			deckPoints.gameObject.SetActive(false);
		}
		else {
			deckPoints.gameObject.SetActive(true);
		}
		*/
	}

	public void SetDrawCount(int count){
		draws.text = count.ToString();
	}

	public void SetHealth(int health){
		for (int i = 0; i < heartImages.Length; i++) {
			heartImages[i].gameObject.SetActive(i < health);
		}
	}

	public void SetDisplayRoom(Card card){
		if(card.format == CardFormat.Room){
			displayRoom.gameObject.SetActive(true);
			displayRoom.ApplyWallConfiguration(card.wallTypes);
			effectImage.gameObject.SetActive(false);
		}
		else{
			displayRoom.gameObject.SetActive(false);
			//effectImage.gameObject.SetActive(true);
		}

	}

	public void ClearCard(Button button){

		RectTransform target = null;
		Image prompt = null;

		if(button == Button.Xbox_A){
			target = cardOne;
			prompt = promptA;
		}
		else if(button == Button.Xbox_B){
			target = cardTwo;
			prompt = promptB;
		}
		else if(button == Button.Xbox_X){
			target = cardThree;
			prompt = promptX;
		}
		else if(button == Button.Xbox_Y){
			target = cardFour;
			prompt = promptY;
		}

		target.gameObject.SetActive(false);
		prompt.gameObject.SetActive(true);
		cardHighlight.gameObject.SetActive(false);
		ClearPreview();
	}

	public void ClearPreview(){
		displayRoom.gameObject.SetActive(false);
		effectImage.gameObject.SetActive(false);
	}

	public void TreasurePreview(){
		displayRoom.gameObject.SetActive(false);
		effectImage.gameObject.SetActive(true);
		effectImage.sprite = treasureSprite;
	}

	#region Animation

	public void HighlightCard(Button button){
		Vector3 targetPos = Vector3.zero;
		if(button == Button.Xbox_A){
			targetPos = cardOne.transform.position;
		}
		else if(button == Button.Xbox_B){
			targetPos = cardTwo.transform.position;
		}
		else if(button == Button.Xbox_X){
			targetPos = cardThree.transform.position;
		}
		else if(button == Button.Xbox_Y){
			targetPos = cardFour.transform.position;
		}
		cardHighlight.transform.position = targetPos;
		cardHighlight.gameObject.SetActive(true);
	}

	public void AnimateDraw(Card card, Button button){

		RectTransform target = null;
		Image prompt = null;
		Vector3 targetPos = Vector2.zero;

		if(button == Button.Xbox_A){
			target = cardOne;
			targetPos = cardOneAnchor;
			prompt = promptA;
		}
		else if(button == Button.Xbox_B){
			target = cardTwo;
			targetPos = cardTwoAnchor;
			prompt = promptB;
		}
		else if(button == Button.Xbox_X){
			target = cardThree;
			targetPos = cardThreeAnchor;
			prompt = promptX;
		}
		else if(button == Button.Xbox_Y){
			target = cardFour;
			targetPos = cardFourAnchor;
			prompt = promptY;
		}

		target.GetComponent<Image>().sprite = cardImages[(int) card.GetType()];
		prompt.gameObject.SetActive(false);
		target.gameObject.SetActive(true);
		StartCoroutine(Animate(target, deckAnchor, targetPos, drawTime));
	}

	public void InitCardImages(Card cardA, Card cardB, Card cardX, Card cardY) {
		cardOne.GetComponent<Image>().sprite = cardImages[(int) cardA.GetType()];
		cardTwo.GetComponent<Image>().sprite = cardImages[(int) cardB.GetType()];
		cardThree.GetComponent<Image>().sprite = cardImages[(int) cardX.GetType()];
		cardFour.GetComponent<Image>().sprite = cardImages[(int) cardY.GetType()];
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
