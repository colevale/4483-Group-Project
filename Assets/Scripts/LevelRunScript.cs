using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;

//we can build our level within the universal 'LevelRun' scene was what i was envisioning
//but you can just link the uibutton onclick using the ChangeLevelScene function
//specifically for levelrun scene
public class LevelRunScript : MonoBehaviour
{
    public TMP_Text title;
    //level progress text
    public TMP_Text runButtonText;
    public Button runButton;

    /* stages are probably too complex for each territory (was thinking of days and stuff, but probably too boring)
    //how many stages are in all levels
    //this is assumed to be a 'universal' level containing all the things
    public static int[] stages;
    public static int[] stagesProgress;
    // How many levels are in this territory? e.g. 1/3
    public int numerator;
    public int denominator;*/

    public int currentLevel;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // c_lvl is current level selected, stored in player preferences to avoid global variable messiness in script e.g. static vars in script
    void Start()
    {
        //after winning unlock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        currentLevel = PlayerPrefs.GetInt("c_lvl"); 
        int progress = PlayerPrefs.GetInt("progress"); 
        title.text = "Level " + currentLevel.ToString();

        //change button text depending on progress
        if (progress <= currentLevel)
        {
            runButton.interactable = true;
            runButtonText.text = "Start Level";
        }
        //after beating level e.g. progress > c_lvl
        else
        {
            runButton.interactable = false;
            runButtonText.text = "Victory";
        }

            //uibutton link on click
            Button btn = runButton.GetComponent<Button>();
        btn.onClick.AddListener(changeScene);
        
    }

  

    //stub to change scene if we are not using this scene for the level
    void changeScene()
    {
        Debug.Log("start button clicked");
        //temporary
        SceneManager.LoadScene("proto_map");
    }
}
