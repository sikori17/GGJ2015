using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour {

	public Wall northWall;
	public Wall southWall;
	public Wall eastWall;
	public Wall westWall;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ApplyCard(Card card){

		if(card.format == CardFormat.Room){
			ApplyRoomCard(card);
		}
		else{
			ApplyEffectCard(card);
		}
	}

	public void ApplyRoomCard(Card card){
		northWall.ApplyType(card.wallTypes[0]);
		southWall.ApplyType(card.wallTypes[1]);
		eastWall.ApplyType(card.wallTypes[2]);
		westWall.ApplyType(card.wallTypes[3]);
	}

	public void ApplyEffectCard(Card card){

	}
}
