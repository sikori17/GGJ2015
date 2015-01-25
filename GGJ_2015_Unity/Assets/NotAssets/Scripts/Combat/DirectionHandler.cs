using UnityEngine;
using System.Collections;

public class DirectionHandler : MonoBehaviour {

	public static DirectionHandler Instance;

	public enum Directions { Up = 0, Down, Left, Right };

	void Start() {
		Instance = this;
	}

	public Vector3 DirectionToVector(Directions d) {
		switch (d) {
			case Directions.Up:
				return Vector3.forward;
				break;
			case Directions.Down:
				return Vector3.back;
				break;
			case Directions.Left:
				return Vector3.left;
				break;
			case Directions.Right:
				return Vector3.right;
				break;
		}
		return Vector3.zero;
	}

	public Directions VectorToDirection(Vector3 v) {
		if (Mathf.Abs(v.x) > Mathf.Abs(v.z)) {
			v.z = 0f;
		}
		else {
			v.x = 0f;
		}
		v.y = 0f;

		v = v.normalized;

		if (v == Vector3.forward) {
			return Directions.Up;
		}
		else if (v == Vector3.back) {
			return Directions.Down;
		}
		else if (v == Vector3.left) {
			return Directions.Left;
		}
		else if (v == Vector3.right) {
			return Directions.Right;
		}

		return Directions.Down;
	}

	public Directions RandomDirection() {
		return (Directions) Random.Range(0,4);
	}

	public Directions OppositeDirection(Directions d) {
		switch (d) {
		case Directions.Up:
			return Directions.Down;
			break;
		case Directions.Down:
			return Directions.Up;
			break;
		case Directions.Left:
			return Directions.Right;
			break;
		case Directions.Right:
			return Directions.Left;
			break;
		}
		return Directions.Down;
	}
}
