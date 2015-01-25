using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Deck : MonoBehaviour {

	public static Deck Instance;
	
	// Room vs Effect Percentage
	public float roomPercentage;

	// Effect percentages
	public float spawnEnemyPercentage;
	public float darkenRoomPercentage;

	// Points & Draws
	public int pointsPerDraw;
	public int currentPoints;
	public int storedDraws;
	public ARLTimer pointRefreshTimer;

	public Card temp;

	// Use this for initialization
	void Start () {
		Instance = this;
	}
	
	// Update is called once per frame
	void Update () {

		if (pointRefreshTimer.IsDone()) {
			AddPoints(1);
			pointRefreshTimer.Restart();
		}
	}

	public void AddPoints(int num) {
		currentPoints += num;
		while (currentPoints >= pointsPerDraw) {
			currentPoints -= pointsPerDraw;
			storedDraws++;
		}
		GameplayUI.Instance.SetDeckPoints( ((float) currentPoints) / ((float) pointsPerDraw) );
		GameplayUI.Instance.SetDrawCount(storedDraws);
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
			card.RandomizeEnemies();
		}

		rand = Random.Range(0.0f, 1.0f);

		if(rand < spawnEnemyPercentage){
			card.SetEffect(Effect.SpawnEnemy);
		}
		else{ // Darken room
			card.SetEffect(Effect.SpawnEnemy); //no more room darkening
		}

		return card;
	}

	public bool CanDraw(){
		return (storedDraws > 0);
	}
}
