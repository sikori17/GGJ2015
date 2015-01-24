﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public class SimpleStateMachine {
	
	public delegate void StateDelegate();
	private StateDelegate enter, update, exit;
	public string currentState;
	
	//call this in the update function
	public void Execute () {
		if (update != null){
			update();
		}
	}
	
	public void SwitchStates(StateDelegate enter, StateDelegate update, StateDelegate exit, string name){
		if (this.exit != null){
			//Debug.Log("State " + currentState + " exited.");
			this.exit(); //exit the current state
		}
		
		//set up the new state
		this.enter = enter;
		this.update = update;
		this.exit = exit;
		this.currentState = name;
		
		if (this.enter != null){
			//Debug.Log("Entering State" + currentState);
			this.enter(); //enter the new state
		}
	}
	
	public void SwitchStates(SimpleState state) {
		SwitchStates(state.enter, state.update, state.exit, state.name);
	}
	
	public SimpleState GetCurrentState(){
		return new SimpleState(enter, update, exit, currentState);
	}
	
	public bool IsState(string name){
		return currentState == name;
	}
}
