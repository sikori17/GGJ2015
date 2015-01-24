using UnityEngine;
using System.Collections;

public class Avatar : MonoBehaviour {

	//INSTANCE
	public static Avatar Instance;

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
	public int lootPoints;

	//POLISH
	public Color normalColor, hurtColor;
	public Oscillator hurtOscillator;

	//MESSAGES
	public delegate void GameOverAction();
	public event GameOverAction OnGameOverMessage;

	// Use this for initialization
	void Start () {
		Instance = this;

		direction = DirectionHandler.Directions.Down;
		invulnerabilityTimer.SetDone();
		hurtOscillator.Restart();
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
		if (ControllerInput.Dpad_Button(1, Dpad.Xbox_Up)) {
			direction = DirectionHandler.Directions.Up;
		}
		else if (ControllerInput.Dpad_Button(1, Dpad.Xbox_Down)) {
			direction = DirectionHandler.Directions.Down;
		}
		else if (ControllerInput.Dpad_Button(1, Dpad.Xbox_Left)) {
			direction = DirectionHandler.Directions.Left;
		}
		else if (ControllerInput.Dpad_Button(1, Dpad.Xbox_Right)) {
			direction = DirectionHandler.Directions.Right;
		}
		else {
			isMoving = false;
		}

		if (ControllerInput.ButtonDown(1, Button.Xbox_A) && !isAttacking && attackCooldownTimer.IsDone()) {
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
	}
	
	void StartAttack() {
		isAttacking = true;
		attackDirection = direction;
		
		attackTimer.Restart();
		swords[(int) attackDirection].SetActive(true);
	}

	public void TakeDamage(int damage, DirectionHandler.Directions dir) { 
		if (invulnerabilityTimer.IsDone()) {
			hitPoints -= damage;
			transform.Translate(DirectionHandler.Instance.DirectionToVector(dir) * knockbackDist);
			invulnerabilityTimer.Restart();
			hurtOscillator.Restart();

			if (hitPoints <= 0) {
				Destroy(gameObject);
				OnGameOverMessage();
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.GetComponent<DoesDamage>() != null && invulnerabilityTimer.IsDone()) {
			Vector3 v = transform.position - other.transform.position;
			TakeDamage(other.gameObject.GetComponent<DoesDamage>().damage, DirectionHandler.Instance.VectorToDirection(v));
		}
	}

	void OnCollisionEnter(Collision other) {
		if (other.gameObject.GetComponent<DoesDamage>() != null && invulnerabilityTimer.IsDone()) {
			Vector3 v = transform.position - other.transform.position;
			TakeDamage(other.gameObject.GetComponent<DoesDamage>().damage, DirectionHandler.Instance.VectorToDirection(v));
		}
	}
}
