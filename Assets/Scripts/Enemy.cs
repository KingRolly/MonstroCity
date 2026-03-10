using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Implements enemy behaviour and stats <br/>
/// Likely going to be an interface for the enemies so that we can implement more complex enemies later on <br/>
/// - Nicholas Liang (Feb. 20th, 2026)
/// </summary>
public class Enemy : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] private string enemyType;
    [SerializeField] private Sprite enemySprite;
    [SerializeField] private int health;
    [SerializeField] private float speed;
    [SerializeField] private int damage; // amount of damage enemy deals to player's health
    [SerializeField] private int moneyReward; // amount of money given to player when killed 

    private EnemyManager enemyManager;
    private UIManager uiManager;
    [SerializeField] AudioClip damageSound;

    [SerializeField] private List<Vector2Int> enemyPath;
    private int pathIndex;
    public IObjectPool<GameObject> enemyObjectPool;

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        // Check if there is remaining path to traverse and that path exists
        if (pathIndex < enemyPath.Count && enemyPath != null)
        {
            TravelPath();

            // Check if enemy has reached the next path coordinate
            if (transform.position == new Vector3(enemyPath[pathIndex].x, enemyPath[pathIndex].y, transform.position.z))
            {
                pathIndex++;
            }
        }
        // Enemy reached end of path
        else
        {
            // TODO: Change this (deals damage to player and despawns upon reaching end of path for debugging rn)
            DamagePlayerHealth();
            Despawn();
        }
    }


    // Enemy reads the coordinates of enemyPath to travel along the path in-game
    private void TravelPath()
    {
        // Read the enemyPath list and move enemy towards to the next path coordinate
        transform.position = Vector2.MoveTowards(transform.position, enemyPath[pathIndex], speed * Time.deltaTime);
    }

    // Used to set info of enemy
    // Enemy Manager should call this upon instantiating an enemy
    public void SetInfo(string type, int hp, float spd, int dmg, int money, Sprite sprite)
    {
        enemyType = type;
        health = hp;
        speed = spd;
        damage = dmg;
        enemySprite = sprite;
        moneyReward = money;
        this.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    /// <summary>
    /// Used to assign references Enemy needs upon instantiation <br/> 
    /// EnemyManager should call this when spawning in enemies
    /// </summary>
    /// <param name="enemyManager"></param>
    /// <param name="uiManager"></param>
    public void AssignReferences(EnemyManager enemyManager, UIManager uiManager)
    {
        this.uiManager = uiManager;
        this.enemyManager = enemyManager;
    }

    // Assign path for enemy to take
    public void SetPath(List<Vector2Int> path)
    {
        enemyPath = path;

        // Check for enemyPath not being assigned for some reason
        if (enemyPath == null)
        {
            Debug.Log("Path doesn't exist");
        }
        else
        {
            // Set starting position of enemy
            Vector3 startingPosition = new Vector3(enemyPath[0].x, enemyPath[0].y, transform.position.z);
            transform.position = startingPosition;
            pathIndex = 0;
        }
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // TODO: Check if enemy collided with a tower attack and take appropriate damage
    }

    /// <summary>
    /// Enemy deals damage to player's healh
    /// </summary>
    public void DamagePlayerHealth()
    {
        // Update health in UIManager
        uiManager.ChangeHealth(-damage);
    }

    /// <summary>
    /// Deal damage to enemy, despawns enemy if damage is fatal
    /// </summary>
    /// <param name="dmg">Amount of damage dealt to enemy</param>
    public void TakeDamage(int dmg)
    {
        // Update enemy's health to take correct amount of damage
        // If the damage kills enemy, then give player money reward, despawn enemy, and return it to object pool
        gameObject.GetComponent<AudioSource>().PlayOneShot(damageSound);
        this.health -= dmg;
        if (this.health <= 0)
        {
            uiManager.ChangeMoney(moneyReward);
            Despawn();
            return;
        }

        // Change enemy sprite to indicate damage
        StartCoroutine(HurtEnemySprite());

    }

    /// <summary>
    /// Private helper for TakeDamage to change enemy sprite to show damage has been taken
    /// </summary>
    private IEnumerator HurtEnemySprite()
    {
        float t = 0.0f;
        float duration = 0.1f;
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;

        while (t < duration)
        {
            t += Time.deltaTime;
            gameObject.GetComponent<SpriteRenderer>().color = Color32.Lerp(Color.red, Color.white, t / duration);
            yield return null;
        }
    }

    /// <summary>
    /// Private helper method for despawning enemy
    /// </summary>
    private void Despawn()
    {
        // Call enemy manager's despawn function to depsawn this enemy
        enemyManager.DespawnEnemy(this.gameObject);
    }

    #region Basic Getters
    public string getType()
    {
        return this.enemyType;
    }
    public Sprite getSprite()
    {
        return this.enemySprite;
    }    
    public int getHealth()
    {
        return this.health;
    }
    public float getSpeed()
    {
        return this.speed;
    }
    public int getDamage()
    {
        return this.damage;
    }
    #endregion
}
