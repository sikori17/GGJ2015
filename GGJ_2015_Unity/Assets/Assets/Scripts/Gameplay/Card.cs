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
	public EnemyManager.EnemyTypes[] enemySpawnList;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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

		wallTypes = new WallType[4];

		for(int i = 0; i < wallTypes.Length; i++){
			wallTypes[i] = (WallType) Random.Range(0, (int) WallType.Length);
		}
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
			EnemyManager.EnemyTypes enemyType = (EnemyManager.EnemyTypes) Random.Range(0,3);
			enemySpawnList = new EnemyManager.EnemyTypes[Random.Range(3,6)];
			for (int i = 0; i < enemySpawnList.Length; i++) {
				enemySpawnList[i] = enemyType;
			}
		}
		else if (numEnemyTypes == 2) {
			int a = Random.Range(0,3);
			int b = (a + Random.Range(1,3)) % 3;

			EnemyManager.EnemyTypes enemyType1 = (EnemyManager.EnemyTypes) a;
			EnemyManager.EnemyTypes enemyType2 = (EnemyManager.EnemyTypes) b;

			int aCount = Random.Range(2,4);
			int bCount = Random.Range(1,3);

			enemySpawnList = new EnemyManager.EnemyTypes[aCount + bCount];
			for (int i = 0; i < aCount; i++) {
				enemySpawnList[i] = enemyType1;
			}
			for (int i = aCount; i < aCount + bCount; i++) {
				enemySpawnList[i] = enemyType2;
			}
		}
		else {
			enemySpawnList = new EnemyManager.EnemyTypes[0];
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
			EnemyManager.EnemyTypes enemyType = (EnemyManager.EnemyTypes) Random.Range(0,3);
			enemySpawnList = new EnemyManager.EnemyTypes[Random.Range(3,6)];
			for (int i = 0; i < enemySpawnList.Length; i++) {
				enemySpawnList[i] = enemyType;
			}
		}
		else if (numEnemyTypes == 2) {
			int a = Random.Range(0,3);
			int b = (a + Random.Range(1,3)) % 3;
			
			EnemyManager.EnemyTypes enemyType1 = (EnemyManager.EnemyTypes) a;
			EnemyManager.EnemyTypes enemyType2 = (EnemyManager.EnemyTypes) b;
			
			int aCount = Random.Range(2,4);
			int bCount = Random.Range(1,3);
			
			enemySpawnList = new EnemyManager.EnemyTypes[aCount + bCount];
			for (int i = 0; i < aCount; i++) {
				enemySpawnList[i] = enemyType1;
			}
			for (int i = aCount; i < aCount + bCount; i++) {
				enemySpawnList[i] = enemyType2;
			}
		}
	}
}

