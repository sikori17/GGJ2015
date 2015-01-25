using UnityEngine;
using System.Collections;

public class ItemRegistry : MonoBehaviour {
	
	public static ItemRegistry Instance;
	
	public enum Items { Potion = 0, Fireball, Bomb, None };
	public Sprite[] itemSpritesList;
	
	delegate void ItemAction();
	ItemAction[] itemActions; 
	
	//FIREBALL
	public GameObject fireballPrefab;
	public ARLTimer crazyFireballTimer, fireballShootTimer;
	
	//BOMB
	public GameObject explosionPrefab;
	
	// Use this for initialization
	void Start () {
		Instance = this;
		itemActions = new ItemAction[]{ PotionAction, FireballAction, BombAction, NoneAction };
		
		crazyFireballTimer.SetDone();
	}
	
	// Update is called once per frame
	void Update () {
		//print ("fireball 2");
		//print (crazyFireballTimer.PercentDone());
		if (!crazyFireballTimer.IsDone()) {
			print ("fireball 3");
			if (fireballShootTimer.IsDone()) {
				
				print ("fireball 4");
				Vector3 dir = DirectionHandler.Instance.DirectionToVector(Avatar.Instance.direction);
				Vector3 startPos = Avatar.Instance.transform.position + dir * 2;
				GameObject newFireball = Instantiate(fireballPrefab,  startPos, fireballPrefab.transform.rotation) as GameObject;
				newFireball.GetComponent<Fireball>().Launch(dir);
				fireballShootTimer.Restart();
			}
		}
	}
	
	public void UseItem(Items type) {
		itemActions[(int) type]();
	}
	
	void PotionAction() {
		Avatar.Instance.AddHP(5);
		AudioHandler.Play(Audio.potion); //sfx
	}
	
	void FireballAction() {
		print ("fireball");
		crazyFireballTimer.Restart();
		fireballShootTimer.SetDone();
		print (crazyFireballTimer.PercentDone());
	}
	
	void BombAction() {
		Vector3 dir = DirectionHandler.Instance.DirectionToVector(Avatar.Instance.direction);
		Vector3 startPos = Avatar.Instance.transform.position + dir * 4;
		Instantiate(explosionPrefab, startPos, explosionPrefab.transform.rotation);
		
		Room curRoom = Grid.Instance.Rooms[Grid.Instance.playerPosX, Grid.Instance.playerPosY];
		DirectionHandler.Directions d = DirectionHandler.Instance.VectorToDirection(Avatar.Instance.transform.position - curRoom.transform.position);
		
		switch (d) {
		case DirectionHandler.Directions.Up:
			curRoom.northWall.ApplyType(WallType.Open);
			break;
		case DirectionHandler.Directions.Down:
			curRoom.southWall.ApplyType(WallType.Open);
			break;
		case DirectionHandler.Directions.Left:
			curRoom.westWall.ApplyType(WallType.Open);
			break;
		case DirectionHandler.Directions.Right:
			curRoom.eastWall.ApplyType(WallType.Open);
			break;
		}

		AudioHandler.Play(Audio.bomb); //sfx
	}
	
	public void SetItemButton(Items type) {
		print (type);
		Debug.Log(type.ToString() + "_" + (int) type + "_" + itemSpritesList.Length);
		GameplayUI.Instance.itemButtonB.sprite = itemSpritesList[(int) type];
	}
	
	void NoneAction() {
		print ("oops no item");
	}
}
