using UnityEngine;
using System.Collections;

public class SlugEnemy : Enemy {

	//MOVEMENT
	public float speed;
	public DirectionHandler.Directions direction;
	public ARLTimer moveTimer;

	protected override void Start ()
	{
		base.Start ();

		direction = DirectionHandler.Directions.Down;
		PickNewDirection();
	}

	protected override void AI ()
	{
		if (moveTimer.IsDone()) {
			PickNewDirection();
		}
	}

	protected override void Move(){
		transform.Translate(DirectionHandler.Instance.DirectionToVector(direction) * speed * Time.deltaTime, Space.World);
	}

	void PickNewDirection() {
		direction = DirectionHandler.Instance.RandomDirection();
		transform.forward = DirectionHandler.Instance.DirectionToVector(direction);
		moveTimer.Restart();
	}

	void GoOppositeDirection() {
		direction = DirectionHandler.Instance.OppositeDirection(direction);
		transform.forward = DirectionHandler.Instance.DirectionToVector(direction);
	}

	void OnCollisionEnter(Collision other) {
		GoOppositeDirection();
	}
}
