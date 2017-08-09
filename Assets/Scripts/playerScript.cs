using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class playerScript : MonoBehaviour {

	float speed = 10.0f;
	bool facingRight = true;
	int jumpForce = 20;
	public float maxHealth = 1000.0f;
	public float currentHealth = 0f;
	public GameObject healthBar;
	public Text healthText;
	public GameObject bullet;
	public float autoAttackTimer = 1.2f;
	private Rigidbody2D rig;
	bool shoot = false;
	GameObject target;
	public GameObject player;
/*	public GameObject redMinionIcon;
	public GameObject blueMinionIcon;
	public GameObject iconBorder;
	public GameObject iconSword;*/
	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;
		healthText.text = "" + currentHealth;
	}

	// Update is called once per frame
	void Update () {
		movement();
		autoAttack();
		if (autoAttackTimer > 0) {
			autoAttackTimer -= Time.deltaTime;
		}
	}

	bool inRange(Vector2 rangePos, int range){
		float pytha = Mathf.Pow(rangePos.x - this.gameObject.transform.position.x, 2) + Mathf.Pow(rangePos.y - this.gameObject.transform.position.y, 2);
		if (Mathf.Sqrt (pytha) <= range) {
			return true;
		} else {
			return false;
		}
	}
	/*
	void changeTargetOverlay(string target){
		if (target == "redMinion") {
			blueMinionIcon.GetComponent<SpriteRenderer> ().enabled = false;
			redMinionIcon.GetComponent<SpriteRenderer> ().enabled = true;
			iconSword.GetComponent<SpriteRenderer> ().enabled = true;
			iconBorder.GetComponent<Image> ().enabled = true;
		} else {
			blueMinionIcon.GetComponent<SpriteRenderer> ().enabled = false;
			redMinionIcon.GetComponent<SpriteRenderer> ().enabled = false;
			iconSword.GetComponent<SpriteRenderer> ().enabled = false;
			iconBorder.GetComponent<Image> ().enabled = false;
		}
	}*/

	void autoAttack() {
		if (Input.GetMouseButtonDown (1)) {
			Vector2 worldPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast (worldPoint, Vector2.zero);
			if (hit.collider != null) {
				if (hit.collider.CompareTag ("redMinion")) {
					shoot = true;
					target = hit.collider.gameObject;
				//	changeTargetOverlay ("redMinion");
				} else {
					shoot = false;
				//	changeTargetOverlay ("");
				}
			} else {
				shoot = false;
				//changeTargetOverlay ("");
			}
		}
		if (shoot == true) {
			if (inRange (target.transform.position, 5)) {
				if (autoAttackTimer <= 0) {
					Instantiate (bullet, this.gameObject.transform.position, this.gameObject.transform.rotation);
					bulletScript bScript = bullet.GetComponent<bulletScript> ();
					bScript.destination = target;
					autoAttackTimer = 1.2f;
				}
			} else {
				Debug.Log ("outofrange");
				this.transform.position = Vector2.MoveTowards (this.transform.position, target.transform.position, Time.deltaTime);
				if (inRange (target.transform.position, 5)) {
					Debug.Log ("inrange");
					rig = GetComponent<Rigidbody2D> ();
					rig.AddForce (Vector2.zero);
					if (autoAttackTimer <= 0) {
						Instantiate (bullet, this.gameObject.transform.position, this.gameObject.transform.rotation);
						bulletScript bScript = bullet.GetComponent<bulletScript> ();
						bScript.destination = target;
						autoAttackTimer = 1.2f;
					}
				}
			}
		}
	}

	void decreaseHealth(float decreaseBy) {
		if (currentHealth > 0) {
			currentHealth -= decreaseBy;
			if (currentHealth < 0) {
				currentHealth = 0;
			}
			float healthCalc = currentHealth / maxHealth;
			setHealth(healthCalc);
			int currentHealthText = (int)currentHealth;
			healthText.text = "" + currentHealthText;
		}
	}

	void movement() {
		if (Input.GetKey (KeyCode.A)) {
			transform.Translate (Vector2.left * speed * Time.deltaTime);
			shoot = false;
			if (facingRight) {
				//flipHorizantally ();
			}
		}
		if (Input.GetKey (KeyCode.D)) {
			transform.Translate (Vector2.right * speed * Time.deltaTime);
			shoot = false;
			if (facingRight == false) {
				//flipHorizantally ();
			}
		}
		if (Input.GetKeyDown (KeyCode.W)) {
			jump();
			shoot = false;
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

	void jump() {
		this.gameObject.GetComponent<Rigidbody2D> ().AddForce(new Vector2 (0, jumpForce), ForceMode2D.Impulse);
		this.gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2(0, this.gameObject.GetComponent<Rigidbody2D> ().velocity.y);
	}

	public void setHealth(float myHealth) {
		healthBar.transform.localScale = new Vector3 (myHealth, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
	}
}
