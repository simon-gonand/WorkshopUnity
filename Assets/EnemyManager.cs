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
    public bool endGame;

    public static EnemyManager instance;
    public int nbEnemies;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        enemyCanSpawn = true;
        nbEnemies = 0;
        endGame = false;
    }

    IEnumerator Timer()
    {
        enemyCanSpawn = false;
        yield return new WaitForSeconds(spawnEvery);
        enemyCanSpawn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyCanSpawn && nbEnemies < maxEnnemies && !endGame)
        {
            int spawnIndex = Random.Range(0, 3);
            GameObject ethan = Instantiate(enemyObject, spawnPoints[spawnIndex], Quaternion.Euler(0.0f, 0.0f, 0.0f));
            ++nbEnemies;
            StartCoroutine(Timer());
        }
            
    }
}
