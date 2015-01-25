using UnityEngine;
using System.Collections;

public class ItemHandler : MonoBehaviour {

	public static ItemHandler Instance;

	public enum Items { Potion, Fireball, Bomb, None };
	public Sprite[] itemSprites;

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
		if (!crazyFireballTimer.IsDone()) {
			if (fireballShootTimer.IsDone()) {
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
	}

	void FireballAction() {
		crazyFireballTimer.Restart();
		fireballShootTimer.SetDone();
	}

	void BombAction() {
		Vector3 dir = DirectionHandler.Instance.DirectionToVector(Avatar.Instance.direction);
		Vector3 startPos = Avatar.Instance.transform.position + dir * 4;
		Instantiate(explosionPrefab, startPos, explosionPrefab.transform.rotation);
	}

	void NoneAction() {
		print ("oops no item");
	}
}
