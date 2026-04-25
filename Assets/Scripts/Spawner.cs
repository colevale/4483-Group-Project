using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner instance;


    public GameObject checkpoints;
    private int numberToSpawn;
    private int counter;
    public GameObject player;

    private bool enemiesSpawned;
    private bool allEnemiesDefeated;

    List<GameObject> spawned;

    private Coroutine routine;
    private bool skipped;

    [SerializeField] private GameObject enemyPreFab;
    //[SerializeField] private LayerMask buildlayer;

    [SerializeField]
    private float spawnInterval = 3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance == null)
            instance = this;


        counter = -1;
        spawned = new List<GameObject>();
        enemiesSpawned = false;
        allEnemiesDefeated = false;
        skipped = false;
    }

    private void FixedUpdate()
    {
        //removes all items destroyed in spawned objects
        spawned.RemoveAll(item => item == null);
        if (counter == numberToSpawn)
        {
            enemiesSpawned = true;
            if (spawned.Count <= 0)
            {
                allEnemiesDefeated = true;
            }
        }
    }

    public void setSpawnNumber(int num)
    {
        numberToSpawn = num;
    }

    public bool allEnemiesSpawned()
    {
        return enemiesSpawned;
    }

    public bool enemiesDefeated()
    {
        return allEnemiesDefeated;
    }

    //following https://www.youtube.com/watch?v=SELTWo1XZ0c

    public void StartWave()
    {
        counter = 0;
        skipped = false;
        routine = StartCoroutine(spawnEnemy(spawnInterval, enemyPreFab));
    }

    public void ResetWave()
    {
        counter = -1;
        enemiesSpawned = false;
        allEnemiesDefeated = false;
    }

    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        if (counter < numberToSpawn || numberToSpawn < 0)
        {

        
            yield return new WaitForSeconds(interval);
            GameObject newEnemy = Instantiate(enemy, transform.position, transform.rotation, gameObject.transform);
            newEnemy.SetActive(false);
            
            newEnemy.layer = 8;
            Enemy enemyScript = newEnemy.GetComponent<Enemy>();

            //sets player target
            enemyScript.player = player;
            enemyScript.setupPlayer();
            enemyScript.Setup(checkpoints);

            //adds checkpoints
            newEnemy.SetActive(true);
            spawned.Add(newEnemy);
            counter++;

            if (skipped != true)
            {
                routine = StartCoroutine(spawnEnemy(interval, enemy));
            }
        }
    }


    public void RemoveEnemy(GameObject enemy)
    {
        spawned.Remove(enemy);
    }

    public void StopSpawner()
    {
        StopCoroutine(routine);
        skipped = true;
    }
}
