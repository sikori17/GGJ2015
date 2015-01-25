using UnityEngine;
using System.Collections;

public class DungeonMaster : MonoBehaviour {

	public static DungeonMaster Instance;
	public int playerNum;

	public SimpleStateMachine stateMachine;
	private SimpleState IdleState;

	public SimpleStateMachine deckMachine;
	private SimpleState DeckIdleState;
	private SimpleState DrawCardState;
	private SimpleState CardSelectedState;
	private SimpleState PlaceTreasureState;
	private SimpleState PlaceCrownState;
	private SimpleState DeathCardState;

	public int selectionX;
	public int selectionY;

	public RoomHighlight highlight;

	public Deck deck;
	public Hand hand;

	public Card selectedCard;
	public Button selectedButton;

	public Treasure treasurePrefab;
	public Crown crownPrefab;

	//DEATH CARD
	public ARLTimer deathCardTimer, spawnEnemyTimer;

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
		PlaceCrownState = new SimpleState(PlaceCrownEnter, PlaceCrownUpdate, PlaceCrownExit, "PLACE_CROWN");
		DeathCardState = new SimpleState(DeathCardEnter, DeathCardUpdate, DeathCardExit, "DEATH_CARD");
		deckMachine.SwitchStates(DeckIdleState);
		//deckMachine.SwitchStates(DeathCardState);
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

	#region Death Card
	public void DeathCardEnter() {
		deathCardTimer.Restart();
		spawnEnemyTimer.Restart();
	}

	public void DeathCardUpdate() {
		//SPAWN ENEMIES
		if (spawnEnemyTimer.IsDone()) {
			spawnEnemyTimer.Restart();

			if (Grid.Instance.RoomActive(selectionX, selectionY)) {
				EnemyHandler.EnemyTypes[] e = new EnemyHandler.EnemyTypes[]{ (EnemyHandler.EnemyTypes) Random.Range(0,3) };
				Grid.Instance.Rooms[selectionX, selectionY].Spawn(e);
			}
		}

		//EXIT
		if (deathCardTimer.IsDone()) {
			deckMachine.SwitchStates(DeckIdleState);
		}
	}

	public void DeathCardExit() {
	}
	#endregion

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
		highlight.SetNeutral();
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

		CardSelectedHighlightLogic();

