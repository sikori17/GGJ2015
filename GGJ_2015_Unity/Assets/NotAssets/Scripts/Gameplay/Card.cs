using UnityEngine;
using System.Collections;

public enum CardFormat{
	Room,
	Effect,
	Length
}

public enum Effect{
	SpawnEnemy,
	DarkenRoom,
	Length
}

public class Card{

	public CardFormat format;
	public WallType[] wallTypes;
	public Effect effect;
	public EnemyHandler.EnemyTypes[] enemySpawnList;
	
	public enum Type { EmptyRoom, SlugRoom, WizardRoom, KnightRoom, SlugEffect, WizardEffect, KnightEffect, Effect };
	int mainEnemy;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Type GetType() {
		if (format == CardFormat.Room) {
			if (mainEnemy == -1) {
				return Type.EmptyRoom;
			}
			else {
				switch ((EnemyHandler.EnemyTypes) mainEnemy) {
				case EnemyHandler.EnemyTypes.Slug:
						return Type.SlugRoom;
						break;
				case EnemyHandler.EnemyTypes.Wizard:
						return Type.WizardRoom;
						break;
				case EnemyHandler.EnemyTypes.Knight:
						return Type.KnightRoom;
						break;
				}
				return Type.EmptyRoom; //buggy stuff
			}
		}
		else {

			if (effect == Effect.SpawnEnemy) {
				switch ((EnemyHandler.EnemyTypes) mainEnemy) {
				case EnemyHandler.EnemyTypes.Slug:
						return Type.SlugEffect;
						break;
				case EnemyHandler.EnemyTypes.Wizard:
						return Type.WizardEffect;
						break;
				case EnemyHandler.EnemyTypes.Knight:
						return Type.KnightEffect;
						break;
				}
				return Type.Effect; //buggy stuff
			}
			return Type.Effect;
		}
	}

	public void SetFormat(CardFormat format){
		this.format = format;
		if(format == CardFormat.Room) RandomizeWallTypes();
	}

	public void SetEffect(Effect effect){
		this.effect = effect;

		if (effect == Effect.SpawnEnemy) {
			RandomizeExtraEnemies();
		}
	}

	public void Randomize(){

		format = (CardFormat) Random.Range(0, 1);

		if(format == CardFormat.Room){
			RandomizeWallTypes();
		}
		else{ // Effect format

		}
	}

	public void RandomizeWallTypes(){
		wallTypes = GetRandomWallConfig();
	}

	public static WallType[] GetRandomWallConfig(){
		WallType[] wallTypes = new WallType[4];
		for(int i = 0; i < wallTypes.Length; i++){
			wallTypes[i] = (WallType) Random.Range(0, ((int) WallType.Length) - 1);
		}
		return wallTypes;
	}

	public void RandomizeEnemies() {
		float r = Random.Range(0, 100f);
		int numEnemyTypes;
		if (r < 50f) {
			numEnemyTypes = 1;
		}
		else if (r < 80f) { 
			numEnemyTypes = 2;
		}
		else {
			numEnemyTypes = 0;
		}

		if (numEnemyTypes == 1) {
			EnemyHandler.EnemyTypes enemyType = (EnemyHandler.EnemyTypes) Random.Range(0,3);
			enemySpawnList = new EnemyHandler.EnemyTypes[Random.Range(2,4)];
			for (int i = 0; i < enemySpawnList.Length; i++) {
				enemySpawnList[i] = enemyType;
			}

			mainEnemy = (int) enemyType;
		}
		else if (numEnemyTypes == 2) {
			int a = Random.Range(0,3);
			int b = (a + Random.Range(1,3)) % 3;

			EnemyHandler.EnemyTypes enemyType1 = (EnemyHandler.EnemyTypes) a;
			EnemyHandler.EnemyTypes enemyType2 = (EnemyHandler.EnemyTypes) b;

			int aCount = Random.Range(2,3);
			int bCount = Random.Range(1,2);

			enemySpawnList = new EnemyHandler.EnemyTypes[aCount + bCount];
			for (int i = 0; i < aCount; i++) {
				enemySpawnList[i] = enemyType1;
			}
			for (int i = aCount; i < aCount + bCount; i++) {
				enemySpawnList[i] = enemyType2;
			}

			if (bCount > aCount) {
				mainEnemy = b;
			}
			else {
				mainEnemy = a;
			}
		}
		else {
			enemySpawnList = new EnemyHandler.EnemyTypes[0];
			mainEnemy = -1;
		}
	}

	void RandomizeExtraEnemies() {
		float r = Random.Range(0, 100f);
		int numEnemyTypes;
		if (r < 50f) {
			numEnemyTypes = 1;
		}
		else { 
			numEnemyTypes = 2;
		}
		
		if (numEnemyTypes == 1) {
			EnemyHandler.EnemyTypes enemyType = (EnemyHandler.EnemyTypes) Random.Range(0,3);
			enemySpawnList = new EnemyHandler.EnemyTypes[Random.Range(2,4)];
			for (int i = 0; i < enemySpawnList.Length; i++) {
				enemySpawnList[i] = enemyType;
			}

			mainEnemy = (int) enemyType;
		}
		else if (numEnemyTypes == 2) {
			int a = Random.Range(0,3);
			int b = (a + Random.Range(1,3)) % 3;
			
			EnemyHandler.EnemyTypes enemyType1 = (EnemyHandler.EnemyTypes) a;
			EnemyHandler.EnemyTypes enemyType2 = (EnemyHandler.EnemyTypes) b;
			
			int aCount = Random.Range(2,3);
			int bCount = Random.Range(1,2);
			
			enemySpawnList = new EnemyHandler.EnemyTypes[aCount + bCount];
			for (int i = 0; i < aCount; i++) {
				enemySpawnList[i] = enemyType1;
			}
			for (int i = aCount; i < aCount + bCount; i++) {
				enemySpawnList[i] = enemyType2;
			}
			
			if (bCount > aCount) {
				mainEnemy = b;
			}
			else {
				mainEnemy = a;
			}
		}
	}
	
}

