using UnityEngine;
using System.Collections;

public class rightleft : MonoBehaviour {

	public int speed = 3;
	float timer = 4.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		speedTimer();
		transform.Translate (Vector2.right * speed * Time.deltaTime);
	}

	void speedTimer() {
		timer -= Time.deltaTime;
		if (timer <= 0) {
			speed = speed * -1;
			timer = 4.0f;
		}
	}
}
