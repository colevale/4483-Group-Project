using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
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

    private int selected_tower;
    private int newTower;
    public List<Image> arrows = new List<Image>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isBuilding = false;
        selected_tower = 0;
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

            TowerSelection();

            //not pointing at building
            if (!Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out var hit, buildDistance, buildingLayer))
            {
                Debug.DrawLine(transform.position, hit.point, Color.green);
                if (Input.GetButtonDown("Shoot")) //intended to be LMB
                {
                    upgradePrompt.gameObject.SetActive(false);
                    PlaceBldg(selectedTowers[selected_tower]);
                }
            }
            //everything in this else statement should be a tower by the raycast, so no error correction needed
            else
            {
                Debug.DrawLine(transform.position, hit.point, Color.green);
                GameObject pointedObject = hit.transform.gameObject;
                //Debug.Log(pointedObject.name);
                //duplicates for now
                PlayerController pc = player.GetComponent<PlayerController>();
                Tower towerScript = pointedObject.GetComponent<Tower>();
                int upgradeCost = towerScript.GetUpgradeCost();

                upgradePrompt.text = "= Upgrade for: " + upgradeCost.ToString() + "Gold\n- Remove for: " + towerScript.GetSellPrice().ToString() + "gold";
                upgradePrompt.gameObject.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Equals))
                {
                    UpgradeBldg(pointedObject, towerScript, pc);
                }
                else if (Input.GetKeyDown(KeyCode.Minus))
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
            buildPosition = new Vector3(hit.point.x, 3, hit.point.z);
            Instantiate(tower, buildPosition, Quaternion.identity);


            pc.RemoveGold((int)tower.GetComponent<Tower>().GetValue());
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

    private void TowerSelection()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            newTower = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            newTower = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            newTower = 2;
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            newTower = (selected_tower + 1) % 3;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            newTower = selected_tower - 1;
            if (newTower == -1)
            {
                newTower = 2;
            }

        }

        if (newTower != selected_tower)
        {
            arrows[selected_tower].color = new Color(1f, 1f, 1f, 0f);
            arrows[newTower].color = new Color(1f, 1f, 1f, 1f);
            selected_tower = newTower;
            tmp_indicator.SetText("Building Mode " + selectedTowers[selected_tower].GetComponent<Tower>().GetValue().ToString() + "G for Tower");
        }
    }
}
