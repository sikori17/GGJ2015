using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(NetworkView))]
public class NetworkHandler : BaseBehaviour {

	public static NetworkHandler Instance;

	// All receiving end messages are stored in these delegate arrays
	// Ie, these are the messages other objects can call on this client
	private IntParamDelegate[] intDels;
	private StringParamDelegate[] stringDels;
	private FloatParamDelegate[] floatDels;

	private int intEnd;
	private int stringEnd;
	private int floatEnd;



	public List<ByteParamDelegate> networkFunctions;

	public string ip;

	// Use this for initialization
	void Awake () {
		if(Instance == null){
			Instance = this;
			networkFunctions = new List<ByteParamDelegate>();
			networkFunctions.Add(NetworkFunction.NetworkReceiveFunction);
		}
		else{
			GameObject.Destroy(this.gameObject);
		}
	}

	public override void Update(){


		if(Input.GetKeyDown(KeyCode.A)){
			Network.InitializeServer(10, 8000, false);
		}

		if(Input.GetKeyDown(KeyCode.B)){
			Network.Connect(ip, 8000);
		}

		if(Input.GetKeyDown(KeyCode.Space)){

			TestNetFunction temp = new TestNetFunction();
			temp.intOne = 17;
			temp.message = "Gooooo";
			temp.vector = new Vector3(1.23f, 4.56f, 7.89f);

			NetworkCallSend(0, temp.Serialize(), RPCMode.Others);
		}
	}


	public static void NetworkCallSend(int functionId, byte[] data, RPCMode mode){
		Instance.networkView.RPC("NetworkCallReceive", mode, functionId, data);
	}

	[RPC]
	private void NetworkCallReceive(int functionId, byte[] data){
		networkFunctions[functionId](data);
	}

	public static int AddNetworkedMessage(IntParamDelegate function){

		if(Instance.intEnd == Instance.intDels.Length){
			IntParamDelegate[] newIntDels = new IntParamDelegate[Instance.intDels.Length * 2];
			Instance.intDels.CopyTo(newIntDels, 0);
			Instance.intDels = newIntDels;
		}

		Instance.intDels[Instance.intEnd] = function;
		Instance.intEnd++;

		return Instance.intEnd - 1;
	}

	public static void RegisterNetworkedMessage(IntParamDelegate function, int messageId){
		if(messageId < Instance.intEnd){
			Instance.intDels[messageId] += function;
		}
	}

	public static void IntMessage(int messageId, int param){
		Instance.networkView.RPC("IntMessageInternal", RPCMode.Others, messageId, param);
	}

	[RPC]
	private void IntMessageInternal(int messageId, int param){
		intDels[messageId](param);
	}
}
