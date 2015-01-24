using UnityEngine;
using System.Collections;

public class DungeonMaster : MonoBehaviour {

	public int playerNum;

	SimpleStateMachine stateMachine;
	private SimpleState IdleState;

	public int selectionX;
	public int selectionY;

	public RoomHighlight highlight;

	// Use this for initialization
	void Start () {
		InitializeStates();
	}

	public void InitializeStates(){
		stateMachine = new SimpleStateMachine();
		IdleState = new SimpleState(IdleEnter, IdleUpdate, IdleExit, "IDLE");
		stateMachine.SwitchStates(IdleState);
	}
	
	// Update is called once per frame
	void Update () {
		stateMachine.Execute();
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
}
