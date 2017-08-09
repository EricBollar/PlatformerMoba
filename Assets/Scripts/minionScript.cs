using UnityEngine;
using System.Collections;

public class minionScript : MonoBehaviour {

	public float maxHealth = 100.0f;
	public float currentHealth = 0f;
	public GameObject healthBar;
	public GameObject bullet;
	public GameObject player;
	public float autoAttackTimer = 1.3f;
	GameObject[] minions;

	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;
		minions = GameObject.FindGameObjectsWithTag("allyMinion");
	}
	
	// Update is called once per frame
	void Update () {
		minions = GameObject.FindGameObjectsWithTag("allyMinion");
		if (minions != null) {
			if (findEnemyInRange ()) {
				shoot (FindClosestEnemyMinion ());
			} else if (playerIsTarget ()) {
				Debug.Log ("yay");
				shoot (player);
			}
		}
		checkHealth ();
	}

	bool findEnemyInRange() {
		if (GameObject.Find ("allyMinion") != null) {
			if (inRange (FindClosestEnemyMinion ().transform.position, 5)) {
				return true;
			} else {
				return false;
			}
		} else {
			return false;
		}
	}

	void shoot(GameObject target) {
		if (autoAttackTimer <= 0) {
			Instantiate (bullet, this.gameObject.transform.position, this.gameObject.transform.rotation);
			bulletScript bScript = bullet.GetComponent<bulletScript> ();
			bScript.destination = target;
			autoAttackTimer = 1.3f;
		} else {
			autoAttackTimer -= Time.deltaTime;
		}
	}

	bool playerIsTarget() {
		if (inRange (GameObject.FindGameObjectWithTag ("Player").transform.position, 5)) {
			return true;
		} else {
			return false;
		}
	}

	GameObject FindClosestEnemyMinion() {
		GameObject[] g;
		g = GameObject.FindGameObjectsWithTag("allyMinion");
		GameObject closest = null;
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
		foreach (GameObject go in g)
		{
			Vector3 diff = go.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance)
			{
				closest = go;
				distance = curDistance;
			}
		}
		return closest;
	}



	bool inRange(Vector2 rangePos, int range){
		float pytha = Mathf.Pow(rangePos.x - this.gameObject.transform.position.x, 2) + Mathf.Pow(rangePos.y - this.gameObject.transform.position.y, 2);
		if (Mathf.Sqrt (pytha) <= range) {
			return true;
		} else {
			return false;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag("bullet")) {
			decreaseHealth(10);
			Destroy(other.gameObject);
		}
	}

	void checkHealth() {
		if (currentHealth <= 0) {
			Destroy (this.gameObject);
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
		}
	}

	public void setHealth(float myHealth) {
		healthBar.transform.localScale = new Vector3 (myHealth, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
	}
}
