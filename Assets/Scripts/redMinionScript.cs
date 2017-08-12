using UnityEngine;
using System.Collections;

public class redMinionScript : MonoBehaviour {

    public float maxHealth = 100.0f;
    public float currentHealth = 0f;
    public GameObject healthBar;
    public GameObject bullet;
    public GameObject player;
    public Rigidbody2D rig;
    public float speed = 3.0f;
    public float autoAttackTimer = 1.3f;
    GameObject[] minions;

    // Use this for initialization
    void Start()
    {
        currentHealth = maxHealth; //set the health to max health when instantiated
        minions = GameObject.FindGameObjectsWithTag("blueMinion"); //the minions array is set to all of the instantiated blue minions
    }

    // Update is called once per frame
    void Update()
    {
        findClosestEnemy();
        checkHealth();
    }

    void findClosestEnemy() //find the nearest enemy and shoot them if they are in range
    {
        minions = GameObject.FindGameObjectsWithTag("blueMinion");
        if (minions != null)
        {
            if (findEnemyInRange())
            {
                shoot(FindClosestEnemyMinion());
            }
            else if (playerIsTarget())
            {
                shoot(GameObject.FindGameObjectWithTag("Player"));
            } else
            {
                transform.Translate(Vector2.left * speed * Time.deltaTime);
            }
        }
    }

    bool findEnemyInRange() //find the nearest enemy
    {
        if (GameObject.Find("blueMinion") != null)
        {
            if (inRange(FindClosestEnemyMinion().transform.position, 5))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    void shoot(GameObject target) //instantiate a bullet that will follow its destination until it hits its target
    {
        if (autoAttackTimer <= 0)
        {
            Instantiate(bullet, this.gameObject.transform.position, this.gameObject.transform.rotation);
            bulletScript bScript = bullet.GetComponent<bulletScript>();
            bScript.destination = target;
            autoAttackTimer = 1.3f;
        }
        else
        {
            autoAttackTimer -= Time.deltaTime;
        }
    }

    bool playerIsTarget() //check if the player is in range
    {
        if (inRange(GameObject.FindGameObjectWithTag("Player").transform.position, 5))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    GameObject FindClosestEnemyMinion() //return a specific gameobject of the closest enemy minion
    {
        GameObject[] g;
        g = GameObject.FindGameObjectsWithTag("blueMinion");
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



    bool inRange(Vector2 rangePos, int range) //check if something is in range
    {
        float pytha = Mathf.Pow(rangePos.x - this.gameObject.transform.position.x, 2) + Mathf.Pow(rangePos.y - this.gameObject.transform.position.y, 2);
        if (Mathf.Sqrt(pytha) <= range)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("blueMinionBullet")) //if I am shot by a blue minion bullet, subtract 10 health, and destroy the bullet
        {
            decreaseHealth(10);
            Destroy(other.gameObject);
        }
    }

    void checkHealth() //if i am dead, delete me
    {
        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    void decreaseHealth(float decreaseBy) //decrease my health by a number
    {
        if (currentHealth > 0)
        {
            currentHealth -= decreaseBy;
            if (currentHealth < 0)
            {
                currentHealth = 0;
            }
            float healthCalc = currentHealth / maxHealth;
            setHealth(healthCalc);
        }
    }

    public void setHealth(float myHealth) //set my healthbar to my health
    {
        healthBar.transform.localScale = new Vector3(myHealth, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
    }
}
