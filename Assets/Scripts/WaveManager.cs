using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    public GameObject[] primarySpawners;
    private GameObject[] levelSpawners;

    public int[] waves; // # of waves equal to array length, input of array equal to total enemies spawned in wave
    int currWave;
    private bool[] eachWaveCheck;
    bool fullCheck;
    bool waveEnd;

    int level;

    public GameObject player;
    [SerializeField] private LayerMask enemyLayer;

    public DayNightCycle dayCycle;
    public float timeForWave;

    public CrystalActivation crystalObject;

    public int[] waveDoors;
    public Animator[] doors;

    public Animator waveClearAnim;
    public TextMeshProUGUI waveText1;
    public TextMeshProUGUI waveText2;

    private void Awake()
    {
        foreach (Animator an in doors)
        {
            an.SetBool("IsOpen", false);
        }
    }

    // i probably didn't have time before activity 5 to fully implement wave oh well
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        level = PlayerPrefs.GetInt("progress");
        GetLevelSpawner(level);
        timeForWave = dayCycle.getTimeInOneDay() / waves.Length;
        currWave = 0;
        waveEnd = true;
    }

    private void Update()
    {
        int i = 0;
        fullCheck = true;
        foreach (GameObject spawner in levelSpawners)
        {
            Spawner spawnScript = spawner.GetComponent<Spawner>();
            if (spawnScript.enemiesDefeated() && spawnScript.allEnemiesSpawned())
            {
                PlayerPrefs.SetInt("progress", level + 1);

                PlayerController.playcon.SaveGold();
                SceneManager.LoadScene("LevelRun");
                eachWaveCheck[i] = true;
            }
            else
            {
                eachWaveCheck[i] = false;
            }
            i++;
        }

        foreach (bool check in eachWaveCheck)
        {
            if (check == false)
            {
                fullCheck = false;
            }
        }

        if (fullCheck && !waveEnd)
        {
            waveEnd = true;
            EndWave();
        }
    }

    public void GetLevelSpawner(int level)
    {
        int howManyDoorsOpen;

        switch (level)
        {
            case 1:
                howManyDoorsOpen = 1;
                break;
            case 2:
                howManyDoorsOpen = 2;
                break;
            case 3:
                howManyDoorsOpen = 4;
                break;
            default:
                howManyDoorsOpen = 1;
                break;
        }

        levelSpawners = new GameObject[howManyDoorsOpen];
        eachWaveCheck = new bool[howManyDoorsOpen];

        for (int i = 0; i < levelSpawners.Length; i++)
        {
            OpenDoors(i);
            levelSpawners[i] = primarySpawners[i];
            eachWaveCheck[i] = false;
        }
    }

    public void StartWave()
    {
        dayCycle.SetPauseTime(timeForWave * (currWave + 1));
        dayCycle.ResumeTime();

        //multi spawn
        foreach (GameObject spawner in levelSpawners)
        {
            Spawner spawnScript = spawner.GetComponent<Spawner>();
            spawnScript.setSpawnNumber(waves[currWave]);
            spawnScript.StartWave();
        }

        currWave++;
        waveEnd = false;
    }

    public void EndWave()
    {
        PlayerController.playcon.Heal();
        //proof of concept with single wave
        //most of the componentry will be extended
        if (currWave >= waves.Length)
        {
            PlayerPrefs.SetInt("progress", level + 1);
            PlayerController.playcon.SaveGold();

            waveClearAnim.SetTrigger("Wave Clear");
            waveText1.text = "Victory!!!\nActivate the horn to return to base!";
            waveText2.text = "Victory!!!\nActivate the horn to return to base!";

            AudioManager.instance.PlaySound("wave_end");
            AudioManager.instance.TransitionSong("victory", 1, 5);

            crystalObject.OnWin();
        }
        else
        {
            int i = 0;
            foreach (GameObject spawner in levelSpawners)
            {
                Spawner spawnScript = spawner.GetComponent<Spawner>();
                spawnScript.ResetWave();
                eachWaveCheck[i] = false;
                i++;
            }
            crystalObject.EndWave();

            waveClearAnim.SetTrigger("Wave Clear");
            waveText1.text = "Wave Clear!\n" + currWave + " / " + waves.Length;
            waveText2.text = "Wave Clear!\n" + currWave + " / " + waves.Length;
        }

        
    }

    private IEnumerator GameEnding(int choice)
    {
        if (choice == 0)
        {
            PlayerPrefs.SetInt("ending", 0);
        }
        else
        {
             PlayerPrefs.SetInt("ending", 1);
        }
        yield return new WaitForSeconds(1.0f);
    }
    


    void OpenDoors(int whichDoors)
    {
         doors[whichDoors].SetBool("IsOpen", true);
    }
}
