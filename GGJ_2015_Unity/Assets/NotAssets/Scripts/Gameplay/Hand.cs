using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hand : MonoBehaviour {

	public static Hand Instance;
	public int handLimit;
	public List<Card> cards;

	public Card cardA;
	public Card cardB;
	public Card cardX;
	public Card cardY;

	public Deck deck;

	void Awake(){
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		DrawStartingHand();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DrawStartingHand(){
		cardA = deck.Draw(false);
		cardB = deck.Draw(false);
		cardX = deck.Draw(false);
		cardY = deck.Draw(false);

		GameplayUI.Instance.InitCardImages(cardA, cardB, cardX, cardY);
	}

	public void DrawCard(Button button){

		if(!Grid.Instance.crownEvent){

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
	}

	public void DrawDeathCard(Button button) {
		Card card = new Card();
		card.format = CardFormat.Effect;
		card.effect = Effect.DeathCard;
		
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

	public void ClearAndDrawEffectCards(){
		cardA.SetFormat(CardFormat.Effect);
		cardA.SetEffect(Effect.SpawnEnemy);
		cardB.SetFormat(CardFormat.Effect);
		cardB.SetEffect(Effect.SpawnEnemy);
		cardX.SetFormat(CardFormat.Effect);
		cardX.SetEffect(Effect.SpawnEnemy);
		cardY.SetFormat(CardFormat.Effect);
		cardY.SetEffect(Effect.SpawnEnemy);
		GameplayUI.Instance.InitCardImages(cardA, cardB, cardX, cardY);
		deck.enabled = false;
	}
}
