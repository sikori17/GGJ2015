using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public int hitPoints;
	public int lootPoints;

	public float knockbackDist;
	
	//POLISH
	public Color normalColor, hurtColor;
	public ARLTimer hurtTimer;
	public GameObject deathEffect;

	// Use this for initialization
	protected virtual void Start () {
		hurtTimer.SetDone();
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
	}
}
