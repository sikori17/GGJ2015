using UnityEngine;
using System.Collections;

public class Avatar : MonoBehaviour {

	//INSTANCE
	public static Avatar Instance;

	//INPUT
	public int controllerNum;

	//MOVEMENT
	public float speed;
	DirectionHandler.Directions direction;
	bool isMoving;

	//ATTACKING
	public int attackDamage;
	public float attackRange;
	public GameObject[] swords;
	DirectionHandler.Directions attackDirection;
	public ARLTimer attackTimer, attackCooldownTimer;
	bool isAttacking;
	public float knockbackDist;
	public ARLTimer invulnerabilityTimer;

	//STATS
	public int hitPoints;
	public int lootPoints, maxLootPoints;
	public int lowHitPoints;

	//POLISH
	public Color normalColor, hurtColor;
	public Oscillator hurtOscillator;
	public GameObject explosion;

	//MESSAGES
	public delegate void GameOverAction();
	public event GameOverAction OnGameOverMessage;

	//SFX
	public ARLTimer bumpTimer, beepTimer;

	// Use this for initialization
	void Start () {
		Instance = this;

		direction = DirectionHandler.Directions.Down;
		invulnerabilityTimer.SetDone();
		hurtOscillator.Restart();

		bumpTimer.SetDone();
		beepTimer.SetDone();
	}
	
	// Update is called once per frame
	void Update () {

		ReadInput();

		Move();

		Attack();

		Polish();

		//stop the character from floating away because of physics
		rigidbody.velocity = Vector3.zero;
	}

	void ReadInput() {
		isMoving = true;
		if (ControllerInput.Dpad_Button(controllerNum, Dpad.Xbox_Up)) {
			direction = DirectionHandler.Directions.Up;
		}
		else if (ControllerInput.Dpad_Button(controllerNum, Dpad.Xbox_Down)) {
			direction = DirectionHandler.Directions.Down;
		}
		else if (ControllerInput.Dpad_Button(controllerNum, Dpad.Xbox_Left)) {
			direction = DirectionHandler.Directions.Left;
		}
		else if (ControllerInput.Dpad_Button(controllerNum, Dpad.Xbox_Right)) {
			direction = DirectionHandler.Directions.Right;
		}
		else {
			isMoving = false;
		}

		if (ControllerInput.ButtonDown(controllerNum, Button.Xbox_A) && !isAttacking && attackCooldownTimer.IsDone()) {
			StartAttack();
		}
	}

	void Move() {
		if (isMoving) {
			transform.Translate(DirectionHandler.Instance.DirectionToVector(direction) * speed * Time.deltaTime);
		}
	}

	void Attack() {
		if (isAttacking) {
			if (attackTimer.IsDone()) {
				swords[(int) attackDirection].SetActive(false);

				attackCooldownTimer.Restart();
				isAttacking = false;
			}
			else {
				Ray ray = new Ray(transform.position, DirectionHandler.Instance.DirectionToVector(attackDirection));
				RaycastHit hit;
				if (Physics.SphereCast(ray, 1f, out hit, attackRange)) {
					if (hit.collider.gameObject.GetComponent<Enemy>() != null) {
						hit.collider.gameObject.GetComponent<Enemy>().TakeDamage(attackDamage, attackDirection);
					}
					else if (hit.collider.tag == "Wall") {
						AudioHandler.Play(Audio.bumpSword); //SFX
					}
				}
			}
		}
	}

	void Polish() {
		if (!invulnerabilityTimer.IsDone()) {
			renderer.material.color = Color.Lerp(hurtColor, normalColor, hurtOscillator.Displacement());
		}
		else {
			renderer.material.color = normalColor;
		}

		if (hitPoints <= lowHitPoints && beepTimer.IsDone()) {
			AudioHandler.Play(Audio.warningBeep); //SFX
			beepTimer.Restart();
		}
	}
	
	void StartAttack() {
		AudioHandler.Play(Audio.sword);

		isAttacking = true;
		attackDirection = direction;
		
		attackTimer.Restart();
		swords[(int) attackDirection].SetActive(true);
	}

	public void GetLoot(int loot) {
		lootPoints += loot;
		GameplayUI.Instance.SetXP( ((float) lootPoints) / ((float) maxLootPoints) );
	}

	public void TakeDamage(int damage, DirectionHandler.Directions dir) { 
		if (invulnerabilityTimer.IsDone()) {
			hitPoints -= damage;

			//transform.Translate(DirectionHandler.Instance.DirectionToVector(dir) * knockbackDist);
			Knockback(dir);

			invulnerabilityTimer.Restart();
			hurtOscillator.Restart();

			if (hitPoints <= 0) {
				AudioHandler.Play(Audio.playerDie); //SFX
				Instantiate(explosion, transform.position, explosion.transform.rotation);

				Destroy(gameObject);
				OnGameOverMessage();
			}
			else {
				AudioHandler.Play(Audio.playerHurt); //SFX
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

		transform.Translate(dirV * dist);
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.GetComponent<DoesDamage>() != null && invulnerabilityTimer.IsDone()) {
			Vector3 v = transform.position - other.transform.position;
			TakeDamage(other.gameObject.GetComponent<DoesDamage>().damage, DirectionHandler.Instance.VectorToDirection(v));
		}
	}

	void OnCollisionEnter(Collision other) {

		Debug.Log(other.gameObject.name);

		if (other.gameObject.GetComponent<DoesDamage>() != null && invulnerabilityTimer.IsDone()) {
			Vector3 v = transform.position - other.transform.position;
			TakeDamage(other.gameObject.GetComponent<DoesDamage>().damage, DirectionHandler.Instance.VectorToDirection(v));
		}
		else if (other.gameObject.tag == "Wall" && bumpTimer.IsDone()) {
			AudioHandler.Play(Audio.bump); //SFX
			bumpTimer.Restart();
		}
	}

	void OnCollisionStay(Collision other) {
		if (other.gameObject.tag == "Wall" && bumpTimer.IsDone()) {
			AudioHandler.Play(Audio.bump); //SFX
			bumpTimer.Restart();
		}
	}
}
