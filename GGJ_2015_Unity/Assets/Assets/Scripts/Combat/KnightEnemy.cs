using UnityEngine;
using System.Collections;

public class KnightEnemy : RandomWalkEnemy {

	//MOVEMENT
	public float followRange;
	public float slowSpeed, fastSpeed;
	public ARLTimer followTimer;
	
	//ATTACK AND DEFENSE
	public SimpleStateMachine stateMachine;
	SimpleState swordState, shieldState, vulnerableState;
	public float attackRange;
	public int attackDamage;
	public ARLTimer shieldTimer, vulnerableTimer, swordTimer;
	public GameObject[] shields, swords;

	public override void Spawn (Vector3 roomPos, Vector3 roomScale)
	{
		base.Spawn (roomPos, roomScale);
		
		followTimer.SetDone();
		
		shieldState = new SimpleState(ShieldEnter, ShieldUpdate, ShieldExit, "SHIELD");
		vulnerableState = new SimpleState(VulnerableEnter, VulnerableUpdate, null, "VULNERABLE");
		swordState = new SimpleState(SwordEnter, SwordUpdate, SwordExit, "SWORD");
		
		stateMachine.SwitchStates(shieldState);
	}

	protected override void AI ()
	{
		if (Vector3.Distance(Avatar.Instance.transform.position, transform.position) < followRange) {
			speed = fastSpeed;
			if (followTimer.IsDone()) {
				Vector3 toAvatarV = Avatar.Instance.transform.position - transform.position;
				direction = DirectionHandler.Instance.VectorToDirection(toAvatarV);
				followTimer.Restart();
			}
		}
		else {
			speed = slowSpeed;
			base.AI ();
		}

		stateMachine.Execute();
	}

	protected override void Attack ()
	{
		if (stateMachine.currentState == "SWORD") {
			
			Ray ray = new Ray(transform.position, DirectionHandler.Instance.DirectionToVector(direction));
			RaycastHit hit;
			if (Physics.SphereCast(ray, 1f, out hit, attackRange)) {
				if (hit.collider.gameObject.GetComponent<Avatar>() != null) {
					hit.collider.gameObject.GetComponent<Avatar>().TakeDamage(attackDamage, direction);
				}
			}
		}
	}

	void ShieldEnter() {
		shieldTimer.Restart();
	}

	void ShieldUpdate() {
		for (int i = 0; i < shields.Length; i++) {
			shields[i].SetActive(i == (int) direction);
		}

		if (shieldTimer.IsDone()) {
			stateMachine.SwitchStates(vulnerableState);
		}
	}

	void ShieldExit() {
		for (int i = 0; i < shields.Length; i++) {
			shields[i].SetActive(false);
		}
	}

	void VulnerableEnter() {
		vulnerableTimer.Restart();
	}

	void VulnerableUpdate() {
		if (vulnerableTimer.IsDone()) {
			stateMachine.SwitchStates(swordState);
		}
	}

	void SwordEnter() {
		swordTimer.Restart();
	}

	void SwordUpdate() {
		for (int i = 0; i < swords.Length; i++) {
			swords[i].SetActive(i == (int) direction);
		}
		
		if (swordTimer.IsDone()) {
			stateMachine.SwitchStates(shieldState);
		}
	}

	void SwordExit() {
		for (int i = 0; i < swords.Length; i++) {
			swords[i].SetActive(false);
		}
	}

	public override void TakeDamage (int damage, DirectionHandler.Directions dir)
	{
		if (stateMachine.currentState != "SHIELD" || DirectionHandler.Instance.OppositeDirection(dir) != direction) {
			base.TakeDamage (damage, dir);
		}
		else {
			//half a knockback
			transform.Translate(DirectionHandler.Instance.DirectionToVector(dir) * knockbackDist * 0.5f, Space.World);
			AudioHandler.Play(Audio.bumpSword); //SFX
		}
	}

}
