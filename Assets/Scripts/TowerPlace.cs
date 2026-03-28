using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerPlace : MonoBehaviour
{
    [SerializeField] private List<GameObject> selectedTowers;
    [SerializeField] private float buildDistance;
    [SerializeField] private LayerMask buildLayer; //valid placement spots layer "ground layer"
    [SerializeField] private LayerMask buildingLayer; //building layer for upgrades "tower layer"

    public Camera playerCam;
    public GameObject player;
    public TMP_Text tmp_indicator; //temporary text before inventory system
    public TMP_Text upgradePrompt;
    private bool isBuilding;
    private bool canBuild;
    Vector3 buildPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isBuilding = false;
    }

    // Update is called once per frame
    void Update()
    {
        //toggle building mode
        if (Input.GetKeyDown(KeyCode.B))
        {
            isBuilding = !isBuilding;
            //Debug.Log(isBuilding);
        }
        if (isBuilding)
        {
            tmp_indicator.gameObject.SetActive(true);
            //drawGhost();
            //temporary [0]
            
            //not pointing at building
            if (!Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out var hit, buildDistance, buildingLayer))
            {
                if (Input.GetButtonDown("Shoot")) //intended to be LMB
                {
                    upgradePrompt.gameObject.SetActive(false);
                    PlaceBldg(selectedTowers[0]);
                }
            }
            //everything in this else statement should be a tower by the raycast, so no error correction needed
            else
            {
                GameObject pointedObject = hit.transform.gameObject;
                //Debug.Log(pointedObject.name);
                //duplicates for now
                PlayerController pc = player.GetComponent<PlayerController>();
                Tower towerScript = pointedObject.GetComponent<Tower>();
                int upgradeCost = towerScript.GetUpgradeCost();

                upgradePrompt.text = "Upgrade for: " +  upgradeCost.ToString() + "Gold\nRemove for: " + towerScript.GetSellPrice().ToString() + "gold";
                upgradePrompt.gameObject.SetActive(true);
                if (Input.GetButtonDown("Shoot")) //intended to be LMB
                {
                    UpgradeBldg(pointedObject,towerScript, pc);
                }
                else if (Input.GetButtonDown("PlaceTower")) //intended to be RMB`
                {
                    RemoveBldg(pointedObject, towerScript, pc);
                }
            }
        }
        else
        {
            tmp_indicator.gameObject.SetActive(false);
            upgradePrompt.gameObject.SetActive(false);
        }
        
    }

    

    //build ghost
    private void drawGhost()
    {
        if (!Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out var hit, buildDistance, buildLayer))
        {

        }
    }

    private void PlaceBldg(GameObject tower)
    {
        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc != null && pc.gold > 200)
        {
            //prevents placement of towers not on ground
            if (!Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out var hit, buildDistance, buildLayer))
            {
                //Debug.Log(hit);
                return;
            }
            buildPosition = new Vector3(hit.point.x,  3, hit.point.z);
            Instantiate(tower, buildPosition, Quaternion.identity);


            pc.RemoveGold(200);
            Debug.Log("build " + pc.gold.ToString());
        }
        
    }
    private void RemoveBldg(GameObject tower, Tower towerScript, PlayerController pc)
    {
        // Debug.Log("Building Remove");
        //PlayerController pc = player.GetComponent<PlayerController>();
        //Tower towerScript = tower.GetComponent<Tower>();
        pc.AddGold(towerScript.GetSellPrice());
        Destroy(tower);
        Debug.Log("Delete " + pc.gold.ToString());
    }

    private void UpgradeBldg(GameObject tower, Tower towerScript, PlayerController pc)
    {

        //Debug.Log("Building Upgrade" + hit.transform.gameObject.name.ToString());
        //PlayerController pc = player.GetComponent<PlayerController>();
        //Tower towerScript = tower.GetComponent<Tower>();
        int upgradeCost = towerScript.GetUpgradeCost();
        if (towerScript != null && pc.gold > upgradeCost && towerScript.CanUpgrade())
        {
            towerScript.Upgrade();
            pc.RemoveGold(upgradeCost);
            Debug.Log("Upgrade " + pc.gold.ToString());
        }

    }
}
