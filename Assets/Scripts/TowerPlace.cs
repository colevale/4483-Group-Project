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

    private GameObject ghostTowerGameObject;
    [SerializeField] private Material ghostMaterialValid;
    [SerializeField] private Material ghostMaterialInvalid;
    [SerializeField] private float connectorOverlapRadius = 1;
    [SerializeField] private float maxGroundAngle = 30f;
    private bool isGhostInValidPosition = false;

    public GameObject playerGun;
    private Gun gunScript;

    private void Awake()
    {
        gunScript = playerGun.GetComponent<Gun>();
    }

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
        }

        if (isBuilding)
        {
            gunScript.SetShot(false);
            playerGun.SetActive(false);
            tmp_indicator.gameObject.SetActive(true);
            upgradePrompt.gameObject.SetActive(false);

            drawGhost();

            if (Input.GetButtonDown("Shoot"))
            {
                placeTower();
            }
        }
        else if (ghostTowerGameObject)
        {
            Destroy(ghostTowerGameObject);
            ghostTowerGameObject = null;
            gunScript.SetShot(true);
            playerGun.SetActive(true);
            tmp_indicator.gameObject.SetActive(false);
            upgradePrompt.gameObject.SetActive(false);
        }
    }


    //build ghost tower obj
    private void drawGhost()
    {
        TowerSelection();

        GameObject currentTower = selectedTowers[selected_tower];
        createGhostPrefab(currentTower);

        moveGhostPrefabToRaycast();
        checkTowerValdity();

    }

    private void createGhostPrefab(GameObject tower)
    {
        if (ghostTowerGameObject == null)
        {
            ghostTowerGameObject = Instantiate(tower);

            ghostifyTower(ghostTowerGameObject.transform);
        }
    }

    private void moveGhostPrefabToRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            ghostTowerGameObject.transform.position = new Vector3(hit.point.x, hit.point.y + 2.8f, hit.point.z);
        }
    }

    private void checkTowerValdity()
    {
        Collider[] colliders = Physics.OverlapSphere(ghostTowerGameObject.transform.position, connectorOverlapRadius, buildLayer);
        if (colliders.Length > 0)
        {
            Destroy(ghostTowerGameObject);
            ghostTowerGameObject = null;
        }
        else
        {
            ghostNewTower();
        }
    }

    private void ghostNewTower()
    {
        PlayerController pc = player.GetComponent<PlayerController>();
        Tower currTower = selectedTowers[selected_tower].GetComponent<Tower>();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.transform.root.CompareTag("Tower"))
            {
                Destroy(ghostTowerGameObject);
                ghostTowerGameObject = null;
                isGhostInValidPosition = false;
                UpgradeFunction(hit.collider.transform.gameObject, pc);
                return;
            }
            
            
            if (Vector3.Angle(hit.normal, Vector3.up) < maxGroundAngle && currTower.GetValue() <= pc.GetGold())
            {
                ghostifyTower(ghostTowerGameObject.transform, ghostMaterialValid);
                isGhostInValidPosition = true;
            }
            else
            {
                ghostifyTower(ghostTowerGameObject.transform, ghostMaterialInvalid);
                isGhostInValidPosition = false;
            }
        }
    }

    private void ghostifyTower(Transform tower, Material ghostMaterial = null)
    {
        if (ghostMaterial != null)
        {
            foreach(MeshRenderer meshRend in tower.GetComponentsInChildren<MeshRenderer>())
            {
                meshRend.material = ghostMaterial;
            }
        }
        else
        {
            foreach (Collider collider in tower.GetComponentsInChildren<Collider>())
            {
                collider.enabled = false;
            }
        }
    }

    private void placeTower()
    {
        PlayerController pc = player.GetComponent<PlayerController>();
        Tower currTower = selectedTowers[selected_tower].GetComponent<Tower>();
        if (pc != null && currTower.GetValue() <= pc.GetGold())
        {
            if (ghostTowerGameObject != null & isGhostInValidPosition)
            {
                GameObject tower = Instantiate(selectedTowers[selected_tower], ghostTowerGameObject.transform.position, ghostTowerGameObject.transform.rotation);

                pc.RemoveGold((int)tower.GetComponent<Tower>().GetValue());
            } 
        }
    }

    private void UpgradeFunction(GameObject tower, PlayerController pc)
    {
        Tower towerScript = tower.GetComponent<Tower>();
        int upgradeCost = towerScript.GetUpgradeCost();
        int sellCost = towerScript.GetSellPrice();

        tmp_indicator.gameObject.SetActive(false);

        if (towerScript.CanUpgrade())
        {
            upgradePrompt.SetText("Left Click To Upgrade (Upgrade Cost = " + upgradeCost.ToString() + "G)\nRight Click To Sell (Sell Cost = " + sellCost.ToString() + "G)");
            upgradePrompt.gameObject.SetActive(true);

            if (Input.GetButtonDown("Shoot"))
            {
                if (towerScript != null && pc.gold >= upgradeCost)
                {
                    towerScript.Upgrade();
                    pc.RemoveGold(upgradeCost);
                }
            }
        }
        else
        {
            upgradePrompt.SetText("Upgrade MAX\nRight Click To Sell (Sell Cost = " + sellCost.ToString() + "G)");
            upgradePrompt.gameObject.SetActive(true);
        }
        

        if (Input.GetButtonDown("PlaceTower"))
        {
            if (towerScript != null && pc.gold >= sellCost)
            {
                Destroy(towerScript.gameObject);
                pc.RemoveGold(sellCost);
            }
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
            Destroy(ghostTowerGameObject);
            ghostTowerGameObject = null;
            arrows[selected_tower].color = new Color(1f, 1f, 1f, 0f);
            arrows[newTower].color = new Color(1f, 1f, 1f, 1f);
            selected_tower = newTower;
            tmp_indicator.SetText("Building Mode " + selectedTowers[selected_tower].GetComponent<Tower>().GetValue().ToString() + "G for Tower");
        }
    }
}
