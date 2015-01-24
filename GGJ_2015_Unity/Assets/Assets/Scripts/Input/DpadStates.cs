using UnityEngine;
using System.Collections;

public class DpadStates{

	[System.Serializable]
	public class DpadState{
		public bool buttonUp;
		public bool buttonDown;
		public bool buttonHeld;
	}

	private DpadState[] dpadStates;

	public DpadStates(){
		dpadStates = new DpadState[(int) Dpad.Length];
		for(int i = 0; i < dpadStates.Length; i++){
			dpadStates[i] = new DpadState();
		}
	}

	public void Clear(){

		for(int i = 0; i < dpadStates.Length; i++){
			dpadStates[i].buttonDown = false;
			dpadStates[i].buttonUp = false;
		}

	}

	public DpadState GetState(Dpad dpad){
		return dpadStates[(int) dpad];
	}
}
