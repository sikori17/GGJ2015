using UnityEngine;
using System.Collections;

public class AnimateAvatar : MonoBehaviour {

	public ARLTimer animateTimer;
	public float moveSpeed;
	public Vector3 moveDir;

	// Use this for initialization
	void Start () {
		GetComponent<Avatar>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {

		if (animateTimer.IsDone()) {
			GetComponent<Avatar>().enabled = true;
		}
		else {
			transform.Translate(moveSpeed * moveDir * Time.deltaTime);
		}
	}
}
