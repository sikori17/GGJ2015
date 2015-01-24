using UnityEngine;
using System.Collections;

public enum Button {
	Xbox_A,
	Xbox_B,
	Xbox_X,
	Xbox_Y,
	Xbox_RightBumper,
	Xbox_LeftBumper,
	Xbox_Start,
	Xbox_Back,
	Xbox_Home,
	Xbox_AnalogLeft,
	Xbox_AnalogRight,
	Length
}

public enum Analog {
	Xbox_Right,
	Xbox_Left,
	Length
}

public enum Trigger {
	Xbox_Right,
	Xbox_Left,
	Length
}

public enum Dpad {
	Xbox_Left,
	Xbox_Right,
	Xbox_Up,
	Xbox_Down,
	Length
}

public class ControllerInput : MonoBehaviour {

	[System.Serializable]
	public class ButtonConfig{
		public Button button;
		public string buttonName;
	}

	[System.Serializable]
	public class AnalogConfig{
		public Analog analog;
		public string analogAxisX;
		public string analogAxisY;
	}

	[System.Serializable]
	public class TriggerConfig{
		public Trigger trigger;
		public string triggerName;
	}

	[System.Serializable]
	public class DpadConfig{
		public Dpad dpad;
		public string dpadName;
		public bool reverseAxis;
	}
	
	private static ControllerInput instance;

	public ButtonConfig[] buttonConfigs;
	public AnalogConfig[] analogConfigs;
	public TriggerConfig[] triggerConfigs;
	public DpadConfig[] dpadConfigs;

	public ButtonConfig[] windowsButtonConfigs;
	public AnalogConfig[] windowsAnalogConfigs;
	public TriggerConfig[] windowsTriggerConfigs;
	public DpadConfig[] windowsDpadConfigs;
	
	public int numberOfControllers;

	// The normalized amount a trigger must be down to
	// be considered down as a button
	public float buttonMinimum;

	#region Dpad Delegate Vars

	public delegate bool IntDpadParamBoolDelegate(int param, Dpad dpad);

	private IntDpadParamBoolDelegate Dpad_Held_Delegate;
	private IntDpadParamBoolDelegate Dpad_Up_Delegate;
	private IntDpadParamBoolDelegate Dpad_Down_Delegate;

	#endregion
	
	private TriggerStates[] triggerStates;
	private DpadStates[] dpadStates;

	void Awake(){
		Debug.Log("Awake");
		if(instance == null){
			instance = this;	
			InitTriggerStates();
			LoadDpadDelegates();
			enabled = true;
			CheckPlatform();
		}
	}

	void CheckPlatform(){
		if(Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsWebPlayer){

			for(int i = 0; i < windowsButtonConfigs.Length; i++){
				buttonConfigs[(int) windowsButtonConfigs[i].button] = windowsButtonConfigs[i];
			}

			for(int i = 0; i < windowsDpadConfigs.Length; i++){
				dpadConfigs[(int) windowsDpadConfigs[i].dpad] = windowsDpadConfigs[i];
			}

			for(int i = 0; i < windowsAnalogConfigs.Length; i++){
				analogConfigs[(int) windowsAnalogConfigs[i].analog] = windowsAnalogConfigs[i];
			}

			for(int i = 0; i < windowsTriggerConfigs.Length; i++){
				triggerConfigs[(int) windowsTriggerConfigs[i].trigger] = windowsTriggerConfigs[i];
			}

			LoadWindowsDpadDelegates();
		}
	}
	
	void InitTriggerStates(){
		triggerStates = new TriggerStates[numberOfControllers];
		for(int i = 0; i < triggerStates.Length; i++){
			triggerStates[i] = new TriggerStates();	
		}
	}

	void LoadDpadDelegates(){

		InitDpadStates();

		Dpad_Held_Delegate = Dpad_Button_Input;
		Dpad_Up_Delegate = Dpad_Button_InputUp;
		Dpad_Down_Delegate = Dpad_Button_InputDown;

	}

