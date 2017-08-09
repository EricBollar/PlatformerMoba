using UnityEngine;
using System.Collections;

public class allyMinionScript : MonoBehaviour {

	public float maxHealth = 100.0f;
	public float currentHealth = 0f;
	public float autoAttackTimer = 1.3f;
	public GameObject bullet;
	public GameObject healthBar;

	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;
	}

	// Update is called once per frame
	void Update () {
		if (findEnemyMinionInRange ()) {
			shoot (FindClosestEnemy());
		}
		checkHealth ();
	}

	bool findEnemyMinionInRange() {
		if (inRange (FindClosestEnemy ().transform.position, 5)) {
			return true;
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

	GameObject FindClosestEnemy() {
		GameObject[] g;
		g = GameObject.FindGameObjectsWithTag("redMinion");
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

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag("enemyBullet")) {
			decreaseHealth(90);
			Destroy(other.gameObject);
		}
	}

	void checkHealth() {
		if (currentHealth <= 0) {
			Destroy (this.gameObject);
		}
	}

	public void setHealth(float myHealth) {
		healthBar.transform.localScale = new Vector3 (myHealth, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
	}
}
