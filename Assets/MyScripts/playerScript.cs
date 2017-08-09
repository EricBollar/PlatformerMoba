using UnityEngine;
using System.Collections;

public class playerScript : MonoBehaviour {

	float speed = 10.0f;
	bool facingRight = true;
	bool onGround = true;
	int jumpForce = 15;
	int platformSpeedrl;
	int platformSpeedud;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		move();
		transform.Translate (Vector3.right * platformSpeedrl * Time.deltaTime);
		transform.Translate (Vector3.up * platformSpeedud * Time.deltaTime);
		this.gameObject.GetComponent<Animation> ().Play ();
	}

	void move() {
		if (Input.GetKey (KeyCode.A)) {
			transform.Translate (Vector2.left * speed * Time.deltaTime);
			if (facingRight) {
				flipHorizantally ();
			}
		}
		if (Input.GetKey (KeyCode.D)) {
			transform.Translate (Vector2.right * speed * Time.deltaTime);
			if (facingRight == false) {
				flipHorizantally ();
			}
		}
		if (Input.GetKeyDown (KeyCode.W)) {
			if (onGround == true) {
				jump ();
			}
		}
	}

	void flipHorizantally() {
			// Switch the way the player is labelled as facing.
			facingRight = !facingRight;

			// Multiply the player's x local scale by -1.
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
	}

	void OnTriggerStay2D (Collider2D other){
		if (other.gameObject.CompareTag("platform")) {
			onGround = true;
			if (other.gameObject.GetComponent<rightleft> ()) {
				platformSpeedrl = other.gameObject.GetComponent<rightleft> ().speed;
			}
			if (other.gameObject.GetComponent<updown> ()) {
				platformSpeedud = other.gameObject.GetComponent<updown> ().speed;
			}
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		platformSpeedrl = 0;
		platformSpeedud = 0;
	}

	void jump() {
		this.gameObject.GetComponent<Rigidbody2D> ().AddForce(new Vector2 (0, jumpForce), ForceMode2D.Impulse);
		this.gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2(0, this.gameObject.GetComponent<Rigidbody2D> ().velocity.y);
		onGround = false;
	}
}
