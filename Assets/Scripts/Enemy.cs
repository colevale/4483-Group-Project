using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{

    public int maxHP = 10, curHP = 10;

    public HPBar hpbar;

    Rigidbody rb;


    // THIS SCRIPT DOESN'T ACCOUNT FOR GRAVITY HOWEVER IT SHOULDN'T BE A PROBLEM JUST BE SURE TO LOCK THE Y POSITION IF THERE NEEDS TO BE GRAVITY

    // Create NavMeshAgent on Enemy Add it to this value
    public NavMeshAgent agent;

    // NOTE YOU WILL NEED TO MAKE AN EMPTY GAMEOBJECT CALLED NAVMESH AND PLACE A NAVMESH SURFACE INTO IT, BE SURE TO DROPDOWN THE OBJECT COLLECTION TAG AND SELECT ONLY GROUND LAYER IN THE "INCLUDE LAYERS" SECTION, if you are confused about layers see the layermask object

    // Create multiple empty game objects as checkpoint places (corners of the maze), place in order where you want enemy to go 1st element first checkpoint, last element crystal
    public List<Transform> checkPoints = new List<Transform>();
    private int currentCheckpoint;

    // Create new layer called ground, make the ground the layer, set this value to the ground layer
    public LayerMask ground;

    private Vector3 nextWalkPoint;
    bool nextWalkPointSet;

    // NOTE SPEED CAN BE CHANGED IN THE NAVMESHAGENT SO DON'T WORRY ABOUT A SPEED VARIABLE

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        currentCheckpoint = 0;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hpbar.SetMaxHP(maxHP);
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!nextWalkPointSet)
        {
            FindNextCheckpoint();
        }

        if (nextWalkPointSet)
        {
            agent.SetDestination(nextWalkPoint);
        }

        Vector3 distanceToCheckpoint = transform.position - nextWalkPoint;

        // Checks if enemy is next at checkpoint
        if (distanceToCheckpoint.magnitude < 1f)
        {
            nextWalkPointSet = false;

            // If there is another checkpoint move to point
            if (currentCheckpoint != checkPoints.Count - 1)
            {
                currentCheckpoint++;
            }
        }
    }


    public void TakeDamage(int damage, Vector3 knockback)
    {
        curHP -= damage;

        hpbar.UpdateHP(curHP);

        if (curHP <= 0)
            Destroy(this.gameObject);

        rb.AddForce(knockback);
    }

    private void FindNextCheckpoint()
    {
        nextWalkPoint = new Vector3(checkPoints[currentCheckpoint].position.x, transform.position.y, checkPoints[currentCheckpoint].position.z);

        if (Physics.Raycast(nextWalkPoint, -transform.up, 2f, ground))
        {
            nextWalkPointSet = true;
        }
    }
}
