using UnityEngine;
using UnityEngine.UI;

public class TerritoryScript : MonoBehaviour
    
{
    public static int currentProgression;
    public Button[] levels;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Progress is the current progress of the player so far in terms of unlocked levels
    void Start()
    {
        ResetLevel();
        //PlayerPrefs.SetInt("progress", 0); //play with this to see the progression working
        currentProgression = PlayerPrefs.GetInt("progress");
        //checks to see if it's initialized
        if (!(currentProgression > 0 ))
        {
            //initializes it to 1 if it is a new game
            PlayerPrefs.SetInt("progress", 1);
            PlayerPrefs.SetInt("gold", 400);
            //default value matches initialization of 1
            currentProgression = 1;
        }

        //simplified for current prototype
        //deactivates all buttons that player has not unlocked so far
        for (int i = 0; i < currentProgression; i++)
        {
            levels[i].interactable = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetLevel()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].interactable = false;
        }
    }
}
