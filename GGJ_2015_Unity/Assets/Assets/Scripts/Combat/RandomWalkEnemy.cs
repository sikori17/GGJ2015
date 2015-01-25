using UnityEngine;
using System.Collections;

public class RandomWalkEnemy : Enemy {

	//MOVEMENT
	public float speed;
	public DirectionHandler.Directions direction;
	public ARLTimer moveTimer;
	Vector3 roomPos, roomScale; //bounds

	public override void Spawn (Vector3 roomPos, Vector3 roomScale)
	{
		base.Spawn (roomPos, roomScale);

		this.roomPos = roomPos;
		this.roomScale = roomScale;
		
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

		if (OutOfBounds()) {
			GoOppositeDirection();
		}
	}

	bool OutOfBounds() {
		return transform.position.x > (roomPos.x + roomScale.x/2) || transform.position.x < (roomPos.x - roomScale.x/2) ||
			transform.position.z > (roomPos.z + roomScale.z/2) || transform.position.z < (roomPos.z - roomScale.z/2);
	}

	void PickNewDirection() {
		direction = DirectionHandler.Instance.RandomDirection();
		moveTimer.Restart();
	}
	
	void GoOppositeDirection() {
		direction = DirectionHandler.Instance.OppositeDirection(direction);
	}
	
	void OnCollisionEnter(Collision other) {
		GoOppositeDirection();
	}
}
