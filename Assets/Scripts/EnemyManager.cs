using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private Vector3[] spawnPoints;
    [SerializeField]
    private int maxEnnemies;
    [SerializeField]
    private float spawnEvery;
    [SerializeField]
    private GameObject enemyObject;
    

    private bool enemyCanSpawn;
    private bool _endGame;
    public bool endGame {get => _endGame; set { _endGame = value; } }
    private int _nbEnemies;
    public int nbEnemies { get => _nbEnemies; set { _nbEnemies += value; } }

    // EnemyManager is a singleton
    public static EnemyManager instance;  

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        enemyCanSpawn = true;
        _nbEnemies = 0;
        _endGame = false;
    }

    // Coroutine that is waiting several seconds before allowing another spawn of enemy
    IEnumerator Timer()
    {
        enemyCanSpawn = false;
        yield return new WaitForSeconds(spawnEvery);
        enemyCanSpawn = true;
    }

    // Update is called once per frame
    void Update()
    {
        // If a enemy can spawn due to the number of enemies on the map or if the cooldown has been finished
        if (enemyCanSpawn && nbEnemies < maxEnnemies && !endGame)
        {
            // Randomly choose a spawn point on the map
            int spawnIndex = Random.Range(0, spawnPoints.Length);
            // Enemy spawn
            Instantiate(enemyObject, spawnPoints[spawnIndex], Quaternion.Euler(0.0f, 0.0f, 0.0f));
            // Increase number of enemies on the map
            ++_nbEnemies;
            // Start cooldown before another enemy can spawn
            StartCoroutine(Timer());
        }
            
    }
}
