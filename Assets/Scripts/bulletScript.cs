using UnityEngine;
using System.Collections;

public class bulletScript : MonoBehaviour {

	public GameObject destination;
	public Vector2 destinationPoint;
    Rigidbody2D myrig;
    public float deathTimer = 5.5f;

	// Use this for initialization
	void Start () {
		if (destination == null) {
			Destroy (this.gameObject);
		}
        myrig = this.gameObject.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		deathTimer -= Time.deltaTime;
		if (deathTimer <= 0) {
			Destroy (this.gameObject);
		}
		destinationPoint = destination.transform.position;
		transform.position = Vector2.MoveTowards (transform.position, destinationPoint, 10.0f * Time.deltaTime);

        Vector2 mypos = new Vector2(this.transform.position.x, this.transform.position.y);

        if (mypos == destinationPoint)
        {
            Destroy(this.gameObject);
        }

        if (myrig.IsSleeping())
        {
            Debug.Log("stopoped");
            Destroy(this.gameObject);
        }
	}
}
