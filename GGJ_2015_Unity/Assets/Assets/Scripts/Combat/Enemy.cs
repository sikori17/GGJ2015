using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public int hitPoints;
	public int lootPoints;
	public int maxLootPoints;

	public float knockbackDist;
	
	//POLISH
	public Color normalColor, hurtColor;
	public ARLTimer hurtTimer;
	public GameObject deathEffect;
	public GameObject lootEffect;
	
	//DROPS
	public GameObject heartPrefab;

	// Use this for initialization
	protected virtual void Start () {
		//gameObject.SetActive(false);
		//Spawn(new Vector3(0,0,0), new Vector3(30, 0, 20));
	}
	
	// Update is called once per frame
	void Update () {

		AI();

		Move();
		
		Attack();

		Polish();

		//stop the character from floating away because of physics
		rigidbody.velocity = Vector3.zero;

	}

	public virtual void Spawn(Vector3 roomPos, Vector3 roomScale) {

		lootPoints = Random.Range(0, maxLootPoints);

		Vector3 spawnPos = roomPos + new Vector3(Random.Range(roomScale.x/2, -roomScale.x/2), 0, Random.Range(roomScale.z/2, -roomScale.z/2));
		bool hasGoodSpawnPos = false;

		while (!hasGoodSpawnPos) {
			Ray ray = new Ray(spawnPos + Vector3.left, Vector3.right);
			RaycastHit hit;
			if (Physics.SphereCast(ray, 2f, out hit, 2f)) {
				hasGoodSpawnPos = false;
				spawnPos = roomPos + new Vector3(Random.Range(roomScale.x/2, -roomScale.x/2), 0, Random.Range(roomScale.z/2, -roomScale.z/2));
			}
			else {
				hasGoodSpawnPos = true;
			}
		}

		transform.position = spawnPos;
		gameObject.SetActive(true);

		hurtTimer.SetDone();
	}

	protected virtual void AI() {}

	protected virtual void Move() {}

	protected virtual void Attack() {}

	protected virtual void Polish() {
		if (hurtTimer.IsDone()) {
			renderer.material.color = normalColor;
		}
	}

	public virtual void TakeDamage(int damage, DirectionHandler.Directions dir) {
		if (hurtTimer.PercentDone() > 0.5f) {
			hitPoints -= damage;

			//transform.Translate(DirectionHandler.Instance.DirectionToVector(dir) * knockbackDist, Space.World);
			Knockback(dir);

			renderer.material.color = hurtColor;
			hurtTimer.Restart();

			if (hitPoints <= 0) {
				OnDeath();
				Destroy(gameObject);
			}
			else {
				AudioHandler.Play(Audio.enemyHurt); //SFX
			}
		}
	}

	void Knockback(DirectionHandler.Directions dir) {
		float dist = knockbackDist;
		Vector3 dirV = DirectionHandler.Instance.DirectionToVector(dir);
		Ray ray = new Ray(transform.position, dirV);
		RaycastHit hit;
		
		if (Physics.Raycast(ray, out hit, dist)) {
			dist = hit.distance / 2;
		}
		
		transform.Translate(dirV * dist, Space.World);
	}

	protected virtual void OnDeath() {
		AudioHandler.Play(Audio.enemyDie); //SFX
		Instantiate(deathEffect, transform.position, deathEffect.transform.rotation);

		Avatar.Instance.GetLoot(lootPoints);
		Deck.Instance.AddPoints(lootPoints);

		if (Random.Range(0f,100f) <= 30f) {
			Instantiate(heartPrefab, transform.position, heartPrefab.transform.rotation);
		}

		GameObject lfx = Instantiate(lootEffect, transform.position, lootEffect.transform.rotation) as GameObject;
		lfx.GetComponent<LootPointEffect>().repeatNum = lootPoints;
	}
}