	void LoadWindowsDpadDelegates(){

		InitDpadStates();

		Dpad_Held_Delegate = Dpad_Axis;
		Dpad_Up_Delegate = Dpad_AxisUp;
		Dpad_Down_Delegate = Dpad_AxisDown;

	}

	void InitDpadStates(){
		if(dpadStates == null){
			
			dpadStates = new DpadStates[numberOfControllers];
			
			for (int i = 0; i < dpadStates.Length; i++){
				dpadStates[i] = new DpadStates();
			}
		}
	}
	
	// Monitors controller inputs
	void Update(){

		for(int i = 1; i < triggerStates.Length + 1; i++){
			CheckTriggerState(i);
		}

		if(dpadStates != null){
			for(int i = 1; i < dpadStates.Length + 1; i++){
				CheckDpadState(i);
			}
		}
	}

	void CheckTriggerState(int controllerNum){

		TriggerStates triggerState = TriggerState(controllerNum);

		bool last = false;
		TriggerStates.TriggerState state;
		for(int i = 0; i < (int) Trigger.Length; i++){
			state = triggerState.GetState((Trigger) i);
			last = state.triggerHeld;
			state.triggerHeld = TriggerAbove(controllerNum, (Trigger) i, buttonMinimum);
			state.triggerDown = !last && state.triggerHeld;
			state.triggerUp = last && !state.triggerHeld;
		}

	}

	void CheckDpadState(int controllerNum){

		DpadStates dpadState = DpadState(controllerNum);

		bool last = false;
		DpadStates.DpadState state;
		for(int i = 0; i < (int) Dpad.Length; i++){
			state = dpadState.GetState((Dpad) i);
			last = state.buttonHeld;
			state.buttonHeld = DpadAbove(controllerNum, (Dpad) i, buttonMinimum);
			state.buttonDown = !last && state.buttonHeld;
		}

	}

	TriggerStates TriggerState(int controllerNum){
		return triggerStates[controllerNum - 1];
	}

	DpadStates DpadState(int controllerNum){
		return dpadStates[controllerNum - 1];
	}
	
	void LateUpdate(){
		ClearStates();	
	}
	
	void ClearStates(){
		for(int i = 0; i < triggerStates.Length; i++){
			triggerStates[i].Clear();
		}	
	}

	#region Face Buttons

	public static bool Button(int controllerNum, Button button){
		return Button(instance.buttonConfigs[(int) button].buttonName, controllerNum);
	}
	
	public static bool ButtonDown(int controllerNum, Button button){
		return ButtonDown(instance.buttonConfigs[(int) button].buttonName, controllerNum);
	}

	public static bool ButtonUp(int controllerNum, Button button){
		return ButtonUp(instance.buttonConfigs[(int) button].buttonName, controllerNum);
	}
	
	#endregion

	#region Dpad Delegate Functions

	private bool Dpad_Button_Input(int controllerNum, Dpad dpad){
		return Button(instance.dpadConfigs[(int) dpad].dpadName, controllerNum);
	}

	private bool Dpad_Button_InputUp(int controllerNum, Dpad dpad){
		return ButtonUp(instance.dpadConfigs[(int) dpad].dpadName, controllerNum);
	}

	private bool Dpad_Button_InputDown(int controllerNum, Dpad dpad){
		return ButtonDown(instance.dpadConfigs[(int) dpad].dpadName, controllerNum);
	}

	private bool Dpad_Axis(int controllerNum, Dpad dpad){
		return DpadAbove(controllerNum, dpad, buttonMinimum);
	}

	private bool Dpad_AxisUp(int controllerNum, Dpad dpad){
		return DpadState(controllerNum).GetState(dpad).buttonUp;
	}

	private bool Dpad_AxisDown(int controllerNum, Dpad dpad){
		return DpadState(controllerNum).GetState(dpad).buttonDown;
	}

	#endregion

	#region Dpad 

