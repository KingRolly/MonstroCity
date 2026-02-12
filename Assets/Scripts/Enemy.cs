using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class Description:
// Implements enemy behaviour and stats
// Likely going to be an interface for the enemies so that we can implement more complex enemies later on
public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private string enemyType;
    [SerializeField] private int health;
    [SerializeField] private float speed;
    [SerializeField] private int damage; // amount of damage enemy deals to player's health

    [SerializeField] private List<Vector2Int> enemyPath;
    private int pathIndex;

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
            travelPath();

            // Check if enemy has reached the next path coordinate
            if (transform.position == new Vector3(enemyPath[pathIndex].x, enemyPath[pathIndex].y, transform.position.z))
            {
                pathIndex++;
            }
        }
    }


    // Enemy reads the coordinates of enemyPath to travel along the path in-game
    private void travelPath()
    {
        // TODO: Read the enemyPath list and move enemy towards to the next path coordinate
        transform.position = Vector2.MoveTowards(transform.position, enemyPath[pathIndex], speed * Time.deltaTime);
    }

    // Used to set stats of enemy
    // Enemy Manager should call this upon instantiating an enemy
    public void setStats(string type, int hp, float spd, int dmg)
    {
        enemyType = type;
        health = hp;
        speed = spd;
        damage = dmg;
    }

    // Assign path for enemy to take
    public void setPath(List<Vector2Int> path)
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

    // Basic getters
    public string getType()
    {
        return this.enemyType;
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
}
