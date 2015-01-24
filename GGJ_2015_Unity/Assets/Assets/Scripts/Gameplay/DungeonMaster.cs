using UnityEngine;
using System.Collections;

public class DungeonMaster : MonoBehaviour {

	public int playerNum;

	SimpleStateMachine stateMachine;
	private SimpleState IdleState;

	SimpleStateMachine deckMachine;
	private SimpleState DeckIdleState;
	private SimpleState DrawCardState;
	private SimpleState CardSelectedState;

	public int selectionX;
	public int selectionY;

	public RoomHighlight highlight;

	public Deck deck;
	public Hand hand;

	public Card selectedCard;
	public Button selectedButton;

	// Use this for initialization
	void Start () {
		InitializeStates();
	}

	public void InitializeStates(){

		stateMachine = new SimpleStateMachine();
		IdleState = new SimpleState(IdleEnter, IdleUpdate, IdleExit, "IDLE");
		stateMachine.SwitchStates(IdleState);

		deckMachine = new SimpleStateMachine();
		DeckIdleState = new SimpleState(DeckIdleEnter, DeckIdleUpdate, DeckIdleExit, "DECK_IDLE");
		CardSelectedState = new SimpleState(CardSelectedEnter, CardSelectedUpdate, CardSelectedExit, "CARD_SELECTED");
		deckMachine.SwitchStates(DeckIdleState);
	}
	
	// Update is called once per frame
	void Update () {
		stateMachine.Execute();
		deckMachine.Execute();
	}

	public void PlayCard(Card card){
		Room room = Grid.GetRoom(selectionX, selectionY);
		room.ApplyCard(card);
		room.gameObject.SetActive(true);
		Grid.GetTile(selectionX, selectionY).gameObject.SetActive(false);
	}

	#region Idle

	public void IdleEnter(){

	}

	public void IdleUpdate(){

		if(ControllerInput.Dpad_ButtonDown(playerNum, Dpad.Xbox_Left)) selectionX = (selectionX - 1) % Grid.Instance.roomsX;
		if(ControllerInput.Dpad_ButtonDown(playerNum, Dpad.Xbox_Right)) selectionX = (selectionX + 1) % Grid.Instance.roomsX;
		if(ControllerInput.Dpad_ButtonDown(playerNum, Dpad.Xbox_Up)) selectionY = (selectionY - 1) % Grid.Instance.roomsY;
		if(ControllerInput.Dpad_ButtonDown(playerNum, Dpad.Xbox_Down)) selectionY = (selectionY + 1) % Grid.Instance.roomsY;

		if(selectionX < 0) selectionX = Grid.Instance.roomsX - 1;
		if(selectionY < 0) selectionY = Grid.Instance.roomsY - 1;

		highlight.transform.position = Grid.GetRoomPosition(selectionX, selectionY) + Vector3.up * 5;

	}

	public void IdleExit(){

	}

	#endregion

	#region DeckIdle

	public void DeckIdleEnter(){

	}

	public void DeckIdleUpdate(){
		Debug.Log("Q");

		if(ControllerInput.ButtonDown(playerNum, Button.Xbox_A)){
			if(hand.CardAvailable(Button.Xbox_A)){
				selectedButton = Button.Xbox_A;
				selectedCard = hand.GetCard(Button.Xbox_A);
				deckMachine.SwitchStates(CardSelectedState);
			}
			else{
				selectedButton = Button.Xbox_A;
				Debug.Log("F");
				hand.DrawCard(selectedButton);
				//stateMachine.SwitchStates(DrawCardState);
			}
		}
		else if(ControllerInput.ButtonDown(playerNum, Button.Xbox_B)){
			if(hand.CardAvailable(Button.Xbox_B)){
				selectedButton = Button.Xbox_B;
				selectedCard = hand.GetCard(Button.Xbox_B);
				//stateMachine.SwitchStates(CardSelectedState);
			}
			else{
				
			}
		}
		else if(ControllerInput.ButtonDown(playerNum, Button.Xbox_X)){
			if(hand.CardAvailable(Button.Xbox_X)){
				selectedButton = Button.Xbox_X;
				selectedCard = hand.GetCard(Button.Xbox_X);
				//stateMachine.SwitchStates(CardSelectedState);
			}
			else{
				
			}
		}
		else if(ControllerInput.ButtonDown(playerNum, Button.Xbox_Y)){
			if(hand.CardAvailable(Button.Xbox_Y)){
				selectedButton = Button.Xbox_Y;
				selectedCard = hand.GetCard(Button.Xbox_Y);
				//stateMachine.SwitchStates(CardSelectedState);
			}
			else{
				
			}
		}
	}

	public void DeckIdleExit(){

	}

	#endregion

	#region CardSelectedState

	public void CardSelectedEnter(){

	}

	public void CardSelectedUpdate(){
		if(ControllerInput.ButtonDown(playerNum, Button.Xbox_A)){
			if(selectedButton == Button.Xbox_A){
				PlayCard(selectedCard);
				hand.RemoveCard(Button.Xbox_A);
				GameplayUI.Instance.ClearCard(Button.Xbox_A);
				deckMachine.SwitchStates(DeckIdleState);
			}
			else if(hand.CardAvailable(Button.Xbox_A)){
				selectedButton = Button.Xbox_A;
				selectedCard = hand.GetCard(Button.Xbox_A);
			}
			else{
				hand.DrawCard(selectedButton);
			}
		}
		else if(ControllerInput.ButtonDown(playerNum, Button.Xbox_B)){
			if(selectedButton == Button.Xbox_B){
				
			}
			else if(hand.CardAvailable(Button.Xbox_B)){
				selectedButton = Button.Xbox_B;
				selectedCard = hand.GetCard(Button.Xbox_B);
			}
			else{
				hand.DrawCard(selectedButton);
			}
		}
		else if(ControllerInput.ButtonDown(playerNum, Button.Xbox_X)){
			if(selectedButton == Button.Xbox_X){
				
			}
			else if(hand.CardAvailable(Button.Xbox_X)){
				selectedButton = Button.Xbox_X;
				selectedCard = hand.GetCard(Button.Xbox_X);
			}
			else{
				hand.DrawCard(selectedButton);
			}
		}
		else if(ControllerInput.ButtonDown(playerNum, Button.Xbox_Y)){
			if(selectedButton == Button.Xbox_Y){
				
			}
			else if(hand.CardAvailable(Button.Xbox_Y)){
				selectedButton = Button.Xbox_Y;
				selectedCard = hand.GetCard(Button.Xbox_Y);
			}
			else{
				hand.DrawCard(selectedButton);
			}
		}
	}

	public void CardSelectedExit(){

	}

	#endregion

	#region DrawCardState

	public void DrawCardEnter(){
		GameplayUI.Instance.AnimateDraw(selectedCard, selectedButton);
	}

	public void DrawCardUpdate(){

	}

	public void DrawCardExit(){

	}

	#endregion
}
