using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hand : MonoBehaviour {

	public int handLimit;
	public List<Card> cards;

	public Card cardA;
	public Card cardB;
	public Card cardX;
	public Card cardY;

	public Deck deck;

	void Awake(){
		DrawStartingHand();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DrawStartingHand(){
		cardA = deck.Draw(false);
		cardB = deck.Draw(false);
		cardX = deck.Draw(false);
		cardY = deck.Draw(false);
	}

	public void DrawCard(Button button){

		Card card = deck.Draw();

		if(button == Button.Xbox_A){
			cardA = card;
		}
		else if(button == Button.Xbox_B){
			cardB = card;
		}
		else if(button == Button.Xbox_X){
			cardX = card;
		}
		else if(button == Button.Xbox_Y){
			cardY = card;
		}

		GameplayUI.Instance.AnimateDraw(card, button);
	}

	public Card GetCard(Button button){

		Card holder = null;
		
		if(button == Button.Xbox_A){
			holder = cardA;
		}
		else if(button == Button.Xbox_B){
			holder = cardB;
		}
		else if(button == Button.Xbox_X){
			holder = cardX;
		}
		else if(button == Button.Xbox_Y){
			holder = cardY;
		}

		return holder;
	}

	public Card RemoveCard(Button button){

		Card card = GetCard(button);

		if(button == Button.Xbox_A){
			cardA = null;
		}
		else if(button == Button.Xbox_B){
			cardB = null;
		}
		else if(button == Button.Xbox_X){
			cardX = null;
		}
		else if(button == Button.Xbox_Y){
			cardY = null;
		}

		return card;
	}

	public bool CardAvailable(Button button){

		Card holder = GetCard(button);

		return !(holder == null);
	}
}
