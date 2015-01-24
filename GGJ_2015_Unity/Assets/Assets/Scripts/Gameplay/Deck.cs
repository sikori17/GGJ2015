﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Deck : MonoBehaviour {
	
	// Room vs Effect Percentage
	public float roomPercentage;

	// Effect percentages
	public float spawnEnemyPercentage;
	public float darkenRoomPercentage;

	// Points & Draws
	public int pointsPerDraw;
	public int currentPoints;
	public int storedDraws;

	public Card temp;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
			temp = Draw();
			GameplayUI.Instance.SetDisplayRoom(temp);
		}
	}

	public Card Draw(){
		return Draw(true);
	}

	public Card Draw(bool chargeDrawPoint){

		Card card = new Card();
		if(chargeDrawPoint) storedDraws -= 1;
		GameplayUI.Instance.SetDrawCount(storedDraws);

		float rand = Random.Range(0.0f, 1.0f);

		// If this should be an effect card
		if(rand > roomPercentage){
			card.SetFormat(CardFormat.Effect);
		}
		else{ // Room card
			card.SetFormat(CardFormat.Room);
			card.RandomizeWallTypes();
		}

		rand = Random.Range(0.0f, 1.0f);

		if(rand < spawnEnemyPercentage){
			card.SetEffect(Effect.SpawnEnemy);
		}
		else{ // Darken room
			card.SetEffect(Effect.DarkenRoom);
		}

		return card;
	}

	public bool CanDraw(){
		return (storedDraws > 0);
	}
}
