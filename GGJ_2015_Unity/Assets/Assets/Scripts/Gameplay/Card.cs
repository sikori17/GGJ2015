using UnityEngine;
using System.Collections;

public enum CardFormat{
	Room,
	Effect,
	Length
}

public enum Effect{
	PlaceholderOne,
	PlaceholderTwo,
	Length
}

public class Card : MonoBehaviour {

	public CardFormat format;
	public WallType[] wallTypes;
	public Effect effect;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Randomize(){

		format = (CardFormat) Random.Range(0, 1);

		if(format == CardFormat.Room){
			RandomizeWallTypes();
		}
		else{ // Effect format

		}
	}

	private void RandomizeWallTypes(){

		wallTypes = new WallType[4];

		for(int i = 0; i < wallTypes.Length; i++){
			wallTypes[i] = (WallType) Random.Range(0, (int) WallType.Length);
		}
	}
}

