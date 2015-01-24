using UnityEngine;
using System.Collections;

[System.Serializable]
public class TriggerStates{

	[System.Serializable]
	public class TriggerState{
		public bool triggerHeld;
		public bool triggerDown;
		public bool triggerUp;
	}

	private TriggerState[] triggerStates;

	public TriggerStates(){
		triggerStates = new TriggerState[(int) Trigger.Length];
		for(int i = 0; i < triggerStates.Length; i++){
			triggerStates[i] = new TriggerState();
		}
	}
	
	public void Clear(){

		for(int i = 0; i < triggerStates.Length; i++){
			triggerStates[i].triggerDown = false;
			triggerStates[i].triggerUp = false;
		}

	}

	public TriggerState GetState(Trigger trigger){
		return triggerStates[(int) trigger];
	}
}
