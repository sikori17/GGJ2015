using UnityEngine;
using System.Collections;

public class LootPointEffect : MonoBehaviour {

	public float speed;
	public ARLTimer timer;
	public int repeatNum;

	int count;
	Vector3 startPos;

	// Use this for initialization
	void Start () {
		startPos = transform.position;
		timer.Restart();
		AudioHandler.Play(Audio.lootPoint);
	}
	
	// Update is called once per frame
	void Update () {
		if (timer.IsDone()) {
			if (count >= repeatNum) {
				Destroy(gameObject);
			}
			else {
				timer.Restart();
				transform.position = startPos;
				AudioHandler.Play(Audio.lootPoint);

				count++;
			}
		}
		else {
			transform.Translate(Vector3.forward * speed * Time.deltaTime);
		}
	}
}
