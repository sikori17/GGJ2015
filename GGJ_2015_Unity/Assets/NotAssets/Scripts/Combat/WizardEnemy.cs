using UnityEngine;
using System.Collections;

public class WizardEnemy : Enemy {

	//MOVEMENT
	public SimpleStateMachine stateMachine;
	SimpleState attackState, dissapearState, invisibleState, appearState;
	public ARLTimer attackTimer, fadeTimer, invisibleTimer;
	public Vector3 roomPos, roomScale;

	//ATTACK
	public GameObject fireballPrefab;
	public ARLTimer fireballTimer;
	public float fireballLaunchRange, attackRange;
	Vector3 attackVector;

	//POLISH
	public Color fadeColor, hatColor;
	public GameObject hat;
	
	public override void Spawn (Vector3 roomPos, Vector3 roomScale)
	{
		base.Spawn (roomPos, roomScale);

		this.roomPos = roomPos;
		this.roomScale = roomScale;
		
		appearState = new SimpleState(AppearEnter, AppearUpdate, AppearExit, "APPEAR");
		attackState = new SimpleState(AttackEnter, AttackUpdate, AttackExit, "ATTACK");
		dissapearState = new SimpleState(DissapearEnter, DissapearUpdate, DissapearExit, "DISSAPEAR");
		invisibleState = new SimpleState(InvisibleEnter, InvisibleUpdate, InvisibleExit, "INVISIBLE");
		
		stateMachine.SwitchStates(appearState);
	}

	protected override void AI ()
	{
		stateMachine.Execute();
	}

	protected override void Attack ()
	{
		if (stateMachine.currentState == "ATTACK" && fireballTimer.IsDone() && 
		    hurtTimer.IsDone() && Vector3.Distance(transform.position, Avatar.Instance.transform.position) < attackRange) {

			GameObject newFireball = Instantiate(fireballPrefab, 
			                                     transform.position + attackVector * fireballLaunchRange, 
			                                     fireballPrefab.transform.rotation) as GameObject;
			newFireball.GetComponent<Fireball>().Launch(attackVector);

			fireballTimer.Restart();
		}
	}

	protected override void Polish ()
	{
		if (stateMachine.currentState == "ATTACK") {
			base.Polish ();
		}
	}

	void NewRandomPosInRoom() {
		transform.position = roomPos + new Vector3(Random.Range(roomScale.x/2, -roomScale.x/2), 0, Random.Range(roomScale.z/2, -roomScale.z/2));
	}
	
	void AppearEnter() {
		NewRandomPosInRoom();

		collider.enabled = false;
		renderer.material.color = fadeColor;

		fadeTimer.Restart();
	}
	
	void AppearUpdate() {
		if (fadeTimer.IsDone()) {
			stateMachine.SwitchStates(attackState);
		}
		else {
			renderer.material.color = Color.Lerp(fadeColor, normalColor, fadeTimer.PercentDone());
			hat.renderer.material.color = Color.Lerp(fadeColor, hatColor, fadeTimer.PercentDone());
		}
	}
	
	void AppearExit() {
		renderer.material.color = normalColor;
	}

	void AttackEnter() {
		collider.enabled = true;
		renderer.material.color = normalColor;

		fireballTimer.SetDone();
		attackVector = (Avatar.Instance.transform.position - transform.position).normalized;

		attackTimer.Restart();
	}

	void AttackUpdate() {
		if (attackTimer.IsDone() && hurtTimer.IsDone()) {
			stateMachine.SwitchStates(dissapearState);
		}
	}

	void AttackExit() {
	}

	void DissapearEnter() {
		collider.enabled = false;
		renderer.material.color = normalColor;
		
		fadeTimer.Restart();
	}

	void DissapearUpdate() {
		if (fadeTimer.IsDone()) {
			stateMachine.SwitchStates(invisibleState);
		}
		else {
			renderer.material.color = Color.Lerp(normalColor, fadeColor, fadeTimer.PercentDone());
			hat.renderer.material.color = Color.Lerp(hatColor, fadeColor, fadeTimer.PercentDone());
		}
	}

	void DissapearExit() {
		renderer.material.color = fadeColor;
	}

	void InvisibleEnter() {
		collider.enabled = false;
		renderer.material.color = fadeColor;

		invisibleTimer.Restart();
	}

	void InvisibleUpdate() {
		if (invisibleTimer.IsDone()) {
			stateMachine.SwitchStates(appearState);
		}
	}

	void InvisibleExit() {
	}
}
