using System.Collections;
using System.Diagnostics.Contracts;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject checkpoints;
    public int numberToSpawn;
    private int counter;

    [SerializeField]
    private GameObject enemyPreFab;

    [SerializeField]
    private float spawnInterval = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        counter = 0;
        StartCoroutine(spawnEnemy(spawnInterval, enemyPreFab));
    }

    //following https://www.youtube.com/watch?v=SELTWo1XZ0c

    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        if (counter < numberToSpawn || numberToSpawn < 0)
        {

        
            yield return new WaitForSeconds(interval);
            GameObject newEnemy = Instantiate(enemy, transform.position, transform.rotation);
            newEnemy.SetActive(false);
            EnemyWaypoints patrolScript = newEnemy.GetComponent<EnemyWaypoints>();


            //adds checkpoints
            patrolScript.Setup(checkpoints);
            newEnemy.SetActive(true);
            counter++;
            StartCoroutine(spawnEnemy(interval, enemy));

        }
    }
}
