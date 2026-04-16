using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    public GameObject[] primarySpawners;

    public int[] waves; // # of waves equal to array length, input of array equal to total enemies spawned in wave
    int currWave;
    bool waveInProgress;
    bool waveStart;

    int level;

    public GameObject player;
    PlayerController playerController;
    [SerializeField] private LayerMask enemyLayer;

    public DayNightCycle dayCycle;
    public float timeForWave;

    public CrystalActivation crystalObject;


    // i probably didn't have time before activity 5 to fully implement wave oh well
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        level = PlayerPrefs.GetInt("progress");
        timeForWave = dayCycle.getTimeInOneDay() / waves.Length;
        currWave = 0;
        waveInProgress = false;
        waveStart = false;
    }

    private void Update()
    {
        foreach (GameObject spawner in primarySpawners)
        {
            Spawner spawnScript = spawner.GetComponent<Spawner>();
            if (spawnScript.enemiesDefeated() && spawnScript.allEnemiesSpawned())
            {
                waveInProgress = false;
            }
            else
            {
                waveInProgress = true;
            }
        }

        if (waveStart && !waveInProgress)
        {
            EndWave();
        }
    }

    public void StartWave()
    {
        dayCycle.SetPauseTime(timeForWave * (currWave + 1));
        dayCycle.ResumeTime();

        //multi spawn
        foreach (GameObject spawner in primarySpawners)
        {
            Spawner spawnScript = spawner.GetComponent<Spawner>();
            spawnScript.setSpawnNumber(waves[currWave] / primarySpawners.Length);
            spawnScript.StartWave();
        }

        currWave++;
        waveStart = true;
    }

    public void EndWave()
    {
        waveInProgress = false;
        waveStart = false;
        //proof of concept with single wave
        playerController = player.GetComponent<PlayerController>();
        //most of the componentry will be extended
        if (currWave >= waves.Length)
        {
            PlayerPrefs.SetInt("progress", level + 1);
            playerController.SaveGold();
            SceneManager.LoadScene("LevelRun");
        }
        else
        {
            foreach (GameObject spawner in primarySpawners)
            {
                Spawner spawnScript = spawner.GetComponent<Spawner>();
                spawnScript.ResetWave();
            }
            crystalObject.EndWave();
        }
    }
}