		if(ControllerInput.ButtonDown(playerNum, Button.Xbox_A)){
			UseCard(Button.Xbox_A);
		}
		if(ControllerInput.ButtonDown(playerNum, Button.Xbox_B)){
			UseCard(Button.Xbox_B);
		}
		if(ControllerInput.ButtonDown(playerNum, Button.Xbox_X)){
			UseCard(Button.Xbox_X);
		}
		if(ControllerInput.ButtonDown(playerNum, Button.Xbox_Y)){
			UseCard(Button.Xbox_Y);
		}
	}

	public void CardSelectedHighlightLogic(){
		if(selectedCard != null){
			if(selectedCard.format == CardFormat.Room){
				if(!Grid.Instance.RoomActive(selectionX, selectionY)){
					Grid.GetRoom(selectionX, selectionY).ApplyWallConfiguration(selectedCard.wallTypes);
					if(LocationValid()){
						highlight.SetValid();
					}
					else{
						highlight.SetInvalid();
					}
				}
			}
			else{
				if(Grid.Instance.RoomActive(selectionX, selectionY) && !Grid.IsPlayerLocation(selectionX, selectionY)){
					highlight.SetValid();
				}
				else{
					highlight.SetInvalid();
				}
			}
		}
	}

	void UseCard(Button b) {
		if(selectedButton == b){
			if (selectedCard.format == CardFormat.Room) {
				Debug.Log("LV "  + LocationValid());
				Grid.GetRoom(selectionX, selectionY).ApplyWallConfiguration(selectedCard.wallTypes);
				if(LocationValid()){
					PlayCard(selectedCard);
					hand.RemoveCard(b);
					GameplayUI.Instance.ClearCard(b);
					deckMachine.SwitchStates(DeckIdleState);

					if (Grid.GetRoom(selectionX, selectionY).rewardSpace) {
						hand.DrawDeathCard(b);
						Grid.GetRoom(selectionX, selectionY).rewardSpace = false;
						Destroy(Grid.GetRoom(selectionX, selectionY).rewardMarker);
						Grid.RewardUsed();
					}
				}
			}
			else { //format == CardFormat.Effect
				if (selectedCard.effect == Effect.SpawnEnemy) {
					if (Grid.Instance.RoomActive(selectionX, selectionY) && !Grid.IsPlayerLocation(selectionX, selectionY)) {
						PlayCard(selectedCard);
						hand.RemoveCard(b);
						GameplayUI.Instance.ClearCard(b);
						deckMachine.SwitchStates(DeckIdleState);
					}
				}
				else if (selectedCard.effect == Effect.DeathCard) {
					hand.RemoveCard(b);
					GameplayUI.Instance.ClearCard(b);
					deckMachine.SwitchStates(DeathCardState);
				}
			}
		}
		else if(hand.CardAvailable(b)){
			selectedButton = b;
			selectedCard = hand.GetCard(b);
			GameplayUI.Instance.SetDisplayRoom(selectedCard);
			GameplayUI.Instance.HighlightCard(b);
		}
		else{
			hand.DrawCard(b);
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

		if(Grid.Instance.RoomActive(selectionX, selectionY) && !(Grid.IsPlayerLocation(selectionX, selectionY)) && !Grid.GetRoom(selectionX, selectionY).hasTreasure){
			highlight.SetValid();
		}
		else{
			highlight.SetInvalid();
		}

		if(ControllerInput.ButtonDown(playerNum, Button.Xbox_A) || ControllerInput.ButtonDown(playerNum, Button.Xbox_B) || ControllerInput.ButtonDown(playerNum, Button.Xbox_X) || ControllerInput.ButtonDown(playerNum, Button.Xbox_Y)){
			if(Grid.Instance.RoomActive(selectionX, selectionY) && !(Grid.IsPlayerLocation(selectionX, selectionY)) && !Grid.GetRoom(selectionX, selectionY).hasTreasure){
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
		treasure.AssignRoom(Grid.GetRoom(x, y));
		Grid.GetRoom(x, y).hasTreasure = true;
	}

	#endregion

	#region Crown

	public void CrownEvent(){
		deckMachine.SwitchStates(PlaceCrownState);
	}

	public void PlaceCrownEnter(){

	}

	public void PlaceCrownUpdate(){
		if(Grid.Instance.RoomActive(selectionX, selectionY) && !(Grid.IsPlayerLocation(selectionX, selectionY)) && !Grid.GetRoom(selectionX, selectionY).hasTreasure){
			highlight.SetValid();
		}
		else{
			highlight.SetInvalid();
		}
		
		if(ControllerInput.ButtonDown(playerNum, Button.Xbox_A) || ControllerInput.ButtonDown(playerNum, Button.Xbox_B) || ControllerInput.ButtonDown(playerNum, Button.Xbox_X) || ControllerInput.ButtonDown(playerNum, Button.Xbox_Y)){
			if(Grid.Instance.RoomActive(selectionX, selectionY) && !(Grid.IsPlayerLocation(selectionX, selectionY)) && !Grid.GetRoom(selectionX, selectionY).hasTreasure){
				PlaceCrown();
				GameplayUI.Instance.ClearPreview();
				deckMachine.SwitchStates(DeckIdleState);
			}
		}
	}

	public void PlaceCrownExit(){

	}

	public void PlaceCrown(){
		Crown crown = GameObject.Instantiate(crownPrefab) as Crown;
		crown.transform.position = Grid.GetRoom(selectionX, selectionY).transform.position + Vector3.up;
		crown.gameObject.SetActive(true);
	}

	#endregion

	public bool LocationValid(){
		if(DoorsAreMatching()){
			if(Grid.Instance.RoomActive(selectionX, selectionY)){
				if(!Grid.GetRoom(selectionX, selectionY).hasTreasure && Grid.GetRoom(selectionX, selectionY).enemyList.Count == 0){
					return true;
				}
			}
			else if(Grid.GetRoom(selectionX, selectionY).HasExit()){
				return true;
			}
		}
		else{
			return false;
		}
		return false;
	}

	public bool DoorsAreMatching(){
		return
			/* Is room to the right existing && doors are open */ ((Grid.Instance.RoomActive(selectionX + 1, selectionY) && Grid.GetRoom(selectionX + 1, selectionY).IsDoorOpen(DirectionHandler.Directions.Left) && Grid.GetRoom(selectionX, selectionY).IsDoorOpen(DirectionHandler.Directions.Right)) ||
       /* Is room to the left existing && doors are open */ (Grid.Instance.RoomActive(selectionX - 1, selectionY) && Grid.GetRoom(selectionX - 1, selectionY).IsDoorOpen(DirectionHandler.Directions.Right) && Grid.GetRoom(selectionX, selectionY).IsDoorOpen(DirectionHandler.Directions.Left)) ||
       /* Is room below existing && doors are open */ (Grid.Instance.RoomActive(selectionX, selectionY + 1) && Grid.GetRoom(selectionX, selectionY + 1).IsDoorOpen(DirectionHandler.Directions.Up) && Grid.GetRoom(selectionX, selectionY).IsDoorOpen(DirectionHandler.Directions.Down)) ||
       /* Is room above existing && doors are open */ (Grid.Instance.RoomActive(selectionX, selectionY - 1) && Grid.GetRoom(selectionX, selectionY - 1).IsDoorOpen(DirectionHandler.Directions.Down) && Grid.GetRoom(selectionX, selectionY).IsDoorOpen(DirectionHandler.Directions.Up)));
	}
}
