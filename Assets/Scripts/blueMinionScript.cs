using UnityEngine;
using System.Collections;

public class blueMinionScript : MonoBehaviour
{

    public float maxHealth = 100.0f;
    public float currentHealth = 0f;
    public float autoAttackTimer = 1.3f;
    public float speed = 3.0f;
    public GameObject bullet;
    public GameObject healthBar;


    // Use this for initialization
    void Start()
    {
        currentHealth = maxHealth; //set my health to full health
    }

    // Update is called once per frame
    void Update()
    {
        findClosestEnemy();
        checkHealth();
    }

    void findClosestEnemy() //find the nearest enemy and shoot them if they are in range, else move to the right
    {
        if (findEnemyMinionInRange())
        {
            shoot(FindClosestEnemy());
        }else {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
    }

    bool findEnemyMinionInRange() //find the nearest enemy minion in range
    {
        int g;
        g = GameObject.FindGameObjectsWithTag("redMinion").Length;
        if (g != 0)
        {
            Debug.Log("have one");
            if (inRange(FindClosestEnemy().transform.position, 5))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        Debug.Log("nope");
        return false;
    }

    void shoot(GameObject target) //instantiate a bullet that will follow its target until it reaches them
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

    GameObject FindClosestEnemy() //return a gameobject of the closest enemy minion
    {
        GameObject[] g;
        g = GameObject.FindGameObjectsWithTag("redMinion");
        if (g != null)
        {
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
        return null;
    }



    bool inRange(Vector2 rangePos, int range) //check if a target is in range
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("redMinionBullet")) //if i am hit with a red minion bullet, decrease my health by 10, and destroy the bullet
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

    public void setHealth(float myHealth) //set my health bar to my current health
    {
        healthBar.transform.localScale = new Vector3(myHealth, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
    }
}
