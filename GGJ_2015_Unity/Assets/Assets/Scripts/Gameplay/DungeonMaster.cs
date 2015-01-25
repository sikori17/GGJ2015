using UnityEngine;
using System.Collections;

public class DungeonMaster : MonoBehaviour {

	public static DungeonMaster Instance;
	public int playerNum;

	SimpleStateMachine stateMachine;
	private SimpleState IdleState;

	SimpleStateMachine deckMachine;
	private SimpleState DeckIdleState;
	private SimpleState DrawCardState;
	private SimpleState CardSelectedState;
	private SimpleState PlaceTreasureState;

	public int selectionX;
	public int selectionY;

	public RoomHighlight highlight;

	public Deck deck;
	public Hand hand;

	public Card selectedCard;
	public Button selectedButton;

	public Treasure treasurePrefab;

	void Awake(){
		Instance = this;
	}

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
		PlaceTreasureState = new SimpleState(PlaceTreasureEnter, PlaceTreasureUpdate, PlaceTreasureExit, "PLACE_TREASURE");
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

		if(ControllerInput.ButtonDown(playerNum, Button.Xbox_A)){
			if(hand.CardAvailable(Button.Xbox_A)){
				selectedButton = Button.Xbox_A;
				selectedCard = hand.GetCard(Button.Xbox_A);
				GameplayUI.Instance.SetDisplayRoom(selectedCard);
				deckMachine.SwitchStates(CardSelectedState);
			}
			else{
				selectedButton = Button.Xbox_A;
				hand.DrawCard(selectedButton);
				//stateMachine.SwitchStates(DrawCardState);
			}
		}
		else if(ControllerInput.ButtonDown(playerNum, Button.Xbox_B)){
			if(hand.CardAvailable(Button.Xbox_B)){
				selectedButton = Button.Xbox_B;
				selectedCard = hand.GetCard(Button.Xbox_B);
				GameplayUI.Instance.SetDisplayRoom(selectedCard);
				deckMachine.SwitchStates(CardSelectedState);
				//stateMachine.SwitchStates(CardSelectedState);
			}
			else{
				selectedButton = Button.Xbox_B;
				hand.DrawCard(selectedButton);
			}
		}
		else if(ControllerInput.ButtonDown(playerNum, Button.Xbox_X)){
			if(hand.CardAvailable(Button.Xbox_X)){
				selectedButton = Button.Xbox_X;
				selectedCard = hand.GetCard(Button.Xbox_X);
				GameplayUI.Instance.SetDisplayRoom(selectedCard);
				deckMachine.SwitchStates(CardSelectedState);
				//stateMachine.SwitchStates(CardSelectedState);
			}
			else{
				selectedButton = Button.Xbox_X;
				hand.DrawCard(selectedButton);
			}
		}
		else if(ControllerInput.ButtonDown(playerNum, Button.Xbox_Y)){
			if(hand.CardAvailable(Button.Xbox_Y)){
				selectedButton = Button.Xbox_Y;
				selectedCard = hand.GetCard(Button.Xbox_Y);
				GameplayUI.Instance.SetDisplayRoom(selectedCard);
				deckMachine.SwitchStates(CardSelectedState);
				//stateMachine.SwitchStates(CardSelectedState);
			}
			else{
				selectedButton = Button.Xbox_Y;
				hand.DrawCard(selectedButton);
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
				Debug.Log("LV "  + LocationValid());
				Grid.GetRoom(selectionX, selectionY).ApplyWallConfiguration(selectedCard.wallTypes);
				if(LocationValid()){
					PlayCard(selectedCard);
					hand.RemoveCard(Button.Xbox_A);
					GameplayUI.Instance.ClearCard(Button.Xbox_A);
					deckMachine.SwitchStates(DeckIdleState);
				}
			}
			else if(hand.CardAvailable(Button.Xbox_A)){
				selectedButton = Button.Xbox_A;
				selectedCard = hand.GetCard(Button.Xbox_A);
				GameplayUI.Instance.HighlightCard(Button.Xbox_A);
				GameplayUI.Instance.SetDisplayRoom(selectedCard);
			}
			else{
				hand.DrawCard(selectedButton);
			}
		}
		if(ControllerInput.ButtonDown(playerNum, Button.Xbox_B)){
			if(selectedButton == Button.Xbox_B){
				Debug.Log("LV "  + LocationValid());
				Grid.GetRoom(selectionX, selectionY).ApplyWallConfiguration(selectedCard.wallTypes);
				if(LocationValid()){
					PlayCard(selectedCard);
					hand.RemoveCard(Button.Xbox_B);
					GameplayUI.Instance.ClearCard(Button.Xbox_B);
					deckMachine.SwitchStates(DeckIdleState);
				}
			}
			else if(hand.CardAvailable(Button.Xbox_B)){
				selectedButton = Button.Xbox_B;
				selectedCard = hand.GetCard(Button.Xbox_B);
				GameplayUI.Instance.HighlightCard(Button.Xbox_B);
				GameplayUI.Instance.SetDisplayRoom(selectedCard);
			}
			else{
				hand.DrawCard(selectedButton);
			}
		}
		if(ControllerInput.ButtonDown(playerNum, Button.Xbox_X)){
			if(selectedButton == Button.Xbox_X){
				Debug.Log("LV "  + LocationValid());
				Grid.GetRoom(selectionX, selectionY).ApplyWallConfiguration(selectedCard.wallTypes);
				if(LocationValid()){
					PlayCard(selectedCard);
					hand.RemoveCard(Button.Xbox_X);
					GameplayUI.Instance.ClearCard(Button.Xbox_X);
					deckMachine.SwitchStates(DeckIdleState);
				}
			}
			else if(hand.CardAvailable(Button.Xbox_X)){
				selectedButton = Button.Xbox_X;
				selectedCard = hand.GetCard(Button.Xbox_X);
				GameplayUI.Instance.HighlightCard(Button.Xbox_X);
				GameplayUI.Instance.SetDisplayRoom(selectedCard);
			}
			else{
				hand.DrawCard(selectedButton);
			}
		}
		if(ControllerInput.ButtonDown(playerNum, Button.Xbox_Y)){
			if(selectedButton == Button.Xbox_Y){
				Debug.Log("LV "  + LocationValid());
				Grid.GetRoom(selectionX, selectionY).ApplyWallConfiguration(selectedCard.wallTypes);
				if(LocationValid()){
					PlayCard(selectedCard);
					hand.RemoveCard(Button.Xbox_Y);
					GameplayUI.Instance.ClearCard(Button.Xbox_Y);
					deckMachine.SwitchStates(DeckIdleState);
				}
			}
			else if(hand.CardAvailable(Button.Xbox_Y)){
				selectedButton = Button.Xbox_Y;
				selectedCard = hand.GetCard(Button.Xbox_Y);
				GameplayUI.Instance.HighlightCard(Button.Xbox_Y);
				GameplayUI.Instance.SetDisplayRoom(selectedCard);
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

	#region PlaceTreasureState

	public void TreasureEvent(){
		deckMachine.SwitchStates(PlaceTreasureState);
	}

	public void PlaceTreasureEnter(){
		GameplayUI.Instance.TreasurePreview();
	}

	public void PlaceTreasureUpdate(){

		Debug.Log("P");

		if(ControllerInput.ButtonDown(playerNum, Button.Xbox_A) || ControllerInput.ButtonDown(playerNum, Button.Xbox_B) || ControllerInput.ButtonDown(playerNum, Button.Xbox_X) || ControllerInput.ButtonDown(playerNum, Button.Xbox_Y)){
			if(Grid.Instance.RoomActive(selectionX, selectionY) && !((selectionX == Grid.Instance.playerPosX) && (selectionY == Grid.Instance.playerPosY))){
				PlaceTreasure(selectionX, selectionY);
				GameplayUI.Instance.ClearPreview();
				deckMachine.SwitchStates(DeckIdleState);
			}
		}
	}

	public void PlaceTreasureExit(){

	}

	public void PlaceTreasure(int x, int y){
		Treasure treasure = GameObject.Instantiate(treasurePrefab) as Treasure;
		treasure.gameObject.SetActive(true);
		treasure.transform.position = Grid.GetRoom(x, y).transform.position + Vector3.up * 1;
	}

	#endregion

	public bool LocationValid(){
		return
		 /* Is selected room empty */ ((!Grid.Instance.RoomActive(selectionX, selectionY)) &&
		 /* Is room to the right existing && doors are open */ ((Grid.Instance.RoomActive(selectionX + 1, selectionY) && Grid.GetRoom(selectionX + 1, selectionY).IsDoorOpen(DirectionHandler.Directions.Left) && Grid.GetRoom(selectionX, selectionY).IsDoorOpen(DirectionHandler.Directions.Right)) ||
		 /* Is room to the left existing && doors are open */ (Grid.Instance.RoomActive(selectionX - 1, selectionY) && Grid.GetRoom(selectionX - 1, selectionY).IsDoorOpen(DirectionHandler.Directions.Right) && Grid.GetRoom(selectionX, selectionY).IsDoorOpen(DirectionHandler.Directions.Left)) ||
		 /* Is room below existing && doors are open */ (Grid.Instance.RoomActive(selectionX, selectionY + 1) && Grid.GetRoom(selectionX, selectionY + 1).IsDoorOpen(DirectionHandler.Directions.Up) && Grid.GetRoom(selectionX, selectionY).IsDoorOpen(DirectionHandler.Directions.Down)) ||
		 /* Is room above existing && doors are open */ (Grid.Instance.RoomActive(selectionX, selectionY - 1) && Grid.GetRoom(selectionX, selectionY - 1).IsDoorOpen(DirectionHandler.Directions.Down) && Grid.GetRoom(selectionX, selectionY).IsDoorOpen(DirectionHandler.Directions.Up))));
	}


}