	public static bool Dpad_Button(int controllerNum, Dpad dpad){
		return instance.Dpad_Held_Delegate(controllerNum, dpad);
	}

	public static bool Dpad_ButtonUp(int controllerNum, Dpad dpad){
		return instance.Dpad_Up_Delegate(controllerNum, dpad);
	}

	public static bool Dpad_ButtonDown(int controllerNum, Dpad dpad){
		return instance.Dpad_Down_Delegate(controllerNum, dpad);
	}
	
	#endregion

	#region Analogs

	public static float Analog_X(int controllerNum, Analog analog){
		return Axis(instance.analogConfigs[(int) analog].analogAxisX, controllerNum);
	}

	public static float Analog_X(int controllerNum, Analog analog, float deadMagnitude){
		return Axis(instance.analogConfigs[(int) analog].analogAxisX, controllerNum, deadMagnitude);
	}

	public static float Analog_Y(int controllerNum, Analog analog){
		return Axis(instance.analogConfigs[(int) analog].analogAxisY, controllerNum);
	}
	
	public static float Analog_Y(int controllerNum, Analog analog, float deadMagnitude){
		return Axis(instance.analogConfigs[(int) analog].analogAxisY, controllerNum, deadMagnitude);
	}

	public static float Analog_Magnitude(int controllerNum, Analog analog){
		return AnalogMagnitude(Analog_X(controllerNum, analog), Analog_Y(controllerNum, analog));
	}
	
	public static Vector3 Analog_Vector(int controllerNum, Analog analog){
		return new Vector3(Analog_X(controllerNum, analog), 0, Analog_Y(controllerNum, analog));
	}

	#endregion Analogs

	#region Triggers

	public static float TriggerAxis(int controllerNum, Trigger trigger){
		return TriggerAxis(instance.triggerConfigs[(int) trigger].triggerName, controllerNum);
	}

	private static bool TriggerAbove(int controllerNum, Trigger trigger, float minimumPercentDown){
		return TriggerAxis(controllerNum, trigger) > minimumPercentDown;
	}

	public static bool TriggerDown(int controllerNum, Trigger trigger){
		return instance.TriggerState(controllerNum).GetState(trigger).triggerDown;
	}

	public static bool TriggerHeld(int controllerNum, Trigger trigger){
		return instance.TriggerState(controllerNum).GetState(trigger).triggerHeld;
	}

	public static bool TriggerUp(int controllerNum, Trigger trigger){
		return instance.TriggerState(controllerNum).GetState(trigger).triggerUp;
	}

	#endregion 

	#region Dpad Axis Functions

	public static bool DpadAbove(int controllerNum, Dpad dpad, float minimumPercentDown){
		DpadConfig config = instance.dpadConfigs[(int) dpad];
		return Axis(config.dpadName, controllerNum) < (1 - 2 * System.Convert.ToInt32(config.reverseAxis)) * minimumPercentDown;
	}

	#endregion

	#region Base Functions
	
	private static bool ButtonDown(string buttonName, int controllerNum){
		return Input.GetButtonDown(buttonName + "_" + controllerNum);
	}
	
	private static bool Button(string buttonName, int controllerNum){
		return Input.GetButton(buttonName + "_" + controllerNum);
	}
	
	private static bool ButtonUp(string buttonName, int controllerNum){
		return Input.GetButtonUp(buttonName + "_" + controllerNum);
	}
	
	private static float Axis(string axisName, int controllerNum){
		return Input.GetAxis(axisName + "_" + controllerNum);	
	}
	
	private static float Axis(string axisName, int controllerNum, float deadMagnitude){
		float result = Axis(axisName, controllerNum);
		if(result < deadMagnitude){
			result = 0;	
		}
		return result;
	}
	
	private static float TriggerAxis(string axisName, int controllerNum){
		return (Axis(axisName, controllerNum) + 1) / 2;
	}
	
	private static float AnalogMagnitude(float x, float y){
		return 	Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2));	
	}

	#endregion
}
