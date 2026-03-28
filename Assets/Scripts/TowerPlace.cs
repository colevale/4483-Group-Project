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
    public TMP_Text tmp_upgrade_prompt;
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
                if (Input.GetMouseButtonDown(0))
                {
                    PlaceBldg(selectedTowers[0]);
                }
            }
            else
            {
                GameObject pointedObject = hit.transform.gameObject;
                Debug.Log(pointedObject.name);
                if (Input.GetMouseButtonDown(0))
                {
                    UpgradeBldg(hit);
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    RemoveBldg();
                }
            }
        }
        else
        {
            tmp_indicator.gameObject.SetActive(false);
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
        if (pc != null && pc.gold > 0)
        {
            //prevents placement of towers not on ground
            if (!Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out var hit, buildDistance, buildLayer))
            {
                //Debug.Log(hit);
                return;
            }
            buildPosition = new Vector3(hit.point.x, 3, hit.point.z);
            Instantiate(tower, buildPosition, Quaternion.identity);


            pc.RemoveGold(200);
            Debug.Log(pc.gold);
        }
        
    }
    private void RemoveBldg()
    {
        Debug.Log("Building Remove");
    }

    private void UpgradeBldg(RaycastHit hit)
    {
        Debug.Log("Building Upgrade" + hit.transform.gameObject.name.ToString());
    }
}
